using NovemberPirates.Components;
using NovemberPirates.Extensions;
using Raylib_CsLo;
using System.Numerics;
using static NovemberPirates.Components.Sprite;

namespace NovemberPirates.Utilities
{
    internal static class ShipSpriteBuilder
    {
        internal static Sprite GenerateBoat(BoatOptions options)
        {
            var shipSize = new Vector2(66, 128);
            var baseHullSprite = options.Hull switch
            {
                BoatType.HullLarge => new Sprite(TextureKey.HullLarge, "Assets/Art/hullLarge") { Position = shipSize / 2 },
                _ => throw new NotImplementedException($"Hull type: '{options.Hull}' does not exist "),
            };
            baseHullSprite.Play("HullLarge1");

            //Cannons


            // Main sail
            var mainSailSprite = options.Hull switch
            {
                BoatType.HullLarge => new Sprite(TextureKey.SailLarge, "Assets/Art/sailLarge1"),
                _ => throw new NotImplementedException($"Hull type: '{options.Hull}' does not exist "),
            };
            mainSailSprite.Play($"{options.Color}{(int)options.Condition}");
            mainSailSprite.Position = new Vector2(shipSize.X / 2, 45);

            //Nest
            var nestSprite = new Sprite(TextureKey.Nest, "Assets/Art/nest") { };
            nestSprite.Position = new Vector2(shipSize.X / 2, 25);

            // flag
            var flagSprite = new Sprite(TextureKey.MainFlag, "Assets/Art/mainFlag") { };
            flagSprite.Position = new Vector2(shipSize.X / 2, 15);
            flagSprite.Play($"{options.Color}Flag");

            //Pole
            var poleSprite = new Sprite(TextureKey.Pole, "Assets/Art/pole") { };
            poleSprite.Position = new Vector2(shipSize.X / 2, 100);

            //front sail
            var frontSailSprite = new Sprite(TextureKey.SailSmall, "Assets/Art/sailSmall1");
            frontSailSprite.Play($"{options.Color}{(int)options.Condition}");
            frontSailSprite.Position = new Vector2(shipSize.X / 2, 105);

            // Build Texture
            var renderTexture = Raylib.LoadRenderTexture((int)shipSize.X, (int)shipSize.Y);

            Raylib.BeginTextureMode(renderTexture);
            Raylib.ClearBackground(new Color(0, 0, 0, 0));

            baseHullSprite.Draw();
            if (options.Sails >= SailStatus.Full)
                mainSailSprite.Draw();
            nestSprite.Draw();
            flagSprite.Draw();
            poleSprite.Draw();
            if (options.Sails >= SailStatus.Half)
                frontSailSprite.Draw();

            Raylib.EndTextureMode();

            var sprite = new Sprite()
            {
                Texture = renderTexture.texture,
                Animations = new Dictionary<string, AnimationSets>()
                {
                    {
                        "idle", new AnimationSets("idle", 0,0, Direction.forward, new List<Frame>
                        {
                            new Frame(0,0,66,128, 100f)
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
    internal record BoatOptions(BoatType Hull, BoatColor Color, SailStatus Sails, BoatCondition Condition = BoatCondition.Good);


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
    internal enum BoatCondition
    { // Values tie to animations, be warned
        Good = 1,
        Torn = 2,
        Broken = 3,
        Empty = 4,

    }
}
