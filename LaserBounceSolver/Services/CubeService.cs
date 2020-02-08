using LaserBounceSolver.Entities;
using System.Linq;

namespace LaserBounceSolver.Services
{
    public static class CubeService
    {
        public static Cube[] GetAvailable()
        {
            var cubes = CubeFaceExtensions.GetAllAvailable().SelectMany(face => face.Adjacent().Select(adjacentFace => new Cube { In = face, Out = adjacentFace })).ToList();
            cubes.AddRange(CubeFaceExtensions.GetAllAvailable().Select(face => new Cube { In = face, Out = face.Opposite() }));

            return cubes.ToArray();
        }

        public static bool IsLinked(this PlacedCube current, PlacedCube toCheck) => current.OutCell.IsLinked(toCheck.InCell);
    }
}