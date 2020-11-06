// NumberOptions.cs 2020

namespace AVCli
{
    /// <summary>
    /// Defines the <see cref="NumberOptions" />.
    /// </summary>
    public class NumberOptions
    {
        /// <summary>
        /// Gets the Number.
        /// </summary>
        public string Number { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="NumberOptions"/> class.
        /// </summary>
        /// <param name="number">The number<see cref="string"/>.</param>
        public NumberOptions(string number)
        {
            Number = number;
        }
    }
}
