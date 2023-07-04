using KeyVaultWebApi.Data;

namespace KeyVaultWebApi.Dto
{
    public class BatchResponseDto
    {
       
        public Guid BatchId { get; set; }
        public string Status { get; set; } 

        public List<Attributes> Attributes { get; set; }
        public string BusinessUnit { get; set; }
        public DateTime BatchPublicationDate { get; set; } 

        public DateTime ExpiryDate { get; set; }

        public List<Files> Files { get; set; }
    }
}
