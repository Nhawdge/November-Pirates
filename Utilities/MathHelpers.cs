namespace NovemberPirates.Utilities
{
    internal static class MathHelpers
    {
        public static float ToDegrees(this float radians)
        {
            return radians * (180 / MathF.PI);
        }
        public static float ToRadians(this float degrees)
        {
            return degrees * (MathF.PI / 180);
        }

    }
}
