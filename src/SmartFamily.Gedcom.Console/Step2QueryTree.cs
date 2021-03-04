using SmartFamily.Gedcom.Models;

using System.Linq;

namespace SmartFamily.Gedcom.Console
{
    /// <summary>
    /// Tiny sample class on how to query a GEDCOM file.
    /// </summary>
    public static class Step2QueryTree
    {
        /// <summary>
        /// Queries the tree for any individual with a name, just to show how to query.
        /// </summary>
        /// <param name="db">The database to query.</param>
        public static void QueryTree(GedcomDatabase db)
        {
            System.Console.WriteLine($"Found {db.Families.Count} families and {db.Individuals.Count} individuals.");
            var individual = db
                .Individuals
                .FirstOrDefault(f => f.Names.Any());

            if (individual == null)
            {
                System.Console.WriteLine($"Couldn't find any individuals in the GEDCOM file with a name, which is odd!");
                return;
            }

            System.Console.WriteLine($"Individual found with a preferred name of '{individual.GetName().Name}'.");
        }
    }
}