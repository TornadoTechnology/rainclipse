using Hypercube.Core.Ecs;
using Hypercube.Core.Ecs.Attributes;
using Hypercube.Core.Ecs.Core.Components;
using Hypercube.Core.Ecs.Events;
using Hypercube.Core.Systems.Rendering;
using Hypercube.Core.Systems.Transform;

namespace Rainclipse;

[RegisterEntitySystem]
public sealed class TestSystem : EntitySystem
{
    public override void Startup()
    {
        base.Startup();
        
        Logger.Debug("Test!");
    }

    [Subscribe]
    public void OnAdded(Entity entity, TestComponent component, ref AddedEvent _)
    {
        Logger.Debug($"{nameof(TestComponent)} added to {entity}");
    }
}