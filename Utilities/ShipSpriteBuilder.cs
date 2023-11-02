using NovemberPirates.Components;
using Raylib_CsLo;
using static NovemberPirates.Components.Sprite;

namespace NovemberPirates.Utilities
{
    internal static class ShipSpriteBuilder
    {
        internal static Sprite GenerateBoat(BoatOptions options)
        {
            var baseHull = options.Hull switch
            {
                BoatType.HullLarge => TextureManager.Instance.GetTexture(TextureKey.HullLarge),
                _ => throw new NotImplementedException($"Hull type: '{options.Hull}' does not exist "),
            };

            var flag = TextureManager.Instance.GetTexture(TextureKey.MainFlag);

            var renderTexture = Raylib.LoadRenderTexture((int)baseHull.width, (int)baseHull.height);

            Raylib.BeginTextureMode(renderTexture);
            Raylib.BeginDrawing();

            Raylib.ClearBackground(new Color(1, 1, 1, 0));
            Raylib.DrawTexture(baseHull, 0, 0, Raylib.WHITE);
            Raylib.DrawTexture(flag, 0, 0, Raylib.WHITE);

            Raylib.EndDrawing();
            Raylib.EndTextureMode();

            var img = Raylib.LoadImageFromTexture(renderTexture.texture);

            Raylib.ExportImage(img, "test.png");

            var sprite = new Sprite()
            {
                Texture = renderTexture.texture,
                Animations = new Dictionary<string, AnimationSets>()
                {
                    {
                        "idle", new AnimationSets("idle", 0,0, Direction.forward, new List<Frame>
                        {
                            new Frame(0,0, baseHull.width/2, baseHull.height/2, 100f)
                        })
                    }
                },
                Column = 0,
                Row = 0,
                Color = Raylib.WHITE,
            };
            sprite.Play("idle");

            return sprite;
        }

    }
    internal record BoatOptions(BoatType Hull, BoatColor Color);


    internal enum BoatType
    {
        HullLarge,
        HullMedium,
        HullSmall,
    }

    internal enum BoatColor
    {
        White,
        Dead,
        Red,
        Blue,
        Green,
        Yellow,
    }
}
