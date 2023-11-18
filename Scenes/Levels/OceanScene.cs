using Arch.Core;
using Arch.Core.Extensions;
using NovemberPirates.Components;
using NovemberPirates.Entities.Archetypes;
using NovemberPirates.Scenes.Levels.Systems;
using NovemberPirates.Utilities;
using NovemberPirates.Utilities.Maps;

namespace NovemberPirates.Scenes.Levels
{
    internal class OceanScene : BaseScene
    {
        internal OceanScene()
        {
            //var mapTilesArchetype = new ComponentType[] { typeof(MapTile), typeof(Render) };
            //var entity = World.Create(mapTilesArchetype);

            Systems.Add(new RenderSystem());
            Systems.Add(new ShipControlSystem());
            Systems.Add(new CameraSystem());
            Systems.Add(new WindSystem());
            Systems.Add(new DebugSystem());
            Systems.Add(new CannonBallSystem());
            Systems.Add(new EnemyControlSystem());
            Systems.Add(new EffectsSystem());
            Systems.Add(new SpawningSystem());
            Systems.Add(new PickupSystem());
            Systems.Add(new AudioSystem());
            Systems.Add(new NavigationSystem());
            Systems.Add(new PlayerControlSystem());
            Systems.Add(new PortSystem());
            Systems.Add(new UiSystem());

            var mapDetails = MapManager.Instance.LoadMap("Level_0", World);
            MapEdge = mapDetails.MapEdge;
            TileSize = mapDetails.TileSize;

            PlayerBuilder.Create(World);

            var singleton = World.Create<Singleton, Wind>();
            singleton.Set(new Wind());
            singleton.Set(new Singleton());

            NavigationUtilities.BuildMap(World);

            World.Create<AudioEvent>().Set(new AudioEvent() { Key = AudioKey.ShipSailingWater, Replay = true, AllowMultiple = false });
            World.Create<AudioEvent>().Set(new AudioEvent() { Key = AudioKey.Wind, Replay = true, AllowMultiple = false });
        }
    }
}
