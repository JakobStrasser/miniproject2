namespace Miniproject2_JakobStrasser
{
    public class Currency
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public double Rate { get; set; }

        public Currency(string name, double rate)
        {
            Name = name;
            Rate = rate;
        }
    }

}
