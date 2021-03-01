using SmartFamily.Gedcom.Enums;

using System.IO;
using System.Text;

namespace SmartFamily.Gedcom.Parser
{
    /// <summary>
    /// Used by unit tests and benchmarks to load and parse GEDCOM files.
    /// </summary>
    public class GedcomLoader
    {
        public GedcomParser LoadAndParse(string file)
        {
            var encoder = new ASCIIEncoding();

            var parser = new GedcomParser
            {
                AllowTabs = false,
                AllowHyphenOrUnderscoreInTag = false
            };

            var dir = ".\\Data";
            var gedcomFile = Path.Combine(dir, file);
            var fi = new FileInfo(gedcomFile);

            using (var stream = new FileStream(gedcomFile, FileMode.Open, FileAccess.Read, FileShare.Read, (int)fi.Length))
            {
                int bufferSize = (int)fi.Length;
                byte[] buffer = new byte[bufferSize];
                int read = 0;
                while ((read = stream.Read(buffer, 0, bufferSize)) != 0)
                {
                    string input = encoder.GetString(buffer, 0, read).Trim();
                    var error = parser.GedcomParse(input);
                    if (error != GedcomErrorState.NoError)
                    {
                        return parser;
                    }
                }

                return parser;
            }
        }
    }
}