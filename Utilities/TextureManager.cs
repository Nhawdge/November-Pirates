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
            TextureStore.Add(TextureKey.HullLarge, Raylib.LoadTexture("Assets/Art/hullLarge.png"));
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
        HullLarge,
    }
}
