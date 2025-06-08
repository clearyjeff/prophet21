namespace Acme.P21.Data.Entities
{
    public class ItemQuantity
    {
        #region Public Properties

        public string ItemId
        {
            get;
            set;
        }

        public int Location
        {
            get;
            set;
        }

        public decimal QuantityAvailable
        {
            get;
            set;
        }

        public string Stockable
        {
            get;
            set;
        }

        public string Buyable
        {
            get;
            set;
        }

        public string ProductionProcessing
        {
            get;
            set;
        }


        public string ProductType
        {
            get;
            set;
        }

        #endregion
    }
}