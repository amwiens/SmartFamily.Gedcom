using SmartFamily.Gedcom.Models;

using System.Linq;

using Xunit;

namespace SmartFamily.Gedcom.Tests.Date
{
    /// <summary>
    /// Unit tests and data for ensuring changed dates and times are output in correct format.
    /// </summary>
    public class GedcomDateOutputTest
    {
        [Fact]
        private void Date_when_output_is_in_english_and_not_the_culture_of_the_current_thread()
        {
            SystemTime.SetDateTime(new System.DateTime(2020, 12, 13));
            System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("de-DE");
            var gedcom = new GedcomDatabase { Header = new GedcomHeader(), };

            var individual = new GedcomIndividualRecord(gedcom, "O'Neill");

            Assert.Equal("13 Dec 2020", individual.Names.Single().ChangeDate.Date1);
        }

        [Fact]
        private void Time_when_output_is_24_hour_format()
        {
            SystemTime.SetDateTime(new System.DateTime(2020, 12, 13, 18, 30, 59));
            var gedcom = new GedcomDatabase { Header = new GedcomHeader(), };

            var individual = new GedcomIndividualRecord(gedcom, "O'Neill");

            Assert.Equal("18:30:59", individual.Names.Single().ChangeDate.Time);
        }
    }
}