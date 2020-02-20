using LaserBounceSolver.Entities;
using System;

namespace LaserBounceSolver.Services
{
    public static class CubeFaceExtensions
    {
        public static CubeFace[] GetAllAvailable() => new[] { CubeFace.Top, CubeFace.Bottom, CubeFace.East, CubeFace.West, CubeFace.North, CubeFace.South };

        public static CubeFace Opposite(this CubeFace face)
        {
            switch (face)
            {
                case CubeFace.Top: return CubeFace.Bottom;
                case CubeFace.Bottom: return CubeFace.Top;
                case CubeFace.South: return CubeFace.North;
                case CubeFace.North: return CubeFace.South;
                case CubeFace.East: return CubeFace.West;
                case CubeFace.West: return CubeFace.East;
            }

            throw new ArgumentException($"The {nameof(CubeFace)} {face.ToString()} has no opposite face");
        }

        public static CubeFace[] Adjacent(this CubeFace face)
        {
            switch (face)
            {
                case CubeFace.Bottom: 
                case CubeFace.Top: return new[] { CubeFace.North, CubeFace.South, CubeFace.East, CubeFace.West };
                case CubeFace.East:
                case CubeFace.West: return new[] { CubeFace.Top, CubeFace.Bottom, CubeFace.North, CubeFace.South };
                case CubeFace.South:
                case CubeFace.North: return new[] { CubeFace.East, CubeFace.West, CubeFace.Top, CubeFace.Bottom };
            }

            throw new ArgumentException($"The {nameof(CubeFace)} {face.ToString()} has no adjacent faces");
        }
    }
}