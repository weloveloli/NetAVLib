// IExtractor.cs 2020

namespace AVCli.AVLib
{
    using System.Threading.Tasks;

    /// <summary>
    /// Defines the <see cref="IExtractor" />.
    /// </summary>
    public interface IExtractor
    {
        /// <summary>
        /// The GetDataAsync.
        /// </summary>
        /// <param name="number">The number<see cref="string"/>.</param>
        /// <returns>The <see cref="Task{AvData}"/>.</returns>
        public Task<AvData> GetDataAsync(string number);

        /// <summary>
        /// The getKey.
        /// </summary>
        /// <returns>The <see cref="string"/>.</returns>
        public string GetKey();
    }
}
