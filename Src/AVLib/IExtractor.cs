﻿// -----------------------------------------------------------------------
// <copyright file="IExtractor.cs" company="Weloveloli">
//     Copyright (c) Weloveloli.  All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Weloveloli.AVLib
{
    using System.Collections.Generic;
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
        /// The SearchDataAsync.
        /// </summary>
        /// <param name="keyword">The keyword<see cref="string"/>.</param>
        /// <returns>The <see cref="Task{List{AvData}}"/>.</returns>
        public Task<List<AvData>> SearchDataAsync(string keyword);

        /// <summary>
        /// The getKey.
        /// </summary>
        /// <returns>The <see cref="string"/>.</returns>
        public string GetKey();
    }
}
