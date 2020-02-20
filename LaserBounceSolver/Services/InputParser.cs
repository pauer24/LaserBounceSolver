using LaserBounceSolver.Entities;
using System;
using System.Linq;

namespace LaserBounceSolver.Services
{
    public static class InputParser
    {
        public static string BoardInputFormat = "amplada,alçada,fondaria. Exemple: 3,3,3";
        public static string CellInputFormat = "orientacio-x,y,z. Exemple: n-2,1,0";

        public static Board ParseBoard(string expectedBoard)
        {
            var dimensions = ParseDimension(expectedBoard);

            if (dimensions.Length != 3) throw new ArgumentException($"Format de taulell incorrecte. Rebut: '{expectedBoard}'. Format esperat: {BoardInputFormat}");

            return new Board(dimensions[0], dimensions[1], dimensions[2]);
        }

        public static Cell ParseCell(string expectedCell)
        {
            var firstSplit = expectedCell.Split('-');
            if (firstSplit.Length != 2) throw new ArgumentException($"Format de cel·la incorrecte. Rebut: '{expectedCell}. Format esperat: {CellInputFormat}");
            var coordinate = ParseDimension(firstSplit[1]);
            if (coordinate.Length != 3) throw new ArgumentException($"Format de cel·la incorrecte. Rebut: '{expectedCell}. Format esperat: {CellInputFormat}");

            return new Cell(ParseCubeFace(firstSplit[0]), coordinate[0], coordinate[1], coordinate[2]);
        }

        private static int[] ParseDimension(string expectedDimensions)
        {
            try
            {
                return expectedDimensions.Split(',').Select(dimension => int.Parse(dimension)).ToArray();
            }
            catch (Exception)
            {
                throw new ArgumentException("Alguna de les dimensions no es pot convertir a número");
            };
        }

        private static CubeFace ParseCubeFace(string expectedCubeFace)
        {
            switch(expectedCubeFace.ToLower().First())
            {
                case 'n': return CubeFace.North;
                case 's': return CubeFace.South;
                case 'w': return CubeFace.West;
                case 'e': return CubeFace.East;
                case 't': return CubeFace.Top;
                case 'b': return CubeFace.Bottom;
                default:
                    throw new ArgumentException($"'{expectedCubeFace}' cannot be parsed as CubeFace");
            }
        }
    }
}