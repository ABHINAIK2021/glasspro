namespace MyApp.Models
{
    public class ItemModel
    {
        public int ItemId { get; set; }
        public string ItemName { get; set; }
        public decimal Rate { get; set; }
        public int Qty { get; set; }
        public decimal Amount { get; set; }
        public int DiscountPercentage { get; set; }
        public decimal DiscountAmount { get; set; }
        public decimal NetAmount { get; set; }
        public int GSTPercentage { get; set; }
        public decimal GSTAmount { get; set; }
        public decimal GrossAmount { get; set; }
    }
}