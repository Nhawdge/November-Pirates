using NovemberPirates.Scenes.Levels.Components;
using NovemberPirates.Utilities;
using NovemberPirates.Utilities.Data;
using System.Numerics;

namespace NovemberPirates.Entities.Archetypes
{
    internal static class CannonBuilder
    {
        internal static Cannon Create(HullType hull, CannonType cannonType, BoatSide boatSide, int row)
        {
            var cannon = new Cannon();
            cannon.Placement = boatSide;
            cannon.Row = row;

            cannon.CannonType = cannonType;

            cannon.ReloadTime = ShipData.Instance.Data[$"{cannonType}{Stats.CannonReloadTime}"];
            cannon.ReloadRate = ShipData.Instance.Data[$"{cannonType}{Stats.CannonReloadRate}"];
            cannon.HullDamage = ShipData.Instance.Data[$"{cannonType}{Stats.CannonHullDamage}"];
            cannon.SailDamage = ShipData.Instance.Data[$"{cannonType}{Stats.CannonSailDamage}"];
            cannon.CrewKillChance = ShipData.Instance.Data[$"{cannonType}{Stats.CannonCrewKillChance}"];
            cannon.CrewKillChainLimit = ShipData.Instance.Data[$"{cannonType}{Stats.CannonCrewKillChainLimit}"];
            cannon.Spread = ShipData.Instance.Data[$"{cannonType}{Stats.CannonSpread}"];
            cannon.BallSpeed = ShipData.Instance.Data[$"{cannonType}{Stats.CannonballSpeed}"];
            cannon.BallDuration = ShipData.Instance.Data[$"{cannonType}{Stats.CannonballDuration}"];
            cannon.ShotPer = ShipData.Instance.Data[$"{cannonType}{Stats.CannonsShotPer}"];


            var cannonPosition = new Vector2(
                  ShipCannonCoords.Data[Enum.GetName(cannon.Placement)],
                  ShipCannonCoords.Data[$"{hull}{cannon.Row}"]
                  );

            var cannonSprite = new Sprite(TextureKey.CannonLoose, "Assets/Art/cannonLoose")
            {
                Position = cannonPosition,
                Rotation = cannon.Placement == BoatSide.Port ? 180 : 0,
            };
            //cannons.Add(cannonSprite);

            //if (cannon.Position == Vector2.Zero)
            // I have no idea why I needed these magic numbers, but they work.
            cannon.Position = cannonPosition with
            {
                X = cannonPosition.X - cannonSprite.SpriteWidth - 10,
                Y = cannonPosition.Y - cannonSprite.SpriteHeight - 25
            };

            return cannon;
        }
    }
}
