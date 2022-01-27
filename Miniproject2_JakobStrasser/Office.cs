using System.Collections.Generic;

namespace Miniproject2_JakobStrasser
{
    public class Office
    {
        public Office(string country, string currency, string culture)
        {

            Country = country;
            Currency = currency;
            Culture = culture;

        }

        public int Id { get; set; }
        public string Country { get; set; }
        public string Currency { get; set; }
        public string Culture { get; set; }
        public List<Asset> Assets { get; set; }

    }

}
