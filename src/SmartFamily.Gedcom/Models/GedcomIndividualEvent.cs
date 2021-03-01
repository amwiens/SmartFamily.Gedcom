#define XML_NODE_UNDEFINED

using SmartFamily.Gedcom.Enums;

using System;
using System.IO;

#if !XML_NODE_UNDEFINED
using System.Xml;
#endif

namespace SmartFamily.Gedcom.Models
{
    /// <summary>
    /// An event relating to a given individual.
    /// </summary>
    /// <seealso cref="GedcomEvent"/>
    public class GedcomIndividualEvent : GedcomEvent
    {
        private GedcomAge _age;
        private string _famc;

        private GedcomAdoptionType adoptedBy;

        /// <summary>
        /// Initializes a new instance of the <see cref="GedcomIndividualEvent"/> class.
        /// </summary>
        public GedcomIndividualEvent()
        {
        }

        /// <summary>
        /// Gets the type of the record.
        /// </summary>
        public override GedcomRecordType RecordType
        {
            get => GedcomRecordType.IndividualEvent;
        }

        /// <summary>
        /// Gets or sets the age.
        /// </summary>
        public GedcomAge Age
        {
            get => _age;
            set
            {
                if (value != _age)
                {
                    _age = value;
                }
            }
        }

        /// <summary>
        /// Gets or sets the family in which an individual appears as a child.
        /// </summary>
        public string Famc
        {
            get => _famc;
            set
            {
                if (value != _famc)
                {
                    _famc = value;
                    Changed();
                }
            }
        }

        /// <summary>
        /// Gets or sets the adoption type.
        /// </summary>
        public GedcomAdoptionType AdoptedBy
        {
            get => adoptedBy;
            set
            {
                if (value != adoptedBy)
                {
                    adoptedBy = value;
                    Changed();
                }
            }
        }

        // utility backpointer to the individual for this event

        /// <summary>
        /// Gets or sets the individual's record.
        /// </summary>
        /// <exception cref="Exception">Must set a GedcomIndividualRecord on a GedcomIndividualEvent.</exception>
        public GedcomIndividualRecord IndiRecord
        {
            get => (GedcomIndividualRecord)Record;
            set
            {
                if (value != Record)
                {
                    Record = value;
                    if (Record != null)
                    {
                        if (Record.RecordType != GedcomRecordType.Individual)
                        {
                            throw new Exception("Must set a GedcomIndividualRecord on a GedcomIndividualEvent");
                        }

                        Database = Record.Database;
                    }
                    else
                    {
                        Database = null;
                    }

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
                if (_age != null)
                {
                    childChangeDate = _age.ChangeDate;
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

#if !XML_NODE_UNDEFINED
        /// <summary>
        /// Generates the pers information XML.
        /// </summary>
        /// <param name="root">The root node.</param>
        public void GeneratePersInfoXML(XmlNode root)
        {
            XmlDocument doc = root.OwnerDocument;

            XmlNode node;
            XmlAttribute attr;

            XmlNode persInfoNode = doc.CreateElement("PersInfo");
            attr = doc.CreateAttribute("Type");

            string type = string.Empty;
            if (EventType == GedcomEventType.GenericEvent ||
                EventType == GedcomEventType.GenericFact)
            {
                type = EventName;
            }
            else
            {
                type = TypeToReadable(EventType);
            }

            attr.Value = type;
            persInfoNode.Attributes.Apend(attr);

            if (!string.IsNullOrEmpty(Classification))
            {
                node = doc.CreateElement("Information");
                node.AppendChild(doc.CreateTextNode(Classification));
                persInfoNode.AppendChild(node);
            }

            if (Date != null)
            {
                node = doc.CreateElement("Date");
                node.AppendChild(doc.CreateTextNode(Date.DateString));
                persInfoNode.AppendChild(node);
            }

            if (Place != null)
            {
                node = doc.CreateElement("Place");
                node.AppendChild(doc.CreateTextNode(Place.Name));
                persInfoNode.AppendChild(node);
            }

            root.AppendChild(persInfoNode);
        }
#endif

        /// <summary>
        /// Output GEDCOM formatted text representing the individual event.
        /// </summary>
        /// <param name="tw">The writer to output to.</param>
        public override void Output(TextWriter tw)
        {
            base.Output(tw);

            if (Age != null)
            {
                Age.Output(tw, Level + 1);
            }
        }
    }
}