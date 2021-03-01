using SmartFamily.Gedcom.Enums;

using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;

namespace SmartFamily.Gedcom.Models
{
    /// <summary>
    /// Defines a generic event or fact.
    /// </summary>
    /// <seealso cref="GedcomRecord"/>
    public class GedcomEvent : GedcomRecord, IComparable, IComparable<GedcomEvent>, IEquatable<GedcomEvent>
    {
        private static readonly string[] _typeStrings = new string[]
        {
            "EVEN",

            // Family Events
            "ANUL",
            "CENS",
            "DIV",
            "DIVF",
            "ENGA",
            "MARB",
            "MARC",
            "MARR",
            "MARL",
            "MARS",
            "RESI",

            // Individual Events
            "BIRT",
            "CHR",
            "DEAT",
            "BURI",
            "CREM",
            "ADOP",
            "BAPM",
            "BARM",
            "BASM",
            "BLES",
            "CHRA",
            "CONF",
            "FCOM",
            "ORDN",
            "NATU",
            "EMIG",
            "IMMI",
            "CENS",
            "PROB",
            "WILL",
            "GRAD",
            "RETI",

            // Facts
            "FACT",
            "CAST",
            "DSCR",
            "EDUC",
            "IDNO",
            "NATI",
            "NCHI",
            "NMR",
            "OCCU",
            "PROP",
            "RELI",
            "RESI",
            "SSN",
            "TITL",

            // GEDCOM allows custom records, beginning with _
            "_UNKN",
        };

        private static readonly List<string> typeDescriptions = new List<string>()
        {
            "Other Event",
            "Annulment",
            "Census",
            "Divorce",
            "Divorce Filed",
            "Engagement",
            "Marriage Bann",
            "Marriage Contract",
            "Marriage",
            "Marriage License",
            "Marriage Settlement",
            "Residence",
            "Birth",
            "Christening",
            "Death",
            "Burial",
            "Cremation",
            "Adoption",
            "Baptism",
            "Bar Mitzvah",
            "Bas Mitzvah",
            "Blessing",
            "Adult Christening",
            "Confirmation",
            "First Communion",
            "Ordination",
            "Naturalization",
            "Emigration",
            "Immigration",
            "Census",
            "Probate",
            "Will",
            "Graduation",
            "Retirement",
            "Other Fact",
            "Caste",
            "Physical Description",
            "Education",
            "Identification Number",
            "Nationality",
            "Number of Children",
            "Number of Marriages",
            "Occupation",
            "Property",
            "Religion",
            "Residence",
            "Social Security Number",
            "Title",
            "Custom"
        };

        /// <summary>
        /// The GEDCOM event type
        /// </summary>
        private GedcomEventType _eventType;

        /// <summary>
        /// The classification
        /// </summary>
        private string _classification;

        /// <summary>
        /// The certainty
        /// </summary>
        private GedcomCertainty _certainty = GedcomCertainty.Unknown;

        /// <summary>
        /// The record
        /// </summary>
        private GedcomRecord _record;

        /// <summary>
        /// Used for Gedcom 6 XML output
        /// </summary>
        private string _eventXRefID;

        /// <summary>
        /// The event name
        /// </summary>
        private string _eventName;

        /// <summary>
        /// The date
        /// </summary>
        private GedcomDate _date;

        /// <summary>
        /// The place
        /// </summary>
        private GedcomPlace _place;

        /// <summary>
        /// The address
        /// </summary>
        private GedcomAddress _address;

        /// <summary>
        /// The responsible agency
        /// </summary>
        private string _responsibleAgency;

        /// <summary>
        /// The religious affiliation
        /// </summary>
        private string _religiousAffiliation;

        /// <summary>
        /// The cause
        /// </summary>
        private string _cause;

        /// <summary>
        /// Initializes a new instance of the <see cref="GedcomEvent"/> class.
        /// </summary>
        public GedcomEvent()
        {
            // default event type is generic, need to set event name
            // or it will not be set if the record actually is a generic event
        }

        /// <summary>
        /// Gets the type of the record.
        /// </summary>
        public override GedcomRecordType RecordType
        {
            get => GedcomRecordType.Event;
        }

        /// <summary>
        /// Gets the gedcom tag.
        /// </summary>
        public override string GedcomTag
        {
            get => GedcomEvent.TypeToTag(EventType);
        }

        /// <summary>
        /// Gets or sets the type of the event.
        /// </summary>
        public GedcomEventType EventType
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
        /// Gets or sets the name of the event.
        /// </summary>
        public string EventName
        {
            get => _eventName;
            set
            {
                if (value != _eventName)
                {
                    _eventName = value;
                    Changed();
                }
            }
        }

        /// <summary>
        /// Gets or sets the classification.
        /// </summary>
        public string Classification
        {
            get => _classification;
            set
            {
                if (value != _classification)
                {
                    _classification = value;
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
        /// Gets or sets the place.
        /// </summary>
        public GedcomPlace Place
        {
            get => _place;
            set
            {
                if (value != _place)
                {
                    _place = value;
                    Changed();
                }
            }
        }

        /// <summary>
        /// Gets or sets the address.
        /// </summary>
        public GedcomAddress Address
        {
            get => _address;
            set
            {
                if (value != _address)
                {
                    _address = value;
                    Changed();
                }
            }
        }

        /// <summary>
        /// Gets or sets the responsible agency.
        /// </summary>
        public string ResponsibleAgency
        {
            get => _responsibleAgency;
            set
            {
                if (value != _responsibleAgency)
                {
                    _responsibleAgency = value;
                    Changed();
                }
            }
        }

        /// <summary>
        /// Gets or sets the religious affiliation.
        /// </summary>
        public string ReligiousAffiliation
        {
            get => _religiousAffiliation;
            set
            {
                if (value != _religiousAffiliation)
                {
                    _religiousAffiliation = value;
                    Changed();
                }
            }
        }

        /// <summary>
        /// Gets or sets the cause.
        /// </summary>
        public string Cause
        {
            get => _cause;
            set
            {
                if (value != _cause)
                {
                    _cause = value;
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
        /// Gets or sets the event x reference identifier.
        /// </summary>
        public string EventXRefID
        {
            get => _eventXRefID;
            set
            {
                if (value != _eventXRefID)
                {
                    _eventXRefID = value;
                    Changed();
                }
            }
        }

        /// <summary>
        /// Gets or sets the record.
        /// </summary>
        public GedcomRecord Record
        {
            get => _record;
            set
            {
                if (value != _record)
                {
                    _record = value;
                    Changed();
                }
            }
        }

        /// <summary>
        /// Gets or sets the change date.
        /// </summary>
        public override GedcomChangeDate ChangeDate
        {
            get
            {
                GedcomChangeDate realChangeDate = base.ChangeDate;
                GedcomChangeDate childChangeDate;
                if (Address != null)
                {
                    childChangeDate = Address.ChangeDate;
                    if (childChangeDate != null && realChangeDate != null && childChangeDate > realChangeDate)
                    {
                        realChangeDate = childChangeDate;
                    }
                }

                if (Place != null)
                {
                    childChangeDate = Place.ChangeDate;
                    if (childChangeDate != null && realChangeDate != null && childChangeDate > realChangeDate)
                    {
                        realChangeDate = childChangeDate;
                    }
                }

                if (Date != null)
                {
                    childChangeDate = Date.ChangeDate;
                    if (childChangeDate != null && realChangeDate != null && childChangeDate > realChangeDate)
                    {
                        realChangeDate = childChangeDate;
                    }
                }

                if (realChangeDate != null)
                {
                    realChangeDate.Level = Level + 2;
                }

                return realChangeDate;
            }
            set => base.ChangeDate = value;
        }

        /// <summary>
        /// Get the user-friendly textual description for a GedcomEventType.
        /// </summary>
        /// <param name="eventType">A GEDCOM event type.</param>
        /// <returns>The textual description for a given GedcomEventType.</returns>
        public static string TypeToReadable(GedcomEventType eventType)
        {
            return typeDescriptions[(int)eventType];
        }

        /// <summary>
        /// Get the tag for a GedcomEventType.
        /// </summary>
        /// <param name="eventType">A GEDCOM event type.</param>
        /// <returns>The tag for a given GedcomEventType.</returns>
        public static string TypeToTag(GedcomEventType eventType)
        {
            return _typeStrings[(int)eventType];
        }

        /// <summary>
        /// Attempts to determine a standard event type from a textual
        /// description. Always returns GenericEvent if one can't be found
        /// even though where the string came from maybe a FACT
        /// </summary>
        /// <param name="readable">The type name as a string.</param>
        /// <returns>A GedcomEventType matching the textual description, or GenericEvent if no match was found.</returns>
        public static GedcomEventType ReadableToType(string readable)
        {
            GedcomEventType ret = GedcomEventType.GenericEvent;

            int i = typeDescriptions.IndexOf(readable);
            if (i != -1)
            {
                ret = (GedcomEventType)i;
            }

            return ret;
        }

        /// <summary>
        /// Deletes this instance.
        /// </summary>
        public override void Delete()
        {
            base.Delete();

            if (_date != null)
            {
                _date.Delete();
            }

            if (_place != null)
            {
                _place.Delete();
            }
        }

        /// <summary>
        /// Compares two events to see if the date and place are the same.
        /// </summary>
        /// <param name="obj">The event instance to compare against.</param>
        /// <returns>Relative position in the sort order.</returns>
        public int CompareTo(object obj)
        {
            return CompareTo(obj as GedcomEvent);
        }

        /// <summary>
        /// Compares two events to see if the date and place are the same.
        /// </summary>
        /// <param name="eventToCompare">The event instance to compare against.</param>
        /// <returns>Relative position in the sort order.</returns>
        public int CompareTo(GedcomEvent eventToCompare)
        {
            if (eventToCompare == null)
            {
                return -1;
            }

            if (eventToCompare.Date == null && Date != null)
            {
                return -1;
            }

            if (Date == null && eventToCompare.Date != null)
            {
                return -1;
            }

            var compare = GedcomDate.CompareByDate(Date, eventToCompare.Date);
            if (compare != 0)
            {
                return compare;
            }

            return string.Compare(_eventName, eventToCompare.EventName);
        }

        /// <summary>
        /// Returns a percentage based score on how similar the passed record is to the current instance.
        /// </summary>
        /// <param name="ev">The event to compare against this instance.</param>
        /// <returns>A score from 0 to 100 representing the percentage match.</returns>
        public decimal CalculateSimilarityScore(GedcomEvent ev)
        {
            var match = decimal.Zero;

            if (ev.EventType == EventType)
            {
                // match date
                var dateMatch = decimal.Zero;
                if (Date == null && ev.Date != null)
                {
                    dateMatch = 100m;
                }
                else if (Date != null && ev.Date != null)
                {
                    dateMatch = Date.CalculateSimilarityScore(ev.Date);
                }

                // match location
                var locMatch = decimal.Zero;
                if (Place == null && ev.Place == null)
                {
                    locMatch = 100m;
                }
                else if (Place != null && ev.Place != null)
                {
                    if (Place.Name == ev.Place.Name)
                    {
                        locMatch = 100m;
                    }
                }

                match = (dateMatch + locMatch) / 2m;
            }

            return match;
        }

        /// <summary>
        /// Generates the XML.
        /// </summary>
        /// <param name="root">The root.</param>
        public override void GenerateXML(XmlNode root)
        {
            XmlDocument doc = root.OwnerDocument;

            XmlNode node;
            XmlAttribute attr;

            XmlNode eventNode = doc.CreateElement("EventRec");
            attr = doc.CreateAttribute("Id");
            attr.Value = EventXRefID;
            eventNode.Attributes.Append(attr);

            attr = doc.CreateAttribute("Type");
            attr.Value = GedcomEvent.TypeToReadable(EventType);
            eventNode.Attributes.Append(attr);

            // TODO: VitalType attribute
            // (marriage | befmarriage | aftmarriage |
            // birth | befbirth | aftbirth |
            // death | befdeath | aftdeath)
            if (RecordType == GedcomRecordType.FamilyEvent)
            {
                GedcomFamilyEvent famEvent = this as GedcomFamilyEvent;
                GedcomFamilyRecord family = famEvent.FamRecord;

                // TODO: <Participant>s
                // probably not right, but always stick husband/wife in as
                // participants
                bool added = false;

                if (!string.IsNullOrEmpty(family.Husband))
                {
                    GedcomIndividualRecord husb = Database[family.Husband] as GedcomIndividualRecord;
                    if (husb != null)
                    {
                        node = doc.CreateElement("Participant");

                        XmlNode linkNode = doc.CreateElement("Link");

                        attr = doc.CreateAttribute("Target");
                        attr.Value = "IndividualRec";
                        linkNode.Attributes.Append(attr);

                        attr = doc.CreateAttribute("Ref");
                        attr.Value = family.Husband;
                        linkNode.Attributes.Append(attr);

                        node.AppendChild(linkNode);

                        eventNode.AppendChild(node);
                        added = true;
                    }
                    else
                    {
                        System.Diagnostics.Debug.WriteLine("Pointer to non existent husband");
                    }
                }

                if (!string.IsNullOrEmpty(family.Wife))
                {
                    GedcomIndividualRecord wife = Database[family.Wife] as GedcomIndividualRecord;
                    if (wife != null)
                    {
                        node = doc.CreateElement("Participant");

                        XmlNode linkNode = doc.CreateElement("Link");

                        attr = doc.CreateAttribute("Target");
                        attr.Value = "IndividualRec";
                        linkNode.Attributes.Append(attr);

                        attr = doc.CreateAttribute("Ref");
                        attr.Value = family.Wife;
                        linkNode.Attributes.Append(attr);

                        node.AppendChild(linkNode);

                        eventNode.AppendChild(node);
                        added = true;
                    }
                    else
                    {
                        System.Diagnostics.Debug.WriteLine("Pointer to non existent wife");
                    }
                }

                if (!added)
                {
                    // TODO: no husband or wife now what? XML will be invalid
                    // without a participant
                }
            }
            else
            {
                GedcomIndividualRecord indi = (this as GedcomIndividualEvent).IndiRecord;

                node = doc.CreateElement("Participant");

                XmlNode linkNode = doc.CreateElement("Link");

                attr = doc.CreateAttribute("Target");
                attr.Value = "IndividualRec";
                linkNode.Attributes.Append(attr);

                attr = doc.CreateAttribute("Ref");
                attr.Value = indi.XRefID;
                linkNode.Attributes.Append(attr);

                XmlNode roleNode = doc.CreateElement("Role");
                if (this == indi.Birth)
                {
                    roleNode.AppendChild(doc.CreateTextNode("child"));
                }
                else
                {
                    roleNode.AppendChild(doc.CreateTextNode("principle"));
                }

                linkNode.AppendChild(roleNode);

                node.AppendChild(linkNode);

                eventNode.AppendChild(node);
            }

            if (Date != null)
            {
                node = doc.CreateElement("Date");
                node.AppendChild(doc.CreateTextNode(Date.DateString));
                eventNode.AppendChild(node);
            }

            if (Place != null)
            {
                node = doc.CreateElement("Place");
                node.AppendChild(doc.CreateTextNode(Place.Name));
                eventNode.AppendChild(node);
            }

            GenerateNoteXML(eventNode);
            GenerateCitationsXML(eventNode);
            GenerateMultimediaXML(eventNode);

            GenerateChangeDateXML(eventNode);

            root.AppendChild(eventNode);
        }

        /// <summary>
        /// Output GEDCOM formatted text representing the event.
        /// </summary>
        /// <param name="tw">The writer to output to.</param>
        public override void Output(TextWriter tw)
        {
            tw.Write(Environment.NewLine);
            tw.Write(Level.ToString());
            tw.Write(" ");

            tw.Write(GedcomTag);

            if (!string.IsNullOrEmpty(_eventName))
            {
                tw.Write(" ");
                tw.Write(_eventName);
            }

            OutputStandard(tw);

            string levelPlusOne = null;

            if (!string.IsNullOrEmpty(_classification))
            {
                if (levelPlusOne == null)
                {
                    levelPlusOne = (Level + 1).ToString();
                }

                tw.Write(Environment.NewLine);
                tw.Write(levelPlusOne);
                tw.Write(" TYPE ");
                tw.Write(_classification);
            }

            if (_date != null)
            {
                _date.Output(tw);
            }

            if (_place != null)
            {
                _place.Output(tw);
            }

            if (_address != null)
            {
                _address.Output(tw, Level + 1);
            }

            if (!string.IsNullOrEmpty(_responsibleAgency))
            {
                if (levelPlusOne == null)
                {
                    levelPlusOne = (Level + 1).ToString();
                }

                tw.Write(Environment.NewLine);
                tw.Write(levelPlusOne);
                tw.Write(" AGNC ");
                string line = _responsibleAgency.Replace("@", "@@");
                tw.Write(line);
            }

            if (!string.IsNullOrEmpty(_religiousAffiliation))
            {
                if (levelPlusOne == null)
                {
                    levelPlusOne = (Level + 1).ToString();
                }

                tw.Write(Environment.NewLine);
                tw.Write(levelPlusOne);
                tw.Write(" RELI ");
                string line = _religiousAffiliation.Replace("@", "@@");
                tw.Write(line);
            }

            if (!string.IsNullOrEmpty(_cause))
            {
                if (levelPlusOne == null)
                {
                    levelPlusOne = (Level + 1).ToString();
                }

                tw.Write(Environment.NewLine);
                tw.Write(levelPlusOne);
                tw.Write(" CAUS ");
                string line = _cause.Replace("@", "@@");
                tw.Write(line);
            }

            if (RestrictionNotice != GedcomRestrictionNotice.None)
            {
                if (levelPlusOne == null)
                {
                    levelPlusOne = (Level + 1).ToString();
                }

                tw.Write(Environment.NewLine);
                tw.Write(levelPlusOne);
                tw.Write(" RESN ");
                tw.Write(RestrictionNotice.ToString());
            }

            // Quality of data should only be on source citations according to
            // the spec.
            // We output it on events as well as it has been seen in GEDCOM
            // files from other apps.
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
            var eventRecord = obj as GedcomEvent;

            if (eventRecord == null)
            {
                return false;
            }

            if (!GedcomGenericComparer.SafeEqualityCheck(Address, eventRecord.Address))
            {
                return false;
            }

            if (!Equals(Cause, eventRecord.Cause))
            {
                return false;
            }

            if (!Equals(Certainty, eventRecord.Certainty))
            {
                return false;
            }

            if (!Equals(Classification, eventRecord.Classification))
            {
                return false;
            }

            if (!Equals(Date, eventRecord.Date))
            {
                return false;
            }

            if (!Equals(EventName, eventRecord.EventName))
            {
                return false;
            }

            if (!Equals(EventType, eventRecord.EventType))
            {
                return false;
            }

            if (!Equals(Place, eventRecord.Place))
            {
                return false;
            }

            if (!Equals(ReligiousAffiliation, eventRecord.ReligiousAffiliation))
            {
                return false;
            }

            if (!Equals(ResponsibleAgency, eventRecord.ResponsibleAgency))
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Compare the user entered data against the passed instance for similarity.
        /// </summary>
        /// <param name="other">The GedcomEvent to compare this instance against.</param>
        /// <returns><c>True</c> if instance matches user data, otherwise <c>False</c>.</returns>
        public bool Equals(GedcomEvent other)
        {
            return IsEquivalentTo(other);
        }
    }
}