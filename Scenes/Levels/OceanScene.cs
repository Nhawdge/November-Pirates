using November_Pirates.Scenes.Levels.Systems;
using NovemberPirates.Entities.Archetypes;

namespace NovemberPirates.Scenes.Levels
{
    internal class OceanScene : BaseScene
    {
        internal OceanScene()
        {
            Systems.Add(new RenderSystem());
            Systems.Add(new BoatMovementSystem());
            Systems.Add(new CameraSystem());

            PlayerBuilder.Create(this.World);
        }
    }
}
