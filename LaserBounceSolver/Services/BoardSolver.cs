using LaserBounceSolver.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Timers;

namespace LaserBounceSolver.Services
{
    public class BoardSolver
    {
        public static IList<Cube[]> Solutions = new List<Cube[]>();
        private static double _attemptsMade;
        private static System.Timers.Timer _timer;

        public static void FindSolutions(Board board, Cell start, Cell end)
        {
            _attemptsMade++;
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

        public static void StartTimerAndShowResults()
        {
            _timer = new System.Timers.Timer(5000);
            _timer.Elapsed += ShowStats;
            _timer.AutoReset = true;
            _timer.Enabled = true;
            _timer.Start();
        }

        private static void ShowStats(object sender, ElapsedEventArgs e)
        {
            Console.WriteLine($"**************************************");
            Console.WriteLine($"# intents: {_attemptsMade}");
            Console.WriteLine($"# solucions trobades: {Solutions.Count()}");
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