namespace Acme.P21.Data.Models
{
    public class PriceRequest
    {
        public string ItemId
        {
            get;
            set;
        }

        public int SourceLocId => 1;

        public decimal UnitQuantity
        {
            get;
            set;
        }

    }
}