using System;
namespace Facade
{
    class Picture
    {
        int tone;
        int brightness;
        int saturation;
        public string Name
        {
            get;
            private set;
        }
        public Picture(string name, int tone, int brightness, int saturation)
        {
            Name = name;
            this.tone = tone;
            this.brightness = brightness;
            this.saturation = saturation;
        }
        public void Change(int tone, int brightness, int saturation)
        {
            this.tone += tone;
            this.brightness += brightness;
            this.saturation += saturation;
        }

        bool IsEqual(string name)
        {
            return Name == name;
        }

        public void View()
        {
            Console.WriteLine("사진 이름:{0}", Name);
            Console.WriteLine("색조:{0} 명도:{1} 채도:{2}", tone, brightness, saturation);
        }
    }
}