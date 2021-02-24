using SmartFamily.Gedcom.Enums;

using System;
using System.IO;
using System.Text;
using System.Xml;

namespace SmartFamily.Gedcom.Models
{
    /// <summary>
    /// GEDCOM Note Record
    /// </summary>
    public class GedcomNoteRecord : GedcomRecord, IEquatable<GedcomNoteRecord>
    {
        private string text;

        /// <summary>
        /// Initializes a new instance of the <see cref="GedcomNoteRecord"/> class.
        /// </summary>
        public GedcomNoteRecord()
        {
            ParsedText = new StringBuilder();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GedcomNoteRecord"/> class.
        /// </summary>
        /// <param name="database">The database to associate with this record.</param>
        public GedcomNoteRecord(GedcomDatabase database)
            : this()
        {
            Level = 0;
            Database = database;
            XRefID = database.GenerateXref("NOTE");
            Text = string.Empty;

            database.Add(XRefID, this);
        }

        /// <summary>
        /// Gets or sets the parsed text. HACK.
        /// </summary>
        public StringBuilder ParsedText { get; set; }

        /// <summary>
        /// Gets the type of the record.
        /// </summary>
        /// <value>
        /// The type of the record.
        /// </value>
        public override GedcomRecordType RecordType
        {
            get { return GedcomRecordType.Note; }
        }

        /// <summary>
        /// Gets the GEDCOM tag for a note record.
        /// </summary>
        /// <value>
        /// The GEDCOM tag.
        /// </value>
        public override string GedcomTag
        {
            get { return "NOTE"; }
        }

        /// <summary>
        /// Gets or sets the text.
        /// </summary>
        /// <value>
        /// The text.
        /// </value>
        public string Text
        {
            get
            {
                return text;
            }
            set
            {
                if (value != text)
                {
                    text = value;
                    Changed();
                }
            }
        }

        /// <summary>
        /// Generates the XML.
        /// </summary>
        /// <param name="root">The root node.</param>
        public override void GenerateXML(XmlNode root)
        {
            XmlDocument doc = root.OwnerDocument;

            XmlNode node = doc.CreateElement("Note");

            XmlCDataSection data = doc.CreateCDataSection(Text);
            node.AppendChild(data);

            root.AppendChild(node);
        }

        /// <summary>
        /// Outputs this instance as a GEDCOM record.
        /// </summary>
        /// <param name="sw">The writer to output to.</param>
        public override void Output(TextWriter sw)
        {
            sw.Write(Environment.NewLine);
            sw.Write(Level.ToString());
            sw.Write(" ");

            if (!string.IsNullOrEmpty(XrefId))
            {
                sw.Write("@");
                sw.Write(XrefId);
                sw.Write("@ ");
            }

            sw.Write("NOTE ");

            if (!string.IsNullOrEmpty(Text))
            {
                Util.SplitLineText(sw, Text, Level, 248);
            }

            OutputStandard(sw);
        }

        /// <summary>
        /// Compare the user entered data against the passed instance for similarity.
        /// </summary>
        /// <param name="obj">The object to compare this instance against.</param>
        /// <returns>True if instance matches user data, otherwise false.</returns>
        public override bool IsEquivalentTo(object obj)
        {
            var note = obj as GedcomNoteRecord;

            if (note == null)
            {
                return false;
            }

            if (!Equals(Text, note.Text))
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Compare the user entered data against the passed instance for similarity.
        /// </summary>
        /// <param name="other">The GedcomNoteRecord to compare this instance against.</param>
        /// <returns>True if instance matches user data, otherwise false.</returns>
        public bool Equals(GedcomNoteRecord other)
        {
            return IsEquivalentTo(other);
        }
    }
}