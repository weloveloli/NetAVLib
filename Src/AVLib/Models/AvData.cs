// AvData.cs 2020

namespace AVCli.AVLib.Models
{
    using System.Collections.Generic;

    /// <summary>
    /// Defines the <see cref="AvData" />.
    /// </summary>
    internal class AvData
    {
        /// <summary>
        /// Gets or sets the Title.
        /// </summary>
        private string Title { get; set; }

        /// <summary>
        /// Gets or sets the Year.
        /// </summary>
        private string Year { get; set; }

        /// <summary>
        /// Gets or sets the Number.
        /// </summary>
        private string Number { get; set; }

        /// <summary>
        /// Gets or sets the Source.
        /// </summary>
        private string Source { get; set; }

        /// <summary>
        /// Gets or sets the Time.
        /// </summary>
        private string Time { get; set; }

        /// <summary>
        /// Gets or sets the Release.
        /// </summary>
        private string Release { get; set; }

        /// <summary>
        /// Gets or sets the Studio.
        /// </summary>
        private string Studio { get; set; }

        /// <summary>
        /// Gets or sets the MainCover.
        /// </summary>
        private string MainCover { get; set; }

        /// <summary>
        /// Gets or sets the Outline.
        /// </summary>
        private string Outline { get; set; }

        /// <summary>
        /// Gets or sets the Actors.
        /// </summary>
        private List<string> Actors { get; set; }

        /// <summary>
        /// Gets or sets the Labels.
        /// </summary>
        private List<string> Labels { get; set; }

        /// <summary>
        /// Gets or sets the Tags.
        /// </summary>
        private List<string> Tags { get; set; }

        /// <summary>
        /// Gets or sets the Covers.
        /// </summary>
        private List<string> Covers { get; set; }

        /// <summary>
        /// Gets or sets the Series.
        /// </summary>
        private List<string> Series { get; set; }

        /// <summary>
        /// Gets or sets the Directors.
        /// </summary>
        private List<string> Directors { get; set; }

        /// <summary>
        /// Gets or sets the Magnets.
        /// </summary>
        private List<string> Magnets { get; set; }

        /// <summary>
        /// Gets or sets the WebSiteUrl.
        /// </summary>
        private string WebSiteUrl { get; set; }

        /// <summary>
        /// Gets or sets the PreviewVideo.
        /// </summary>
        private string PreviewVideo { get; set; }
    }
}
