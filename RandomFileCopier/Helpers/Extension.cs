using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RandomFileCopier.Helpers
{
    class ExtensionWithDefault
    {
        public ExtensionWithDefault(string extension, bool defaultSelected)
        {
            Extension = extension;
            DefaultSelected = defaultSelected;
        }

        public string Extension { get; set; }

        public bool DefaultSelected { get; set; }
    }
}
