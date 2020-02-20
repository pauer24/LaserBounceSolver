using System;

namespace LaserBounceSolver.Entities
{
    public class Cell : Coordinate
    {
        public Cell(CubeFace orientation, int x, int y, int z) : base(x, y, z)
        {
            Orientation = orientation;
        }

        public CubeFace Orientation { get; set; }

        public Cell Mirror()
        {
            switch(Orientation)
            {
                case CubeFace.Top: return new Cell(CubeFace.Bottom, X, Y + 1, Z);
                case CubeFace.Bottom: return new Cell(CubeFace.Top, X, Y - 1, Z);
                case CubeFace.South: return new Cell(CubeFace.North, X, Y, Z - 1);
                case CubeFace.North: return new Cell(CubeFace.South, X, Y, Z + 1);
                case CubeFace.East: return new Cell(CubeFace.West, X + 1, Y, Z);
                case CubeFace.West: return new Cell(CubeFace.East, X - 1, Y, Z);
            }

            throw new ArgumentException($"{ToString()} cannot create a Mirror Cell");
        }

        public override bool Equals(object obj)
        {
            return obj is Cell c &&
                c.Orientation == Orientation && c.Equals(X, Y, Z);
        }

        public override string ToString()
        {
            return $"{base.ToString()}[{Orientation.ToString()}]";
        }
    }
}
