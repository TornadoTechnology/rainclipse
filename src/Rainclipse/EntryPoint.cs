using Hypercube.Core.Ecs;
using Hypercube.Core.Execution.Attributes;
using Hypercube.Core.Execution.Enums;
using Hypercube.Core.Graphics.Patching;
using Hypercube.Core.Systems.Rendering;
using Hypercube.Core.Systems.Transform;
using Hypercube.Utilities.Dependencies;

namespace Rainclipse;

public static class EntryPoint
{
    [EntryPoint(EntryPointStage.BeforeInit)]
    public static void Init(DependenciesContainer container)
    {
        // Init
    }
    
    [EntryPoint(EntryPointStage.AfterInit)]
    public static void Start(DependenciesContainer container)
    {
        var patchManager = container.Resolve<IPatchManager>();
        var patch = new TestPatch();
        
        container.Inject(patch);
        patchManager.AddPatch(patch);

        var world = container.Resolve<IEntitySystemManager>();
        var entity = world.Create();
        world.Add<TransformComponent>(entity);
        world.Add<TestComponent>(entity);
        world.Add(entity, new SpriteComponent { Path = "/textures/default.png" });
    }
}