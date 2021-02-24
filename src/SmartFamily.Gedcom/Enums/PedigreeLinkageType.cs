namespace SmartFamily.Gedcom.Enums
{
    /// <summary>
    /// Indicates the child-to-family relationship for pedigree navigation purposes.
    /// </summary>
    public enum PedigreeLinkageType
    {
        /// <summary>
        /// Unknown
        /// </summary>
        Unknown = 0,

        /// <summary>
        /// Adopted
        /// </summary>
        Adopted,

        /// <summary>
        /// Biological
        /// </summary>
        Birth,

        /// <summary>
        /// Foster
        /// </summary>
        Foster,

        /// <summary>
        /// Sealing
        /// </summary>
        Sealing,

        // not part of standard GEDCOM
        // Family Tree Maker (at least in some versions) has cutom _FREL and _MREL tags
        // on CHIL in the FAM record

        /// <summary>
        /// Father Adopted
        /// </summary>
        FatherAdopted,

        /// <summary>
        /// Mother Adopted
        /// </summary>
        MotherAdopted,
    }
}