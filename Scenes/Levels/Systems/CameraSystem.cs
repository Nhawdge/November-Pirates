using Arch.Core;
using Arch.Core.Extensions;
using NovemberPirates.Components;
using NovemberPirates.Systems;
using Raylib_CsLo;

namespace NovemberPirates.Scenes.Levels.Systems
{
    internal class CameraSystem : GameSystem
    {
        public CameraSystem()
        {
        }

        internal override void Update(World world)
        {
            var query = new QueryDescription().WithAll<Player>();

            float LeftEdge = 0 + Raylib.GetScreenWidth() / 2;
            float RightEdge = NovemberPiratesEngine.Instance.ActiveScene.MapEdge.X - Raylib.GetScreenWidth() / 2;
            float TopEdge = 0 + Raylib.GetScreenHeight() / 2;
            float BottomEdge = NovemberPiratesEngine.Instance.ActiveScene.MapEdge.Y - Raylib.GetScreenHeight() / 2;

            world.Query(in query, (entity) =>
            {
                var targetPos = entity.Get<Sprite>().Position;

                var xdiff = 0; 
                var ydiff = 0;

                var x = targetPos.X - xdiff;
                if (x < LeftEdge)
                {
                    x = LeftEdge;
                } 
                else if (x > RightEdge)
                {
                    x = RightEdge;
                }
                var y = targetPos.Y - ydiff;
                if (y < TopEdge)
                {
                    y = TopEdge;
                }
                else if (y > BottomEdge)
                {
                    y = BottomEdge;
                }
                NovemberPiratesEngine.Instance.Camera.target.X = x;
                NovemberPiratesEngine.Instance.Camera.target.Y = y;
            });
        }
    }
}
