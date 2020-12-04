// -----------------------------------------------------------------------
// <copyright file="StringExtensions.cs" company="Weloveloli">
//     Copyright (c) Weloveloli.  All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Weloveloli.AVLib.Extensions
{
    using System;
    using System.Text;

    /// <summary>
    /// Defines the <see cref="StringExtensions" />.
    /// </summary>
    public static class StringExtensions
    {
        /// <summary>
        /// The SHA256.
        /// </summary>
        /// <param name="data">The data<see cref="string"/>.</param>
        /// <returns>The <see cref="string"/>.</returns>
        public static string SHA256(this string data)
        {
            if (data == null)
            {
                return null;
            }

            byte[] bytes = Encoding.UTF8.GetBytes(data);
            byte[] hash = System.Security.Cryptography.SHA256.Create().ComputeHash(bytes);

            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < hash.Length; i++)
            {
                builder.Append(hash[i].ToString("x2"));
            }
            return builder.ToString();
        }

        /// <summary>
        /// The Shorten.
        /// </summary>
        /// <param name="data">The data<see cref="string"/>.</param>
        /// <param name="length">The length<see cref="int"/>.</param>
        /// <returns>The <see cref="string"/>.</returns>
        public static string Shorten(this string data, int length)
        {
            if (data == null)
            {
                return null;
            }
            if (length < 0)
            {
                throw new ArgumentException("length should be large than 0");
            }
            if (data.Length <= length)
            {
                return data;
            }
            else
            {
                return data.Substring(0, length - 3) + "...";
            }
        }
    }
}
