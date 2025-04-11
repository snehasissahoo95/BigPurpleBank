using BigPurpleBank.Application.Enums;

namespace BigPurpleBank.Model.DTOs
{
    public class AccountFilter
    {
        public ProductCategory? ProductCategory { get; set; }
        public OpenStatus OpenStatus { get; set; } = OpenStatus.ALL;
        public bool? IsOwned { get; set; }
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 25;
    }
}
