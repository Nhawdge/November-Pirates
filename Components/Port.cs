using System.Numerics;

namespace NovemberPirates.Components
{
    internal class Port
    {
        public Port()
        {
            Guid = Guid.NewGuid();
        }
        public float Currency = 0f;
        public float Population = 100f;
        internal Vector2 Position = Vector2.Zero;
        internal Guid Guid;
        internal string ShortId() => Guid.ToString().Substring(0, 4);


        public Team Team = Team.None;
    }
}
