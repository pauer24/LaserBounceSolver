using LaserBounceSolver.Entities;
using System;

namespace LaserBounceSolver.Services
{
    public static class CubeFaceExtensions
    {
        public static CubeFace[] GetAllAvailable() => new[] { CubeFace.Top, CubeFace.Bottom, CubeFace.Right, CubeFace.Left, CubeFace.Back, CubeFace.Front };

        public static CubeFace Opposite(this CubeFace face)
        {
            switch (face)
            {
                case CubeFace.Top: return CubeFace.Bottom;
                case CubeFace.Bottom: return CubeFace.Top;
                case CubeFace.Front: return CubeFace.Back;
                case CubeFace.Back: return CubeFace.Front;
                case CubeFace.Right: return CubeFace.Left;
                case CubeFace.Left: return CubeFace.Right;
            }

            throw new ArgumentException($"The {nameof(CubeFace)} {face.ToString()} has no opposite face");
        }

        public static CubeFace[] Adjacent(this CubeFace face)
        {
            switch (face)
            {
                case CubeFace.Bottom: 
                case CubeFace.Top: return new[] { CubeFace.Back, CubeFace.Front, CubeFace.Right, CubeFace.Left };
                case CubeFace.Right:
                case CubeFace.Left: return new[] { CubeFace.Top, CubeFace.Bottom, CubeFace.Back, CubeFace.Front };
                case CubeFace.Front:
                case CubeFace.Back: return new[] { CubeFace.Right, CubeFace.Left, CubeFace.Top, CubeFace.Bottom };
            }

            throw new ArgumentException($"The {nameof(CubeFace)} {face.ToString()} has no adjacent faces");
        }
    }
}