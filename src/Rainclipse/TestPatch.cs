using System.Text;
using Hypercube.Core.Graphics.Patching;
using Hypercube.Core.Graphics.Rendering;
using Hypercube.Core.Graphics.Rendering.Context;
using Hypercube.Core.Graphics.Rendering.Manager;
using Hypercube.Core.Graphics.Resources;
using Hypercube.Core.Resources;
using Hypercube.Core.Viewports;
using Hypercube.Mathematics;
using Hypercube.Mathematics.Vectors;
using Hypercube.Utilities.Debugging.Logger;
using Hypercube.Utilities.Dependencies;

namespace Rainclipse;

public sealed class TestPatch : Patch, IPostInject
{
    private const int ChunkSize = 256;
    
    [Dependency] private readonly IRenderManager _render = null!;
    [Dependency] private readonly IResourceManager _resource = null!;
    [Dependency] private readonly ILogger _logger = null!;
    
    private Font _font = null!;
    
    public void OnPostInject()
    {
        _font = _resource.Load<Font>("/fonts/OpenSans.ttf", [("size", 18)]);
    }

    public override void Draw(IRenderContext renderer, DrawPayload payload)
    {
        DrawCameraInfo(renderer, payload.Camera);
        DrawChunkGrid(renderer, payload.Camera);
    }
    
    private void DrawCameraInfo(IRenderContext renderer, ICamera camera)
    {
        var cameraStringBuilder = new StringBuilder();
        
        cameraStringBuilder.AppendLine("Camera params");
        cameraStringBuilder.AppendLine($" > Pos: {camera.Position}");
        cameraStringBuilder.AppendLine($" > Rot: {camera.Rotation}");
        cameraStringBuilder.AppendLine($" > Scl: {camera.Scale}");
        cameraStringBuilder.AppendLine($" > Siz: {camera.Size}");
        cameraStringBuilder.AppendLine("Graphics");
        cameraStringBuilder.AppendLine($" > FPS: {_render.Fps.ToString("0.###")}");


        renderer.DrawText(cameraStringBuilder.ToString(), _font, camera.Position.Xy - new Vector2(camera.Size.X, -camera.Size.Y) / 2f - Vector2.UnitY * 18f, Color.White);
    }
    
    private void DrawChunkGrid(IRenderContext renderer, ICamera camera)
    {
        var halfSize = camera.Size / 2f;
        
        var min = camera.Position.Xy - halfSize;
        var max = camera.Position.Xy + halfSize;
        
        var gridColor = Color.White.WithA(0.2f);
        
        var startX = float.Floor(min.X / ChunkSize) * ChunkSize;
        var startY = float.Floor(min.Y / ChunkSize) * ChunkSize;
        
        for (var x = startX; x <= max.X; x += ChunkSize)
        {
            var start = new Vector2(x, min.Y);
            var end = new Vector2(x, max.Y);
            
            renderer.DrawLine(start, end, gridColor);
        }

        for (var y = startY; y <= max.Y; y += ChunkSize)
        {
            var start = new Vector2(min.X, y);
            var end = new Vector2(max.X, y);
            
            renderer.DrawLine(start, end, gridColor);
        }
        
        for (var x = startX; x < max.X; x += ChunkSize)
        {
            for (var y = startY; y < max.Y; y += ChunkSize)
            {
                var chunkX = (int)float.Floor(x / ChunkSize);
                var chunkY = (int)float.Floor(y / ChunkSize);
                var chunkId = $"[{chunkX}, {chunkY}]";
                var center = new Vector2(x + ChunkSize / 2f, y + ChunkSize / 2f);
                renderer.DrawText(chunkId, _font, center, gridColor);
            }
        }
    }
}