using System.Numerics;

namespace NovemberPirates.Scenes.Levels.Components
{
    internal class Npc
    {
        public Npc(NpcPurpose purpose)
        {
            Purpose = purpose;
        }

        internal NpcGoals Goal;
        internal NpcPurpose Purpose;
        internal float TimeSinceLastGoalChange = 100;
        internal NpcGoals CurrentAction;
        internal float VisionRange = 50;
        internal Vector2 TargetPosition;
        internal float TargetOffsetInDegrees;
    }

    public enum NpcPurpose
    {
        Merchant,
        Patrol,
        Escort,
        PirateHunter,
    }

    public enum NpcGoals
    {
        Sailing,
        Trading,
        Repairing,
        Escaping,
        GetReinforcements,
        Attacking
    }

}
