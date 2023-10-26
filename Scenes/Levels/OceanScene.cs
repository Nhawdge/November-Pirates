using Arch.Core.Extensions;
using NovemberPirates.Components;
using NovemberPirates.Entities.Archetypes;
using NovemberPirates.Scenes.Levels.Systems;
using NovemberPirates.Utilities;
using System.Numerics;

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

            MapManager.Instance.LoadMap("Level_0", World);

            PlayerBuilder.Create(World);
            var singleton = World.Create<Singleton, Wind>();
            singleton.Set(new Wind());
        }
    }
}
