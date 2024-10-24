namespace SalesPredictionProject.Models.Entity
{
    public class SalesData
    {
        public long Id { get; set; }
        public DateTime Date { get; set; }
        public decimal SalesAmount { get; set; }
        public string ProductCategory { get; set; }
        public string Region { get; set; }
        public bool PromotionApplied { get; set; }
    }
}
