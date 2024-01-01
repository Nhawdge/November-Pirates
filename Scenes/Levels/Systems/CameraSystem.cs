using Arch.Core;
using Arch.Core.Extensions;
using NovemberPirates.Components;
using NovemberPirates.Systems;
using Raylib_CsLo;
using System.Numerics;

namespace NovemberPirates.Scenes.Levels.Systems
{
    internal class CameraSystem : GameSystem
    {
        public CameraSystem()
        {
        }

        private float CameraSpeed = 3f;

        internal override void Update(World world)
        {
            var query = new QueryDescription().WithAll<Player>();

            float LeftEdge = 0 + Raylib.GetScreenWidth() / 2;
            float RightEdge = NovemberPiratesEngine.Instance.ActiveScene.MapEdge.X - Raylib.GetScreenWidth() / 2;
            float TopEdge = 0 + Raylib.GetScreenHeight() / 2;
            float BottomEdge = NovemberPiratesEngine.Instance.ActiveScene.MapEdge.Y - Raylib.GetScreenHeight() / 2;

            world.Query(in query, (entity) =>
            {
                var shipSprite = entity.Get<Sprite>();
                var ship = entity.Get<Ship>();
                var offset = ship.Sail switch
                {
                    SailStatus.Half => -75,
                    SailStatus.Full => -150,
                    _ => 0
                };
                var targetPos = shipSprite.Position + RayMath.Vector2Rotate(new Vector2(0, offset), shipSprite.RotationAsRadians);
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
                var direction = new Vector2(x, y) - NovemberPiratesEngine.Instance.Camera.target;
                var currentPos = NovemberPiratesEngine.Instance.Camera.target;
                var futurePos = currentPos + (direction * CameraSpeed * Raylib.GetFrameTime());

                NovemberPiratesEngine.Instance.Camera.target.X = futurePos.X;
                NovemberPiratesEngine.Instance.Camera.target.Y = futurePos.Y;
            });

            var scroll = Raylib.GetMouseWheelMove();
            if (scroll < 0)
            {
                NovemberPiratesEngine.Instance.Camera.zoom -= 0.1f;
            }
            else if (scroll > 0)
            {
                NovemberPiratesEngine.Instance.Camera.zoom += 0.1f;
            }
        }
    }
}
