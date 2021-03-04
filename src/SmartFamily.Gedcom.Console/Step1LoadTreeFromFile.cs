using SmartFamily.Gedcom.Enums;
using SmartFamily.Gedcom.Models;
using SmartFamily.Gedcom.Parser;

namespace SmartFamily.Gedcom.Console
{
    /// <summary>
    /// Tiny sample class on how to load a GEDCOM file.
    /// </summary>
    public static class Step1LoadTreeFromFile
    {
        /// <summary>
        /// Loads the presidents tree.
        /// </summary>
        /// <returns>A database reader that can be used to access the parsed database.</returns>
        public static GedcomDatabase LoadPresidentsTree()
        {
            var db = LoadGedcomFromFile();
            if (db == null)
            {
                return null;
            }

            System.Console.WriteLine("Loaded presidents test file.");
            return db;
        }

        private static GedcomDatabase LoadGedcomFromFile()
        {
            var gedcomReader = GedcomRecordReader.CreateReader("Data\\presidents.ged");
            if (gedcomReader.Parser.ErrorState != GedcomErrorState.NoError)
            {
                System.Console.WriteLine($"Could not read file, encountered error {gedcomReader.Parser.ErrorState} press a key to continue.");
                System.Console.ReadKey();
                return null;
            }

            return gedcomReader.Database;
        }
    }
}