namespace Facade
{
    class SmartManager // 사용자가 하위 계층의 기능을 쉽게 사용할 수 있게 제공
    {
        Compensator compensator = new Compensator(); //사진을 수정하는 개체
        PictureManager pic_manager = new PictureManager();//사진을 관리하는 개체

        public bool Exist(string name)
        {
            return pic_manager.Exist(name); //내부 개체 이용
        }

        public bool AddPicture(Picture picture)
        {
            return pic_manager.AddPicture(picture); //내부 개체 이용
        }

        Picture FindPicture(string name)
        {
            return pic_manager.FindPicture(name); //내부 개체 이용
        }

        public bool Change(string name, int tone, int brightness, int saturation)
        {
            Picture picture = FindPicture(name);
            if (picture == null)
            {
                return false;
            }
            picture.Change(tone, brightness, saturation); //내부 개체 이용
            return true;
        }

        public void View()
        {
            pic_manager.View(); //내부 개체 이용
        }
    }
}