namespace GameLogic.Entities.Boards
{
    public interface IRectangularBoard : IBoard
    {
        int Width { get; }
        int Height { get; }
    }
}
