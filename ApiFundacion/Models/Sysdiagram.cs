﻿using System;
using System.Collections.Generic;

#nullable disable

namespace ApiFundacion.Models
{
    public partial class Sysdiagram
    {
        public string Name { get; set; }
        public int PrincipalId { get; set; }
        public int DiagramId { get; set; }
        public int? Version { get; set; }
        public byte[] Definition { get; set; }
        public char? Trial141 { get; set; }
    }
}
