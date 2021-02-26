namespace SmartFamily.Gedcom.Parser
{
    /// <summary>
    /// Lists of error strings etc that don't change.
    /// </summary>
    public class StaticData
    {
        // TODO: These should be in the same place as the codes they map to otherwise the'll get out of sync. Change to Dictionary and merge.
        /// <summary>
        /// Descriptions for each parse error.
        /// </summary>
        public static readonly string[] ParseErrorDescriptions = new string[]
        {
            "No Error",

            "Level expected but not found",
            "Level needs trailing delimeter",
            "Level is invalid",

            "Xref id needs trailing delimeter",
            "Xref too long",

            "Tag expected",
            "Tag needs trailing delimeter or newline",

            "Line value expected",
            "Line value needs trailing newline",
            "Line value invalid",

            "Unknown Error",
        };
    }
}