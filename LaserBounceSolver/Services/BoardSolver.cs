using LaserBounceSolver.Entities;
using System.Collections.Generic;
using System.Linq;

namespace LaserBounceSolver.Services
{
    public class BoardSolver
    {
        public static IList<Cube[]> Solutions = new List<Cube[]>();

        public static void FindSolutions(Board board, Cell start, Cell end)
        {
            var nextCubes = GetPossibleCubes(board, start);

            foreach (var cube in nextCubes)
            {
                board.SetCube(cube, cube.Position);

                if (IsSolved(board, end))
                {
                    Solutions.Add(board.GetPath().Reverse().ToArray());
                } else
                {
                    FindSolutions(board, cube.OutCell, end);
                }
                    
                board.Undo();
            }
        }

        public static bool IsSolved(Board board, Cell end) => board.LastCube.OutCell.Mirror().Equals(end);

        public static PlacedCube[] GetPossibleCubes(Board board, Cell lastPosition)
        {
            var mirror = lastPosition.Mirror();
            if (!IsViable(board, mirror)) return new PlacedCube[0];

            var potentialCubes = mirror.GetPossibleCubes()
                                    .Select(cube =>
                                        new PlacedCube(cube, mirror));

            return potentialCubes.Where(cube => board.IsAvailable(cube.Position)).ToArray();
        }

        private static bool IsViable(Board board, Coordinate position) =>
                position.X >= 0 && position.X < board.Length &&
                position.Y >= 0 && position.Y < board.Height &&
                position.Z >= 0 && position.Z < board.Width &&
                board.IsAvailable(position);
    }
}