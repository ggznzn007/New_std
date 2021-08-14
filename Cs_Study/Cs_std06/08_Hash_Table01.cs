using System;
using System.Collections;
using System.Xml;

namespace HashTable01
{
    //해시테이블은 기본적으로 배열을 저장 장소로 사용한다.
    //아래의 간단한 해시테이블 구현에서도 buckets라는 배열을 기본 데이타 멤버로
    //사용하고 있다. 해시함수를 사용하여 정해진 배열내에서 특정 배열요소
    //위치를 계산하게 되며, 만약 키 중복이 발생하는 경우 Chaining방식을
    //사용하여 링크드 리스트로 연결하여 저장하게 된다.
    //이 예제는 기본 개념을 예시하기 위한 것으로 많은 기능들이 생략되어 있다.
    public class SimpleHashTable
    {
        private const int INITIAL_SIZE = 16;
        private int size;
        private Node[] buckets;

        public SimpleHashTable()
        {
            this.size = INITIAL_SIZE;
            this.buckets = new Node[size];
        }

        public SimpleHashTable(int capacity)
        {
            this.size = capacity;
            this.buckets = new Node[size];
        }

        public void Put(object key, object value)
        {
            int index = HashFunction(key);
            if (buckets[index] == null)
            {
                buckets[index] = new Node(key, value);
            }
            else
            {
                Node newNode = new Node(key, value);
                newNode.Next = buckets[index];
                buckets[index] = newNode;
            }
        }

        public object Get(object key)
        {
            int index = HashFunction(key);

            if (buckets[index] != null)
            {
                for (Node n = buckets[index]; n != null; n = n.Next)
                {
                    if (n.Key == key)
                    {
                        return n.Value;
                    }
                }
            }
            return null;
        }

        public bool Contains(object key)
        {
            int index = HashFunction(key);
            if (buckets[index] != null)
            {
                for (Node n = buckets[index]; n != null; n = n.Next)
                {
                    if (n.Key == key)
                    {
                        return true;
                    }
                }
            }
            return false;
        }
        /*
먼저 입력 key 객체의 해시코드를 Object.GetHashCode()로부터 얻은 후
(메모리 위치와 관련됨),이 해시코드를 일정부분 Shift (여기서는 5비트 왼쪽으로 Shift, 
하지만 임의로 정할 수 있음)하여 Bucket 크기로 나눈 나머지를 구하고,
이 둘을 더하여 임의 조작에 따른 랜덤 값을 구한 것입니다. 
이러한 결과값이 다시 Bucket 크기를 초과하면 안되므로 다시 % size를 수행하여
버켓 0 ~ size 사이에 들어가게 합니다.
*/
        protected virtual int HashFunction(object key)
        {
            return Math.Abs(key.GetHashCode() + 1 +
                (((key.GetHashCode() >> 5) + 1) % (size))) % size;
        }

        private class Node
        {
            public object Key { get; set; }
            public object Value { get; set; }
            public Node Next { get; set; }

            public Node(object key, object value)
            {
                this.Key = key;
                this.Value = value;
                this.Next = null;
            }
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            // Chaining 테스트를 위해 
            // capacity를 1로 셋팅할 수 있음            
            //SimpleHashTable ht = new SimpleHashTable(1);

            SimpleHashTable ht = new SimpleHashTable();
            ht.Put("Kim D", "Sales 01");
            ht.Put("Lee K", "Sales 02");
            ht.Put("Park S", "IT 03");
            ht.Put("Shin O", "IT 04");

            Console.WriteLine(ht.Get("Lee K"));
            Console.WriteLine(ht.Get("Shin O"));
            Console.WriteLine(ht.Contains("Unknown"));
        }
    }
}

//--Hashtable보다 Dictionary가 선호 되는 이유
//Dictionary :
//1.존재하지 않는 키를 찾으려고하면 Exception을 반환하거나 throw합니다.
//2. 박싱 및 언박싱이 없기 때문에 Hashtable보다 빠릅니다.
//3. 공용 정적 멤버 만 스레드로부터 안전합니다.
//4. Dictionary는 모든 데이터 유형 (생성시 키와 값 모두에 대한 데이터 유형을 지정해야 함)과 함께 사용할 수 있음을 의미하는 일반적인 유형입니다.
//예 : Dictionary<string, string> < NameOfDictionaryVar > = new Dictionary<string, string>();
//5.Dictionay는 Hashtable의 유형 안전 구현이며 Keys 및 Values 은 강력하게 형식화됩니다.

//Hashtable :
//1.존재하지 않는 키를 찾으려고하면 null을 반환합니다.
//2. 박싱과 언박싱이 필요하기 때문에 Dictionay보다 느립니다.
//3. Hashtable의 모든 멤버는 스레드로부터 안전합니다.
//4. Hashtable은 제네릭 형식이 아닙니다.
//5. Hashtable은 느슨하게 형식화 된 데이터 구조이므로 모든 유형의 키와 값을 추가 할 수 있습니다.​

//--DIctionary<>는 제네릭 형식이므로 형식이 안전합니다. 
//HashTable에 값 유형을 삽입 할 수 있으며 이로 인해 가끔 예외가 발생할 수 있습니다. 
//그러나 Dictionary<int> 는 정수 값만 허용하며 Dictionary<string> 은 문자열 만 허용합니다.
//따라서 HashTable 대신 Dictionary<> 를 사용하는 것이 좋습니다.
