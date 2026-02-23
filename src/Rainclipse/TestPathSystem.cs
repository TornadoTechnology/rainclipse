using Hypercube.Core.Ecs.Attributes;
using Hypercube.Core.Ecs.Core.Query;
using Hypercube.Core.Graphics.Rendering;
using Hypercube.Core.Graphics.Rendering.Context;
using Hypercube.Core.Graphics.Resources;
using Hypercube.Core.Resources;
using Hypercube.Core.Systems;
using Hypercube.Core.Systems.Transform;
using Hypercube.Mathematics;
using Hypercube.Mathematics.Vectors;
using Hypercube.Utilities.Debugging.Logger;
using Hypercube.Utilities.Dependencies;

namespace Rainclipse;

[RegisterEntitySystem]
public sealed class TestPathSystem : PatchEntitySystem
{
    [Dependency] private readonly IResourceManager _resource = null!;
    
    private EntityQuery _spriteQuery = null!;
    private Font _font = null!;

    public override void Startup()
    {
        base.Startup();
        
        _font = _resource.Load<Font>("/fonts/OpenSans.ttf", [("size", 24)]);
        
        _spriteQuery = EntityQueryBuilder
            .With<TransformComponent>()
            .Build();
    }
    
    public override void Draw(IRenderContext renderer, DrawPayload payload)
    {
        var enumerator = _spriteQuery.GetEnumerator;
        while (enumerator.MoveNext(out var entity))
        {
            var transformComponent = GetComponent<TransformComponent>(entity);
            var transformString = string.Empty;

            transformString += $"Pos: {transformComponent.LocalPosition}\r\n";
            transformString += $"Rot: {transformComponent.LocalRotation} ({transformComponent.LocalRotation.ToEulerDeg()})\r\n";
            transformString += $"Scl: {transformComponent.LocalScale}";

            
            var position = transformComponent.LocalPosition + new Vector2(32, 32);
            renderer.DrawText(transformString, _font, position.Xy, Color.White);
        }
    }
}