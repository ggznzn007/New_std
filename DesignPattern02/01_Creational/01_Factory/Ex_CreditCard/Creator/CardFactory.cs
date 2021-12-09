namespace FactoryMethod_ex
{
    // 상품생성 추상 클래스
    abstract class CardFactory
    {
        public abstract CreditCard GetCreditCard();
    }
}