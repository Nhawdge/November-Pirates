using Arch.Core;
using Arch.Core.Extensions;
using NovemberPirates.Components;
using NovemberPirates.Extensions;
using NovemberPirates.Scenes.Levels.Components;
using NovemberPirates.Systems;
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

            var player = world.QueryFirst<Player>();
            var playerSprite = player.Get<Sprite>();

            var crewQuery = new QueryDescription().WithAll<Sprite, CrewMember>();
            var woodQuery = new QueryDescription().WithAll<Sprite, Wood>();

            world.Query(crewQuery, (entity) =>
            {
                var sprite = entity.Get<Sprite>();
                var crewMember = entity.Get<CrewMember>();
                crewMember.Elapsed += Raylib.GetFrameTime();

                if (playerSprite.Position.DistanceTo(sprite.Position) < 50)
                {
                    world.Destroy(entity);
                    var playerShip = player.Get<Ship>();
                    playerShip.Crew += 1;
                    world.Create<AudioEvent>().Set(new AudioEvent() { Key = Utilities.AudioKey.Yarr, Position = sprite.Position });
                }

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

            world.Query(woodQuery, (entity) =>
            {
                var sprite = entity.Get<Sprite>();

                if (playerSprite.Position.DistanceTo(sprite.Position) < 50)
                {
                    world.Destroy(entity);
                    var playerShip = player.Get<Ship>();
                    playerShip.Wood += 1;
                    world.Create<AudioEvent>().Set(new AudioEvent() { Key = Utilities.AudioKey.CollectWood, Position = sprite.Position });
                }

            });
        }
    }
}
