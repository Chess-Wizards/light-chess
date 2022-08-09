namespace GameLogic.Entities
{
    // Player colors.
    public enum Color
    {
        White,
        Black
    }

    public static class ColorExtension // extension?
    {
        // maybe rename to smth like "Inversed" ?
        public static Color Change(this Color color) => (color == Color.White) ? Color.Black : Color.White;
    }
}
