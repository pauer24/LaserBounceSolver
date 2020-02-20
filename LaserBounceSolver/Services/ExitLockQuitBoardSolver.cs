using LaserBounceSolver.Entities;

namespace LaserBounceSolver.Services
{
    public class ExitLockQuitBoardSolver : AllPathExplorerBoardSolver
    {
        protected override bool IsExitLocked(Board board, Cell end) => !board.IsAvailable(FinalCubeCoordinate(end));
    }
}