using System;
using System.Collections.Generic;
using System.Text;

namespace LaserBounceSolver.Entities
{
    public class Coordinate
    {
        public Coordinate(int x, int y, int z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        public int X { get; }

        public int Y { get; }

        public int Z { get; }

        public bool Equals(int x, int y, int z)
        {
            return x == X && y == Y && z == Z;
        }

        public override string ToString()
        {
            return $"({X},{Y},{Z})";
        }

    }
}
