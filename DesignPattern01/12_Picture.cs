using System;

namespace Visitor

{

    class Picture : Element

    {

        int tone;

        int brightness;

        int saturation;

        public Picture(string name, int tone, int brightness, int saturation) : base(name)

        {

            SetInfo(tone, brightness, saturation);

        }

        public void ViewInfo()

        {

            Console.WriteLine("색조:{0} 명도:{1} 채도:{2}", tone, brightness, saturation);

        }

        public void SetInfo(int tone, int brightness, int saturation)

        {

            this.tone = tone;

            this.brightness = brightness;

            this.saturation = saturation;

        }

        public override void Accept(IVisit visitor)

        {

            visitor.VisitPicture(this);

        }

    }

}



