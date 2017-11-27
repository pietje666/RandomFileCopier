using System.Collections.Generic;

namespace RandomFileCopier.Helpers
{
    interface IConfigurationHelper
    {
        string GetAppsetting(ExtensionsAppsettingKey key);
        IEnumerable<string> GetSettingAsIEnumerableOfString(ExtensionsAppsettingKey key);
        IEnumerable<ExtensionWithDefault> GetExtensions(ExtensionsAppsettingKey key);
    }
}