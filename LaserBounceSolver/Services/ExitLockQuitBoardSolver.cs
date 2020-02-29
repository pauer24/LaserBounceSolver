using LaserBounceSolver.Entities;

namespace LaserBounceSolver.Services
{
    public class ExitLockQuitBoardSolver : AllPathExplorerBoardSolver
    {
        public override string UnderstandableName => "Camí impossible si la sortida queda bloquejada";

        protected override bool StopExploring(Board board, Cell end) => IsExitLocked(board, end);

        private bool IsExitLocked(Board board, Cell end) => !board.IsAvailable(end?.Mirror());
    }
}