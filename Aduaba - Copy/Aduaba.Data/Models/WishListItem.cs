namespace Aduaba.Data.Models
{
    public class WishListItem
    {
        public string Id { get; set; }
        public Product Product { get; set; }
        public string ProductId { get; set; }
        public WishList WishList { get; set; }
        public string WishListId { get; set; }

    }
}