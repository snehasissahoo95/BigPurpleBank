using BigPurpleBank.Application.Enums;

namespace BigPurpleBank.Model.Entities
{
    public class Account
    {
        public Guid AccountId { get; set; }
        public DateTime CreationDate { get; set; }
        public string DisplayName { get; set; }
        public string Nickname { get; set; }
        public OpenStatus OpenStatus { get; set; }
        public bool IsOwned { get; set; }
        public string AccountOwnership { get; set; }
        public string MaskedNumber { get; set; }
        public ProductCategory ProductCategory { get; set; }
        public string ProductName { get; set; }
    }
}
