﻿using System.Text.Json.Serialization;

namespace KeyVaultWebApi.Data
{
    public class Acl
    {
        [JsonIgnore]
        public int Id { get; set; }
    
        public List<ReadUsers>? ReadUsers { get; set; }
    
        public List<ReadGroups>? ReadGroups { get; set; }

        [JsonIgnore]
        public Guid BatchId { get; set; }
    }
}
