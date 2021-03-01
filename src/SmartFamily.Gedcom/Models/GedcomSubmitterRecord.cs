using SmartFamily.Gedcom.Enums;

using System;
using System.Collections.Generic;
using System.IO;

namespace SmartFamily.Gedcom.Models
{
    /// <summary>
    /// An individual or organization who contributes genealogical data to a file or transfers it to someone else.
    /// </summary>
    /// <seealso cref="GedcomRecord"/>
    public class GedcomSubmitterRecord : GedcomRecord, IEquatable<GedcomSubmitterRecord>
    {
        private string _name;
        private GedcomAddress _address;
        private List<string> _languagePreferences;
        private string _registeredRFN;

        /// <summary>
        /// Initializes a new instance of the <see cref="GedcomSubmitterRecord"/> class.
        /// </summary>
        public GedcomSubmitterRecord()
        {
            LanguagePreferences = new List<string>(3);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GedcomCustomRecord"/> class.
        /// </summary>
        /// <param name="database">The database to associate with this record.</param>
        public GedcomSubmitterRecord(GedcomDatabase database)
            : this()
        {
            Database = database;

            Level = 0;
            XRefID = database.GenerateXref("S");

            database.Add(XRefID, this);
        }

        /// <summary>
        /// Gets the type of the record.
        /// </summary>
        public override GedcomRecordType RecordType
        {
            get => GedcomRecordType.Submitter;
        }

        /// <summary>
        /// Gets the GEDCOM tag for a submitter record.
        /// </summary>
        public override string GedcomTag
        {
            get => "SUBM";
        }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        public string Name
        {
            get=> _name;
            set
            {
                if (value != _name)
                {
                    _name = value;
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
        /// Gets or sets the language preferences.
        /// </summary>
        public List<string> LanguagePreferences
        {
            get => _languagePreferences;
            set
            {
                if (value != _languagePreferences)
                {
                    _languagePreferences = value;
                    Changed();
                }
            }
        }

        /// <summary>
        /// Gets or sets the registered RFN.
        /// </summary>
        public string RegisteredRFN
        {
            get => _registeredRFN;
            set
            {
                if (value != _registeredRFN)
                {
                    _registeredRFN = value;
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

                if (realChangeDate != null)
                {
                    realChangeDate.Level = Level + 2;
                }

                return realChangeDate;
            }
            set => base.ChangeDate = value;
        }

        /// <summary>
        /// Output GEDCOM formatted text representing the submitter record.
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

            tw.Write(GedcomTag);

            string levelPlusOne = (Level + 1).ToString();

            string name = Name;
            if (string.IsNullOrEmpty(name))
            {
                name = "Unknown";
            }

            tw.Write(Environment.NewLine);
            tw.Write(levelPlusOne);
            tw.Write(" NAME ");
            tw.Write(name);

            if (Address != null)
            {
                Address.Output(tw, Level + 1);
            }

            foreach (string languagePreference in LanguagePreferences)
            {
                if (!string.IsNullOrEmpty(languagePreference))
                {
                    tw.Write(Environment.NewLine);
                    tw.Write(levelPlusOne);
                    tw.Write(" LANG ");
                    tw.Write(languagePreference);
                }
            }

            if (!string.IsNullOrEmpty(RegisteredRFN))
            {
                tw.Write(Environment.NewLine);
                tw.Write(levelPlusOne);
                tw.Write(" RFN ");
                tw.Write(RegisteredRFN);
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
            GedcomSubmitterRecord submitter = obj as GedcomSubmitterRecord;

            if (submitter == null)
            {
                return false;
            }

            if (!Equals(Address, submitter.Address))
            {
                return false;
            }

            if (!GedcomGenericListComparer.CompareLists(LanguagePreferences, submitter.LanguagePreferences))
            {
                return false;
            }

            if (!Equals(Name, submitter.Name))
            {
                return false;
            }

            if (!Equals(RegisteredRFN, submitter.RegisteredRFN))
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Compare the user entered data against the passed instance for similarity.
        /// </summary>
        /// <param name="other">The GedcomSubmitterRecord to compare this instance against.</param>
        /// <returns><c>True</c> if instance matches user data, otherwise <c>false</c>.</returns>
        public bool Equals(GedcomSubmitterRecord other)
        {
            return IsEquivalentTo(other);
        }
    }
}