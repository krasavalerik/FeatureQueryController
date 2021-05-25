namespace WebApp.Network
{
    /// <summary>
    /// Указывает тип носителя, сведения о вложении сообщения электронной почты.
    /// </summary>
    public static class MediaTypeNames
    {
        /// <summary>
        /// Указывает тип данных приложения во вложении сообщения электронной почты.
        /// </summary>
        public static class Application
        {
            /// <summary>
            ///  Указывает, что значения кодируются в кортежах с ключом,
            ///     разделенных символом '&', с '=' между ключом и значением.
            /// </summary>
            public const string X_WWW_FORM_URL_ENCODED = "application/x-www-form-urlencoded";
        }
    }
}
