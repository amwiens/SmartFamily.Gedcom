using SmartFamily.Gedcom.Enums;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;

namespace SmartFamily.Gedcom.Models
{
    /// <summary>
    /// Defines a family, consisting of husband/wife and children, and
    /// family events.
    /// </summary>
    public class GedcomFamilyRecord : GedcomRecord, IEquatable<GedcomFamilyRecord>
    {
        private readonly GedcomRecordList<GedcomFamilyEvent> _events;

        private string _husband;
        private string _wife;

        private readonly GedcomRecordList<string> _children;

        // not just _Children.Count, may be unknown children
        private int _numberOfChildren;

        private GedcomRecordList<string> _submitterRecords;

        private GedcomSpouseSealingRecord _spouseSealing;

        private MarriageStartStatus _startStatus;

        // only used during parsing
        private Dictionary<string, PedigreeLinkageType> _linkageTypes;

        private Dictionary<string, PedigreeLinkageType> _husbLinkageTypes;
        private Dictionary<string, PedigreeLinkageType> _wifeLinkageTypes;

        /// <summary>
        /// Initializes a new instance of the <see cref="GedcomFamilyRecord"/> class.
        /// </summary>
        public GedcomFamilyRecord()
        {
            _events = new GedcomRecordList<GedcomFamilyEvent>();
            _events.CollectionChanged += ListChanged;
            _children = new GedcomRecordList<string>();
            _children.CollectionChanged += ListChanged;

            _startStatus = MarriageStartStatus.Unknown;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GedcomFamilyRecord" /> class.
        /// </summary>
        /// <param name="database">The database to associate with this record.</param>
        /// <param name="indi1">The first individual.</param>
        /// <param name="indi2">The second individual.</param>
        public GedcomFamilyRecord(GedcomDatabase database, GedcomIndividualRecord indi1, GedcomIndividualRecord indi2)
            : this()
        {
            Level = 0;
            Database = database;
            XRefID = database.GenerateXref("FAM");

            if (indi1 != null)
            {
                GedcomFamilyLink link = new GedcomFamilyLink
                {
                    Database = database,
                    Family = XRefID,
                    Individual = indi1.XRefID
                };
                indi1.SpouseIn.Add(link);

                if (indi2 != null)
                {
                    link = new GedcomFamilyLink
                    {
                        Database = database,
                        Family = XRefID,
                        Individual = indi2.XRefID
                    };
                    indi2.SpouseIn.Add(link);
                }

                switch (indi1.Sex)
                {
                    case GedcomSex.Female:
                        Wife = indi1.XRefID;
                        if (indi2 != null)
                        {
                            Husband = indi2.XRefID;
                        }

                        break;

                    default:
                        // got to put some where if not male or female,
                        // go with same as male
                        Husband = indi1.XRefID;
                        if (indi2 != null)
                        {
                            Wife = indi2.XRefID;
                        }

                        break;
                }
            }

            database.Add(XRefID, this);
        }

        /// <summary>
        /// Gets the type of the record.
        /// </summary>
        /// <value>
        /// The type of the record.
        /// </value>
        public override GedcomRecordType RecordType
        {
            get => GedcomRecordType.Family;
        }

        /// <summary>
        /// Gets the gedcom tag for a family record.
        /// </summary>
        /// <value>
        /// The gedcom tag for a family record.
        /// </value>
        public override string GedcomTag
        {
            get => "FAM";
        }

        /// <summary>
        /// Gets the family events.
        /// </summary>
        /// <value>
        /// The family events.
        /// </value>
        public GedcomRecordList<GedcomFamilyEvent> Events
        {
            get => _events;
        }

        /// <summary>
        /// Gets or sets the husband.
        /// </summary>
        /// <value>
        /// The husband.
        /// </value>
        public string Husband
        {
            get => _husband;
            set
            {
                if (value != _husband)
                {
                    _husband = value;
                    Changed();
                }
            }
        }

        /// <summary>
        /// Gets or sets the wife.
        /// </summary>
        /// <value>
        /// The wife.
        /// </value>
        public string Wife
        {
            get => _wife;
            set
            {
                if (value != _wife)
                {
                    _wife = value;
                    Changed();
                }
            }
        }

        /// <summary>
        /// Gets the children.
        /// </summary>
        /// <value>
        /// The children.
        /// </value>
        public GedcomRecordList<string> Children
        {
            get => _children;
        }

        /// <summary>
        /// Gets or sets the number of children.
        /// </summary>
        /// <value>
        /// The number of children.
        /// </value>
        public int NumberOfChildren
        {
            get => _numberOfChildren;
            set
            {
                if (value != _numberOfChildren)
                {
                    _numberOfChildren = value;
                    Changed();
                }
            }
        }

        /// <summary>
        /// Gets the submitter records.
        /// </summary>
        /// <value>
        /// The submitter records.
        /// </value>
        public GedcomRecordList<string> SubmitterRecords
        {
            get
            {
                if (_submitterRecords == null)
                {
                    _submitterRecords = new GedcomRecordList<string>();
                    _submitterRecords.CollectionChanged += ListChanged;
                }

                return _submitterRecords;
            }
        }

        // Utility properties to get marriage event

        /// <summary>
        /// Gets the marriage.
        /// </summary>
        /// <value>
        /// The marriage.
        /// </value>
        public GedcomFamilyEvent Marriage
        {
            get
            {
                GedcomFamilyEvent marriage = null;

                foreach (GedcomFamilyEvent e in _events)
                {
                    if (e.EventType == GedcomEventType.MARR)
                    {
                        marriage = e;
                        break;
                    }
                }

                return marriage;
            }
        }

        /// <summary>
        /// Gets or sets the start status.
        /// </summary>
        /// <value>
        /// The start status.
        /// </value>
        public MarriageStartStatus StartStatus
        {
            get => _startStatus;
            set
            {
                if (value != _startStatus)
                {
                    _startStatus = value;
                    Changed();
                }
            }
        }

        /// <summary>
        /// Gets or sets the spousal sealing record for this family or null if one does not exist.
        /// </summary>
        public GedcomSpouseSealingRecord SpouseSealing
        {
            get => _spouseSealing;
            set
            {
                if (value != _spouseSealing)
                {
                    _spouseSealing = value;
                    Changed();
                }
            }
        }

        /// <summary>
        /// Gets or sets the change date.
        /// </summary>
        /// <value>
        /// The change date.
        /// </value>
        public override GedcomChangeDate ChangeDate
        {
            get
            {
                GedcomChangeDate realChangeDate = base.ChangeDate;
                GedcomRecord record;
                GedcomChangeDate childChangeDate;
                foreach (GedcomFamilyEvent famEvent in Events)
                {
                    childChangeDate = famEvent.ChangeDate;
                    if (childChangeDate != null && realChangeDate != null && childChangeDate > realChangeDate)
                    {
                        realChangeDate = childChangeDate;
                    }
                }

                foreach (string submitterID in SubmitterRecords)
                {
                    record = Database[submitterID];
                    childChangeDate = record.ChangeDate;
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
        /// Deletes this instance.
        /// </summary>
        public override void Delete()
        {
            base.Delete();

            if (_events != null && RefCount == 0)
            {
                foreach (GedcomEvent ev in _events)
                {
                    ev.Delete();
                }
            }
        }

        /// <summary>
        /// Add a new family event for a given event type.
        /// </summary>
        /// <param name="type">The event type.</param>
        /// <returns>
        /// The new family event based on the given event type.
        /// </returns>
        public GedcomFamilyEvent AddNewEvent(GedcomEventType type)
        {
            GedcomFamilyEvent familyEvent = new GedcomFamilyEvent
            {
                EventType = type,
                Level = Level + 1,
                FamRecord = this
            };

            Events.Add(familyEvent);

            return familyEvent;
        }

        /// <summary>
        /// Add a child.
        /// </summary>
        /// <param name="indi">The child.</param>
        /// <returns>
        /// Returns True if a new child record is added; otherwise False.
        /// </returns>
        public bool AddChild(GedcomIndividualRecord indi)
        {
            bool added = false;

            if (indi != null && !Children.Contains(indi.XRefID))
            {
                if (string.IsNullOrEmpty(XRefID))
                {
                    XRefID = Database.GenerateXref("FAM");
                    Database.Add(XRefID, this);
                }

                if (!indi.ChildInFamily(XRefID))
                {
                    GedcomFamilyLink link = new GedcomFamilyLink
                    {
                        Database = Database,
                        Family = XRefID,
                        Individual = indi.XRefID,
                        Level = 1
                    };
                    indi.ChildIn.Add(link);
                }

                Children.Add(indi.XRefID);

                added = true;
            }

            return added;
        }

        /// <summary>
        /// Add a new child.
        /// </summary>
        /// <returns>
        /// The child's record.
        /// </returns>
        public GedcomIndividualRecord AddNewChild()
        {
            GedcomIndividualRecord husband = null;
            GedcomIndividualRecord wife = null;

            if (!string.IsNullOrEmpty(this._husband))
            {
                husband = Database[this._husband] as GedcomIndividualRecord;
            }

            if (!string.IsNullOrEmpty(this._wife))
            {
                wife = Database[this._wife] as GedcomIndividualRecord;
            }

            string surname = "unknown";

            if (husband != null)
            {
                GedcomName husbandName = husband.GetName();
                if (husbandName != null)
                {
                    surname = husbandName.Surname;
                }
            }
            else if (wife != null)
            {
                GedcomName wifeName = wife.GetName();
                if (wifeName != null)
                {
                    surname = wifeName.Surname;
                }
            }

            GedcomIndividualRecord indi = new GedcomIndividualRecord(Database, surname);

            // don't care about failure here, won't happen as indi isn't null
            // and they aren't already in the family
            AddChild(indi);

            return indi;
        }

        /// <summary>
        /// Remove a child.
        /// </summary>
        /// <param name="child">The child.</param>
        public void RemoveChild(GedcomIndividualRecord child)
        {
            Children.Remove(child.XRefID);

            if (child.ChildInFamily(XRefID, out GedcomFamilyLink link))
            {
                child.ChildIn.Remove(link);
            }
        }

        /// <summary>
        /// Changes the husband.
        /// </summary>
        /// <param name="indi">The husband.</param>
        public void ChangeHusband(GedcomIndividualRecord indi)
        {
            GedcomIndividualRecord husband = null;
            GedcomIndividualRecord wife = null;

            if (!string.IsNullOrEmpty(this._husband))
            {
                husband = Database[this._husband] as GedcomIndividualRecord;
            }

            if (!string.IsNullOrEmpty(this._wife))
            {
                wife = Database[this._wife] as GedcomIndividualRecord;
            }

            if (string.IsNullOrEmpty(XRefID))
            {
                XRefID = Database.GenerateXref("FAM");
                Database.Add(XRefID, this);
            }

            if (husband != null)
            {
                if (husband.SpouseInFamily(XRefID, out GedcomFamilyLink link))
                {
                    husband.SpouseIn.Remove(link);
                }
            }

            husband = indi;
            this._husband = string.Empty;

            if (husband != null)
            {
                this._husband = husband.XRefID;

                if (!husband.SpouseInFamily(XRefID))
                {
                    GedcomFamilyLink link = new GedcomFamilyLink
                    {
                        Database = Database,
                        Family = XRefID,
                        Individual = this._husband
                    };
                    husband.SpouseIn.Add(link);
                }
            }

            if (wife != null)
            {
                this._wife = wife.XRefID;

                if (!wife.SpouseInFamily(XRefID))
                {
                    GedcomFamilyLink link = new GedcomFamilyLink
                    {
                        Database = Database,
                        Family = XRefID,
                        Individual = this._wife
                    };
                    wife.SpouseIn.Add(link);
                }
            }
        }

        /// <summary>
        /// Changes the wife.
        /// </summary>
        /// <param name="indi">The wife.</param>
        public void ChangeWife(GedcomIndividualRecord indi)
        {
            GedcomIndividualRecord husband = null;
            GedcomIndividualRecord wife = null;

            if (!string.IsNullOrEmpty(this._husband))
            {
                husband = Database[this._husband] as GedcomIndividualRecord;
            }

            if (!string.IsNullOrEmpty(this._wife))
            {
                wife = Database[this._wife] as GedcomIndividualRecord;
            }

            if (string.IsNullOrEmpty(XRefID))
            {
                XRefID = Database.GenerateXref("FAM");
                Database.Add(XRefID, this);
            }

            if (wife != null)
            {
                if (wife.SpouseInFamily(XRefID, out GedcomFamilyLink link))
                {
                    wife.SpouseIn.Remove(link);
                }
            }

            wife = indi;
            this._wife = string.Empty;

            if (husband != null)
            {
                this._husband = husband.XRefID;

                if (!husband.SpouseInFamily(XRefID))
                {
                    GedcomFamilyLink link = new GedcomFamilyLink
                    {
                        Database = Database,
                        Family = XRefID,
                        Individual = this._husband
                    };
                    husband.SpouseIn.Add(link);
                }
            }

            if (wife != null)
            {
                this._wife = wife.XRefID;

                if (!wife.SpouseInFamily(XRefID))
                {
                    GedcomFamilyLink link = new GedcomFamilyLink
                    {
                        Database = Database,
                        Family = XRefID,
                        Individual = this._wife
                    };
                    wife.SpouseIn.Add(link);
                }
            }
        }

        /// <summary>
        /// Removes the husband.
        /// </summary>
        /// <param name="indi">The husband.</param>
        public void RemoveHusband(GedcomIndividualRecord indi)
        {
            if (_husband == indi.XRefID)
            {
                _husband = string.Empty;
            }

            if (indi.SpouseInFamily(XRefID, out GedcomFamilyLink link))
            {
                indi.SpouseIn.Remove(link);
            }
        }

        /// <summary>
        /// Removes the wife.
        /// </summary>
        /// <param name="indi">The wife.</param>
        public void RemoveWife(GedcomIndividualRecord indi)
        {
            if (_wife == indi.XRefID)
            {
                _wife = string.Empty;
            }

            if (indi.SpouseInFamily(XRefID, out GedcomFamilyLink link))
            {
                indi.SpouseIn.Remove(link);
            }
        }

        /// <summary>
        /// Clears the linkage types.
        /// </summary>
        public void ClearLinkageTypes()
        {
            if (_linkageTypes != null)
            {
                _linkageTypes.Clear();
                _linkageTypes = null;
            }

            if (_husbLinkageTypes != null)
            {
                _husbLinkageTypes.Clear();
                _husbLinkageTypes = null;
            }

            if (_wifeLinkageTypes != null)
            {
                _wifeLinkageTypes.Clear();
                _wifeLinkageTypes = null;
            }
        }

        /// <summary>
        /// Sets the type of the linkage.
        /// </summary>
        /// <param name="childXrefID">The child xref identifier.</param>
        /// <param name="type">The pedigree linkage type.</param>
        public void SetLinkageType(string childXrefID, PedigreeLinkageType type)
        {
            SetLinkageType(childXrefID, type, GedcomAdoptionType.HusbandAndWife);
        }

        /// <summary>
        /// Sets the type of the linkage.
        /// </summary>
        /// <param name="childXrefID">The child xref identifier.</param>
        /// <param name="type">The pedigree linkage type.</param>
        /// <param name="to">The adoption type.</param>
        public void SetLinkageType(string childXrefID, PedigreeLinkageType type, GedcomAdoptionType to)
        {
            Dictionary<string, PedigreeLinkageType> dict;

            switch (to)
            {
                case GedcomAdoptionType.Husband:
                    if (_husbLinkageTypes == null)
                    {
                        _husbLinkageTypes = new Dictionary<string, PedigreeLinkageType>();
                    }

                    dict = _husbLinkageTypes;
                    break;

                case GedcomAdoptionType.Wife:
                    if (_wifeLinkageTypes == null)
                    {
                        _wifeLinkageTypes = new Dictionary<string, PedigreeLinkageType>();
                    }

                    dict = _wifeLinkageTypes;
                    break;

                case GedcomAdoptionType.HusbandAndWife:
                default:
                    if (_linkageTypes == null)
                    {
                        _linkageTypes = new Dictionary<string, PedigreeLinkageType>();
                    }

                    dict = _linkageTypes;
                    break;
            }

            if (dict.ContainsKey(childXrefID))
            {
                dict[childXrefID] = type;
            }
            else
            {
                dict.Add(childXrefID, type);
            }
        }

        /// <summary>
        /// Gets the type of the husband linkage.
        /// </summary>
        /// <param name="childXrefID">The child xref identifier.</param>
        /// <returns>
        /// Pedigree linkage type for husband.
        /// </returns>
        public PedigreeLinkageType GetHusbandLinkageType(string childXrefID)
        {
            PedigreeLinkageType ret = PedigreeLinkageType.Unknown;

            if (_husbLinkageTypes != null && _husbLinkageTypes.ContainsKey(childXrefID))
            {
                ret = _husbLinkageTypes[childXrefID];
            }
            else
            {
                GedcomIndividualRecord child = (GedcomIndividualRecord)Database[childXrefID];
                if (child != null)
                {
                    if (child.ChildInFamily(XrefId, out GedcomFamilyLink link))
                    {
                        ret = link.FatherPedigree;
                    }
                    else
                    {
                        System.Diagnostics.Debug.WriteLine("Child " + childXrefID + " is not in family " +
                                                           XrefId + " in GetLinkageType");
                    }
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine("Attempt to GetLinkageType of unknown child " +
                                                       childXrefID + " in " + XrefId);
                }
            }

            return ret;
        }

        /// <summary>
        /// Gets the type of the wife linkage.
        /// </summary>
        /// <param name="childXrefID">The child xref identifier.</param>
        /// <returns>
        /// Pedigree linkage type for wife.
        /// </returns>
        public PedigreeLinkageType GetWifeLinkageType(string childXrefID)
        {
            PedigreeLinkageType ret = PedigreeLinkageType.Unknown;

            if (_wifeLinkageTypes != null && _wifeLinkageTypes.ContainsKey(childXrefID))
            {
                ret = _wifeLinkageTypes[childXrefID];
            }
            else
            {
                GedcomIndividualRecord child = (GedcomIndividualRecord)Database[childXrefID];
                if (child != null)
                {
                    if (child.ChildInFamily(XrefId, out GedcomFamilyLink link))
                    {
                        ret = link.MotherPedigree;
                    }
                    else
                    {
                        System.Diagnostics.Debug.WriteLine("Child " + childXrefID + " is not in family " +
                                                           XrefId + " in GetLinkageType");
                    }
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine("Attempt to GetLinkageType of unknown child " +
                                                       childXrefID + " in " + XrefId);
                }
            }

            return ret;
        }

        /// <summary>
        /// Gets the type of the linkage.
        /// </summary>
        /// <param name="childXrefID">The child xref identifier.</param>
        /// <returns>
        /// Pedigree linkage type.
        /// </returns>
        public PedigreeLinkageType GetLinkageType(string childXrefID)
        {
            PedigreeLinkageType ret = PedigreeLinkageType.Unknown;

            if (_linkageTypes != null && _linkageTypes.ContainsKey(childXrefID))
            {
                ret = _linkageTypes[childXrefID];
            }
            else
            {
                GedcomIndividualRecord child = (GedcomIndividualRecord)Database[childXrefID];
                if (child != null)
                {
                    if (child.ChildInFamily(XrefId, out GedcomFamilyLink link))
                    {
                        ret = link.Pedigree;
                    }
                    else
                    {
                        System.Diagnostics.Debug.WriteLine("Child " + childXrefID + " is not in family " +
                                                           XrefId + " in GetLinkageType");
                    }
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine("Attempt to GetLinkageType of unknown child " +
                                                       childXrefID + " in " + XrefId);
                }
            }

            return ret;
        }

        /// <summary>
        /// Generates the XML.
        /// </summary>
        /// <param name="root">The root node.</param>
        public override void GenerateXML(XmlNode root)
        {
            XmlDocument doc = root.OwnerDocument;

            XmlNode node;
            XmlAttribute attr;

            XmlNode famNode = doc.CreateElement("FamilyRec");
            attr = doc.CreateAttribute("Id");
            attr.Value = XRefID;
            famNode.Attributes.Append(attr);

            if (!string.IsNullOrEmpty(Husband))
            {
                GedcomIndividualRecord husb = Database[Husband] as GedcomIndividualRecord;
                if (husb != null)
                {
                    node = doc.CreateElement("HusbFath");

                    XmlNode linkNode = doc.CreateElement("Link");

                    attr = doc.CreateAttribute("Target");
                    attr.Value = "IndividualRec";
                    linkNode.Attributes.Append(attr);

                    attr = doc.CreateAttribute("Ref");
                    attr.Value = Husband;
                    linkNode.Attributes.Append(attr);

                    node.AppendChild(linkNode);

                    famNode.AppendChild(node);
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine("Pointer to non existent husband");
                }
            }

            if (!string.IsNullOrEmpty(Wife))
            {
                GedcomIndividualRecord wife = Database[Wife] as GedcomIndividualRecord;
                if (wife != null)
                {
                    node = doc.CreateElement("WifeMoth");

                    XmlNode linkNode = doc.CreateElement("Link");

                    attr = doc.CreateAttribute("Target");
                    attr.Value = "IndividualRec";
                    linkNode.Attributes.Append(attr);

                    attr = doc.CreateAttribute("Ref");
                    attr.Value = Wife;
                    linkNode.Attributes.Append(attr);

                    node.AppendChild(linkNode);

                    famNode.AppendChild(node);
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine("Pointer to non existent wife");
                }
            }

            foreach (string child in Children)
            {
                GedcomIndividualRecord indi = Database[child] as GedcomIndividualRecord;
                if (indi != null)
                {
                    node = doc.CreateElement("Child");

                    XmlNode linkNode = doc.CreateElement("Link");

                    attr = doc.CreateAttribute("Target");
                    attr.Value = "IndividualRec";
                    linkNode.Attributes.Append(attr);

                    attr = doc.CreateAttribute("Ref");
                    attr.Value = child;
                    linkNode.Attributes.Append(attr);

                    node.AppendChild(linkNode);

                    // TODO: add in <ChildNbr>

                    if (indi.ChildInFamily(XRefID, out GedcomFamilyLink link))
                    {
                        XmlNode relNode = doc.CreateElement("RelToFath");
                        string relType = string.Empty;
                        relType = link.FatherPedigree switch
                        {
                            PedigreeLinkageType.Adopted => "adopted",
                            PedigreeLinkageType.Birth => "birth",
                            PedigreeLinkageType.Foster => "foster",
                            PedigreeLinkageType.Sealing => "sealing",
                            _ => "unknown",
                        };
                        relNode.AppendChild(doc.CreateTextNode(relType));

                        relNode = doc.CreateElement("RelToMoth");
                        relType = string.Empty;
                        relType = link.MotherPedigree switch
                        {
                            PedigreeLinkageType.Adopted => "adopted",
                            PedigreeLinkageType.Birth => "birth",
                            PedigreeLinkageType.Foster => "foster",
                            PedigreeLinkageType.Sealing => "sealing",
                            _ => "unknown",
                        };
                        relNode.AppendChild(doc.CreateTextNode(relType));
                    }

                    famNode.AppendChild(node);
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine("Pointer to non existent child");
                }
            }

            XmlNode basedOnNode = doc.CreateElement("BasedOn");

            foreach (GedcomFamilyEvent famEvent in Events)
            {
                node = doc.CreateElement("Event");

                XmlNode linkNode = doc.CreateElement("Link");

                attr = doc.CreateAttribute("Target");
                attr.Value = "EventRec";
                linkNode.Attributes.Append(attr);

                attr = doc.CreateAttribute("Ref");
                attr.Value = famEvent.EventXRefID;
                linkNode.Attributes.Append(attr);

                node.AppendChild(linkNode);

                basedOnNode.AppendChild(node);
            }

            famNode.AppendChild(basedOnNode);

            GenerateNoteXML(famNode);
            GenerateCitationsXML(famNode);
            GenerateMultimediaXML(famNode);

            GenerateChangeDateXML(famNode);

            root.AppendChild(famNode);
        }

        /// <summary>
        /// Output GEDCOM format for this instance.
        /// </summary>
        /// <param name="sw">Where to output the data to.</param>
        public override void Output(TextWriter sw)
        {
            base.Output(sw);

            string levelPlusOne = (Level + 1).ToString();

            if (RestrictionNotice != GedcomRestrictionNotice.None)
            {
                sw.Write(Environment.NewLine);
                sw.Write(levelPlusOne);
                sw.Write(" RESN ");
                sw.Write(RestrictionNotice.ToString().ToLower());
            }

            foreach (GedcomFamilyEvent familyEvent in _events)
            {
                familyEvent.Output(sw);
            }

            if (!string.IsNullOrEmpty(_husband))
            {
                sw.Write(Environment.NewLine);
                sw.Write(levelPlusOne);
                sw.Write(" HUSB ");
                sw.Write("@");
                sw.Write(_husband);
                sw.Write("@");
            }

            if (!string.IsNullOrEmpty(_wife))
            {
                sw.Write(Environment.NewLine);
                sw.Write(levelPlusOne);
                sw.Write(" WIFE ");
                sw.Write("@");
                sw.Write(_wife);
                sw.Write("@");
            }

            string levelPlusTwo = (Level + 2).ToString();
            foreach (string childID in _children)
            {
                sw.Write(Environment.NewLine);
                sw.Write(levelPlusOne);
                sw.Write(" CHIL ");
                sw.Write("@");
                sw.Write(childID);
                sw.Write("@");

                GedcomIndividualRecord child = (GedcomIndividualRecord)Database[childID];
                if (child != null)
                {
                    // only output _FREL / _MREL value here,
                    // real PEDI goes on the FAMC on the INDI tag
                    if (child.ChildInFamily(XrefId, out GedcomFamilyLink link))
                    {
                        switch (link.Pedigree)
                        {
                            case PedigreeLinkageType.FatherAdopted:
                                sw.Write(Environment.NewLine);
                                sw.Write(levelPlusTwo);
                                sw.Write("_FREL Adopted");
                                break;

                            case PedigreeLinkageType.MotherAdopted:
                                sw.Write(Environment.NewLine);
                                sw.Write(levelPlusTwo);
                                sw.Write("_MREL Adopted");
                                break;
                        }
                    }
                    else
                    {
                        System.Diagnostics.Debug.WriteLine("Missing child linkage for " + childID + " to family " + XrefId);
                    }
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine("Missing child " + childID + " when outputting family " + XrefId);
                }
            }

            if (_numberOfChildren != 0)
            {
                sw.Write(Environment.NewLine);
                sw.Write(levelPlusOne);
                sw.Write(" NCHI ");
                sw.Write("@");
                sw.Write(_numberOfChildren.ToString());
                sw.Write("@");
            }

            if (_spouseSealing != null)
            {
                _spouseSealing.Output(sw);
            }

            if (_submitterRecords != null)
            {
                foreach (string submitter in SubmitterRecords)
                {
                    sw.Write(Environment.NewLine);
                    sw.Write(levelPlusOne);
                    sw.Write(" SUBM ");
                    sw.Write("@");
                    sw.Write(submitter);
                    sw.Write("@");
                }
            }

            if (StartStatus != MarriageStartStatus.Unknown)
            {
                sw.Write(Environment.NewLine);
                sw.Write(levelPlusOne);
                sw.Write(" _MSTAT ");
                sw.Write(StartStatus.ToString());
            }
        }

        /// <summary>
        /// Compare the user entered data against the passed instance for similarity.
        /// </summary>
        /// <param name="obj">The object to compare this instance against.</param>
        /// <returns>
        /// True if instance matches user data, otherwise false.
        /// </returns>
        public override bool IsEquivalentTo(object obj)
        {
            var family = obj as GedcomFamilyRecord;

            if (family == null)
            {
                return false;
            }

            if (!Equals(NumberOfChildren, family.NumberOfChildren))
            {
                return false;
            }

            if (!Children.All(family.Children.Contains))
            {
                return false;
            }

            if (!Events.All(family.Events.Contains))
            {
                return false;
            }

            if (!Equals(Husband, family.Husband))
            {
                return false;
            }

            if (!Equals(Wife, family.Wife))
            {
                return false;
            }

            if (!Equals(Marriage, family.Marriage))
            {
                return false;
            }

            if (!Equals(StartStatus, family.StartStatus))
            {
                return false;
            }

            if (!SubmitterRecords.All(family.SubmitterRecords.Contains))
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Compare the user entered data against the passed instance for similarity.
        /// </summary>
        /// <param name="other">The GedcomFamilyRecord to compare this instance against.</param>
        /// <returns>
        /// True if instance matches user data, otherwise false.
        /// </returns>
        public bool Equals(GedcomFamilyRecord other)
        {
            return IsEquivalentTo(other);
        }

        /// <summary>
        /// Compares the current and passed-in object to see if they are the same.
        /// </summary>
        /// <param name="obj">The object to compare the current instance against.</param>
        /// <returns>True if they match, False otherwise.</returns>
        public override bool Equals(object obj)
        {
            return Equals(obj as GedcomFamilyRecord);
        }

        public override int GetHashCode()
        {
            return new
            {
                NumberOfChildren,
                Children,
                Events,
                Husband,
                Wife,
                Marriage,
                StartStatus,
                SubmitterRecords,
            }.GetHashCode();
        }
    }
}