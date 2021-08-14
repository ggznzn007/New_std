using System;

namespace Indexer_T
{
    class MyCollection<T>// 제네릭 클래스 마이컬렉션 T를 정의
    {
        private T[] array = new T[100];// 클래스의 필드로 T형 자료를 100개 저장 가능한 배열

        public T this[int i]// 인덱서 정의
        {//this 키워드를 사용해서 정수 인덱스를 사용가능
            get { return array[i]; }//array[i]를 리턴
            set { array[i] = value; }//value 키워드를 사용하여 array[i]를 설정 
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            var myString = new MyCollection<string>();//인스턴스 myString 정의
            myString[0] = "Hello, World!";//myString[0]에 헬로 월드를 넣고
            myString[1] = "Hello, C#";//MyCollection 클래스에 인덱서를 만들어서 배열과같이[]연산자 사용가능
            myString[2] = "Hello, Indexer!";

            for (int i = 0; i < 3; i++)//인덱서 i를 0~2까지 변화하면서 myString[i]를 출력
                Console.WriteLine(myString[i]);
        }
    }
}