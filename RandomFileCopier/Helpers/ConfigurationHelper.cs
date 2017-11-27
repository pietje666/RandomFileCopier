using System.Collections.Generic;
using System.Configuration;
using System.Linq;

namespace RandomFileCopier.Helpers
{
    enum ExtensionsAppsettingKey
    {
        VideoExtensions,
        AudioExtensions
    }

    class ConfigurationHelper 
        : IConfigurationHelper
    {       

        public string GetAppsetting(ExtensionsAppsettingKey key)
        {
            return ConfigurationManager.AppSettings[key.ToString()];
        }

        public IEnumerable<string> GetSettingAsIEnumerableOfString(ExtensionsAppsettingKey key)
        {
            return GetAppsetting(key).Split(';').Select(x =>x.Trim());
        }

        public IEnumerable<ExtensionWithDefault> GetExtensions(ExtensionsAppsettingKey key)
        {
          return  GetSettingAsIEnumerableOfString(key).Select(x =>
            {
                var splitted = x.Split(':');
                var defaultSelected = true;
                if (splitted.Length == 2)
                {
                    defaultSelected = bool.Parse(splitted.Last());
                }
                return new ExtensionWithDefault("." + splitted.First(), defaultSelected);
            });
        }
    }
}
