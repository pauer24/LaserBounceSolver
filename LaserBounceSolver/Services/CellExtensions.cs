using LaserBounceSolver.Entities;
using System.Linq;

namespace LaserBounceSolver.Services
{
    public static class CellExtensions
    {
        private static Cube[] _possibleCubes = CubeService.GetAvailable();

        public static Cube[] GetPossibleCubes(this Cell cell) => _possibleCubes.Where(c => c.In == cell.Orientation).ToArray();

        public static bool IsLinked(this Cell source, Cell toCheck) => source.Mirror().Equals(toCheck);
    }
}
