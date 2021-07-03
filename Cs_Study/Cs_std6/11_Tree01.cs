using System;
using System.Collections;

/*트리(Tree)는 계층적인 자료를 나타내는데 자주 사용되는 자료 구조로서 하나이하의 부모노드와
    복수 개의 자식노드들을 가질 수 있다. 트리는 하나의 루트(Root) 노드에서 출발하여
    자식노드들을 갖게 되며, 각각의 자식 노드는 또한 자신의 자식노드들을 가질 수 있다.
    트리 구조는 한 노드에서 출발하여 다시 자기 자신의 노드로 돌아오는 순환(Cycle)구조를 가질 수 없다.
    트리구조는 계층적인 정부 혹은 기업 조직도, 대중소 지역 구조, 데이타 인덱스 파일 등에 적합한 자료구조이다.

  이진 트리 (Binary Tree)
    트리(Tree)에서 많이 사용되는 특별한 트리로서 이진트리를 들 수 있는데,
    이진 트리는 자식노드가 0개 ~ 2개인 트리를 말한다. 따라서 이진트리 노드는
    데이타필드와 왼쪽노드 및 오른쪽노드를 갖는 자료 구조로 되어 있다.
    이진 트리는 루트 노드로부터 출발하여 원하는 특정 노드에 도달할 수 있는데, 
    이때의 검색 시간(Search Time)은 O(n)이 소요된다.*/

namespace Tree01
{
    // 이진 트리 노드 클래스
    public class BinaryTreeNode<T>
    {
        public T Data { get; set; }
        public BinaryTreeNode<T> Left { get; set; }
        public BinaryTreeNode<T> Right { get; set; }

        public BinaryTreeNode(T data)
        {
            this.Data = data;
        }
    }

    // 이진 트리 클래스
    public class BinaryTree<T>
    {
        public BinaryTreeNode<T> Root { get; set; }

        // 트리 데이타 출력 예
        public void PreOrderTraversal(BinaryTreeNode<T> node)
        {
            if (node == null) return;

            Console.WriteLine(node.Data);
            PreOrderTraversal(node.Left);
            PreOrderTraversal(node.Right);
        }
    }

    // 테스트 예제
    class Program
    {
        static void Main(string[] args)
        {
            BinaryTree<int> btree = new BinaryTree<int>();
            btree.Root = new BinaryTreeNode<int>(1);
            btree.Root.Left = new BinaryTreeNode<int>(2);
            btree.Root.Right = new BinaryTreeNode<int>(3);
            btree.Root.Left.Left = new BinaryTreeNode<int>(4);

            btree.PreOrderTraversal(btree.Root);
        }
    }
}