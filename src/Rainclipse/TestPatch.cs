using Hypercube.Graphics.Patching;
using Hypercube.Graphics.Rendering.Context;
using Hypercube.Mathematics;
using Hypercube.Mathematics.Shapes;

namespace Rainclipse;

public sealed class TestPatch : Patch
{
    public override void Draw(IRenderContext renderer)
    {
        renderer.DrawRectangle(new Box2(0, 1, 1, 0), Color.Blue);
    }
}