using System.IO;
using System.Linq;

namespace TeamTimeWarp.Rest.Controllers
{
    public static class StringExtensions
    {
        public static bool HasInvalidChars(this string str)
        {
            return str.Intersect(Path.GetInvalidPathChars()).Any();
        }
    }
}