using Hypercube.Core.Audio.Manager;
using Hypercube.Core.Audio.Resources;
using Hypercube.Core.Ecs;
using Hypercube.Core.Ecs.Attributes;
using Hypercube.Core.Ecs.Core.Query;
using Hypercube.Core.Ecs.Events;
using Hypercube.Core.Input;
using Hypercube.Core.Input.Handler;
using Hypercube.Core.Resources;
using Hypercube.Core.Systems.Transform;
using Hypercube.Core.Viewports;
using Hypercube.Mathematics.Quaternions;
using Hypercube.Mathematics.Vectors;
using Hypercube.Utilities.Dependencies;

namespace Rainclipse;

[RegisterEntitySystem]
public sealed class TestSystem : EntitySystem
{
    [Dependency] private readonly IAudioManager _audio = null!;
    [Dependency] private readonly ICameraManager _camera = null!;
    [Dependency] private readonly IInputHandler _inputHandler = null!;
    [Dependency] private readonly IResourceManager _resource = null!;

    private EntityQuery _testQuery = null!;
    
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
            
            var dt = deltaTime;
            if (_inputHandler.IsKeyHeld(Key.LeftShift))
                dt /= 100;
                
            if (_inputHandler.IsKeyHeld(Key.D))
                transform.LocalPosition += Vector3.UnitX * dt;

            if (_inputHandler.IsKeyHeld(Key.A))
                transform.LocalPosition -= Vector3.UnitX * dt;

            if (_inputHandler.IsKeyHeld(Key.W))
                transform.LocalPosition += Vector3.UnitY * dt;

            if (_inputHandler.IsKeyHeld(Key.S))
                transform.LocalPosition -= Vector3.UnitY * dt;
            
            if (_inputHandler.IsKeyHeld(Key.E))
                transform.LocalRotation *= Quaternion.FromEulerZ(-dt / 100);
            
            if (_inputHandler.IsKeyHeld(Key.Q))
                transform.LocalRotation *= Quaternion.FromEulerZ(dt / 100);
            
            _camera.MainCamera.Position = _camera.MainCamera.Position.WithXy(transform.LocalPosition);
        }
    }

    [Subscribe]
    public void OnAdded(Entity entity, TestComponent component, ref AddedEvent _)
    {
        Logger.Debug($"{nameof(TestComponent)} added to {entity}");
    }
}