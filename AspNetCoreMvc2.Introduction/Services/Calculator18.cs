namespace AspNetCoreMvc2.Introduction.Services
{
    public class Calculator18 : ICalculator
    {
        public decimal Calculate(decimal amount)
        {
            return amount + (amount * 18 / 100);
        }
    }
}
