using Arch.Core;
using NovemberPirates.Systems;
using NovemberPirates.Utilities;
using System.Numerics;

namespace NovemberPirates.Scenes
{
    internal abstract class BaseScene
    {
        public BaseScene()
        {
            AudioManager.Instance.StopAllSounds();
        }

        internal World World = World.Create();

        internal List<GameSystem> Systems = new();

        public Vector2 MapEdge;
        public int TileSize;
    }
}
