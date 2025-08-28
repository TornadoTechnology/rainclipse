using Hypercube.Core.Ecs;
using Hypercube.Core.Execution.Attributes;
using Hypercube.Core.Execution.Enums;
using Hypercube.Core.Graphics.Patching;
using Hypercube.Core.Systems.Rendering;
using Hypercube.Core.Systems.Transform;
using Hypercube.Mathematics;
using Hypercube.Mathematics.Quaternions;
using Hypercube.Mathematics.Vectors;
using Hypercube.Utilities.Dependencies;

namespace Rainclipse;

public static class EntryPoint
{
    [EntryPoint(EntryPointLevel.BeforeInit)]
    public static void Init(DependenciesContainer container)
    {
        // Init
    }
    
    [EntryPoint(EntryPointLevel.AfterInit)]
    public static void Start(DependenciesContainer container)
    {
        var patchManager = container.Resolve<IPatchManager>();
        var patch = new TestPatch();
        
        container.Inject(patch);
        patchManager.AddPatch(patch);

        var world = container.Resolve<IEntitySystemManager>().Main;
        var entity = world.CreateEntity();
        world.AddComponent<TransformComponent>(entity);
        world.AddComponent<TestComponent>(entity);
        world.AddComponent(entity, new SpriteComponent { Path = "/textures/default.png" });
    }
}