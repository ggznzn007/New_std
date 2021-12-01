using System.Collections.Generic;
namespace Facade
{
    class PictureManager //하위 계층 서비스
    {
        List<Picture> pictures = new List<Picture>();
        public bool Exist(string name)
        {
            return FindPicture(name) != null;
        }
        public bool AddPicture(Picture picture)
        {
            if (Exist(picture.Name))
            {
                return false;
            }
            pictures.Add(picture);
            return true;
        }

        public Picture FindPicture(string name)
        {
            foreach (Picture picture in pictures)
            {
                if (picture.Name == name)
                {
                    return picture;
                }
            }
            return null;
        }
        public void View()
        {
            foreach (Picture picture in pictures)
            {
                picture.View();
            }
        }
    }
}