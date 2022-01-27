using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Miniproject2_JakobStrasser
{
    public class Asset
    {
        public Asset(string model, DateTime purchaseDate, double uSDPrice, int officeId)
        {
            Model = model;

            PurchaseDate = purchaseDate;
            USDPrice = uSDPrice;
            OfficeId = officeId;
        }

        public int Id { get; set; }
        public string Model { get; set; }

        public DateTime PurchaseDate { get; set; }
        public double USDPrice { get; set; }
        public int OfficeId { get; set; }

        //Don't save this to table
        [NotMapped]
        public int DaysOld { get { return (int)DateTime.Now.Subtract(PurchaseDate).TotalDays; } }
    }

}
