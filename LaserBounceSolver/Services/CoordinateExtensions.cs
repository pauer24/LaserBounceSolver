using LaserBounceSolver.Entities;

namespace LaserBounceSolver.Services
{
    public static class CoordinateExtensions
    {
        public static Cell ToCell(this Coordinate coordinate, CubeFace orientation) =>
            new Cell(orientation, coordinate.X, coordinate.Y, coordinate.Z);
    }
}