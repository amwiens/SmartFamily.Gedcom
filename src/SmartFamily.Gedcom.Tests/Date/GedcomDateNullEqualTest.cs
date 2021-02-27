using SmartFamily.Gedcom.Models;

using Xunit;

namespace SmartFamily.Gedcom.Tests.Date
{
    /// <summary>
    /// Null comparison tests for dates, ensuring a real date never matches a null.
    /// </summary>
    public class GedcomDateNullEqualTest
    {
        [Fact]
        private void Two_null_dates_are_equal_using_type_comparison()
        {
            Assert.True(GedcomDate.Equals(null, null));
        }

        [Fact]
        private void Compare_first_date_against_null_is_not_equal_using_type_comparison()
        {
            Assert.False(GedcomDate.Equals(new GedcomDate(), null));
        }

        [Fact]
        private void Compare_second_date_against_null_is_not_equal_using_type_comparison()
        {
            Assert.False(GedcomDate.Equals(null, new GedcomDate()));
        }

        [Fact]
        private void Compare_empty_date_against_null_is_not_equal_using_instance_comparison()
        {
            var dateA = new GedcomDate();
            Assert.False(dateA.Equals(null));
        }
    }
}