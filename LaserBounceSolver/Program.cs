using LaserBounceSolver.Entities;
using LaserBounceSolver.Services;

namespace LaserBounceSolver
{
    class Program
    {
        static void Main(string[] args)
        {
            var board = new Board(2, 2, 1);
            var start = new Cell(CubeFace.Back, 0, 0, -1);
            var end = new Cell(CubeFace.Front, 0, 0, 2);

            BoardSolver.FindSolutions(board, start, end);
        }
    }
}
