using Arch.Core;
using Arch.Core.Extensions;
using NovemberPirates.Components;
using NovemberPirates.Scenes.Levels.Components;
using NovemberPirates.Scenes.Levels.Systems;
using NovemberPirates.Utilities;
using NovemberPirates.Utilities.Maps;

namespace NovemberPirates.Scenes.Levels
{
    internal class OceanScene : BaseScene
    {
        internal OceanScene()
        {
            StopSounds();
            var singleton = World.Create<Singleton, Wind>();
            singleton.Set(new Wind());
            singleton.Set(new Singleton() { Music = AudioKey.Drifting });

            LoadingTasks.Add("Map From File", () =>
            {
                var mapDetails = MapManager.Instance.LoadMap("Level_0", World);
                MapEdge = mapDetails.MapEdge;
                TileSize = mapDetails.TileSize;
            });

            LoadingTasks.Add("World", () =>
            {
                NavigationUtilities.BuildMap(World);
            });
            //LoadingTasks.Add("Trade Routes", () =>
            //{
            //    NavigationUtilities.AddTradeRoutes(World);
            //});

            LoadingTasks.Add("Audio", () =>
            {
                World.Create<AudioEvent>().Set(new AudioEvent() { Key = AudioKey.ShipSailingWater, Replay = true, AllowMultiple = false });
                World.Create<AudioEvent>().Set(new AudioEvent() { Key = AudioKey.Wind, Replay = true, AllowMultiple = false });
            });

            LoadingTasks.Add("Systems", () =>
            {
                Systems.RemoveAll(x => true);
                Systems.Add(new RenderSystem());
                Systems.Add(new ShipControlSystem());
                Systems.Add(new CameraSystem());
                Systems.Add(new WindSystem());
                Systems.Add(new DebugSystem());
                Systems.Add(new CannonBallSystem());
                Systems.Add(new EnemyAISystem());
                Systems.Add(new EnemyControlSystem());
                Systems.Add(new EffectsSystem());
                Systems.Add(new InventoryManagementSystem());
                Systems.Add(new SpawningSystem());
                Systems.Add(new PickupSystem());
                Systems.Add(new AudioSystem());
                Systems.Add(new NavigationSystem());
                Systems.Add(new PlayerControlSystem());
                Systems.Add(new PortSystem());
                Systems.Add(new UiSystem());
            });
        }
    }
}
