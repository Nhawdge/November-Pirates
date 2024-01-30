using Arch.Core;
using Arch.Core.Extensions;
using NovemberPirates.Components;
using NovemberPirates.Extensions;
using NovemberPirates.Scenes.Levels.Components;
using NovemberPirates.Systems;
using NovemberPirates.Utilities.Data;
using Raylib_CsLo;
using System.Numerics;

namespace NovemberPirates.Scenes.Levels.Systems
{
    internal class PickupSystem : GameSystem
    {
        internal override void Update(World world)
        {
            var singletonEntity = world.QueryFirst<Singleton>();
            var singleton = singletonEntity.Get<Singleton>();

            var crewQuery = new QueryDescription().WithAll<Sprite, CrewMember>();

            world.Query(crewQuery, (entity) =>
            {
                var sprite = entity.Get<Sprite>();
                var crewMember = entity.Get<CrewMember>();
                crewMember.Elapsed += Raylib.GetFrameTime();

                if (crewMember.Elapsed > crewMember.Duration)
                {
                    world.Destroy(entity);
                }
                var dist = (sprite.Position - crewMember.Target).Length();
                if (dist > 10)
                {
                    sprite.Position += Vector2.Normalize(crewMember.Target - sprite.Position) * Raylib.GetFrameTime() * crewMember.Speed;
                }

            });
        }
    }
}
