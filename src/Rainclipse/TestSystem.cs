using Hypercube.Core.Ecs;
using Hypercube.Core.Ecs.Attributes;
using Hypercube.Core.Ecs.Events;
using Hypercube.Core.Ecs.Systems;

namespace Rainclipse;

[RegisterEntitySystem]
public sealed class TestSystem : EntitySystem
{
    [Subscribe]
    public void OnAdded(Entity entity, TestComponent component, ref AddedEvent _)
    {
        Logger.Debug($"{nameof(TestComponent)} added to {entity}");
    }
}