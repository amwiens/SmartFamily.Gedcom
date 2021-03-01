using SmartFamily.Gedcom.Enums;

using System;
using System.Linq;

namespace SmartFamily.Gedcom.Models
{
    /// <summary>
    /// TODO: Doc
    /// </summary>
    public class GedcomRecordedEvent : IComparable<GedcomRecordedEvent>, IComparable, IEquatable<GedcomRecordedEvent>
    {
        private GedcomDatabase _database;

        private GedcomRecordList<GedcomEventType> _types;
        private GedcomDate _date;
        private GedcomPlace _place;

        private GedcomChangeDate _changeDate;

        /// <summary>
        /// Initializes a new instance of the <see cref="GedcomRecordedEvent"/> class.
        /// </summary>
        public GedcomRecordedEvent()
        {
        }

        /// <summary>
        /// Gets or sets the date that this record was changed.
        /// </summary>
        /// <value>
        /// the date of the change.
        /// </value>
        public GedcomChangeDate ChangeDate { get; set; }

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
        /// Gets or sets the types.
        /// </summary>
        /// <value>
        /// The types.
        /// </value>
        public GedcomRecordList<GedcomEventType> Types
        {
            get
            {
                if (_types == null)
                {
                    _types = new GedcomRecordList<GedcomEventType>();
                }

                return _types;
            }
            set
            {
                if (_types != value)
                {
                    _types = value;
                    Changed();
                }
            }
        }

        /// <summary>
        /// Gets or sets the date.
        /// </summary>
        /// <value>
        /// The date.
        /// </value>
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
        /// <value>
        /// The place.
        /// </value>
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
        /// Compares this event to another record.
        /// </summary>
        /// <param name="other">A recorded event.</param>
        /// <returns>
        /// &lt;0 if the first event precedes the second in the sort order;
        /// &gt;0 if the second event precedes the first;
        /// 0 if the events are equal.
        /// </returns>
        public int CompareTo(GedcomRecordedEvent other)
        {
            if (other == null)
            {
                return 1;
            }

            var compare = CompareEvents(Types, other.Types);
            if (compare != 0)
            {
                return compare;
            }

            compare = GedcomGenericComparer.SafeCompareOrder(Date, other.Date);
            if (compare != 0)
            {
                return compare;
            }

            compare = GedcomGenericComparer.SafeCompareOrder(Place, other.Place);
            if (compare != 0)
            {
                return compare;
            }

            return compare;
        }

        /// <summary>
        /// Compares this event to another record.
        /// </summary>
        /// <param name="obj">A recorded event.</param>
        /// <returns>
        /// &lt;0 if the first event precedes the second in the sort order;
        /// &gt;0 if the second event precedes the first;
        /// 0 if the events are equal.
        /// </returns>
        public int CompareTo(object obj)
        {
            return CompareTo(obj as GedcomRecordedEvent);
        }

        /// <summary>
        /// Compare the GedcomRecordedEvent against the passed instance for similarity.
        /// </summary>
        /// <param name="other">The other instance to compare this instance against.</param>
        /// <returns>True if other instance matches this instance, otherwise False.</returns>
        public bool Equals(GedcomRecordedEvent other)
        {
            return CompareTo(other) == 0;
        }

        /// <summary>
        /// Compare the GedcomREcordedEvent against the passed instance for similarity.
        /// </summary>
        /// <param name="obj">The other instance to compare this instance against.</param>
        /// <returns>True if other instance matches this instance, otherwise False.</returns>
        public override bool Equals(object obj)
        {
            return CompareTo(obj as GedcomRecordedEvent) == 0;
        }

        public override int GetHashCode()
        {
            return new
            {
                Types,
                Date,
                Place,
            }.GetHashCode();
        }

        /// <summary>
        /// Updates the changed date and time.
        /// </summary>
        protected virtual void Changed()
        {
            if (_database == null)
            {
                //System.Console.WriteLine("Changed() called on record with no database set");

                //System.Diagnostics.StackTrace trace = new System.Diagnostics.StackTrace();
                //foreach (System.Diagnostics.StackFrame f in trace.GetFrames())
                //{
                //    System.Console.WriteLine(f);
                //}
            }
            else if (!_database.Loading)
            {
                if (_changeDate == null)
                {
                    _changeDate = new GedcomChangeDate(Database); // TODO: what level?
                }

                DateTime now = DateTime.Now;

                _changeDate.Date1 = now.ToString("dd MMM yyyy");
                _changeDate.Time = now.ToString("hh:mm:ss");
            }
        }

        private static int CompareEvents(GedcomRecordList<GedcomEventType> list1, GedcomRecordList<GedcomEventType> list2)
        {
            if (list1.Count > list2.Count)
            {
                return 1;
            }

            if (list1.Count < list2.Count)
            {
                return -1;
            }

            var sortedList1 = list1.OrderBy(n => n.GetHashCode()).ToList();
            var sortedList2 = list2.OrderBy(n => n.GetHashCode()).ToList();
            for (var i = 0; i < sortedList1.Count; i++)
            {
                var compare = sortedList1.ElementAt(i).CompareTo(sortedList2.ElementAt(i));
                if (compare != 0)
                {
                    return compare;
                }
            }

            return 0;
        }
    }
}