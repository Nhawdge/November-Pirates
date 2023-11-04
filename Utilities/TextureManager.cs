using Raylib_CsLo;

namespace NovemberPirates.Utilities
{
    internal class TextureManager
    {
        internal static TextureManager Instance { get; } = new();
        internal Dictionary<TextureKey, Texture> TextureStore { get; set; } = new();

        private TextureManager()
        {
            LoadTextures();
        }

        private void LoadTextures()
        {
            TextureStore.Add(TextureKey.MapTileset, Raylib.LoadTexture("Assets/Maps/tiles_sheet.png"));

            TextureStore.Add(TextureKey.HullLarge, Raylib.LoadTexture("Assets/Art/hullLarge.png"));
            TextureStore.Add(TextureKey.MainFlag, Raylib.LoadTexture("Assets/Art/mainFlag.png"));
            TextureStore.Add(TextureKey.Nest, Raylib.LoadTexture("Assets/Art/nest.png"));
            TextureStore.Add(TextureKey.Pole, Raylib.LoadTexture("Assets/Art/pole.png"));
            TextureStore.Add(TextureKey.SailLarge, Raylib.LoadTexture("Assets/Art/sailLarge1.png"));
            TextureStore.Add(TextureKey.SailSmall, Raylib.LoadTexture("Assets/Art/sailSmall1.png"));
        }

        internal Texture GetTexture(TextureKey key)
        {
            if (TextureStore.Count <= 0)
            {
                LoadTextures();
            }
            return TextureStore[key];
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
    }
}
