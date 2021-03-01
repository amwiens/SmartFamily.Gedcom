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
    /// <seealso cref="GedcomRecord"/>
    public class GedcomNoteRecord : GedcomRecord, IEquatable<GedcomNoteRecord>
    {
        private string _text;

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
        public override GedcomRecordType RecordType
        {
            get => GedcomRecordType.Note;
        }

        /// <summary>
        /// Gets the GEDCOM tag for a note record.
        /// </summary>
        public override string GedcomTag
        {
            get => "NOTE";
        }

        /// <summary>
        /// Gets or sets the text.
        /// </summary>
        public string Text
        {
            get => _text;
            set
            {
                if (value != _text)
                {
                    _text = value;
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
        /// Output GEDCOM formatted text representing the note record.
        /// </summary>
        /// <param name="tw">The writer to output to.</param>
        public override void Output(TextWriter tw)
        {
            tw.Write(Environment.NewLine);
            tw.Write(Level.ToString());
            tw.Write(" ");

            if (!string.IsNullOrEmpty(XrefId))
            {
                tw.Write("@");
                tw.Write(XrefId);
                tw.Write("@ ");
            }

            tw.Write("NOTE ");

            if (!string.IsNullOrEmpty(Text))
            {
                Util.SplitLineText(tw, Text, Level, 248);
            }

            OutputStandard(tw);
        }

        /// <summary>
        /// Compare the user entered data against the passed instance for similarity.
        /// </summary>
        /// <param name="obj">The object to compare this instance against.</param>
        /// <returns><c>True</c> if instance matches user data, otherwise <c>false</c>.</returns>
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
        /// <returns><c>True</c> if instance matches user data, otherwise <c>false</c>.</returns>
        public bool Equals(GedcomNoteRecord other)
        {
            return IsEquivalentTo(other);
        }
    }
}