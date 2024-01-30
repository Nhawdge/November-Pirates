using NovemberPirates.Scenes.Levels.Components;
using NovemberPirates.Utilities.Data;

namespace NovemberPirates.Entities.Archetypes
{
    internal static class CannonBuilder
    {
        internal static Cannon Create(CannonType cannonType, BoatSide boatSide, int row)
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

            return cannon;
        }
    }
}
