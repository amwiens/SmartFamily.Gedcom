namespace SmartFamily.Gedcom.Console
{
    /// <summary>
    /// Sample console app showing how to read, query, change and save a GEDCOM file.
    /// </summary>
    internal class Program
    {
        /// <summary>
        /// App entry point.
        /// </summary>
        /// <param name="args"></param>
        private static void Main(string[] args)
        {
            var db = Step1LoadTreeFromFile.LoadPresidentsTree();
            if (db == null)
            {
                return;
            }

            Step2QueryTree.QueryTree(db);

            System.Console.WriteLine($"Count of people before adding new person - {db.Individuals.Count}.");
            Step3AddAPerson.AddPerson(db);
            System.Console.WriteLine($"Count of people after adding new person - {db.Individuals.Count}.");

            Step4SaveTree.Save(db);

            System.Console.WriteLine("Finished, press a key to continue.");
            System.Console.ReadKey();
        }
    }
}