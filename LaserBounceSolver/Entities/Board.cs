using LaserBounceSolver.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LaserBounceSolver.Entities
{
    public class Board
    {
        private Cube[,,] _cubes;

        private Stack<PlacedCube> _path = new Stack<PlacedCube>();


        public int Height { get; set; } // Y
        
        public int Width { get; set; } // Z

        public int Length { get; set; } // X

        public Board(int width, int height, int length)
        {
            if (width <= 0) throw new ArgumentException($"{nameof(width)} must be greater than 0");
            if (length <= 0) throw new ArgumentException($"{nameof(length)} must be greater than 0");
            if (height <= 0) throw new ArgumentException($"{nameof(height)} must be greater than 0");

            Height = height;
            Width = width;
            Length = length;

            _cubes = new Cube[Length, Height, Width];
        }

        public PlacedCube LastCube => _path.Peek();

        internal PlacedCube[] GetPath() => _path.ToArray();

        internal PlacedCube Undo()
        {
            var lastCube = _path.Peek();

            if (IsAvailable(lastCube.Position)) throw new Exception("Something went wrong: the position of the last cube is still available when undoing");

            _cubes[lastCube.Position.X, lastCube.Position.Y, lastCube.Position.Z] = null;
            return _path.Pop();
        }

        internal bool IsAvailable(Coordinate coord) =>
            coord != null &&
            (0 >= coord.X || coord.X < Length) && 
            (0 >= coord.Y || coord.Y < Height) && 
            (0 >= coord.Z || coord.X < Width) && 
            _cubes[coord.X, coord.Y, coord.Z] == null;

        public Cube GetCube(Coordinate c)
        {
            AssertCoordinateIsValid(c);

            return _cubes[c.X, c.Y, c.Z];
        }

        public PlacedCube SetCube(Cube cube, Coordinate c)
        {
            AssertCoordinateIsValid(c);

            if (!IsAvailable(c))
            {
                throw new ArgumentException($"Coordinate {c.ToString()} is already occupied");
            }

            var placedCube = new PlacedCube(cube, c);
            _path.Push(placedCube);
            _cubes[c.X, c.Y, c.Z] = placedCube;
            return placedCube;
        }        

        private void AssertCoordinateIsValid(Coordinate c)
        {
            if (c.X < 0 || c.X >= Length) throw new ArgumentException($"{nameof(c.X)} ({c.X}) must be greater than 0 and smaller than {nameof(Length)} ({Length})");
            if (c.Y < 0 || c.Y >= Height) throw new ArgumentException($"{nameof(c.Y)} ({c.Y}) must be greater than 0 and smaller than {nameof(Height)} ({Height})");
            if (c.Z < 0 || c.Z >= Width) throw new ArgumentException($"{nameof(c.Z)} ({c.Z}) must be greater than 0 and smaller than {nameof(Width)} ({Width})");
        }
    }
}
