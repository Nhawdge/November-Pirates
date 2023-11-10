using Arch.Core;
using NovemberPirates.Systems;
using System.Numerics;

namespace NovemberPirates.Scenes
{
    internal class BaseScene
    {
        internal World World = World.Create();

        internal List<GameSystem> Systems = new();

        public Vector2 MapEdge;
        public int TileSize;
    }
}
