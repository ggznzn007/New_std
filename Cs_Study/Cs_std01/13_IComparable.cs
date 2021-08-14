using System;
using System.Collections.Generic;

namespace IComparableSort
{
    class Program
    {
        static void Main(string[] args)
        {
            Artists[] famousArtists =
            {
                new Artists("레오나르도 다빈치", "이탈리아", 1452,1519),
                new Artists("빈센트 반 고흐","네덜란드", 1853,1890),
                new Artists("클로드 모네","프랑스",1840,1926),
                new Artists("파블로 피카소","스페인",1881,1973),
                new Artists("베르메르","네덜란드",1632,1675),
                new Artists("르노아르", "프랑스",1841,1919)
            }; //객체 생성

            List<Artists> artists19c = new List<Artists>();//리스트 생성
            foreach (var artist in famousArtists)
            {
                if (artist.Birth > 1800 && artist.Birth <= 1900)
                    artists19c.Add(artist);
            }//famousArtists 베열에서 조건에 맞는 객체를 artists19c리스트에 추가

            //IComparable를 사용하여 정렬하고 재정의된 ToString메소드로 출력
            artists19c.Sort();
            Console.WriteLine("19세기 미술가를 탄생 순 정렬: IComparable");
            foreach (var a in artists19c)
                Console.WriteLine(a.ToString());
        }
    }

    class Artists : IComparable //IComparable 인터페이스를 기반으로 Artists 클래스를 정의
    {
        public string Name { get; set; }
        public string Country { get; set; }
        public int Birth { get; set; }
        public int Die { get; set; }

        public Artists(string name, string country, int birth, int die)
        {                             //Artists 클래스는 4개의 속성과 생성자 메소드를 갖는다
            Name = name;
            Country = country;
            Birth = birth;
            Die = die;
        }

        public int CompareTo(object obj) //obj를 Artists로 캐스팅하고 Birth를 비교하여 리턴한다
        {
            Artists a = (Artists)obj;
            return this.Birth.CompareTo(a.Birth);
        }

        public override string ToString() //ToString 메소드를 재정의한다
        {
            return string.Format(" {0}, {1}, {2}, {3}", Name, Country, Birth, Die);
        }

    }
}