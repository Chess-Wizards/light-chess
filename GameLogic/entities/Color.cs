namespace GameLogic.Entities
{
    // Chess pieces colors.
    public enum Color
    {
        White,
        Black
    }

    public static class ColorExtension
    {
        public static Color Inversed(this Color color) => (color == Color.White) ? Color.Black : Color.White;
    }
}
