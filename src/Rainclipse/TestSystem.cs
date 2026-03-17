using Hypercube.Core.Audio.Manager;
using Hypercube.Core.Audio.Resources;
using Hypercube.Core.Input;
using Hypercube.Core.Input.Handler;
using Hypercube.Core.Resources;
using Hypercube.Core.Systems.Transform;
using Hypercube.Core.Viewports;
using Hypercube.Ecs.Queries;
using Hypercube.Ecs.System;
using Hypercube.Mathematics.Quaternions;
using Hypercube.Mathematics.Vectors;
using Hypercube.Utilities.Dependencies;

namespace Rainclipse;

public sealed class TestSystem : EntitySystem
{
    [Dependency] private readonly IAudioManager _audio = null!;
    [Dependency] private readonly ICameraManager _camera = null!;
    [Dependency] private readonly IInputHandler _inputHandler = null!;
    [Dependency] private readonly IResourceManager _resource = null!;

    private Query _testQuery = null!;
    
    public override void Initialize()
    {
        _testQuery = CreateQuery(new QueryMeta()
            .WithAll<TestComponent>()
            .WithAll<TransformComponent>()
        );

        var sound = _resource.Load<Audio>("/audio/game_boi_3.wav");
        var source = _audio.CreateSource(sound);
        // source.Start();
    }

    public override void Update(float deltaTime)
    {
        base.Update(deltaTime);
        
        _testQuery.With<TransformComponent, TestComponent>((_, ref transform, ref _) =>
        {
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
        });
    }
}