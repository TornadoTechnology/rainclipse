using Hypercube.Core.Ecs;
using Hypercube.Core.Ecs.Attributes;
using Hypercube.Core.Ecs.Core.Query;
using Hypercube.Core.Ecs.Events;
using Hypercube.Core.Input;
using Hypercube.Core.Systems.Rendering;
using Hypercube.Core.Systems.Transform;
using Hypercube.Mathematics.Vectors;
using Hypercube.Utilities.Dependencies;

namespace Rainclipse;

[RegisterEntitySystem]
public sealed class TestSystem : EntitySystem
{
    [Dependency] private readonly IInputHandler _inputHandler = default!;
    
    private EntityQuery _testQuery = default!;
    
    public override void Startup()
    {
        base.Startup();
        
        _testQuery = EntityQueryBuilder
            .With<TestComponent>()
            .Build();
        
        Logger.Debug("Test!");
    }

    public override void Update(float deltaTime)
    {
        base.Update(deltaTime);

        var enumerator = _testQuery.GetEnumerator;
        while (enumerator.MoveNext(out var entity))
        {
            var transform = GetComponent<TransformComponent>(entity);
            var sprite = GetComponent<SpriteComponent>(entity);

            if (_inputHandler.IsKeyHeld(Key.D))
                transform.LocalPosition += Vector2.UnitX * deltaTime;

            if (_inputHandler.IsKeyHeld(Key.A))
                transform.LocalPosition += -Vector2.UnitX * deltaTime;

            if (_inputHandler.IsKeyHeld(Key.W))
                transform.LocalPosition += Vector2.UnitY * deltaTime;

            if (_inputHandler.IsKeyHeld(Key.S))
                transform.LocalPosition += -Vector2.UnitY * deltaTime;
        }
    }

    [Subscribe]
    public void OnAdded(Entity entity, TestComponent component, ref AddedEvent _)
    {
        Logger.Debug($"{nameof(TestComponent)} added to {entity}");
    }
}