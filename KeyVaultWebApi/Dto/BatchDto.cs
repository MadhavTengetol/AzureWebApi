using KeyVaultWebApi.Data;
using System.Text.Json.Serialization;

namespace KeyVaultWebApi.Dto
{
    public class BatchDto
    {
        [JsonIgnore]
        public Guid BatchId { get; set; }
        [JsonIgnore]
        public string Status { get; set; } = "Incomplete";

        public string BusinessUnit { get; set; }
        public Acl Acl { get; set; }
        public List<Attributes> Attributes { get; set; }

        public DateTime ExpiryDate { get; set; }  = DateTime.UtcNow.AddDays(1);

        public List<Files> Files { get; set; }
    }
}
