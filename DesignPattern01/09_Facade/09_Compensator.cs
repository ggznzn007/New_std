namespace Facade
{
    class Compensator //하위 계층 서비스
    {
        public void Change(Picture picture, int tone, int brightness, int saturation)
        {
            picture.Change(tone, brightness, saturation);
        }
    }
}