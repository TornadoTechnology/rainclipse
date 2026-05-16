using Hypercube.Core.Ecs;
using Hypercube.Core.Execution.LifeCycle;
using Hypercube.Core.Systems.Rendering;
using Hypercube.Core.Systems.Transform;
using Hypercube.Ecs;
using Hypercube.Ecs.Components;
using Hypercube.Ecs.Queries;
using Hypercube.Mathematics;
using Hypercube.Physics;
using Hypercube.Physics.Collision;
using Hypercube.Physics.Mathematics;
using Hypercube.Physics.Shapes;
using Hypercube.Physics.Shapes.Structs;

namespace Rainclipse;

public sealed class StressTestSystem : EntitySystem
{
    private static readonly TimeSpan SpawnDelay = TimeSpan.FromSeconds(1f);
    private static readonly QueryMeta Meta = new QueryMeta().WithAll<PhysicsTestComponent, TransformComponent>();

    private readonly List<Entity> _entities = [];

    private TimeSpan _acc = TimeSpan.Zero;
    
    public override void Initialize()
    {
        Contacts.Initialize();
    }

    public override void Update(FrameEventArgs args)
    {
        _entities.Clear();
        Query(Meta).Write(_entities);

        foreach (var entityA in _entities)
        {
            foreach (var entityB in _entities.Where(entityB => entityA != entityB))
            {
                Process(entityA, entityB);
            }
        }
        
        _acc += args.Delta;
        
        if (_acc <= SpawnDelay)
            return;
        
        _acc -= SpawnDelay;
        
        var entity = EntityCreate();
        AddComponent<TransformComponent>(entity);
            
        AddComponent(entity, new PhysicsTestComponent
        {
            Fixture = new ShapeUnionTyped
            {
                Type = ShapeType.Polygon,
                Shape = new ShapeUnion
                {
                    Polygon = ShapePolygon.CreateSquare(16f),
                }
            },
        });
            
        AddComponent(entity, new SpriteComponent
        {
            Path = "/textures/default.png"
        });
    }

    private void Process(Entity entityA, Entity entityB)
    {
        ref var transformA = ref GetComponent<TransformComponent>(entityA);
        ref var transformB = ref GetComponent<TransformComponent>(entityB);
        
        ref var physicsA = ref GetComponent<PhysicsTestComponent>(entityA);
        ref var physicsB = ref GetComponent<PhysicsTestComponent>(entityB);
        
        var physTransformA = new Transform(transformA.LocalPosition.Xy, transformA.LocalRotation.ToEuler().Z * HyperMath.DegreesToRadiansF);
        var physTransformB = new Transform(transformB.LocalPosition.Xy, transformA.LocalRotation.ToEuler().Z * HyperMath.DegreesToRadiansF);
        
        var manifold = Contacts.Resolve(physicsA.Fixture, physTransformA, physicsB.Fixture, physTransformB);
        if (manifold.IsEmpty)
            return;
        
        var normal = manifold.Normal;
        var separation = float.Max(manifold.Points[0].Separation, manifold.Points[1].Separation);
        
        transformA.LocalPosition += normal * separation * 0.5f;
        transformB.LocalPosition -= normal * separation * 0.5f;
    }
}

public struct PhysicsTestComponent : IComponent
{
    public ShapeUnionTyped Fixture;
}
