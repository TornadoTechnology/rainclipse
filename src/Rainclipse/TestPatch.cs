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
    
    public void PostInject()
    {
        _font = _resource.Get<Font>("/fonts/OpenSans.ttf");
        _model = _resource.Get<Model>("/models/teapot.obj");
    }

    public override void Draw(IRenderContext renderer)
    {
        renderer.DrawText(_render.BatchCount.ToString(), _font, new Vector2(-300, 200), Color.White, 1.5f);
        renderer.DrawModel(_model, Vector3.Zero, Quaternion.FromEuler(Vector3.Zero), Vector3.One * 30, Color.White);
    }
}