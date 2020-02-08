using LaserBounceSolver.Entities;
using LaserBounceSolver.Services;

namespace LaserBounceSolver
{
    class Program
    {
        static void Main(string[] args)
        {
            var board = new Board(3, 3, 3);
            var start = new Cell(CubeFace.Front, 0, 0, 0).Mirror();
            var end = new Cell(CubeFace.Right, 2, 2, 2).Mirror();

            BoardSolver.StartTimerAndShowResults();
            BoardSolver.FindSolutions(board, start, end);
            var solutions = BoardSolver.Solutions;

            /*  Optimizations:
             * 
             * - If the exit is locked, the solution is impossible
             */
        }
    }
}
