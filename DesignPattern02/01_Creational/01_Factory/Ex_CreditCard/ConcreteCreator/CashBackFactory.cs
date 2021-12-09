namespace FactoryMethod_ex
{
    // 상품 구조 클래스
    class CashBackFactory : CardFactory
    {
        private int _creditLimit;
        private int _annualCharge;

        public CashBackFactory(int creditLimit, int annualCharge)
        {
            _creditLimit = creditLimit;
            _annualCharge = annualCharge;
        }

        public override CreditCard GetCreditCard()
        {
            return new CashBackCreditCard(_creditLimit, _annualCharge);
        }
    }
}