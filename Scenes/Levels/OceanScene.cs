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
            Systems.Add(new RenderSystem());
            Systems.Add(new BoatMovementSystem());
            Systems.Add(new CameraSystem());
            Systems.Add(new WindSystem());
            Systems.Add(new UiSystem());
            Systems.Add(new DebugSystem());

            MapManager.Instance.LoadMap("Level_0", World);

            PlayerBuilder.Create(World);
            var singleton = World.Create<Singleton, Wind>();
            singleton.Set(new Wind());
            singleton.Set(new Singleton());
        }
    }
}
