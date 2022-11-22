namespace TestNinja.Mocking
{
    public interface ICustomer
    {
        bool IsGold { get; set; }
    }

    public class Customer : ICustomer
    {
        public bool IsGold { get; set; }
    }

    public class Product
    {
        public float ListPrice { get; set; }

        public float GetPrice(ICustomer customer)
        {
            if (customer.IsGold)
                return ListPrice * 0.7f;

            return ListPrice;
        }
    }
}