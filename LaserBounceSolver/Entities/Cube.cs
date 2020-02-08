
namespace LaserBounceSolver.Entities
{
    public class Cube
    {
        public CubeFace In { get; set; }

        public CubeFace Out { get; set; }
    }

    public class PlacedCube : Cube
    {
        public PlacedCube(Cube cube, Coordinate position)
        {
            In = cube.In;
            Out = cube.Out;
            Position = position;
        }

        public Coordinate Position { get; set; }

        public Cell InCell => new Cell(In, Position.X, Position.Y, Position.Z);

        public Cell OutCell => new Cell(Out, Position.X, Position.Y, Position.Z);

        public override string ToString()
        {
            return $"[{((Coordinate)Position).ToString()} {In.ToString()} -> {Out.ToString()}]";
        }
    }
}
