using Hypercube.Graphics.Patching;
using Hypercube.Graphics.Rendering.Context;
using Hypercube.Graphics.Rendering.Manager;
using Hypercube.Graphics.Resources;
using Hypercube.Mathematics;
using Hypercube.Mathematics.Quaternions;
using Hypercube.Mathematics.Vectors;
using Hypercube.Resources;
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
    
    public void PostInject()
    {
        _font = _resource.Get<Font>("/fonts/OpenSans.ttf");
        _model = _resource.Get<Model>("/models/teapot.obj");
        _texture = _resource.Get<Texture>("/textures/default.png");
    }

    public override void Draw(IRenderContext renderer)
    {
        _segments += 0.01f;
        _rotation = _rotation.WithX(_rotation.X + 0.01f);
        
        renderer.DrawText($"{_render.BatchCount} / {_render.VerticesCount}", _font, new Vector2(-300, 200), Color.White);
        renderer.DrawCircle(new Vector2(100, 0), 50, Color.Beige, (int) _segments);
        renderer.DrawModel(_model, Vector3.Zero, Quaternion.FromEuler(_rotation), Vector3.One * 30, Color.White, _texture);
    }
}