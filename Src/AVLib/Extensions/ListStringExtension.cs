namespace AVCli.AVLib.Extensions
{
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Defines the <see cref="ListStringExtension" />.
    /// </summary>
    public static class ListStringExtension
    {
        /// <summary>
        /// The Join.
        /// </summary>
        /// <param name="strings">The strings<see cref="List{string}"/>.</param>
        /// <param name="decimiler">The decimiler<see cref="string"/>.</param>
        /// <returns>The <see cref="string"/>.</returns>
        public static string Join(this List<string> strings, string decimiler)
        {
            return strings.Aggregate("", (x, y) => string.IsNullOrEmpty(x) ? y : x + decimiler + y);
        }
    }
}
