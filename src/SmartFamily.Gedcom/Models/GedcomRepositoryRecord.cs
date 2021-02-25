using System;

namespace SmartFamily.Gedcom.Models
{
    /// <summary>
    /// An institution or person that has the specified item as part of their collection(s).
    /// </summary>
    /// <seealso cref="GedcomRecord"/>
    /// <seealso cref="IComparable"/>
    public class GedcomRepositoryRecord : GedcomRecord, IComparable, IComparable<GedcomRepositoryRecord>, IEquatable<GedcomRepositoryRecord>
    {
    }
}