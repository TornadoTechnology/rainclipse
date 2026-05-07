using Hypercube.Core.Audio.Manager;
using Hypercube.Core.Audio.Resources;
using Hypercube.Core.Ecs;
using Hypercube.Core.Execution.Attributes;
using Hypercube.Core.Execution.Enums;
using Hypercube.Core.Execution.LifeCycle;
using Hypercube.Core.Graphics.Patching;
using Hypercube.Core.Graphics.Resources;
using Hypercube.Core.Resources;
using Hypercube.Core.Systems.Rendering;
using Hypercube.Core.Systems.Transform;
using Hypercube.Core.UI;
using Hypercube.Core.UI.Elements;
using Hypercube.Core.UI.Elements.Buttons;
using Hypercube.Core.Viewports;
using Hypercube.Core.Windowing.Manager;
using Hypercube.Mathematics;
using Hypercube.Mathematics.Dimensions;
using Hypercube.Mathematics.Shapes;
using Hypercube.Mathematics.Vectors;
using Hypercube.Physics.Shapes;
using Hypercube.Physics.Shapes.Structs;
using Hypercube.Utilities.Debugging.Logger;
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
        var logger = container.Resolve<ILogger>();
        var patchManager = container.Resolve<IPatchManager>();
        var world = container.Resolve<IEntitySystemManager>();
        var camera = container.Resolve<ICameraManager>();
        var window = container.Resolve<IWindowingManager>();
        var audio = container.Resolve<IAudioManager>();
        var runtimeLoop = container.Resolve<IRuntimeLoop>();
        var uiManager = container.Resolve<IUIManager>();
        var resourceManager = container.Resolve<IResourceManager>();

        _ = RecordAndPlayAsync(audio, logger);
        
        /*
        window.Create(new WindowCreateSettings
        {
            Title = "Sub windows test",
            Size = new Vector2i(1270, 720),
            ContextShare = window.MainWindow!.Handle
        });
        */
            
        var patch = new TestPatch();

        container.Inject(patch);
        patchManager.AddPatch(patch);
        
        var entity = world.Create();
        world.Add<TestComponent>(entity);
        world.Add<TransformComponent>(entity);
        world.Add(entity, new SpriteComponent { Path = "/textures/default.png" });
        world.Add(entity, new PhysicsTestComponent
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
        
        var sound = resourceManager.Load<Audio>("/audio/game_boi_3.wav");
        var source = audio.CreateSource(sound);
        
        var uiRect = new Rectangle
        {
            AnchorPoint = new Vector2(1, 0.5f),
            Position = new HDim2(1, 0, 0.5f, 0),
            Size = new HDim2(0.2f, 0, 1, 0),
            Color = new Color("#77e36d55")
        };

        uiManager.Root.AddChild(uiRect);

        var uiRect2 = new ButonLabel
        {
            AnchorPoint = new Vector2(0, 0.5f),
            Position = new HDim2(0.05f, 0, 0.95f, 0),
            Size = new HDim2(0.9f, 0, 0.05f, 0),
        };
        
        uiRect.AddChild(uiRect2);
        
        uiRect2.Fill.Color = new Color("#22e36d55");

        uiRect2.Label.DrawDebugRect = true;
        uiRect2.Label.FontSize = 12;
        uiRect2.Label.Text = "Играть крутой музон";
        
        uiRect2.OnClicked += () =>
        {
            if (source.Playing)
            {
                source.Stop();
                return;
            }
            
            source.Start();
        };
    }
    
    public static async Task RecordAndPlayAsync(IAudioManager audio, ILogger logger)
    {
        if (!audio.Ready)
        {
            logger.Error("Audio system is not ready");
            return;
        }
        
        logger.Debug("Start recording...");

        if (!audio.StartRecording())
        {
            logger.Error("Failed to start recording");
            return;
        }

        await Task.Delay(TimeSpan.FromSeconds(5));
        

        logger.Debug("Recording stopped");
        
        var data = audio.GetRecordedAudioData();
        audio.StopRecording();
        
        if (data.IsEmpty)
        {
            logger.Error("Recorded data is empty");
            return;
        }
        
        logger.Debug($"Recorded {data.Length} bytes");
        logger.Debug($"Meta: SR={data.MetaData.SampleRate}, CH={data.MetaData.Channels}, DR={data.MetaData.Length.TotalSeconds}s");

        for (var i = 0; i < Math.Min(32, data.Length); i++)
        {
            logger.Debug(data.Source[i] + " ");
        }
        
        var stream = audio.CreateStream(data);
        var source = audio.CreateSource(stream);
        
        source.Start();
        logger.Debug("Playback started");
    }
}