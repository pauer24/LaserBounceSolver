using LaserBounceSolver.Entities;
using LaserBounceSolver.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaserBounceSolver
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var (board, start, end) = args.Length >= 3 ? ParseArgs(args) : AskForArgs();
            AllPathExplorerBoardSolver boardSolver = AskBoardSolverStrategy();

            if (board == null) return;

            var result = boardSolver.FindSolutions(board, start, end);

            await ShowAndExportSolutionWithStatistics(board, start, end, result, boardSolver);

            Console.WriteLine("Ja s'ha acabat. Pulsa qualsevol tecla per sortir...");
            Console.ReadKey();
        }

        private static AllPathExplorerBoardSolver AskBoardSolverStrategy()
        {
            //var board = new Board(3, 3, 3);
            //var start = new Cell(CubeFace.North, 0, 0, 0).Mirror();
            //var end = new Cell(CubeFace.East, 2, 2, 2).Mirror();
            //var board = new Board(2, 1, 2);
            //var start = new Cell(CubeFace.South, 0, 0, 0).Mirror();
            //var end = new Cell(CubeFace.East, 1, 0, 1).Mirror();

            Console.WriteLine("Escull l'estratègia amb la que vols resoldre-ho:");
            Console.WriteLine("1. Explorar tots els camins");
            Console.WriteLine("2. Deixar d'explorar quan la sortida està bloquejada");

            while(true)
            {
                var selectedOption = Console.ReadLine();
                switch(selectedOption)
                {
                    case "1": return new AllPathExplorerBoardSolver();
                    case "2": return new ExitLockQuitBoardSolver();
                }

                Console.WriteLine("Has escollit una opció invàlida. Siusplau, torna a probar");
            }
        }

        private static (Board board, Cell start, Cell end) ParseArgs(string[] args)
        {
            try
            {
                return (
                    InputParser.ParseBoard(args[0]),
                    InputParser.ParseCell(args[1]).Mirror(),
                    InputParser.ParseCell(args[2]).Mirror());
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                return (null, null, null);
            }
        }

        private static (Board board, Cell start, Cell end) AskForArgs() => (AskForBoard(), AskForCell("INICI"), AskForCell("FINAL"));

        private static Cell AskForCell(string tipus)
        {
            Console.WriteLine($"Introdueix la cel·la del {tipus}. El format esperat és {InputParser.CellInputFormat}");

            Cell cell = null;
            while (cell == null)
            {
                try
                {
                    cell = InputParser.ParseCell(Console.ReadLine());
                }
                catch
                {
                    Console.WriteLine("Format incorrecte. Torna-ho a probar.");
                }
            }

            return cell.Mirror();
        }

        private static Board AskForBoard()
        {
            Console.WriteLine($"Introdueix les dimensions del taulell. El format esperat és {InputParser.BoardInputFormat}");
            Board board = null;
            while (board == null)
            {
                try
                {
                    board = InputParser.ParseBoard(Console.ReadLine());
                }
                catch
                {
                    Console.WriteLine("Format incorrecte. Torna-ho a probar.");
                }
            }

            return board;
        }

        private static Task ShowAndExportSolutionWithStatistics(
            Board board, Cell start, Cell end, 
            (Cube[][] SolutionsFound, double TotalStepsTried, double ImpossiblePaths, TimeSpan SpendTime) result, 
            AllPathExplorerBoardSolver boardSolver)
        {
            var understandableSolution = new StringBuilder();

            understandableSolution.AppendLine($"DIMENSIONS TAULELL: S->N={board.Length}; B->T={board.Height}; W->E={board.Width}");
            understandableSolution.AppendLine($"\tINICI={start.Mirror().ToString()}; FI={end.Mirror().ToString()}");
            understandableSolution.AppendLine($"\tEstratègia utilitzada={boardSolver.UnderstandableName}");
            understandableSolution.AppendLine($"\t# passos realitzats: {result.TotalStepsTried}");
            understandableSolution.AppendLine($"\t# camins impossibles explorats: {result.ImpossiblePaths}");
            understandableSolution.AppendLine($"\t# solucions trobades: {result.SolutionsFound.Length}");
            understandableSolution.AppendLine($"\tTemps invertit: {result.SpendTime.ToString()}");

            understandableSolution.Append(FormatSolutionWithStatistics(result.SolutionsFound));

            Directory.CreateDirectory("./resultats");

            var filePath = Path.Combine(Directory.GetCurrentDirectory(), $"resultats\\{board.Length}x{board.Width}x{board.Height}_{DateTime.Now.ToString("yyMMdd_HH.mm.ss")}.txt");
            Console.WriteLine("S'han guardat els resultats a: {0}", filePath);

            return File.WriteAllTextAsync(filePath, understandableSolution.ToString());
        }

        private static StringBuilder FormatSolutionWithStatistics(Cube[][] solutionsFound)
        {
            var sortedSolutions = TranslateAndSortSolutions(solutionsFound);
            
            var solutionsCountPerChangeDirectionCubesCount = new List<(int UsedCubes, int SolutionsCount)>();

            var sortedAndTranslatedSolutionOutput = new StringBuilder();

            var solutionsPerCubesCount = new StringBuilder();
            var previousCubesCount = 0;
            var solutionsCount = 0;
            foreach (var solution in sortedSolutions)
            {
                if (solution.ChangeDirectionCubes != previousCubesCount)
                {
                    WriteSolutionsPerCubesCount(solutionsCountPerChangeDirectionCubesCount, sortedAndTranslatedSolutionOutput, solutionsPerCubesCount, previousCubesCount, solutionsCount);

                    solutionsPerCubesCount.Clear();
                    previousCubesCount = solution.ChangeDirectionCubes;
                    solutionsCount = 0;
                }

                solutionsCount++;
                solutionsPerCubesCount.AppendLine(solution.TranslatedSolution);
            }
            WriteSolutionsPerCubesCount(solutionsCountPerChangeDirectionCubesCount, sortedAndTranslatedSolutionOutput, solutionsPerCubesCount, previousCubesCount, solutionsCount);
            var solutionsWithStatistics = new StringBuilder();
            solutionsWithStatistics.AppendLine("\n******************* RECOMPTE SOLUCIONS TROBADES *********************");
            solutionsWithStatistics.AppendLine("# prismes   | # solucions");
            const int maxAllowedCubes = 6;
            solutionsCountPerChangeDirectionCubesCount.ForEach(count => solutionsWithStatistics.AppendLine($"{count.UsedCubes}{new string(' ', maxAllowedCubes - count.UsedCubes.ToString().Length)}{count.SolutionsCount}"));


            solutionsWithStatistics.AppendLine("\n****************** SOLUCIONS TROBADES PER NOMBRE DE PRISMES *********************");
            return solutionsWithStatistics.Append(sortedAndTranslatedSolutionOutput);
        }

        private static void WriteSolutionsPerCubesCount(List<(int UsedCubes, int SolutionsCount)> solutionsCountPerChangeDirectionCubesCount, StringBuilder sortedAndTranslatedSolutionOutput, StringBuilder solutionsPerCubesCount, int previousCubesCount, int solutionsCount)
        {
            if (solutionsCount > 0)
            {
                solutionsCountPerChangeDirectionCubesCount.Add((previousCubesCount, solutionsCount));

                sortedAndTranslatedSolutionOutput.AppendLine($"\n·· SOLUCIONS AMB {previousCubesCount} PRISMES:");
                sortedAndTranslatedSolutionOutput.Append(solutionsPerCubesCount);
            }
        }

        private static System.Collections.Generic.List<(string TranslatedSolution, int ChangeDirectionCubes, int KeepStraightCubes)> TranslateAndSortSolutions(Cube[][] solutionsFound)
        {
            var convertedSolutions = solutionsFound.Select(s => SolutionTranslator.Convert(s).ToString());
            var processedSolutions = convertedSolutions.Select(solution =>
            {
                var keepStraightCubes = solution.Count(c => c == SolutionTranslator.EmptyCubeChar);
                var changeDirectionCubes = solution.Length - keepStraightCubes;
                return (solution, changeDirectionCubes, keepStraightCubes);
            });

            return processedSolutions.OrderBy(s => s.changeDirectionCubes).ToList();
        }
    }
}
