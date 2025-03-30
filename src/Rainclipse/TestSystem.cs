using Hypercube.Core.Ecs;
using Hypercube.Core.Ecs.Attributes;
using Hypercube.Core.Ecs.Events;

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