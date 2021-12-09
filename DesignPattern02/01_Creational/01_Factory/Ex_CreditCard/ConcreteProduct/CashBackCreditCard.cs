namespace FactoryMethod_ex
{
    //구조 상품 추상 클래스
    class CashBackCreditCard : CreditCard
    {
        private readonly string _cardType;
        private int _creditLimit;
        private int _annualCharge;

        public CashBackCreditCard(int creditLimit, int annualCharge)
        {
            _cardType = "CashBack";
            _creditLimit = creditLimit;
            _annualCharge = annualCharge;
        }

        public override string CardType
        {
            get { return _cardType; }
        }

        public override int CreditLimit
        {
            get { return _creditLimit; }
            set { _creditLimit = value; }
        }

        public override int AnnualCharge
        {
            get { return _annualCharge; }
            set { _annualCharge = value; }
        }
    }
}