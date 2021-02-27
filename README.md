# SmartFamily.Gedcom

A .Net Standard library for loading, saving, working with and analyzing family trees stored in the GEDCOM format.

## Installation



## Basic usage

Check the sample project out for working code, basic operations are;

### Loading a tree

To load a tree into memory use the following static helper.

    var gedcomReader = GedcomRecordReader.CreateReader("Data\\presidents.ged");

There are other variants of this helper and non static methods that allow you to specify additional parameters such as encoding.

You'll want to make sure that the file you just read was parsed OK and handle any failures;

    if (gedcomReader.Parser.ErrorState != Parser.Enums.GedcomErrorState.NoError)
    {
        Console.WriteLine($"Could not read file, encountered error {gedcomReader.Parser.ErrorState}.");
    }

### Querying the tree

    Console.WriteLine($"Found {db.Families.Count} families and {db.Individuals.Count} individuals.");
    var individual = db
        .Individuals
        .FirstOrDefault(f => f.Names.Any());

    if (individual != null)
    {
        Console.WriteLine($"Individual found with a preferred name of '{individual.GetName()}'.");
    }

### Adding a person to the tree

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
        EventType = Enums.GedcomEventType.Birth
    });

### Saving the tree

    GedcomRecordWriter.OutputGedcom(db, "Rewritten.ged");

### Current build status


### Code quality


### Contributing