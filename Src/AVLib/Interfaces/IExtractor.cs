// IExtractor.cs 2020

namespace AVCli.AVLib.Interfaces
{
    using AVCli.AVLib.Models;
    using System.Threading.Tasks;

    /// <summary>
    /// Defines the <see cref="IExtractor" />.
    /// </summary>
    interface IExtractor
    {
        /// <summary>
        /// The GetDataAsync.
        /// </summary>
        /// <param name="number">The number<see cref="string"/>.</param>
        /// <returns>The <see cref="Task{AvData}"/>.</returns>
        Task<AvData> GetDataAsync(string number);
    }
}
