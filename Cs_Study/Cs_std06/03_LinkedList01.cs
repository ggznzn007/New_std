﻿using System;
using System.Collections.Generic;
using System.Linq;
/*링크드 리스트(Linked List, 연결 리스트)는 데이타를 포함하는 노드들을 연결하여 컬렉션을
  만든 자료 구조로서 각 노드는 데이타와 다음/이전 링크 포인터를 갖게 된다. 
  단일 연결 리스트(Singly Linked List)는 노드를 다음 링크로만 연결한 리스트이고
  이중 연결 리스트는 각 노드를 다음 링크와 이전 링크 모두 연결한 리스트이다. 
  만약 링크를 순환해서 마지막 노드의 다음 링크가 처음 노드를 가리키게 했을 경우
  이를 순환 연결 리스트 (Circular Linked List)라 부른다.
링크드 리스트는 특정 노드에서 노드를 삽입, 삭제하기 편리 하지만 (O(1) ),
  특정 노드를 검색하기 위해서는 O(n)의 시간이 소요된다.*/

namespace L_List
{
    class Program
    {
        static void Main(string[] args)
        {
            LinkedList<string> list = new LinkedList<string>();
            list.AddLast("Apple");
            list.AddLast("Banana");
            list.AddLast("Lemon");
            list.AddLast("Orange");
            list.AddLast("Tomato");
            list.AddLast("Melon");
            list.AddLast("Watermelon");

            LinkedListNode<string> node = list.Find("Banana");
            LinkedListNode<string> newNode = new LinkedListNode<string>("Grape");

            // 새 Grape 노드를 Banana 노드 뒤에 추가
            list.AddAfter(node, newNode);

            // 리스트 출력
            list.ToList<string>().ForEach(p => Console.WriteLine(p));

            // Enumerator 리스트 출력
            foreach (var item in list)
            {
                Console.WriteLine(item);
            }
        }
    }
}