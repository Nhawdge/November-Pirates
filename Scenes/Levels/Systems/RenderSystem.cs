﻿using Arch.Core;
using Arch.Core.Extensions;
using NovemberPirates.Components;
using NovemberPirates.Extensions;
using NovemberPirates.Scenes.Levels.Components;
using NovemberPirates.Systems;
using Raylib_CsLo;
using System.Numerics;

namespace NovemberPirates.Scenes.Levels.Systems
{
    internal class RenderSystem : GameSystem
    {
        public Texture BackgroundTexture { get; private set; }
        public RenderSystem() : base()
        {
        }

        internal override void Update(World world)
        {
            var singletonEntity = world.QueryFirst<Singleton>();
            var singleton = singletonEntity.Get<Singleton>();

            var renders = new QueryDescription().WithAll<Render>().WithNone<Effect>();
            var camera = NovemberPiratesEngine.Instance.Camera;
            world.Query(in renders, (entity) =>
            {
                var myRender = entity.Get<Render>();
                if (Vector2.Abs(myRender.Position - camera.target).Length() > Math.Max(Raylib.GetScreenWidth(), Raylib.GetScreenHeight()) + 200)
                    return;

                if (singleton.Debug != DebugLevel.High)
                {
                    Raylib.DrawTexturePro(myRender.Texture, myRender.Source, myRender.Destination, myRender.Origin, myRender.RenderRotation, myRender.Color);
                }
                if (singleton.Debug >= DebugLevel.Medium)
                {
                    if (entity.Has<MapTile>())
                    {
                        var color = myRender.Collision switch
                        {
                            CollisionType.None => new Color(0, 0, 0, 1),
                            CollisionType.Solid => Raylib.RED with { a = 128 },
                            CollisionType.Slow => Raylib.PINK with { a = 128 },
                            _ => Raylib.BLACK
                        };
                        Raylib.DrawRectangle((int)myRender.Destination.X, (int)myRender.Destination.Y, (int)myRender.Destination.width, (int)myRender.Destination.height, color);
                    }
                }
            });

            var effectSprites = new QueryDescription().WithAll<Sprite, Effect, LayerWater>().WithNone<MapTile>();
            world.Query(in effectSprites, (entity) =>
            {
                var myRender = entity.Get<Sprite>();
                myRender.Draw();
            });

            var sprites = new QueryDescription().WithAll<Sprite>().WithNone<MapTile, Effect>();

            world.Query(in sprites, (entity) =>
            {
                var myRender = entity.Get<Sprite>();
                myRender.Draw();
            });

            effectSprites = new QueryDescription().WithAll<Sprite, Effect, LayerAir>().WithNone<MapTile>();
            world.Query(in effectSprites, (entity) =>
            {
                var myRender = entity.Get<Sprite>();
                myRender.Draw();
            });
        }
    }
}
