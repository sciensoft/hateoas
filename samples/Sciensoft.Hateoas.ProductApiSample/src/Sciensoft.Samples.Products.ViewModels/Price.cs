namespace Sciensoft.Samples.Products.ViewModels
{
    public class Price
    {
        public Price()
        { }

        public Price(decimal value, bool approved)
        {
            Value = value;
            Approved = approved;
        }

        public decimal Value { get; set; }

        public bool Approved { get; set; }

        public static implicit operator decimal(Price price)
            => price.Value;
    }
}
