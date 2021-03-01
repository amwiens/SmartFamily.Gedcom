using SmartFamily.Gedcom.Enums;

using System;
using System.IO;
using System.Text;

namespace SmartFamily.Gedcom.Models
{
    /// <summary>
    /// A name for a given individual, allowing different variations to be
    /// stored.
    /// </summary>
    public class GedcomName : GedcomRecord, IComparable<GedcomName>, IComparable, IEquatable<GedcomName>
    {
        private string _type;

        // name pieces
        private string _prefix;

        private string _given; // no same as firstname, includes middle etc.
        private string _surnamePrefix;

        // already got surname
        private string _suffix;

        private string _nick;

        private GedcomRecordList<GedcomVariation> _phoneticVariations;
        private GedcomRecordList<GedcomVariation> _romanizedVariations;

        // cached surname / firstname split, this is expensive
        // when trying to filter a list of individuals, so do it
        // upon setting the name
        private string _surname;

        private string _surnameSoundex;
        private string _firstnameSoundex;

        private StringBuilder _builtName;

        /// <summary>
        /// Initializes a new instance of the <see cref="GedcomName"/> class.
        /// </summary>
        public GedcomName()
        {
            _prefix = string.Empty;
            _given = string.Empty;
            _surnamePrefix = string.Empty;
            _suffix = string.Empty;
            _nick = string.Empty;
            _surname = string.Empty;
            _surnameSoundex = string.Empty;
            _firstnameSoundex = string.Empty;
        }

        /// <summary>
        /// Gets the type of the record.
        /// </summary>
        /// <value>
        /// The type of the record.
        /// </value>
        public override GedcomRecordType RecordType
        {
            get => GedcomRecordType.Name;
        }

        /// <summary>
        /// Gets the GEDCOM tag.
        /// </summary>
        /// <value>
        /// The GEDCOM tag.
        /// </value>
        public override string GedcomTag
        {
            get => "NAME";
        }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        public string Name
        {
            get
            {
                if (_builtName == null)
                {
                    _builtName = BuildName();
                }

                return _builtName.ToString();
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
                        int surnameEndPos = name.IndexOf("/", surnameStartPos + 1);
                        if (surnameEndPos == -1)
                        {
                            surnameLength = name.Length - surnameStartPos - 1;
                            Surname = Database.NameCollection[name, surnameStartPos + 1, surnameLength];
                        }
                        else
                        {
                            surnameLength = surnameEndPos - surnameStartPos - 1;
                            Surname = Database.NameCollection[name, surnameStartPos + 1, surnameLength];
                        }
                    }

                    if (surnameStartPos != -1)
                    {
                        // given is everything up to the surname, not right
                        // but will do for now
                        Given = Database.NameCollection[name, 0, surnameStartPos];

                        // prefix is foo. e.g. Prof. Dr. Lt. Cmd.
                        // strip it from the given name
                        // prefix must be > 2 chars so we avoid initials
                        // begin treated as prefixes
                        int l = _given.IndexOf(".");
                        int n = _given.IndexOf(" ");
                        if (l > 2)
                        {
                            if (n != -1 && l < n)
                            {
                                int o = l;
                                int p = n;

                                do
                                {
                                    l = o;
                                    n = p;

                                    o = _given.IndexOf(".", o + 1);
                                    p = _given.IndexOf(" ", p + 1);
                                }
                                while (o != -1 && (p != -1 && o < p));

                                Prefix = Database.NameCollection[_given, 0, l + 1];
                                Given = Database.NameCollection[_given, l + 1, _given.Length - l - 1];
                            }
                        }

                        // get surname prefix, everything before the last space
                        // is part of the surname prefix
                        int m = _surname.LastIndexOf(" ");
                        if (m != -1)
                        {
                            SurnamePrefix = Database.NameCollection[_surname, 0, m];
                            Surname = Database.NameCollection[_surname, m + 1, _surname.Length - m - 1];
                        }
                        else
                        {
                            SurnamePrefix = string.Empty;
                        }

                        // TODO: anything after surname is suffix, again not right
                        // but works for now
                        int offset = surnameStartPos + 1 + surnameLength + 1;
                        if (!string.IsNullOrEmpty(_surnamePrefix))
                        {
                            offset += _surnamePrefix.Length + 1;
                        }

                        if (offset < name.Length)
                        {
                            Suffix = Database.NameCollection[name, offset, name.Length - offset];
                        }
                        else
                        {
                            _suffix = string.Empty;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Gets or sets the type.
        /// </summary>
        /// <value>
        /// The type.
        /// </value>
        public string Type
        {
            get => _type;
            set
            {
                if (value != _type)
                {
                    _type = value;
                    Changed();
                }
            }
        }

        /// <summary>
        /// Gets the phonetic variations.
        /// </summary>
        /// <value>
        /// The phonetic variations.
        /// </value>
        public GedcomRecordList<GedcomVariation> PhoneticVariations
        {
            get
            {
                if (_phoneticVariations == null)
                {
                    _phoneticVariations = new GedcomRecordList<GedcomVariation>();
                    _phoneticVariations.CollectionChanged += ListChanged;
                }

                return _phoneticVariations;
            }
        }

        /// <summary>
        /// Gets the romanized variations.
        /// </summary>
        /// <value>
        /// The romanized variations.
        /// </value>
        public GedcomRecordList<GedcomVariation> RomanizedVariations
        {
            get
            {
                if (_romanizedVariations == null)
                {
                    _romanizedVariations = new GedcomRecordList<GedcomVariation>();
                    _romanizedVariations.CollectionChanged += ListChanged;
                }

                return _romanizedVariations;
            }
        }

        /// <summary>
        /// Gets or sets the surname.
        /// </summary>
        /// <value>
        /// The surname.
        /// </value>
        public string Surname
        {
            get => _surname;
            set
            {
                if (_surname != value)
                {
                    _surname = value;
                    _surnameSoundex = Util.GenerateSoundex(_surname);
                    _builtName = null;
                    Changed();
                }
            }
        }

        /// <summary>
        /// Gets the surname soundex.
        /// </summary>
        /// <value>
        /// The surname soundex.
        /// </value>
        public string SurnameSoundex
        {
            get => _surnameSoundex;
        }

        /// <summary>
        /// Gets the firstname soundex.
        /// </summary>
        /// <value>
        /// The firstname soundex.
        /// </value>
        public string FirstnameSoundex
        {
            get => _firstnameSoundex;
        }

        /// <summary>
        /// Gets or sets the prefix.
        /// </summary>
        /// <value>
        /// The prefix.
        /// </value>
        public string Prefix
        {
            get => _prefix;
            set
            {
                if (_prefix != value)
                {
                    _prefix = value;
                    _builtName = null;
                    Changed();
                }
            }
        }

        /// <summary>
        /// Gets or sets the given.
        /// </summary>
        /// <value>
        /// The given.
        /// </value>
        public string Given
        {
            get => _given;
            set
            {
                if (_given != value)
                {
                    _given = value;
                    _firstnameSoundex = Util.GenerateSoundex(_given);
                    _builtName = null;
                    Changed();
                }
            }
        }

        /// <summary>
        /// Gets or sets the surname prefix.
        /// </summary>
        /// <value>
        /// The surname prefix.
        /// </value>
        public string SurnamePrefix
        {
            get => _surnamePrefix;
            set
            {
                if (_surnamePrefix != value)
                {
                    _surnamePrefix = value;
                    _builtName = null;
                    Changed();
                }
            }
        }

        /// <summary>
        /// Gets or sets the suffix.
        /// </summary>
        /// <value>
        /// The suffix.
        /// </value>
        public string Suffix
        {
            get => _suffix;
            set
            {
                if (_suffix != null)
                {
                    _suffix = value;
                    _builtName = null;
                    Changed();
                }
            }
        }

        /// <summary>
        /// Gets or sets the nick.
        /// </summary>
        /// <value>
        /// The nick.
        /// </value>
        public string Nick
        {
            get => _nick;
            set
            {
                if (value != _nick)
                {
                    _nick = value;
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
                GedcomChangeDate childChangeDate;
                if (_phoneticVariations != null)
                {
                    foreach (GedcomVariation variation in _phoneticVariations)
                    {
                        childChangeDate = variation.ChangeDate;
                        if (childChangeDate != null && realChangeDate != null && childChangeDate > realChangeDate)
                        {
                            realChangeDate = childChangeDate;
                        }
                    }
                }

                if (_romanizedVariations != null)
                {
                    foreach (GedcomVariation variation in _romanizedVariations)
                    {
                        childChangeDate = variation.ChangeDate;
                        if (childChangeDate != null && realChangeDate != null && childChangeDate > realChangeDate)
                        {
                            realChangeDate = childChangeDate;
                        }
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
        /// Gets or sets a value indicating whether this is the individuals preferred name.
        /// </summary>
        /// <value>
        /// <c>true</c> if [preferred name]; otherwise, <c>false</c>.
        /// </value>
        public bool PreferredName { get; set; }

        private bool IsSet
        {
            get
            {
                return (!string.IsNullOrEmpty(_prefix)) ||
                    (!string.IsNullOrEmpty(_given)) ||
                    (!string.IsNullOrEmpty(_nick)) ||
                    (!string.IsNullOrEmpty(_surnamePrefix)) ||
                    (!string.IsNullOrEmpty(_surname)) ||
                    (!string.IsNullOrEmpty(_suffix));
            }
        }

        /// <summary>
        /// Compares two GedcomName instances by using the full name.
        /// </summary>
        /// <param name="other">The name to compare against this instance.</param>
        /// <returns>An integer specifying the relative sort order.</returns>
        public int CompareTo(GedcomName other)
        {
            if (other == null)
            {
                return 1;
            }

            var compare = string.Compare(Type, other.Type);
            if (compare != 0)
            {
                return compare;
            }

            compare = GedcomGenericListComparer.CompareListOrder(PhoneticVariations, other.PhoneticVariations);
            if (compare != 0)
            {
                return compare;
            }

            compare = GedcomGenericListComparer.CompareListOrder(RomanizedVariations, other.RomanizedVariations);
            if (compare != 0)
            {
                return compare;
            }

            compare = string.Compare(Surname, other.Surname);
            if (compare != 0)
            {
                return compare;
            }

            compare = string.Compare(Prefix, other.Prefix);
            if (compare != 0)
            {
                return compare;
            }

            compare = string.Compare(Given, other.Given);
            if (compare != 0)
            {
                return compare;
            }

            compare = string.Compare(SurnamePrefix, other.SurnamePrefix);
            if (compare != 0)
            {
                return compare;
            }

            compare = string.Compare(Suffix, other.Suffix);
            if (compare != 0)
            {
                return compare;
            }

            compare = string.Compare(Nick, other.Nick);
            if (compare != 0)
            {
                return compare;
            }

            compare = PreferredName.CompareTo(other.PreferredName);
            if (compare != 0)
            {
                return compare;
            }

            return compare;
        }

        /// <summary>
        /// Compares two GedcomName instances by using the full name.
        /// </summary>
        /// <param name="obj">The name to compare against this instance.</param>
        /// <returns>An integer specifying the relative sort order.</returns>
        public int CompareTo(object obj)
        {
            return CompareTo(obj as GedcomName);
        }

        /// <summary>
        /// Compare the user-entered data against the passed instance for similarity.
        /// </summary>
        /// <param name="other">The GedcomName to compare this instance against.</param>
        /// <returns>True if instance matches user data, otherwise False</returns>
        public bool Equals(GedcomName other)
        {
            return CompareTo(other) == 0;
        }

        /// <summary>
        /// Compare the user-entered data against the passed instance for similarity.
        /// </summary>
        /// <param name="obj">The object to compare this instance against.</param>
        /// <returns>True if instance matches user data, otherwise False.</returns>
        public override bool IsEquivalentTo(object obj)
        {
            return CompareTo(obj as GedcomName) == 0;
        }

        /// <summary>
        /// Returns a percentage based score on how similar the passed record is to the current instance.
        /// </summary>
        /// <param name="name">The event to compare against this instance.</param>
        /// <returns>A score from 0 to 100 representing the percentage match.</returns>
        public decimal CalculateSimilarityScore(GedcomName name)
        {
            var match = decimal.Zero;

            int parts = 0;

            // TODO: perform soundex check as well?
            // how would that affect returning a % match?
            var matches = decimal.Zero;

            bool surnameMatched = false;

            if (!(string.IsNullOrEmpty(name.Prefix) && string.IsNullOrEmpty(_prefix)))
            {
                parts++;
                if (name.Prefix == _prefix)
                {
                    matches++;
                }
            }

            if (!(string.IsNullOrEmpty(name.Given) && string.IsNullOrEmpty(_given)))
            {
                parts++;
                if (name.Given == _given)
                {
                    matches++;
                }
            }

            if (!(string.IsNullOrEmpty(name.Surname) && string.IsNullOrEmpty(_surname)))
            {
                if ((name.Surname == "?" && _surname == "?") ||
                    ((string.Compare(name.Surname, "unknown", true) == 0) &&
                    (string.Compare(_surname, "unknown", true) == 0)))
                {
                    // not really matched, surname isn't known,
                    // don't count as part being checked, and don't penalize
                    surnameMatched = true;
                }
                else
                {
                    parts++;
                    if (name.Surname == _surname)
                    {
                        matches++;
                        surnameMatched = true;
                    }
                }
            }
            else
            {
                // pretend the surname matches
                surnameMatched = true;
            }

            if (!(string.IsNullOrEmpty(name.SurnamePrefix) && string.IsNullOrEmpty(_surnamePrefix)))
            {
                parts++;
                if (name.SurnamePrefix == _surnamePrefix)
                {
                    matches++;
                }
            }

            if (!(string.IsNullOrEmpty(name.Suffix) && string.IsNullOrEmpty(_suffix)))
            {
                parts++;
                if (name.Suffix == _suffix)
                {
                    matches++;
                }
            }

            if (!(string.IsNullOrEmpty(name.Nick) && string.IsNullOrEmpty(_nick)))
            {
                parts++;
                if (name.Nick == _nick)
                {
                    matches++;
                }
            }

            match = (matches / parts) * 100m;

            // TODO: heavily penalize the surname not matching
            // for this to work correctly better matching needs to be
            // performed, not just string comparison
            if (!surnameMatched)
            {
                match *= 0.25m;
            }

            return match;
        }

        /// <summary>
        /// Outputs this instance as a GEDCOM record.
        /// </summary>
        /// <param name="sw">The writer to output to.</param>
        public override void Output(TextWriter sw)
        {
            sw.Write(Environment.NewLine);
            sw.Write(Level.ToString());
            sw.Write(" NAME ");
            sw.Write(Name);

            string levelPlusOne = null;
            string levelPlusTwo = null;

            if (!string.IsNullOrEmpty(Type))
            {
                if (levelPlusOne == null)
                {
                    levelPlusOne = (Level + 1).ToString();
                }

                sw.Write(Environment.NewLine);
                sw.Write(levelPlusOne);
                sw.Write(" TYPE ");
                string line = Type.Replace("@", "@@");
                sw.Write(line);
            }

            // Gedcom 5.5.5 spec says to always output these fields, even if blank.
            OutputNamePart(sw, "NPFX", Prefix, Level + 1);
            OutputNamePart(sw, "GIVN", Given, Level + 1);
            OutputNamePart(sw, "NICK", Nick, Level + 1);
            OutputNamePart(sw, "SPFX", SurnamePrefix, Level + 1);
            OutputNamePart(sw, "SURN", Surname, Level + 1);
            OutputNamePart(sw, "NSFX", Suffix, Level + 1);

            OutputStandard(sw);

            if (_phoneticVariations != null)
            {
                if (levelPlusOne == null)
                {
                    levelPlusOne = (Level + 1).ToString();
                }

                if (levelPlusTwo == null)
                {
                    levelPlusTwo = (Level + 2).ToString();
                }

                foreach (GedcomVariation variation in PhoneticVariations)
                {
                    sw.Write(levelPlusOne);
                    sw.Write(" FONE ");
                    string line = variation.Value.Replace("@", "@@");
                    sw.Write(line);
                    sw.Write(Environment.NewLine);
                    if (!string.IsNullOrEmpty(variation.VariationType))
                    {
                        sw.Write(levelPlusTwo);
                        sw.Write(" TYPE ");
                        line = variation.VariationType.Replace("@", "@@");
                        sw.Write(line);
                        sw.Write(Environment.NewLine);
                    }
                }
            }

            if (_romanizedVariations != null)
            {
                if (levelPlusOne == null)
                {
                    levelPlusOne = (Level + 1).ToString();
                }

                if (levelPlusTwo == null)
                {
                    levelPlusTwo = (Level + 2).ToString();
                }

                foreach (GedcomVariation variation in RomanizedVariations)
                {
                    sw.Write(levelPlusOne);
                    sw.Write(" ROMN ");
                    string line = variation.Value.Replace("@", "@@");
                    sw.Write(line);
                    sw.Write(Environment.NewLine);
                    if (!string.IsNullOrEmpty(variation.VariationType))
                    {
                        sw.Write(levelPlusTwo);
                        sw.Write(" TYPE ");
                        line = variation.VariationType.Replace("@", "@@");
                        sw.Write(line);
                        sw.Write(Environment.NewLine);
                    }
                }
            }
        }

        private void OutputNamePart(TextWriter sw, string tagName, string tagValue, int level)
        {
            sw.Write(Environment.NewLine);
            sw.Write(level.ToString());
            sw.Write(" " + tagName + " ");
            var line = tagValue.Replace("@", "@@");
            sw.Write(line);
        }

        private StringBuilder BuildName()
        {
            int capacity = 0;
            if (!string.IsNullOrEmpty(_prefix))
            {
                capacity += _prefix.Length;
            }

            if (!string.IsNullOrEmpty(_given))
            {
                capacity += _given.Length;
            }

            if (!string.IsNullOrEmpty(_surnamePrefix))
            {
                capacity += _surnamePrefix.Length;
            }

            if (!string.IsNullOrEmpty(_surname))
            {
                capacity += _surname.Length;
            }

            if (!string.IsNullOrEmpty(_suffix))
            {
                capacity += _suffix.Length;
            }

            // for the // surrounding surname + potential spaces
            capacity += 4;

            StringBuilder name = new StringBuilder(capacity);

            if (!string.IsNullOrEmpty(_prefix))
            {
                name.Append(_prefix);
            }

            if (!string.IsNullOrEmpty(_given))
            {
                if (name.Length != 0)
                {
                    name.Append(" ");
                }

                name.Append(_given);
            }

            // ALWYAS output a surname, event if it is empty
            if (name.Length != 0)
            {
                name.Append(" ");
            }

            name.Append("/");
            if (!string.IsNullOrEmpty(_surnamePrefix))
            {
                name.Append(_surnamePrefix);
                name.Append(" ");
            }

            if (!string.IsNullOrEmpty(_surname))
            {
                name.Append(_surname);
            }

            name.Append("/");

            if (!string.IsNullOrEmpty(_suffix))
            {
                // some data in test set has ,foobar on the end,
                // in this instance don't append a space.
                if (!_suffix.StartsWith(","))
                {
                    if (name.Length != 0)
                    {
                        name.Append(" ");
                    }
                }

                name.Append(_suffix);
            }

            return name;
        }
    }
}