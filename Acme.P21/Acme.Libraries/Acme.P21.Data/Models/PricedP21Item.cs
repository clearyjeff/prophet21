namespace Acme.P21.Data.Models
{
    public class PricedP21Item
    {
        public double UnitPrice { get; set; }
        public double BaseUnitPrice { get; set; }
        public string UOM { get; set; }
        public double UOMUnitSize { get; set; }
        public int PricePageUid { get; set; }
        public string PriceUOM { get; set; }
        public double PriceUnitSize { get; set; }
        public double ExtendedPrice { get; set; }
        public double CalcValue { get; set; }
        public double UnitCommissionCost { get; set; }
        public double UnitOtherCost { get; set; }
        public double UnitSalesCost { get; set; }
        public string LotCosted { get; set; }
        public string ItemId { get; set; }
        public object CompanyId { get; set; }
        public int LocationId { get; set; }
        public double QuantityAvailable { get; set; }
        public double QuantityOnHand { get; set; }
        public double QuantityAllocated { get; set; }
        public double QuantityNonPickable { get; set; }
        public double QuantityQuarantined { get; set; }
        public double QuantityFrozen { get; set; }
        public string LocationType { get; set; }
    }
}