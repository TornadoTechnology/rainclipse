using Hypercube.Core.Audio.Manager;
using Hypercube.Core.Audio.Resources;
using Hypercube.Core.Ecs;
using Hypercube.Core.Ecs.Attributes;
using Hypercube.Core.Ecs.Core.Query;
using Hypercube.Core.Ecs.Events;
using Hypercube.Core.Input;
using Hypercube.Core.Resources;
using Hypercube.Core.Systems.Rendering;
using Hypercube.Core.Systems.Transform;
using Hypercube.Mathematics.Vectors;
using Hypercube.Utilities.Dependencies;

namespace Rainclipse;

[RegisterEntitySystem]
public sealed class TestSystem : EntitySystem
{
    [Dependency] private readonly IInputHandler _inputHandler = default!;
    [Dependency] private readonly IResourceManager _resource = default!;
    [Dependency] private readonly IAudioManager _audio = default!;
    
    private EntityQuery _testQuery = default!;
    
    public override void Startup()
    {
        base.Startup();
        
        _testQuery = EntityQueryBuilder
            .With<TestComponent>()
            .Build();

        var sound = _resource.Load<Audio>("/audio/game_boi_3.wav");
        var source = _audio.CreateSource(sound);
        // source.Start();
        
        Logger.Debug("Test!");
    }

    public override void Update(float deltaTime)
    {
        base.Update(deltaTime);

        var enumerator = _testQuery.GetEnumerator;
        while (enumerator.MoveNext(out var entity))
        {
            var transform = GetComponent<TransformComponent>(entity);
            
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