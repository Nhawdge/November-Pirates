namespace NovemberPirates.Scenes.Levels.Components
{
    internal class Npc
    {
        internal NpcGoals Goal;
        internal float TimeSinceLastGoalChange;
    }

    public enum NpcGoals
    {
        Sailing,
        Trading,
        Repairing,
        Escaping,
        GetReinforcements,
        Fighting
    }

}
