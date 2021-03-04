using SmartFamily.Gedcom.Enums;
using SmartFamily.Gedcom.Models;

namespace SmartFamily.Gedcom.Console
{
    /// <summary>
    /// Tiny sample class on how to add a person to a database.
    /// </summary>
    public static class Step3AddAPerson
    {
        /// <summary>
        /// Adds a sample person (well, a cartoon mouse) to the presidents file. The mouse may do a better job if elected president.
        /// </summary>
        /// <param name="db">The database to add the individual to.</param>
        public static void AddPerson(GedcomDatabase db)
        {
            var individual = new GedcomIndividualRecord(db);

            var name = individual.Names[0];
            name.Given = "Michael";
            name.Surname = "Mouse";
            name.Nick = "Mickey";

            individual.Names.Add(name);

            var birthDate = new GedcomDate(db);
            birthDate.ParseDateString("24 Jan 1933");
            individual.Events.Add(new GedcomIndividualEvent
            {
                Database = db,
                Date = birthDate,
                EventType = GedcomEventType.Birth,
            });

            System.Console.WriteLine($"Added record for '{individual.GetName().Name}' with birth date {individual.Birth.Date.Date1}.");
        }
    }
}