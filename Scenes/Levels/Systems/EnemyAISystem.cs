using Arch.Core;
using Arch.Core.Extensions;
using NovemberPirates.Extensions;
using NovemberPirates.Scenes.Levels.Components;
using NovemberPirates.Systems;
using Raylib_CsLo;
using System.Formats.Asn1;
using System.Numerics;

namespace NovemberPirates.Scenes.Levels.Systems
{
    internal class EnemyAISystem : GameSystem
    {
        internal override void Update(World world)
        {
            var enemyQuery = new QueryDescription().WithAll<Sprite, Ship, Npc>();

            world.Query(enemyQuery, (entity) =>
            {
                var npc = entity.Get<Npc>();
                var ship = entity.Get<Ship>();
                var sprite = entity.Get<Sprite>();

                npc.TimeSinceLastGoalChange += Raylib.GetFrameTime();

                if (npc.TimeSinceLastGoalChange < 10)
                {
                    return;
                }
                npc.TimeSinceLastGoalChange = 0;

                var goalWeights = new Dictionary<NpcGoals, float> {
                    { NpcGoals.Sailing, 0f },
                    { NpcGoals.Attacking, 0f },
                    { NpcGoals.Escaping, 0f },
                    { NpcGoals.Trading, 0f },
                    { NpcGoals.Repairing, 0f },
                };

                if (npc.Purpose == NpcPurpose.Merchant)
                {
                    goalWeights[NpcGoals.Trading] += 10f;
                }

                npc.CurrentAction = goalWeights.OrderByDescending(x => x.Value).First().Key;

                // End Goal weighing 

                if (npc.CurrentAction == NpcGoals.Trading)
                {
                    if (npc.TargetPosition == Vector2.Zero)
                    {
                        Console.WriteLine("fresh ship");
                        npc.TargetPosition = GetNearestPort(world, sprite.Position);
                    }

                    if (npc.TargetPosition.DistanceTo(sprite.Position) < 200)
                    {
                        Console.WriteLine("Port Hit!");
                        npc.TargetPosition = GetSecondNearestPort(world, sprite.Position);
                    }
                }
            });
        }

        #region Weighting helpers 
        private float GetRecentDamage()
        {
            throw new NotImplementedException();
        }

        private float DistanceToPlayer()
        {
            throw new NotImplementedException();
        }

        private float DistanceToAlliedShip()
        {
            throw new NotImplementedException();
        }

        private float DistanceToSafePort()
        {
            throw new NotImplementedException();
        }

        private float GetCurrentGoldValue()
        {
            throw new NotImplementedException();
        }

        #endregion

        #region Action helpers

        private Vector2 GetNearestPort(World world, Vector2 fromPosition)
        {
            var coords = new Vector2();

            var query = new QueryDescription().WithAll<Port>();
            var shortestDistance = float.MaxValue;

            world.Query(query, (entity) =>
            {
                var port = entity.Get<Port>();
                var distance = fromPosition.DistanceTo(port.Position);

                if (distance < shortestDistance)
                {
                    coords = port.Position;
                    shortestDistance = distance;
                }
            });

            return coords;
        }

        private Vector2 GetSecondNearestPort(World world, Vector2 fromPosition)
        {
            var query = new QueryDescription().WithAll<Port>();
            var shortestDistance = float.MaxValue;
            var targets = new List<(float distance, Vector2 position)>();
            world.Query(query, (entity) =>
            {
                var port = entity.Get<Port>();
                var distance = fromPosition.DistanceTo(port.Position);

                targets.Add((distance, port.Position));
            });

            return targets.OrderBy(targets => targets.distance).Skip(1).First().position;
        }

        #endregion
    }

}
