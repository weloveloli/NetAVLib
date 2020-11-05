// AvData.cs 2020

namespace AVCli.AVLib
{
    using System.Collections.Generic;

    /// <summary>
    /// Defines the <see cref="AvData" />.
    /// </summary>
    public class AvData : AvMetaData
    {
        /// <summary>
        /// Gets or sets the Year.
        /// </summary>
        public string Year { get; set; }

        /// <summary>
        /// Gets or sets the Source.
        /// </summary>
        public string Source { get; set; }

        /// <summary>
        /// Gets or sets the Time.
        /// </summary>
        public string Time { get; set; }

        /// <summary>
        /// Gets or sets the Release.
        /// </summary>
        public string Release { get; set; }

        /// <summary>
        /// Gets or sets the Studio.
        /// </summary>
        public string Studio { get; set; }

        /// <summary>
        /// Gets or sets the MainCover.
        /// </summary>
        public string MainCover { get; set; }

        /// <summary>
        /// Gets or sets the Outline.
        /// </summary>
        public string Outline { get; set; }

        /// <summary>
        /// Gets or sets the Actors.
        /// </summary>
        public List<string> Actors { get; set; }

        /// <summary>
        /// Gets or sets the Labels.
        /// </summary>
        public List<string> Labels { get; set; }

        /// <summary>
        /// Gets or sets the Tags.
        /// </summary>
        public List<string> Tags { get; set; }

        /// <summary>
        /// Gets or sets the Covers.
        /// </summary>
        public List<string> Covers { get; set; }

        /// <summary>
        /// Gets or sets the Series.
        /// </summary>
        public List<string> Series { get; set; }

        /// <summary>
        /// Gets or sets the Directors.
        /// </summary>
        public List<string> Directors { get; set; }

        /// <summary>
        /// Gets or sets the Magnets.
        /// </summary>
        public List<string> Magnets { get; set; }


        /// <summary>
        /// Gets or sets the PreviewVideo.
        /// </summary>
        public string PreviewVideo { get; set; }
    }
}
