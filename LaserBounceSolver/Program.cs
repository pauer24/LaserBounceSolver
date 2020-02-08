using LaserBounceSolver.Entities;
using LaserBounceSolver.Services;

namespace LaserBounceSolver
{
    class Program
    {
        static void Main(string[] args)
        {
            var board = new Board(3, 3, 2);
            var start = new Cell(CubeFace.Front, 0, 0, 0).Mirror();
            var end = new Cell(CubeFace.Left, 0, 0, 0).Mirror();

            BoardSolver.FindSolutions(board, start, end);
            var solutions = BoardSolver.Solutions;

            /*  Optimizations:
             * 
             * - If the exit is locked, the solution is impossible
             */
        }
    }
}
