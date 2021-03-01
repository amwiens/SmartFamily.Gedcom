using System;
using System.IO;

namespace SmartFamily.Gedcom.Models
{
    /// <summary>
    /// Used for holding the age of an individual for a given event,
    /// this is an object rather than it just being a straight forward
    /// number to allow for vague values to be given, e.g. &lt; 10
    /// </summary>
    public class GedcomAge
    {
        private int _equality = 0; // -1 if <, 0 if =, 1 if >

        private int _years = -1;
        private int _months = -1;
        private int _days = -1;

        /// <summary>
        /// Initializes a new instance of the <see cref="GedcomAge"/> class.
        /// </summary>
        public GedcomAge()
        {
        }

        /// <summary>
        /// Gets or sets the date on which the age of an individual changed.
        /// </summary>
        public GedcomChangeDate ChangeDate { get; set; }

        /// <summary>
        /// Gets or sets
        /// </summary>
        /// TODO: Docs
        public int Equality
        {
            get => _equality;
            set
            {
                if (value != _equality)
                {
                    _equality = value;
                    Changed();
                }
            }
        }

        /// <summary>
        /// Gets a value indicating whether the individual is considered stillborn.
        /// </summary>
        public bool StillBorn
        {
            get => _equality == 0 && _years == 0 && _months == 0 && _days == 0;
        }

        /// <summary>
        /// Gets a value indicating whether the individual is considered an infant.
        /// </summary>
        public bool Infant
        {
            get => _equality == 0 && _years < 1;
        }

        /// <summary>
        /// Gets a value indicating whether the individual is considered a child.
        /// </summary>
        public bool Child
        {
            get => _equality == 0 && _years < 8;
        }

        /// <summary>
        /// Gets or sets the year portion of the individual's age.
        /// </summary>
        public int Years
        {
            get => _years;
            set
            {
                if (value != _years)
                {
                    _years = value;
                    Changed();
                }
            }
        }

        /// <summary>
        /// Gets or sets the month portion of the individual's age.
        /// </summary>
        public int Months
        {
            get => _months;
            set
            {
                if (value != _months)
                {
                    _months = value;
                    Changed();
                }
            }
        }

        /// <summary>
        /// Gets or sets the day portion of the individual's age.
        /// </summary>
        public int Days
        {
            get => _days;
            set
            {
                if (value != _days)
                {
                    _days = value;
                    Changed();
                }
            }
        }

        private GedcomDatabase Database { get; set; }

        /// <summary>
        /// Parses a string for a GEDCOM format date.
        /// </summary>
        /// <param name="str">The string to parse.</param>
        /// <param name="database">The database to associate the result of the parsing with.</param>
        /// <returns>The parsed age or a null if the date is not recognized.</returns>
        public static GedcomAge Parse(string str, GedcomDatabase database)
        {
            GedcomAge age = null;

            if (string.Compare(str, "INFANT", true) == 0)
            {
                age = new GedcomAge
                {
                    Database = database,
                    Equality = -1,
                    Years = 1
                };
            }
            else if (string.Compare(str, "CHILD", true) == 0)
            {
                age = new GedcomAge
                {
                    Database = database,
                    Equality = -1,
                    Years = 8
                };
            }
            else if (string.Compare(str, "STILLBORN", true) == 0)
            {
                age = new GedcomAge
                {
                    Database = database,
                    Equality = 0,
                    Years = 0,
                    Months = 0,
                    Days = 0
                };
            }
            else
            {
                int equality = 0;
                int off = 0;
                if (str[0] == '<')
                {
                    equality = -1;
                    off = 1;
                }
                else if (str[0] == '>')
                {
                    equality = 1;
                    off = 1;
                }

                int val = -1;
                bool isAge = true;
                int year = -1;
                int month = -1;
                int day = -1;
                while (isAge && (off < str.Length))
                {
                    char c = str[off];

                    if (!char.IsWhiteSpace(c))
                    {
                        bool isDigit = char.IsDigit(c);

                        if (val == -1 && !isDigit)
                        {
                            isAge = false;
                        }
                        else if (isDigit)
                        {
                            int thisVal = val = c - '0';
                            if (val == -1)
                            {
                                val = thisVal;
                            }
                            else
                            {
                                val *= 10;
                                val += thisVal;
                            }
                        }
                        else if (c == 'Y' || c == 'y')
                        {
                            if (year != -1)
                            {
                                isAge = false;
                            }
                            else
                            {
                                year = val;
                                val = -1;
                            }
                        }
                        else if (c == 'M' || c == 'm')
                        {
                            if (month != -1)
                            {
                                isAge = false;
                            }
                            else
                            {
                                month = val;
                                val = -1;
                            }
                        }
                        else if (c == 'D' || c == 'd')
                        {
                            if (day != -1)
                            {
                                isAge = false;
                            }
                            else
                            {
                                day = val;
                                val = -1;
                            }
                        }
                        else
                        {
                            isAge = false;
                        }
                    }

                    off++;
                }

                isAge &= year != -1 || month != -1 || day != -1;

                if (isAge)
                {
                    age = new GedcomAge
                    {
                        Database = database,
                        Equality = equality,
                        Years = year,
                        Months = month,
                        Days = day
                    };
                }
            }

            return age;
        }

        /// <summary>
        /// Output GEDCOM formatted text representing the age.
        /// </summary>
        /// <param name="sw">The writer to output to.</param>
        /// <param name="level">The GEDCOM level.</param>
        public void Output(TextWriter sw, int level)
        {
            sw.Write(Environment.NewLine);
            sw.Write(level);
            sw.Write(" AGE ");

            // never write out INFANT CHILD, this potentially loses information,
            // always write out < 1 or < 8 and includes months days if set
            if (StillBorn)
            {
                sw.Write("STILLBORN");
            }
            else
            {
                if (Equality < 0)
                {
                    sw.Write("< ");
                }
                else if (Equality > 0)
                {
                    sw.Write("> ");
                }

                if (Years != -1)
                {
                    sw.Write(Years.ToString());
                    sw.Write("y ");
                }

                if (Months != -1)
                {
                    sw.Write(Months.ToString());
                    sw.Write("m ");
                }
                // TODO: change this to just an if instead of else if?
                else if (Days != -1)
                {
                    sw.Write(Days.ToString());
                    sw.Write("d");
                }
            }
        }

        /// <summary>
        /// Called after one of the date elements for this instance is changed.
        /// </summary>
        protected virtual void Changed()
        {
            if (Database == null)
            {
                //System.Console.WriteLine("Changed() called on record with no database set");

                //System.Diagnostics.StackTrace trace = new System.Diagnostics.StackTrace();
                //foreach (System.Diagnostics.StackFrame f in trace.GetFrames())
                //{
                //    System.Console.WriteLine(f);
                //}
            }
            else if (!Database.Loading)
            {
                if (ChangeDate == null)
                {
                    ChangeDate = new GedcomChangeDate(Database); // TODO: what level?
                }

                // TODO: change to SystemTime?
                DateTime now = DateTime.Now;

                ChangeDate.Date1 = now.ToString("dd MMM yyyy");
                ChangeDate.Time = now.ToString("hh:mm:ss");
            }
        }
    }
}