namespace FactoryMethod_ex
{
    class BlackFactory : CardFactory
    {
        private int _creditLimit;
        private int _annualCharge;

        public BlackFactory(int creditLimit, int annualCharge)
        {
            _creditLimit = creditLimit;
            _annualCharge = annualCharge;
        }

        public override CreditCard GetCreditCard()
        {
            return new BlackCreditCard(_creditLimit, _annualCharge);
        }
    }
}