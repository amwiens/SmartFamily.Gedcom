using System;

namespace SmartFamily.Gedcom.Models
{
    /// <summary>
    /// Used to wrap DateTime.Now() so that it can be replaced for unit testing.
    /// </summary>
    public static class SystemTime
    {
        private static Func<DateTime> now = () => DateTime.Now;

        /// <summary>
        /// Gets the current time or the time under test for unit tests.
        /// </summary>
        public static DateTime Now
        {
            get
            {
                return now();
            }
        }

        /// <summary>
        /// Used to set the time to return when SystemTime.Now() is called.
        /// </summary>
        /// <param name="dateTimeNow">The time you want to return for the unit test.</param>
        public static void SetDateTime(DateTime dateTimeNow)
        {
            now = () => dateTimeNow;
        }

        /// <summary>
        /// Resets SystemTime.Now() to return the real time via DateTime.Now.
        /// </summary>
        public static void ResetDateTime()
        {
            now = () => DateTime.Now;
        }
    }
}