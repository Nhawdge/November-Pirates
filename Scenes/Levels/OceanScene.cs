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

            MapManager.Instance.LoadMap("Level_0", World);

            PlayerBuilder.Create(World);

        }
    }
}
