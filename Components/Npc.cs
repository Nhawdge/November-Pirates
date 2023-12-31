namespace NovemberPirates.Components
{
    internal class Npc
    {
        internal Purpose Purpose;
        internal NpcAction CurrentAction;
    }

    public enum Purpose
    {
        None,
        Idle,
        Patrol,
        Attack,
        Trade,
    }

    public enum NpcAction
    {
        MoveTo,
        Attack,
        Trade,
    }
}
