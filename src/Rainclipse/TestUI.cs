using Hypercube.Core.UI;
using Hypercube.Core.UI.Elements;
using Hypercube.Core.UI.Elements.Buttons;
using Hypercube.Core.UI.Elements.Containers;
using Hypercube.Core.UI.Manager;
using Hypercube.Mathematics;
using Hypercube.Mathematics.Dimensions;
using Hypercube.Mathematics.Vectors;

namespace Rainclipse;

public static class TestUI
{
    public static void CreateUI(IUIManager manager)
    {
        var root = manager.Root.AddChild(new Rectangle
        {
            AnchorPoint = new Vector2(1, 0.5f),
            Position = new HDim2(1, 0, 0.5f, 0),
            Size = new HDim2(0.2f, 0, 1, 0),
            Color = new Color("#77e36d55"),
        });
        
        var container = root.AddChild(new LayoutContainer
        {
            Orientation = Orientation.Vertical,
            Position = HDim2.Zero,
            Size = HDim2.ScalarOne,
            Spacing = new HDim(0, 16),
            VAlignment = Alignment.SpaceBetween,
            HAlignment = Alignment.Center,
            Padding = HDimRect.All(new HDim(0, 16)),
        });

        for (var i = 0; i < 6; i++)
        {
            var button = container.AddChild(new ButonRect
            {
                Size = new HDim2(1.0f, 0, 0, 50),
            });

            button.Fill.Color = i switch
            {
                0 => Color.Aqua,
                1 => Color.Blue,
                2 => Color.Coral,
                3 => Color.Indigo,
                4 => Color.Lime,
                _ => Color.White
            };
        }
    }
}
