namespace Sciensoft.Samples.Products.ViewModels
{
    public class Product
    {
        public string Name { get; set; }

        public string PhotoReference { get; set; }

        public class Output : Product, IVersionedViewModel
        {
            public string ETag { get; set; }

            public string Code { get; set; }

            public decimal Price { get; set; }

            public int Version { get; private set; }
        }

        public class Create : Product
        {
            public string Code { get; set; }

            public Price Price { get; set; }
        }

        public class Update : Product
        {
            public Price Price { get; set; }
        }

        public class Delete
        {
            public string Reason { get; set; }
        }
    }
}
