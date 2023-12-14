namespace NovemberPirates.Components
{
    internal class Npc
    {
        internal Purpose Purpose;
    }

    public enum Purpose
    {
        None,
        Idle,
        Patrol,
        Attack,
        Trade,
    }
}
