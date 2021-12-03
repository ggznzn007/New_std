using System;

using System.Collections.Generic;

namespace Visitor

{

    class MyTour

    {

        IVisit[] visitors = new IVisit[2];

        List<Element> collection = new List<Element>();

        public MyTour()

        {

            visitors[0] = new Viewer();

            visitors[1] = new Initializer();

        }

        public void AddElement(Element element)

        {

            collection.Add(element);

        }

        public void ViewAll()

        {

            foreach (Element element in collection)

            {

                element.Accept(visitors[0]);

            }

        }

        public void Reset()

        {

            foreach (Element element in collection)

            {

                element.Accept(visitors[1]);

            }

            Console.WriteLine("모든 정보를 초기 상태로 바꾸었습니다.");

        }

    }

}



출처: https://ehclub.co.kr/2245 [언제나 휴일]