using LaserBounceSolver.Entities;
using System.Linq;
using System.Text;

namespace LaserBounceSolver.Services
{
    public class SolutionTranslator
    {
        public const char EmptyCubeChar = '-';
        
        public static StringBuilder Convert(Cube[][] solutions)
        {
            var appendedTranslation = new StringBuilder();

            foreach (var solution in solutions)
            {
                appendedTranslation.Append(Convert(solution));
                appendedTranslation.AppendLine();
            }

            return appendedTranslation;
        }
        
        public static StringBuilder Convert(Cube[] solution)
        {
            var translation = new StringBuilder();

            foreach (var cube in solution)
            {
                translation.Append(cube.In.Opposite() == cube.Out ? EmptyCubeChar : cube.Out.ToString().First());
            }

            return translation;
        }
    }
}