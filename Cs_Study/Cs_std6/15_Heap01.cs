using System;
using System.Collections.Generic;

/*Heap은 힙 속성(Heap Property)을 만족하는 트리 기반의 자료구조이다. 
  힙 속성에 따르면 부모와 자식 노드간의 순서는 일정해야 하고,
  같은 자식노드들 사이의 순서는 상관이 없다. 
  Heap은 부모 노드가 항상 자식 노드보다 크거나 같아야 하는 경우(max heap)와
  부모 노드가 항상 자식 노드보다 작거나 같아야 하는 경우(min heap)로 나눌 수 있다.
  즉, Max Heap은 루트노드에 데이타를 꺼내면 항상 해당 컬렉션의 최대값을 리턴하고,
  Min Heap은 최소값을 리턴한다. Heap은 여러가지 형태로 구현 가능한데, 
  일반적인 구현의 하나로 Binary Tree를 기반으로 한 Binary Heap을 들 수 있다. 
  자료구조에서 말하는 Heap은 메모리 구조에서 말하는 Heap과는 별개인 서로 다른 개념이다.*/

/*Add() : 데이타를 마지막에 추가한 후, 추가 데이타의 부모, 상위부모 등을 찾아 
        계속 추가 데이타가 부모보다 크면 부모와 치환한다.

Remove() : 루트노드를 꺼내고, 마지막 요소를 루트노드 (즉 첫 배열요소)에 넣은 후,
    루트와 좌우 자식중 큰 요소와 비교하여, 만약 자식이 크면 치환한다. 이 과정을 계속 반복.

MinHeap 클래스는 위의 3군테 코멘트(MinHeap에선 반대)에서 부등호를 반대로 해주면 된다.*/

namespace Heap01
{
    public class MaxHeap
    {
        private List<int> A = new List<int>();

        public static void Main()
        {
            
        }
        public void Add(int value)
        {
            // add at the end
            A.Add(value);

            // bubble up
            int i = A.Count - 1;
            while (i > 0)
            {
                int parent = (i - 1) / 2;
                if (A[parent] < A[i]) // MinHeap에선 반대
                {
                    Swap(parent, i);
                    i = parent;
                }
                else
                {
                    break;
                }
            }
        }

        public int RemoveOne()
        {
            if (A.Count == 0)
                throw new InvalidOperationException();

            int root = A[0];

            // move last to first
            // and remove last one
            A[0] = A[A.Count - 1];
            A.RemoveAt(A.Count - 1);

            // bubble down
            int i = 0;
            int last = A.Count - 1;
            while (i < last)
            {
                // get left child index
                int child = i * 2 + 1;

                // use right child if it is bigger                
                if (child < last &&
                    A[child] < A[child + 1]) // MinHeap에선 반대
                    child = child + 1;

                // if parent is bigger or equal, stop
                if (child > last ||
                   A[i] >= A[child]) // MinHeap에선 반대
                    break;

                // swap parent/child
                Swap(i, child);
                i = child;
            }

            return root;
        }

        private void Swap(int i, int j)
        {
            int t = A[i];
            A[i] = A[j];
            A[j] = t;
        }
    }
}

/*간단한 BinaryHeap 클래스
위에서 언급한 바와 같이 Binary Tree 개념에 기반한 Heap을 Binary Heap이라 부른다.
Binary Heap 클래스의 핵심 메서드는 데이타를 새로 추가하는 Add(혹은 Insert)와
하나의 최상위 루트노드 데이타를 가져오는 Remove 메서드이다.
Heap은 한번에 하나씩 최대 혹은 최소의 데이타를 가져오는 기능이 가장 핵심적인 기능이다.
이런 Remove() 메서드를 여러 번 호출하면, 완전히 정렬(Sort)되어 있지 않은 자료 구조로부터
최대 (혹은 최소)의 Top N 개 데이타를 가져오는 효과를 갖게 된다. 
Binary Heap 클래스를 구현하기 위해 Binary Tree를 사용할 수도 있고,
동적 배열을 사용하여 이진트리를 구현하는 방식으로 아래 예제와 같이
.NET의 동적배열인 List 템플릿 클래스를 활용할 수도 있다. 아래는 예제를 단순화하기 위해
내부 데이타로 int 를 사용하였으나, 약간의 수정을 통해 C#의 Generics를 사용하여 일반화할 수 있다.
Binary Heap으로 Max Heap 혹은 Min Heap을 구현할 수 있는데, 차이는 Add, Remove 메서드의 
키값 비교에서 부등호를 서로 반대로 하는 것 정도이다. 
(참조: 아래는 Max Heap의 예만 표현하였는데, IComparer를 입력 받아 한 클래스에서
Max Heap/Min Heap을 같이 구현할 수도 있다)*/