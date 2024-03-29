﻿using System;
using System.ComponentModel;
using Newtonsoft.Json;

namespace RandomFileCopier.Models
{
    class MovedOrCopiedFile
    {

        public MovedOrCopiedFile(string name, long size, DateTime copiedDateTime)
        {
            Name = name;
            Size = size;
            CopiedDatetime = copiedDateTime;
        }

        public string Name { get; set; }
        public long Size { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        public DateTime CopiedDatetime { get; set; }
    }
}
