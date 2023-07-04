﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace KeyVaultWebApi.Data
{
    public class BusinessUnit
    {
        [JsonIgnore]
        public int Id { get; set; }
        public string UnitName { get; set; }

        //[JsonIgnore]
        //public Guid BatchId { get; set; }
    }
}
