using Arch.Core;
using Arch.Core.Extensions;
using NovemberPirates.Components;
using NovemberPirates.Utilities;

namespace NovemberPirates.Entities.Archetypes
{
    internal static class PlayerBuilder
    {
        internal static void Create(World world)
        {
            var player = world.Create<Player, Sprite>();

            var playerComponent = new Player();
            player.Set(playerComponent);

            var playerSprite = new Sprite(TextureKey.HullLarge, "Assets/Art/hullLarge", 1f, true);
            playerSprite.Position = new System.Numerics.Vector2(3000, 3000);
            playerSprite.RotationOffset = -180f;

            player.Set(playerSprite);
        }
    }
}
