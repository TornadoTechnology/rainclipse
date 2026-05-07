using Hypercube.Core.Audio.Manager;
using Hypercube.Core.Audio.Resources;
using Hypercube.Core.Ecs;
using Hypercube.Core.Execution.LifeCycle;
using Hypercube.Core.Input;
using Hypercube.Core.Input.Handler;
using Hypercube.Core.Resources;
using Hypercube.Core.Systems.Transform;
using Hypercube.Core.Viewports;
using Hypercube.Ecs.Queries;
using Hypercube.Mathematics.Quaternions;
using Hypercube.Mathematics.Vectors;
using Hypercube.Utilities.Dependencies;
using JetBrains.Annotations;

namespace Rainclipse;

[UsedImplicitly]
public sealed class TestSystem : EntitySystem
{
    [Dependency] private readonly IAudioManager _audio = null!;
    [Dependency] private readonly ICameraManager _camera = null!;
    [Dependency] private readonly IInputHandler _inputHandler = null!;
    [Dependency] private readonly IResourceManager _resource = null!;

    private readonly QueryMeta _query = new QueryMeta().WithAll<TransformComponent, TestComponent>();
    
    public override void BeforeUpdate(FrameEventArgs args)
    {
        Query(_query).With<TransformComponent, TestComponent>((_, ref transform, ref _) =>
        {
            var dt = (float) args.Delta.TotalMilliseconds;
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