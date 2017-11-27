using System;
using System.ComponentModel;
using Newtonsoft.Json;

namespace RandomFileCopier.Models
{
    class CopiedFile
    {

        public CopiedFile(string name, long size, DateTime copiedDateTime)
        {
            Name = name;
            Size = size;
            CopiedDatetime = copiedDateTime;
        }

        public string Name { get; set; }
        public long Size { get; set; }
        public DateTime CopiedDatetime { get; set; }
    }
}
