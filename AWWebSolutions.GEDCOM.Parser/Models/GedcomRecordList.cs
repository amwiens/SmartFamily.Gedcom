using System.Collections.ObjectModel;

namespace AWWebSolutions.GEDCOM.Parser.Models
{
    /// <summary>
    /// TODO: Doc + i think we might be able to use an Observable List instead.
    /// </summary>
    /// <typeparam name="T">TODO: Not sure what uses this yet.</typeparam>
    /// <seealso cref="System.Collections.Generic.List{T}"/>
    public class GedcomRecordList<T> : ObservableCollection<T>
    {
        public override int GetHashCode()
        {
            int hc = 0;
            if (Items != null)
            {
                foreach (var p in Items)
                {
                    hc ^= p.GetHashCode();
                }
            }

            return hc;
        }
    }
}