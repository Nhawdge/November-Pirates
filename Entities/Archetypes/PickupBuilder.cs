using Arch.Core;
using Arch.Core.Extensions;
using NovemberPirates.Components;
using NovemberPirates.Utilities;
using System.Numerics;

namespace NovemberPirates.Entities.Archetypes
{
    internal static class PickupBuilder
    {
        internal static void CreateCrewMember(World world, Vector2 position)
        {
            var entity = world.Create<CrewMember, Sprite>();
            var crewMember = new CrewMember();
            var spread = 100;
            crewMember.Target = position + new Vector2(Random.Shared.Next(-spread, spread), Random.Shared.Next(-spread, spread));
            crewMember.Duration = 15f;
            crewMember.Speed = 10f;

            var sprite = new Sprite(TextureKey.Crew, "Assets/Art/crew", 1f, true);
            sprite.Position = position;

            entity.Set(crewMember);
            entity.Set(sprite);
        }

        internal static void CreateWood(World world, Vector2 position)
        {
            var entity = world.Create<Wood, Sprite>();
            var wood = new Wood();
            var spread = 100;
            wood.Target = position + new Vector2(Random.Shared.Next(-spread, spread), Random.Shared.Next(-spread, spread));
            wood.Duration = 25f;
            wood.Speed = 10f;

            var sprite = new Sprite(TextureKey.Wood, "Assets/Art/wood", 1f, true);
            sprite.Position = position;

            entity.Set(wood);
            entity.Set(sprite);
        }
    }
}
