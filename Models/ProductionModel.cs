namespace MyApp.Models
{
    public class ProductionModel
    {
        public int ItemId { get; set; }
        public string ItemName { get; set; }
        public int Qty { get; set; }
        public bool Cutting { get; set; }
        public bool Polishing { get; set; }
        public bool Fabrication { get; set; }
        public bool Toughening { get; set; }
        public bool DGU { get; set; }
        public string WorkOrderNo { get; set; }
        public int Count { get; set; }
        public int SrNo { get; set; }
        public int ActualQty { get; set; }
        public int BalancedQty { get; set; }
        public int ProducedQty { get; set; }
        public int ActualBalancedQty { get; set; }
        public int ActualProducedQty { get; set; }
    }
}