using LaserBounceSolver.Entities;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Timers;

namespace LaserBounceSolver.Services
{
    public class AllPathExplorerBoardSolver
    {
        public IList<Cube[]> Solutions = new List<Cube[]>();
        private double _recursiveEntrance;
        private double _impossiblePathsFound;
        private Timer _timer;
        private DateTime _startTime = DateTime.Now;

        public virtual string UnderstandableName => "Explorador de totes les possibles solucions";

        public (Cube[][] SolutionsFound, double TotalStepsTried, double ImpossiblePaths, TimeSpan SpendTime) FindSolutions(Board board, Cell start, Cell end)
        {
            StartTimerToShowResults();
            var stopWatch = new Stopwatch();
            stopWatch.Start();
            ExploreSolutions(board, start, end);
            stopWatch.Stop();
            _timer.Stop();
            return (Solutions.ToArray(), _recursiveEntrance, _impossiblePathsFound, stopWatch.Elapsed);
        }

        private void ExploreSolutions(Board board, Cell start, Cell end)
        {
            _recursiveEntrance++;
            var nextCubes = GetPossibleCubes(board, start);

            foreach (var cube in nextCubes)
            {
                board.SetCube(cube, cube.Position);

                if (IsSolved(board, end))
                {
                    Solutions.Add(board.GetPath().Reverse().ToArray());
                }
                if (StopExploring(board, end))
                {
                    _impossiblePathsFound++;
                }
                else
                {
                    ExploreSolutions(board, cube.OutCell, end);
                }

                board.Undo();
            }
        }

        protected virtual bool StopExploring(Board board, Cell end) => false;

        protected virtual PlacedCube[] GetPossibleCubes(Board board, Cell lastPosition)
        {
            var mirror = lastPosition.Mirror();
            if (!IsViable(board, mirror)) return new PlacedCube[0];

            var potentialCubes = mirror.GetPossibleCubes()
                                    .Select(cube =>
                                        new PlacedCube(cube, mirror));

            return potentialCubes.Where(cube => board.IsAvailable(cube.Position)).ToArray();
        }

        protected bool IsSolved(Board board, Cell end) => board.LastCube.OutCell.Mirror().Equals(end);

        private void StartTimerToShowResults()
        {
            _timer = new System.Timers.Timer(5000);
            _timer.Elapsed += ShowStats;
            _timer.AutoReset = true;
            _timer.Enabled = true;
            _timer.Start();
        }

        private void ShowStats(object sender, ElapsedEventArgs e)
        {
            Console.WriteLine($"**************************************");
            Console.WriteLine($"# intents: {_recursiveEntrance}");
            Console.WriteLine($"# solucions trobades: {Solutions.Count()}");
            Console.WriteLine($"Temps invertit: {(e.SignalTime - _startTime).ToString()}");
        }

        private bool IsViable(Board board, Coordinate position) =>
                position.X >= 0 && position.X < board.Length &&
                position.Y >= 0 && position.Y < board.Height &&
                position.Z >= 0 && position.Z < board.Width &&
                board.IsAvailable(position);
    }
}