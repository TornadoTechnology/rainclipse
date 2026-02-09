using Hypercube.Core.Graphics.Patching;
using Hypercube.Core.Graphics.Rendering.Context;
using Hypercube.Mathematics;
using Hypercube.Mathematics.Vectors;

namespace Rainclipse;

public sealed class HypercubePatch : Patch
{
    private float _angle = 0f;

    public override void Draw(IRenderContext renderer)
    {
        _angle += 0.001f;

        var vertices4D = GenerateHypercubeVertices(1f);
        var projected2D = new List<Vector2>();

        foreach (var v in vertices4D)
        {
            var rotated = Rotate4D(v, _angle);
            var v3 = Project4Dto3D(rotated, 3f);
            var v2 = Project3Dto2D(v3, 3f);

            // screen space offset
            projected2D.Add(v2 * 360f + new Vector2(0, 0));
        }

        var edges = GenerateHypercubeEdges();

        foreach (var (a, b) in edges)
        {
            renderer.DrawLine(
                projected2D[a],
                projected2D[b],
                Color.Cyan,
                2f
            );
        }
    }

    // =========================
    // Math + Geometry
    // =========================

    private static Vector4[] GenerateHypercubeVertices(float size)
    {
        var list = new List<Vector4>();

        for (int x = -1; x <= 1; x += 2)
        for (int y = -1; y <= 1; y += 2)
        for (int z = -1; z <= 1; z += 2)
        for (int w = -1; w <= 1; w += 2)
            list.Add(new Vector4(x, y, z, w) * size);

        return list.ToArray();
    }

    private static List<(int, int)> GenerateHypercubeEdges()
    {
        var edges = new List<(int, int)>();

        for (int i = 0; i < 16; i++)
        {
            for (int j = i + 1; j < 16; j++)
            {
                int diff = BitCount(i ^ j);
                if (diff == 1)
                    edges.Add((i, j));
            }
        }

        return edges;
    }

    private static int BitCount(int v)
    {
        int c = 0;
        while (v != 0)
        {
            v &= v - 1;
            c++;
        }
        return c;
    }

    // =========================
    // 4D Rotation & Projection
    // =========================

    private static Vector4 Rotate4D(Vector4 v, float a)
    {
        // Rotate in XW and YZ planes
        float ca = MathF.Cos(a);
        float sa = MathF.Sin(a);

        // XW
        float x = v.X * ca - v.W * sa;
        float w = v.X * sa + v.W * ca;

        // YZ
        float y = v.Y * ca - v.Z * sa;
        float z = v.Y * sa + v.Z * ca;

        return new Vector4(x, y, z, w);
    }

    private static Vector3 Project4Dto3D(Vector4 v, float distance)
    {
        float w = 1f / (distance - v.W);
        return new Vector3(v.X * w, v.Y * w, v.Z * w);
    }

    private static Vector2 Project3Dto2D(Vector3 v, float distance)
    {
        float z = 1f / (distance - v.Z);
        return new Vector2(v.X * z, v.Y * z);
    }
}
