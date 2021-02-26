using SmartFamily.Gedcom.Enums;

using System;
using System.Collections.Generic;
using System.Text;

namespace SmartFamily.Gedcom.Models
{
    /// <summary>
    /// A name for a given individual, allowing different variations to be
    /// stored.
    /// </summary>
    public class GedcomName : GedcomRecord, IComparable<GedcomName>, IComparable, IEquatable<GedcomName>
    {
        private string type;

        // name pieces
        private string prefix;
        private string given; // no same as firstname, includes middle etc.
        private string surnamePrefix;

        // already got surname
        private string suffix;
        private string nick;

        private GedcomRecordList<GedcomVariation> phoneticVariations;
        private GedcomRecordList<GedcomVariation> romanizedVariations;

        // cached surname / firstname split, this is expensive
        // when trying to filter a list of individuals, so do it
        // upon setting the name
        private string surname;

        private string surnameSoundex;
        private string firstnameSoundex;

        private StringBuilder builtName;

        /// <summary>
        /// Initializes a new instance of the <see cref="GedcomName"/> class.
        /// </summary>
        public GedcomName()
        {
            prefix = string.Empty;
            given = string.Empty;
            surnamePrefix = string.Empty;
            suffix = string.Empty;
            nick = string.Empty;
            surname = string.Empty;
            surnameSoundex = string.Empty;
            firstnameSoundex = string.Empty;
        }

        /// <summary>
        /// Gets the type of the record.
        /// </summary>
        /// <value>
        /// The type of the record.
        /// </value>
        public override GedcomRecordType RecordType
        {
            get { return GedcomRecordType.Name; }
        }

        /// <summary>
        /// Gets the GEDCOM tag.
        /// </summary>
        /// <value>
        /// The GEDCOM tag.
        /// </value>
        public override string GedcomTag
        {
            get { return "NAME"; }
        }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        public string Name
        {
            get
            {
                if (builtName == null)
                {
                    builtName = BuildName();
                }

                return builtName.ToString();
            }
            set
            {
                // check to see if a name has been set before
                // checking if it has changed, otherwise
                // we end up building a name string up twice
                // while loading. IsSet is true if any name
                // parts are non null.
                if ((!IsSet && !string.IsNullOrEmpty(value)) || (value != Name))
                {
                    string name = value;

                    name = name is null ? string.Empty : name.Trim();

                    int surnameStartPos = name.IndexOf("/");
                    int surnameLength = 0;
                    if (surnameStartPos == -1)
                    {
                        surnameStartPos = name.LastIndexOf(" ");
                        if (surnameStartPos != -1)
                        {
                            surnameLength = name.Length - surnameStartPos - 1;
                            Surname = Database.NameCollection[name, surnameStartPos + 1, surnameLength];
                        }
                        else
                        {
                            Surname = string.Empty;
                            surnameStartPos = name.Length; // No surname, must just be a given name only.
                        }
                    }
                    else
                    {

                    }
                }
            }
        }
    }
}