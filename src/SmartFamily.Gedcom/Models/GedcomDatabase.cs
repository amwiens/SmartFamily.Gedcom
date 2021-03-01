using SmartFamily.Gedcom.Helpers;

using System.Collections;
using System.Collections.Generic;

namespace SmartFamily.Gedcom.Models
{
    /// <summary>
    /// The database for all the GEDCOM records.
    /// This is currently just in memory. To implement a "real"
    /// database you should derive from this class and override
    /// the necessary methods / properties.
    /// </summary>
    public class GedcomDatabase
    {
        private readonly List<GedcomIndividualRecord> _individuals;
        private readonly List<GedcomFamilyRecord> _families;
        private readonly List<GedcomSourceRecord> _sources;
        private readonly List<GedcomRepositoryRecord> _repositories;
        private readonly List<GedcomMultimediaRecord> _media;
        private readonly List<GedcomNoteRecord> _notes;
        private readonly List<GedcomSubmitterRecord> _submitters;

        private int _xrefCounter = 0;

        private readonly IndexedKeyCollection _placeNameCollection;

        // NOTE: having a collection for date strings saves memory
        // but kills GEDCOM reading time to an extent that it isn't worth it
        private readonly Dictionary<string, int> _surnames;

        /// <summary>
        /// Initializes a new instance of the <see cref="GedcomDatabase"/> class.
        /// </summary>
        public GedcomDatabase()
        {
            Table = new Hashtable();
            _individuals = new List<GedcomIndividualRecord>();
            _families = new List<GedcomFamilyRecord>();
            _sources = new List<GedcomSourceRecord>();
            _repositories = new List<GedcomRepositoryRecord>();
            _media = new List<GedcomMultimediaRecord>();
            _notes = new List<GedcomNoteRecord>();
            _submitters = new List<GedcomSubmitterRecord>();

            _placeNameCollection = new IndexedKeyCollection();

            _surnames = new Dictionary<string, int>();
        }

        /// <summary>
        /// Gets or sets the header for a GEDCOM file.
        /// </summary>
        public virtual GedcomHeader Header { get; set; }

        /// <summary>
        /// Gets or sets hashtable of all top level GEDCOM records, key is the XRef.
        /// Top level records are Individuals, Families, Sources, Repositories, and Media.
        /// </summary>
        public virtual Hashtable Table { get; set; }

        /// <summary>
        /// Gets total number of top level GEDCOM records in the database.
        /// Top level records are Individuals, Families, Sources, Repositories, and Media.
        /// </summary>
        public virtual int Count
        {
            get => Table.Count;
        }

        /// <summary>
        /// Gets the current GedcomRecord when enumerating the database.
        /// </summary>
        public virtual object Current
        {
            get => Table.GetEnumerator().Current;
        }

        /// <summary>
        /// Gets a list of all the Individuals in the database.
        /// </summary>
        public virtual List<GedcomIndividualRecord> Individuals
        {
            get => _individuals;
        }

        /// <summary>
        /// Gets a list of all the Families in the database.
        /// </summary>
        public virtual List<GedcomFamilyRecord> Families
        {
            get => _families;
        }

        /// <summary>
        /// Gets a list of all the sources in the database.
        /// </summary>
        public virtual List<GedcomSourceRecord> Sources
        {
            get => _sources;
        }

        /// <summary>
        /// Gets a list of all the repositories in the database.
        /// </summary>
        public virtual List<GedcomRepositoryRecord> Repositories
        {
            get => _repositories;
        }

        /// <summary>
        /// Gets a list of all the media items in the database.
        /// </summary>
        public virtual List<GedcomMultimediaRecord> Media
        {
            get => _media;
        }

        /// <summary>
        /// Gets a list of all the notes in the database.
        /// </summary>
        public virtual List<GedcomNoteRecord> Notes
        {
            get => _notes;
        }

        /// <summary>
        /// Gets a list of all the submitters in the database.
        /// </summary>
        public virtual List<GedcomSubmitterRecord> Submitters
        {
            get => _submitters;
        }

        /// <summary>
        /// Gets or sets the name of the database, this is currently the full filename
        /// of the GEDCOM file the database was read from / saved to,
        /// but could equally be a connection string for a real back-end database.
        /// </summary>
        public virtual string Name { get; set; }

        /// <summary>
        /// Gets all the names used in the database, used primarily to save
        /// memory by storing names only once.
        /// </summary>
        public IndexedKeyCollection NameCollection { get; } = new IndexedKeyCollection();

        /// <summary>
        /// Gets all the place names used in the database, used primarily to save
        /// memory by storing names only once.
        /// </summary>
        public IndexedKeyCollection PlaceNameCollection
        {
            get => _placeNameCollection;
        }

        /// <summary>
        /// Gets or sets utility property providing all the surnames in the database, along with
        /// a count of how many people have that surname.
        /// </summary>
        public virtual Dictionary<string, int> Surnames { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the database is being loaded.
        /// </summary>
        public bool Loading { get; set; }

        /// <summary>
        /// Gets or sets the GedcomRecord associated with the given XRef
        /// </summary>
        /// <param name="key">TODO: Doc.</param>
        /// <returns></returns>
        public virtual GedcomRecord this[string key]
        {
            get => Table[key] as GedcomRecord;
            set
            {
                Remove(key, value);
                Add(key, value);
            }
        }

        /// <summary>
        /// Determines whether the specified <see cref="object"/>, is equal (in contents, not structure) to this instance.
        /// </summary>
        /// <param name="gedcomDb">The <see cref="object"/> to compare with this instance.</param>
        /// <returns><c>true</c> if the specified <see cref="object"/> is equal to this instance; otherwise, <c>false</c>.</returns>
        public bool Equals(GedcomDatabase gedcomDb)
        {
            if (gedcomDb == null)
            {
                return false;
            }

            if (!Equals(Header, gedcomDb.Header))
            {
                return false;
            }

            if (!GedcomGenericListComparer.CompareGedcomRecordLists(Individuals, gedcomDb.Individuals))
            {
                return false;
            }

            return true;
        }

        /// <inheritdoc/>
        public override bool Equals(object obj)
        {
            return Equals(obj as GedcomDatabase);
        }

        /// <inheritdoc/>
        public override int GetHashCode()
        {
            // Overflow is fine, just wrap.
            unchecked
            {
                int hash = 17;
                hash *= 23 + Individuals.GetHashCode();

                return hash;
            }
        }

        /// <summary>
        /// Add the given record to the database with the given XRef.
        /// </summary>
        /// <param name="xrefID">A <see cref="string"/>.</param>
        /// <param name="record">A <see cref="GedcomRecord"/>.</param>
        public virtual void Add(string xrefID, GedcomRecord record)
        {
            Table.Add(xrefID, record);

            if (record is GedcomIndividualRecord)
            {
                GedcomIndividualRecord indi = (GedcomIndividualRecord)record;

                int pos = _individuals.BinarySearch(indi);
                if (pos < 0)
                {
                    pos = ~pos;
                }

                _individuals.Insert(pos, indi);
            }
            else if (record is GedcomFamilyRecord)
            {
                _families.Add((GedcomFamilyRecord)record);
            }
            else if (record is GedcomSourceRecord)
            {
                GedcomSourceRecord source = (GedcomSourceRecord)record;

                int pos = _sources.BinarySearch(source);
                if (pos < 0)
                {
                    pos = ~pos;
                }

                _sources.Insert(pos, source);
            }
            else if (record is GedcomRepositoryRecord)
            {
                GedcomRepositoryRecord repo = (GedcomRepositoryRecord)record;

                int pos = _repositories.BinarySearch(repo);
                if (pos < 0)
                {
                    pos = ~pos;
                }

                _repositories.Insert(pos, repo);
            }
            else if (record is GedcomMultimediaRecord)
            {
                _media.Add((GedcomMultimediaRecord)record);
            }
            else if (record is GedcomNoteRecord)
            {
                _notes.Add((GedcomNoteRecord)record);
            }
            else if (record is GedcomSubmitterRecord)
            {
                _submitters.Add((GedcomSubmitterRecord)record);
            }

            record.Database = this;
        }

        /// <summary>
        /// Builds up the surname list for use with the Surnames property.
        /// </summary>
        public virtual void BuildSurnameList()
        {
            foreach (GedcomIndividualRecord indi in _individuals)
            {
                BuildSurnameList(indi);
            }
        }

        /// <summary>
        /// Remove the given record with the given XREf from the database.
        /// </summary>
        /// <param name="xrefID">A <see cref="string"/>.</param>
        /// <param name="record">A <see cref="GedcomRecord"/>.</param>
        public virtual void Remove(string xrefID, GedcomRecord record)
        {
            if (Table.Contains(xrefID))
            {
                Table.Remove(xrefID);

                if (record is GedcomIndividualRecord)
                {
                    GedcomIndividualRecord indi = (GedcomIndividualRecord)record;

                    _individuals.Remove(indi);

                    // remove names from surname cache
                    foreach (GedcomName name in indi.Names)
                    {
                        // TODO: not right, need to include prefix + suffix
                        string surname = name.Surname;

                        if (_surnames.ContainsKey(surname))
                        {
                            int count = _surnames[surname];
                            count--;
                            if (count > 0)
                            {
                                _surnames[surname] = count;
                            }
                            else
                            {
                                _surnames.Remove(surname);
                            }
                        }
                    }
                }
                else if (record is GedcomFamilyRecord)
                {
                    _families.Remove((GedcomFamilyRecord)record);
                }
                else if (record is GedcomSourceRecord)
                {
                    _sources.Remove((GedcomSourceRecord)record);
                }
                else if (record is GedcomRepositoryRecord)
                {
                    _repositories.Remove((GedcomRepositoryRecord)record);
                }
                else if (record is GedcomMultimediaRecord)
                {
                    _media.Remove((GedcomMultimediaRecord)record);
                }
                else if (record is GedcomNoteRecord)
                {
                    _notes.Remove((GedcomNoteRecord)record);
                }
                else if (record is GedcomSubmitterRecord)
                {
                    _submitters.Remove((GedcomSubmitterRecord)record);
                }

                // TODO: should we set this to null? part of the deletion
                // methods may still want to access the database
                // record.Database = null;
            }
        }

        /// <summary>
        /// Does the database contain a record with the given XRef.
        /// </summary>
        /// <param name="xrefID">A <see cref="string"/>.</param>
        /// <returns>A <see cref="bool"/>.</returns>
        public virtual bool Contains(string xrefID)
        {
            return Table.Contains(xrefID);
        }

        /// <summary>
        /// Advances the enumerator to the next element of the collection.
        /// </summary>
        /// <returns>
        /// True if the enumerator was successfully advanced to the next element;
        /// False if the enumerator has passed the end of the collection.
        /// </returns>
        public virtual bool MoveNext()
        {
            return Table.GetEnumerator().MoveNext();
        }

        /// <summary>
        /// Sets the enumerator to its initial position, which is before the first element in the collection.
        /// </summary>
        public virtual void Reset()
        {
            Table.GetEnumerator().Reset();
        }

        /// <summary>
        /// TODO: Doc.
        /// </summary>
        /// <returns>TODO: Doc 2.</returns>
        public virtual IDictionaryEnumerator GetEnumerator()
        {
            return Table.GetEnumerator();
        }

        /// <summary>
        /// Create a new XRef.
        /// </summary>
        /// <param name="prefix">A <see cref="string"/>.</param>
        /// <returns>A <see cref="string"/> TODO: Doc.</returns>
        public string GenerateXref(string prefix)
        {
            return string.Format("{0}{1}", prefix, (++_xrefCounter).ToString());
        }

        /// <summary>
        /// Combines the given database with this one.
        /// This is literally what it says, no duplicate removal is performed
        /// combine will not take place if there are duplicate xrefs.
        /// </summary>
        /// <param name="database">A <see cref="GedcomDatabase"/>.</param>
        /// <returns>A <see cref="bool"/>.</returns>
        public virtual bool Combine(GedcomDatabase database)
        {
            // check the databases can be combined, i.e. unique xrefs
            bool canCombine = true;
            foreach (GedcomRecord record in database.Table.Values)
            {
                if (Contains(record.XRefID))
                {
                    canCombine = false;
                    break;
                }
            }

            if (canCombine)
            {
                foreach (GedcomRecord record in database.Table.Values)
                {
                    Add(record.XRefID, record);
                }
            }

            return canCombine;
        }

        /// <summary>
        /// Add the given individual to the surnames list.
        /// </summary>
        /// <param name="indi">A <see cref="GedcomIndividualRecord"/>.</param>
        protected virtual void BuildSurnameList(GedcomIndividualRecord indi)
        {
            foreach (GedcomName name in indi.Names)
            {
                // TODO: not right, need to include prefix + suffix
                string surname = name.Surname;

                if (!_surnames.ContainsKey(surname))
                {
                    _surnames[surname] = 1;
                }
                else
                {
                    _surnames[surname] = 1 + _surnames[surname];
                }
            }
        }
    }
}