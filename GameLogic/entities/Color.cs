namespace GameLogic
{
    // Player colors.
    public enum Color
    {
        White,
        Black
    }

    static class ColorExtension
    {
        public static Color Change(this Color color)
        {
            return color == Color.White ? Color.Black : Color.White;
        }
    }
}
