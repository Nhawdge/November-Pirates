using System.Numerics;

namespace NovemberPirates.Components
{
    internal class Port
    {
        public float Currency = 0f;
        public float Population = 100f;
        internal Vector2 Position = Vector2.Zero;
        internal Guid Guid = Guid.NewGuid();


        public Team Team = Team.None;
    }
}
