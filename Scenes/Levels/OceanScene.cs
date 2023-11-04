using Arch.Core;
using Arch.Core.Extensions;
using NovemberPirates.Components;
using NovemberPirates.Entities.Archetypes;
using NovemberPirates.Scenes.Levels.Systems;
using NovemberPirates.Utilities;

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
            Systems.Add(new UiSystem());
            Systems.Add(new DebugSystem());
            Systems.Add(new CannonBallSystem());

            var mapDetails = MapManager.Instance.LoadMap("Level_0", World);
            MapEdge = mapDetails.MapEdge;

            PlayerBuilder.Create(World);


            var singleton = World.Create<Singleton, Wind>();
            singleton.Set(new Wind());
            singleton.Set(new Singleton());
        }
    }
}
