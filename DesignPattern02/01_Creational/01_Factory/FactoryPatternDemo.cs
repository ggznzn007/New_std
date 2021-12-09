using System;

namespace FactoryMethod_ex
{
    // 데모
    public class ClientApplication
    {
        static void Main()
        {
            CardFactory factory = null;
            Console.Write("Enter the card type you would like to visit: ");
            string cardType = Console.ReadLine();

            switch (cardType.ToLower())
            {
                case "cashback":
                    factory = new CashBackFactory(50000, 0);
                    break;
                case "titanium":
                    factory = new TitaniumFactory(100000, 500);
                    break;
                case "platinum":
                    factory = new PlatinumFactory(500000, 1000);
                    break;
                case "black":
                    factory = new PlatinumFactory(10000000, 1000);
                    break;
                default:
                    break;
            }

            CreditCard creditCard = factory.GetCreditCard();
            Console.WriteLine("\nYour card details are below : \n");
            Console.WriteLine("Card Type: {0}\nCredit Limit: {1}\nAnnual Charge: {2}",
                creditCard.CardType, creditCard.CreditLimit, creditCard.AnnualCharge);
            Console.ReadKey();
        }
    }
}