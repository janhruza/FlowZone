using System.Security.Policy;

namespace ResourceRadar
{
    /// <summary>
    /// Representing a data class with various in-app messages.
    /// </summary>
    public static class Messages
    {
        /// <summary>
        /// Please provide a valid username and optionally the description too.
        /// </summary>
        public const string CREATE_USER_CHECK_FAILED = "Please provide a valid username, locale information and optionally the description too.";

        /// <summary>
        /// Internal error. Unable to create a new user.
        /// </summary>
        public const string CREATE_USER_FAILED = "Internal error. Unable to create a new user.";

        /// <summary>
        /// No user profile loaded. Please load created profile or create a new one.
        /// </summary>
        public const string NO_USER_LOGGED = "No user profile loaded. Please load created profile or create a new one.";

        /// <summary>
        /// No inventory item was created.
        /// </summary>
        public const string NO_ITEM_CREATED = "No inventory item was created.";

        /// <summary>
        /// Remove Item
        /// </summary>
        public const string REMOVE_ITEM_CAPTION = "Remove Item";
    }
}
