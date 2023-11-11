using Arch.Core;
using Arch.Core.Extensions;
using NovemberPirates.Components;
using NovemberPirates.Utilities;
using Raylib_CsLo;
using System.Numerics;

namespace NovemberPirates.Entities.Archetypes
{
    internal static class CannonballBuilder
    {
        internal static void Create(World world, Vector2 pos, float rotation, Team team)
        {
            var cannonballEntity = world.Create<Sprite, Cannonball>();

            var cannonball = new Cannonball();
            var rotationInRadians = rotation * (float)(Math.PI / 180);
            cannonball.Motion = RayMath.Vector2Rotate(new Vector2(1000, 0), rotationInRadians);
            cannonball.FiredBy = team;
            cannonballEntity.Set(cannonball);

            var cannonballSprite = new Sprite(TextureKey.Cannonball, "Assets/Art/cannonball", 1f, true);
            cannonballSprite.Position = pos;
            cannonballEntity.Set(cannonballSprite);

        }
    }
}
