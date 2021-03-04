using SmartFamily.Gedcom.Models;
using SmartFamily.Gedcom.Parser;

namespace SmartFamily.Gedcom.Console
{
    /// <summary>
    /// Tiny sample class on how to save a GEDCOM file.
    /// </summary>
    public static class Step4SaveTree
    {
        /// <summary>
        /// Saves the sample database out to a new file.
        /// </summary>
        /// <param name="db">The database to save.</param>
        public static void Save(GedcomDatabase db)
        {
            GedcomRecordWriter.OutputGedcom(db, "Rewritten.ged");
            System.Console.WriteLine($"Output database to rewritten.ged.");
        }
    }
}