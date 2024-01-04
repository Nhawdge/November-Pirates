using NovemberPirates.Components;
using Raylib_CsLo;

namespace NovemberPirates.Utilities
{
    internal class TextureManager
    {
        internal static TextureManager Instance { get; } = new();
        internal Dictionary<TextureKey, Texture> TextureStore { get; set; } = new();
        internal Dictionary<string, Texture> TextureCache { get; set; } = new();

        private TextureManager()
        {
            LoadTextures();
        }

        private void LoadTextures()
        {
            TextureStore.Add(TextureKey.MapTileset, Raylib.LoadTexture("Assets/Maps/tiles_sheet2.png"));
            //Raylib.SetTextureWrap(TextureStore[TextureKey.MapTileset], TextureWrap.TEXTURE_WRAP_REPEAT);
            //Raylib.SetTextureFilter(TextureStore[TextureKey.MapTileset], TextureFilter.TEXTURE_FILTER_BILINEAR);

            TextureStore.Add(TextureKey.MainMenuBackground, Raylib.LoadTexture("Assets/Art/Main_Menu.png"));
            TextureStore.Add(TextureKey.Words, Raylib.LoadTexture("Assets/Art/words.png"));
            TextureStore.Add(TextureKey.Button, Raylib.LoadTexture("Assets/Art/Button.png"));

            TextureStore.Add(TextureKey.HullLarge, Raylib.LoadTexture("Assets/Art/hullLarge.png"));
            TextureStore.Add(TextureKey.HullMedium, Raylib.LoadTexture("Assets/Art/hullMedium.png"));
            TextureStore.Add(TextureKey.HullSmall, Raylib.LoadTexture("Assets/Art/hullSmall.png"));
            TextureStore.Add(TextureKey.MainFlag, Raylib.LoadTexture("Assets/Art/mainFlag.png"));
            TextureStore.Add(TextureKey.Nest, Raylib.LoadTexture("Assets/Art/nest.png"));
            TextureStore.Add(TextureKey.Pole, Raylib.LoadTexture("Assets/Art/pole.png"));
            TextureStore.Add(TextureKey.SailLarge, Raylib.LoadTexture("Assets/Art/sailLarge1.png"));
            TextureStore.Add(TextureKey.SailSmall, Raylib.LoadTexture("Assets/Art/sailSmall1.png"));
            TextureStore.Add(TextureKey.Cannon, Raylib.LoadTexture("Assets/Art/cannon.png"));
            TextureStore.Add(TextureKey.CannonLoose, Raylib.LoadTexture("Assets/Art/cannonLoose.png"));
            TextureStore.Add(TextureKey.Cannonball, Raylib.LoadTexture("Assets/Art/cannonBall.png"));
            TextureStore.Add(TextureKey.Explosion, Raylib.LoadTexture("Assets/Art/explosion.png"));
            TextureStore.Add(TextureKey.Fire, Raylib.LoadTexture("Assets/Art/fire1.png"));
            TextureStore.Add(TextureKey.WhitePixel, Raylib.LoadTexture("Assets/Art/whitepixel.png"));
            TextureStore.Add(TextureKey.Crew, Raylib.LoadTexture("Assets/Art/crew.png"));
            TextureStore.Add(TextureKey.Wood, Raylib.LoadTexture("Assets/Art/wood.png"));
        }

        internal Texture GetTexture(TextureKey key)
        {
            if (TextureStore.Count <= 0)
            {
                LoadTextures();
            }
            return TextureStore[key];
        }
        internal Texture? GetCachedTexture(string key)
        {
            if (TextureCache.TryGetValue(key, out var texture))
                return texture;
            return null;
        }
    }

    internal enum TextureKey
    {
        Empty,
        HullLarge,
        MapTileset,
        MainFlag,
        Nest,
        Pole,
        SailLarge,
        SailSmall,
        Cannon,
        CannonLoose,
        Cannonball,
        Explosion,
        Fire,
        WhitePixel,
        Crew,
        HullSmall,
        HullMedium,
        Wood,
        MainMenuBackground,
        Words,
        Button,
    }
}
