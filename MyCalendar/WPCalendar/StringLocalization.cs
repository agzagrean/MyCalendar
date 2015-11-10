using WPCalendar.Resources;

namespace WPCalendar
{
    /// <summary>
    /// Provides access to string resources.
    /// </summary>
    internal class StringLocalization
    {
        private static ApplicationResources _resourceLocalization = new ApplicationResources();

        public ApplicationResources ResourceLocalization { get { return _resourceLocalization; } }
    }
}