using Arch.Core;
using Arch.Core.Extensions;
using NovemberPirates.Components;
using NovemberPirates.Extensions;
using NovemberPirates.Systems;
using Raylib_CsLo;

namespace NovemberPirates.Scenes.Levels.Systems
{
    internal class UiSystem : GameSystem
    {
        internal override void Update(World world)
        {
        }

        internal override void UpdateNoCamera(World world)
        {
            var playerEntity = world.QueryFirst<Player>();
            var player = playerEntity.Get<Player>();

            var topleft = new Rectangle(10, 10, 50, 50);

            RayGui.GuiLabel(topleft, Enum.GetName<SailStatus>(player.Sail));
        }
    }
}
