// -----------------------------------------------------------------------
// <copyright file="IHtmlContentReader.cs" company="Weloveloli">
//     Copyright (c) Weloveloli.  All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Weloveloli.AVLib
{
    using System.Threading.Tasks;

    /// <summary>
    /// Defines the <see cref="IHtmlContentReader" />.
    /// </summary>
    public interface IHtmlContentReader
    {
        /// <summary>
        /// The LoadFromUrlAsync.
        /// </summary>
        /// <param name="url">The url<see cref="string"/>.</param>
        /// <returns>The <see cref="Task{string}"/>.</returns>
        public Task<string> LoadFromUrlAsync(string url);
    }
}
