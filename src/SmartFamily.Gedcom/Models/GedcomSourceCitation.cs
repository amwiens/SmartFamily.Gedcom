using SmartFamily.Gedcom.Enums;

using System;
using System.IO;
using System.Text;
using System.Xml;

namespace SmartFamily.Gedcom.Models
{
    /// <summary>
    /// TODO: Doc
    /// </summary>
    /// <seealso cref="GedcomRecord"/>
    public class GedcomSourceCitation : GedcomRecord, IEquatable<GedcomSourceCitation>, IComparable<GedcomSourceCitation>, IComparable
    {
        private string _source;

        // source citation fields
        private string _page;

        private string _eventType;
        private string _role;
        private GedcomCertainty _certainty = GedcomCertainty.Unknown;

        private GedcomDate _date;
        private string _text;

        /// <summary>
        /// Initializes a new instance of the <see cref="GedcomSourceCitation"/> class.
        /// </summary>
        public GedcomSourceCitation()
        {
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
            get => GedcomRecordType.SourceCitation;
        }

        /// <summary>
        /// Gets the GEDCOM tag for a source citation.
        /// </summary>
        public override string GedcomTag => "SOUR";

        /// <summary>
        /// Gets or sets the source.
        /// </summary>
        public string Source
        {
            get => _source;
            set
            {
                if (value != _source)
                {
                    _source = value;
                    Changed();
                }
            }
        }

        /// <summary>
        /// Gets or sets the page.
        /// </summary>
        public string Page
        {
            get => _page;
            set
            {
                if (value != _page)
                {
                    _page = value;
                    Changed();
                }
            }
        }

        /// <summary>
        /// Gets or sets the type of the event.
        /// </summary>
        public string EventType
        {
            get => _eventType;
            set
            {
                if (value != _eventType)
                {
                    _eventType = value;
                    Changed();
                }
            }
        }

        /// <summary>
        /// Gets or sets the role.
        /// </summary>
        public string Role
        {
            get => _role;
            set
            {
                if (value != _role)
                {
                    _role = value;
                    Changed();
                }
            }
        }

        /// <summary>
        /// Gets or sets the certainty.
        /// </summary>
        public GedcomCertainty Certainty
        {
            get => _certainty;
            set
            {
                if (value != _certainty)
                {
                    _certainty = value;
                    Changed();
                }
            }
        }

        /// <summary>
        /// Gets or sets the date.
        /// </summary>
        public GedcomDate Date
        {
            get => _date;
            set
            {
                if (value != _date)
                {
                    _date = value;
                    Changed();
                }
            }
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
        /// Deletes this instance.
        /// </summary>
        public override void Delete()
        {
            base.Delete();

            GedcomSourceRecord source = (GedcomSourceRecord)Database[Source];

            source.Citations.Remove(this);

            source.Delete();
        }

        /// <summary>
        /// Generates the XML.
        /// </summary>
        /// <param name="root">The root node.</param>
        public override void GenerateXML(XmlNode root)
        {
            XmlDocument doc = root.OwnerDocument;

            XmlNode node = doc.CreateElement("Citation");
            XmlAttribute attr;

            if (!string.IsNullOrEmpty(Source))
            {
                GedcomSourceRecord source = Database[Source] as GedcomSourceRecord;
                if (source != null)
                {
                    XmlNode sourceNode = doc.CreateElement("Source");

                    XmlNode linkNode = doc.CreateElement("Link");

                    attr = doc.CreateAttribute("Target");
                    attr.Value = "SourceRec";
                    linkNode.Attributes.Append(attr);

                    attr = doc.CreateAttribute("Ref");
                    attr.Value = Source;
                    linkNode.Attributes.Append(attr);

                    sourceNode.AppendChild(linkNode);

                    node.AppendChild(sourceNode);
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine("Pointer to non existent source");
                }

                if (!string.IsNullOrEmpty(Page))
                {
                    XmlNode whereNode = doc.CreateElement("WhereInSource");
                    whereNode.AppendChild(doc.CreateTextNode(Page));

                    node.AppendChild(whereNode);
                }

                if (Date != null)
                {
                    XmlNode whenNode = doc.CreateElement("WhenRecorded");
                    whenNode.AppendChild(doc.CreateTextNode(Date.DateString));
                }

                // TODO: output source citation fields
                //   Caption,     element
                //   Extract,     element
                GenerateNoteXML(node);
            }

            root.AppendChild(node);
        }

        /// <summary>
        /// Output GEDCOM formatted text representing the source citation.
        /// </summary>
        /// <param name="tw">The writer to output to.</param>
        public override void Output(TextWriter tw)
        {
            tw.Write(Environment.NewLine);
            tw.Write(Level.ToString());
            tw.Write(" SOUR ");

            // should always have a Source, but check anyway
            if (!string.IsNullOrEmpty(Source))
            {
                tw.Write("@");
                tw.Write(Source);
                tw.Write("@");
            }

            OutputStandard(tw);

            string levelPlusOne = null;
            string levelPlusTwo = null;

            if (!string.IsNullOrEmpty(Page))
            {
                if (levelPlusOne == null)
                {
                    levelPlusOne = (Level + 1).ToString();
                }

                tw.Write(Environment.NewLine);
                tw.Write(levelPlusOne);
                tw.Write(" PAGE ");
                string line = Page.Replace("@", "@@");
                tw.Write(line);
            }

            if (!string.IsNullOrEmpty(EventType))
            {
                if (levelPlusOne == null)
                {
                    levelPlusOne = (Level + 1).ToString();
                }

                tw.Write(Environment.NewLine);
                tw.Write(levelPlusOne);
                tw.Write(" EVEN ");
                string line = EventType.Replace("@", "@@");
                tw.Write(line);

                if (!string.IsNullOrEmpty(Role))
                {
                    if (levelPlusTwo == null)
                    {
                        levelPlusTwo = (Level + 2).ToString();
                    }

                    tw.Write(Environment.NewLine);
                    tw.Write(levelPlusTwo);
                    tw.Write(" ROLE ");
                    line = Role.Replace("@", "@@");
                    tw.Write(line);
                }
            }

            if (Date != null || !string.IsNullOrEmpty(Text))
            {
                if (levelPlusOne == null)
                {
                    levelPlusOne = (Level + 1).ToString();
                }

                tw.Write(Environment.NewLine);
                tw.Write(levelPlusOne);
                tw.Write(" DATA ");

                if (Date != null)
                {
                    Date.Output(tw);
                }

                if (!string.IsNullOrEmpty(Text))
                {
                    if (levelPlusTwo == null)
                    {
                        levelPlusTwo = (Level + 2).ToString();
                    }

                    tw.Write(Environment.NewLine);
                    tw.Write(levelPlusTwo);
                    tw.Write(" TEXT ");

                    Util.SplitLineText(tw, Text, Level + 2, 248);
                }
            }

            if (Certainty != GedcomCertainty.Unknown)
            {
                if (levelPlusOne == null)
                {
                    levelPlusOne = (Level + 1).ToString();
                }

                tw.Write(Environment.NewLine);
                tw.Write(levelPlusOne);
                tw.Write(" QUAY ");
                tw.Write(((int)Certainty).ToString());
            }
        }

        /// <summary>
        /// Compare the user entered data against the passed instance for similarity.
        /// </summary>
        /// <param name="obj">The object to compare this instance against.</param>
        /// <returns><c>True</c> if instance matches user data, otherwise <c>false</c>.</returns>
        public override bool IsEquivalentTo(object obj)
        {
            return CompareTo(obj as GedcomSourceCitation) == 0;
        }

        /// <summary>
        /// Compare the user entered data against the passed instance for similarity.
        /// </summary>
        /// <param name="other">The GedcomSourceCitation to compare this instance against.</param>
        /// <returns><c>True</c> if instance matches user data, otherwise <c>false</c>.</returns>
        public bool Equals(GedcomSourceCitation other)
        {
            return IsEquivalentTo(other);
        }

        /// <summary>
        /// Compare the user entered data against the passed instance for similarity.
        /// </summary>
        /// <param name="obj">The object to compare this instance against.</param>
        /// <returns><c>True</c> if instance matches user data, otherwise <c>false</c>.</returns>
        public override bool Equals(object obj)
        {
            return IsEquivalentTo(obj);
        }

        /// <summary>
        /// Compares another source citation to the current instance.
        /// </summary>
        /// <param name="citation">A citation.</param>
        /// <returns>
        /// &lt;0 if this citation precedes the other in the sort order;
        /// &gt;0 if the other citation precedes this one;
        /// 0 if the citations are equal
        /// </returns>
        public int CompareTo(GedcomSourceCitation citation)
        {
            if (citation == null)
            {
                return 1;
            }

            var compare = Certainty.CompareTo(citation.Certainty);
            if (compare != 0)
            {
                return compare;
            }

            compare = GedcomGenericComparer.SafeCompareOrder(Date, citation.Date);
            if (compare != 0)
            {
                return compare;
            }

            compare = GedcomGenericComparer.SafeCompareOrder(EventType, citation.EventType);
            if (compare != 0)
            {
                return compare;
            }

            compare = GedcomGenericComparer.SafeCompareOrder(Page, citation.Page);
            if (compare != 0)
            {
                return compare;
            }

            compare = GedcomGenericComparer.SafeCompareOrder(Role, citation.Role);
            if (compare != 0)
            {
                return compare;
            }

            compare = GedcomGenericComparer.SafeCompareOrder(Text, citation.Text);
            if (compare != 0)
            {
                return compare;
            }

            return compare;
        }

        /// <summary>
        /// Compares another object to the current instance.
        /// </summary>
        /// <param name="obj">A citation.</param>
        /// <returns>
        /// &lt;0 if this object precedes the other in the sort order;
        /// &gt;0 if the other object precedes this one;
        /// 0 if the objects are equal
        /// </returns>
        public int CompareTo(object obj)
        {
            return CompareTo(obj as GedcomSourceCitation);
        }

        public override int GetHashCode()
        {
            return new
            {
                Certainty,
                Date,
                EventType,
                Page,
                Role,
                Text,
            }.GetHashCode();
        }
    }
}