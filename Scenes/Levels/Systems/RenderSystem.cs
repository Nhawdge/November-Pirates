using Arch.Core;
using Arch.Core.Extensions;
using NovemberPirates.Components;
using NovemberPirates.Systems;
using NovemberPirates.Utilities;
using Raylib_CsLo;

namespace NovemberPirates.Scenes.Levels.Systems
{
    internal class RenderSystem : GameSystem
    {
        public Texture BackgroundTexture { get; private set; }
        public RenderSystem() : base()
        {
            //this.BackgroundTexture = Raylib.LoadTexture("Assets/Maps/FreestyleRanchBackground.png");
        }

        internal override void Update(World world)
        {
            var singletonEntity = world.QueryFirst<Singleton>();
            var singleton = singletonEntity.Get<Singleton>();

            ////Raylib.DrawTextureEx(BackgroundTexture, Vector2.Zero, 0f, 3, Raylib.WHITE);
            //var mapEntity = Engine.Entities.FirstOrDefault(x => x.HasTypes(typeof(Map)));
            //if (mapEntity is not null)
            //{
            //    var map = mapEntity.GetComponent<Map>();
            //    var cells = map.Cells;
            //    if (cells != null)
            //    {
            //        foreach (var cell in cells)
            //        {
            //            //Raylib.DrawTexturePro(myRender.Texture, myRender.Source, myRender.Destination, myRender.Origin, myRender.RenderRotation, myRender.Color);
            //            Raylib.DrawTexturePro(cell.Tilemap.Texture, cell.Tilemap.Source, cell.Tilemap.Destination, cell.Tilemap.Origin, cell.Tilemap.Rotation, Raylib.BROWN);
            //        }
            //    }
            //}

            var renders = new QueryDescription().WithAll<Render>();
            world.Query(in renders, (entity) =>
            {
                var myRender = entity.Get<Render>();

                if (singleton.Debug != DebugLevel.High)
                {
                    Raylib.DrawTexturePro(myRender.Texture, myRender.Source, myRender.Destination, myRender.Origin, myRender.RenderRotation, myRender.Color);
                }
                if (singleton.Debug >= DebugLevel.Low)
                {
                    if (entity.Has<MapTile>())
                    {
                        var color = myRender.Collision switch
                        {
                            CollisionType.None => new Color(0, 0, 0, 0),
                            CollisionType.Solid => Raylib.RED with { a = 128 },
                            CollisionType.Slow => Raylib.PINK with { a = 128 },
                            _ => Raylib.BLACK
                        };
                        Raylib.DrawRectangle((int)myRender.Destination.X, (int)myRender.Destination.Y, (int)myRender.Destination.width, (int)myRender.Destination.height, color);
                    }
                }
                //Raylib.DrawRectangleLines((int)myRender.Destination.X, (int)myRender.Destination.Y, (int)myRender.Destination.width, (int)myRender.Destination.height, Raylib.BLACK);
                //Raylib.DrawCircle((int)myRender.Destination.X, (int)myRender.Destination.Y, 5, Raylib.GREEN);
            });

            var sprites = new QueryDescription().WithAll<Sprite>();

            world.Query(in sprites, (entity) =>
            {
                var myRender = entity.Get<Sprite>();
                Raylib.DrawTexturePro(myRender.Texture, myRender.Source, myRender.Destination, myRender.Origin, myRender.RenderRotation, myRender.Color);
                //Raylib.DrawRectangleLines((int)myRender.Destination.X, (int)myRender.Destination.Y, (int)myRender.Destination.width, (int)myRender.Destination.height, Raylib.BLACK);
                //Raylib.DrawCircle((int)myRender.Destination.X, (int)myRender.Destination.Y, 5, Raylib.GREEN);
            });
        }
    }
}
