using Hypercube.Core.Graphics.Patching;
using Hypercube.Core.Graphics.Rendering.Context;
using Hypercube.Core.Graphics.Rendering.Manager;
using Hypercube.Core.Graphics.Resources;
using Hypercube.Core.Resources;
using Hypercube.Mathematics;
using Hypercube.Mathematics.Quaternions;
using Hypercube.Mathematics.Vectors;
using Hypercube.Utilities.Dependencies;

namespace Rainclipse;

public sealed class TestPatch : Patch, IPostInject
{
    [Dependency] private readonly IRenderManager _render = default!;
    [Dependency] private readonly IResourceManager _resource = default!;
    
    private Font _font = default!;
    private Model _model = default!;
    private Texture _texture = default!;
    
    private Vector3 _rotation = Vector3.Zero;
    private float _segments = 3;
    
    public void OnPostInject()
    {
        _font = _resource.Load<Font>("/fonts/OpenSans.ttf", [("size", 24)]);
        _model = _resource.Load<Model>("/models/teapot.obj");
        _texture = _resource.Load<Texture>("/textures/default.png");
    }

    public override void Draw(IRenderContext renderer)
    {
        _segments += 0.01f;
        _rotation = _rotation.WithX(_rotation.X + 0.01f);
        
        // renderer.DrawTexture(_font.Texture, Vector2.Zero, Angle.Zero, Vector2.One, Color.White);
        renderer.DrawText($"{_render.BatchCount}|{_render.VerticesCount}|{_render.Fps}|異体", _font, new Vector2(-300, 200), Color.White);
        renderer.DrawCircle(new Vector2(100, 0), 50, Color.Beige, (int) _segments);
        renderer.DrawModel(_model, Vector3.Zero, Quaternion.FromEuler(_rotation), Vector3.One * 30, Color.White, _texture);
    }
}