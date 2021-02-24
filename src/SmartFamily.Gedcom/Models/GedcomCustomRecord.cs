using SmartFamily.Gedcom.Enums;

using System.IO;

namespace SmartFamily.Gedcom.Models
{
    /// <summary>
    /// GEDCOM allows for custom tags to be added by applications.
    /// This is essentially a dummy object.
    /// </summary>
    public class GedcomCustomRecord : GedcomEvent
    {
        private const string DefaultTagName = "_CUST";

        /// <summary>
        /// Initializes a new instance of the <see cref="GedcomCustomRecord"/> class.
        /// </summary>
        public GedcomCustomRecord()
        {
            EventType = GedcomEventType.Custom;
        }

        /// <inheritdoc/>
        public override GedcomRecordType RecordType
        {
            get { return GedcomRecordType.CustomRecord; }
        }

        /// <inheritdoc/>
        public override string GedcomTag
        {
            get { return Tag; }
        }

        /// <summary>
        /// Gets or sets the tag associated with this custom record.
        /// </summary>
        public string Tag { get; set; } = DefaultTagName;

        /// <summary>
        /// Placeholder for GEDCOM output code, does not actually output any data.
        /// </summary>
        /// <param name="sw">The writer to output to.</param>
        public override void Output(TextWriter sw)
        {
        }
    }
}