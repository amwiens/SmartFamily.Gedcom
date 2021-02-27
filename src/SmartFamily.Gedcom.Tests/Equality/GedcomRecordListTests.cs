using SmartFamily.Gedcom.Models;

using Xunit;

namespace SmartFamily.Gedcom.Tests.Equality
{
    public class GedcomRecordListTests
    {
        [Fact]
        private void Hash_codes_for_identical_lists_are_the_same()
        {
            var list1 = new GedcomRecordList<string> { "item 1" };
            var list2 = new GedcomRecordList<string> { "item 1" };

            Assert.Equal(list1.GetHashCode(), list2.GetHashCode());
        }
    }
}