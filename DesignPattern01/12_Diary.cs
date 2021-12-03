using System;

namespace Visitor

{

    class Diary : Element

    {

        string content;

        public Diary(string name, string content) : base(name)

        {

            SetContent(content);

        }

        public void ViewContent()

        {

            Console.WriteLine(content);

        }

        public void SetContent(string content)

        {

            this.content = content;

        }

        public override void Accept(IVisit visitor)

        {

            visitor.VisitDiary(this);

        }

    }

}



