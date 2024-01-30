using Arch.Core;
using Arch.Core.Extensions;
using NovemberPirates.Components;
using NovemberPirates.Extensions;
using NovemberPirates.Systems;
using Raylib_CsLo;
using System.Numerics;

namespace NovemberPirates.Scenes.Levels.Systems
{
    internal class RenderSystem : GameSystem
    {
        private Shader WaterShader;
        private Texture WaterTexture;
        private float Seconds;

        public RenderSystem() : base()
        {
            WaterShader = Raylib.LoadShader(null, "Assets/Shaders/water.frag");

            int freqXLoc = Raylib.GetShaderLocation(WaterShader, "freqX");
            int freqYLoc = Raylib.GetShaderLocation(WaterShader, "freqY");
            int ampXLoc = Raylib.GetShaderLocation(WaterShader, "ampX");
            int ampYLoc = Raylib.GetShaderLocation(WaterShader, "ampY");
            int speedXLoc = Raylib.GetShaderLocation(WaterShader, "speedX");
            int speedYLoc = Raylib.GetShaderLocation(WaterShader, "speedY");

            // Shader uniform values that can be updated at any time
            float freqX = 12.0f;
            float freqY = 12.0f;
            float ampX = 10.0f;
            float ampY = 8.0f;
            float speedX = 8.0f;
            float speedY = 6.0f;

            var screenSize = new Vector2(Raylib.GetScreenWidth(), Raylib.GetScreenHeight());
            Raylib.SetShaderValue(WaterShader, Raylib.GetShaderLocation(WaterShader, "size"), screenSize, ShaderUniformDataType.SHADER_UNIFORM_VEC2);
            Raylib.SetShaderValue(WaterShader, freqXLoc, freqX, ShaderUniformDataType.SHADER_UNIFORM_FLOAT);
            Raylib.SetShaderValue(WaterShader, freqYLoc, freqY, ShaderUniformDataType.SHADER_UNIFORM_FLOAT);
            Raylib.SetShaderValue(WaterShader, ampXLoc, ampX, ShaderUniformDataType.SHADER_UNIFORM_FLOAT);
            Raylib.SetShaderValue(WaterShader, ampYLoc, ampY, ShaderUniformDataType.SHADER_UNIFORM_FLOAT);
            Raylib.SetShaderValue(WaterShader, speedXLoc, speedX, ShaderUniformDataType.SHADER_UNIFORM_FLOAT);
            Raylib.SetShaderValue(WaterShader, speedYLoc, speedY, ShaderUniformDataType.SHADER_UNIFORM_FLOAT);
        }

        internal override void Update(World world)
        {
            var singletonEntity = world.QueryFirst<Singleton>();
            var singleton = singletonEntity.Get<Singleton>();

            int secondsLoc = Raylib.GetShaderLocation(WaterShader, "seconds");
            Seconds += Raylib.GetFrameTime();

            Raylib.SetShaderValue(WaterShader, secondsLoc, Seconds, ShaderUniformDataType.SHADER_UNIFORM_FLOAT);
            var renders = new QueryDescription().WithAll<Render>().WithNone<Effect>();
            var camera = NovemberPiratesEngine.Instance.Camera;
            var waterRendered = false;
            world.Query(in renders, (entity) =>
            {
                var myRender = entity.Get<Render>();
                if (Vector2.Abs(myRender.Position - camera.target).Length() > Math.Max(Raylib.GetScreenWidth(), Raylib.GetScreenHeight()) + 200)
                    return;


                if (singleton.Debug != DebugLevel.High)
                {
                    if (myRender.Collision is CollisionType.None)
                    {
                        if (!waterRendered)
                        {
                            var offset = 6;
                            Raylib.BeginShaderMode(WaterShader);
                            Raylib.DrawTextureNPatch(myRender.Texture,
                            //528.0;
                            //264.0;
                            new NPatchInfo(myRender.Source, 0, 64, 0, 64, NPatchLayout.NPATCH_NINE_PATCH),
                                //myRender.Source,
                                //myRender.Destination with { x = myRender.Destination.x - offset, y = myRender.Destination.y - offset, width = myRender.Destination.width + offset, height = myRender.Destination.height + offset },
                                new Rectangle(0, 0, 10000, 10000),
                                myRender.Origin, myRender.RenderRotation, myRender.Color) ;
                            Raylib.EndShaderMode();
                            waterRendered = true;
                        }
                    }
                    else
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
