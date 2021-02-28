using SmartFamily.Gedcom.Helpers;
using SmartFamily.Gedcom.Models;

using System.Collections.Generic;

namespace SmartFamily.Gedcom
{
    /// <summary>
    /// TODO: Doc
    /// </summary>
    /// <seealso cref="IndexedKeyCollection"/>
    public class XRefIndexedKeyCollection : IndexedKeyCollection
    {
        private GedcomDatabase _database;

        private readonly List<string> _replacementXRefs;

        private bool _replaceXrefs;

        /// <summary>
        /// Initializes a new instance of the <see cref="XRefIndexedKeyCollection"/> class.
        /// </summary>
        public XRefIndexedKeyCollection()
        {
            _replacementXRefs = new List<string>();
        }

        /// <summary>
        /// Gets or sets the database.
        /// </summary>
        /// <value>
        /// The database.
        /// </value>
        public GedcomDatabase Database
        {
            get => _database;
            set => _database = value;
        }

        /// <summary>
        /// Gets or sets a value indicating whether [replace x refs].
        /// </summary>
        /// <value>
        /// <c>true</c> if [replace x refs]; otherwise, <c>false</c>.
        /// </value>
        public bool ReplaceXRefs
        {
            get => _replaceXrefs;
            set => _replaceXrefs = value;
        }

        /// <summary>
        /// Gets the TODO: Doc
        /// </summary>
        /// <value>
        /// The <see cref="string"/>.
        /// </value>
        /// <param name="str">The string.</param>
        /// <param name="startIndex">The start index.</param>
        /// <param name="length">The length.</param>
        /// <returns>TODO: Doc</returns>
        public override string this[string str, int startIndex, int length]
        {
            get
            {
                bool found = Find(str, startIndex, length, out int pos);

                if (!found)
                {
                    Strings.Insert(pos, str.Substring(startIndex, length).Trim());
                    if (_replaceXrefs)
                    {
                        int prefixLen = 0;
                        while (char.IsLetter(str[prefixLen]))
                        {
                            prefixLen++;
                        }

                        string prefix;
                        if (prefixLen > 0)
                        {
                            prefix = str.Substring(0, prefixLen);
                        }
                        else
                        {
                            prefix = "XREF";
                        }

                        _replacementXRefs.Insert(pos, _database.GenerateXref(prefix));
                    }
                }

                string ret;
                if (_replaceXrefs)
                {
                    ret = _replacementXRefs[pos];
                }
                else
                {
                    ret = Strings[pos];
                }

                return ret;
            }
        }
    }
}