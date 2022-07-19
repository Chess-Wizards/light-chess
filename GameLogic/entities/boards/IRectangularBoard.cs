namespace GameLogic.Entities.Boards {
    interface IRectangularBoard : IBoard
    {
        int Width { get; }
        int Height { get; }
    }
}