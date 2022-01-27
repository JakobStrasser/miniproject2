using System;

namespace Miniproject2_JakobStrasser
{
    public class Phone : Asset
    {
        public Phone(string model, DateTime purchaseDate, double uSDPrice, int officeId, string phoneNumber, string pUK) : base(model, purchaseDate, uSDPrice, officeId)
        {
            PhoneNumber = phoneNumber;
            PUK = pUK;
        }

        public string PhoneNumber { get; set; }
        public string PUK { get; set; }
    }

}
