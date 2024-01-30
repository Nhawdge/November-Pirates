using Arch.Core;
using Arch.Core.Extensions;
using NovemberPirates.Scenes.Levels.Components;
using NovemberPirates.Utilities;
using Raylib_CsLo;
using System.Numerics;

namespace NovemberPirates.Entities.Archetypes
{
    internal static class CannonballBuilder
    {
        internal static void Create(World world, Cannon cannon, Vector2 pos, float rotation, Team team)
        {
            var cannonballEntity = world.Create<Sprite, Cannonball>();

            var cannonball = new Cannonball();
            var rotationInRadians = rotation * (float)(Math.PI / 180);
            var spread = Random.Shared.Next(-(int)cannon.Spread, (int)cannon.Spread);
            cannonball.Motion = RayMath.Vector2Rotate(new Vector2(1000, spread), rotationInRadians);
            cannonball.FiredBy = team;
            cannonball.FiredByCannon = cannon;
            cannonballEntity.Set(cannonball);

            var cannonballSprite = new Sprite(TextureKey.Cannonball, "Assets/Art/cannonball", 1f, true);
            cannonballSprite.Position = pos;
            cannonballEntity.Set(cannonballSprite);
        }
    }
}
