﻿using System;

namespace LinkedList
{
    class Node
    {
        internal int data;
        internal Node next;
        public Node(int data)//생성자
        {
            this.data = data;
            next = null;
        }
    }

    class LinkedList
    {
        Node head;

        internal void InsertFront(int data)
        {
            Node node = new Node(data);
            node.next = head;
            head = node;
        }

        internal void InsertLast(int data)
        {
            Node node = new Node(data);
            if(head==null)
            {
                head = node;
                return;
            }
            Node lastNode = GetLastNode();
            lastNode.next = node;
        }

        internal Node GetLastNode()
        {
            Node temp = head;
            while (temp.next != null)
            {
                temp = temp.next;
            }
            return temp;
        }

        //prev 뒤에 data를 갖는 노드 삽입하기
        internal void InsertAfter(int prev, int data)
        {
            Node prevNode = null;

            //find prev
            for (Node temp = head; temp != null; temp = temp.next)
                if (temp.data == prev)
                    prevNode = temp;

            if(prevNode==null)
            {
                Console.WriteLine("{0} data is not in the list");
                return;
            }
            Node node = new Node(data);
            node.next = prevNode.next;
            prevNode.next = node;
        }

        //key값을 저장하고 있는 노드 삭제하기
        internal void DeleteNode(int key)
        {
            Node temp = head;
            Node prev = null;
            if(temp != null && temp.data==key)// head가 찾는 값이면
            {
                head = temp.next;
                return;
            }
            while(temp != null && temp.data != key)
            {
                prev = temp;
                temp = temp.next;
            }
            if(temp==null)//끝까지 찾는 값이 없으면
            {
                return;
            }
            prev.next = temp.next;
        }

        internal void Reverse()
        {
            Node prev = null;
            Node current = head;
            Node temp = null;
            while(current!=null)
            {
                temp = current.next;
                current.next = prev;
                prev = current;
                current = temp;
            }
            head = prev;
        }

        internal void Print()
        {
            for (Node node = head; node != null; node = node.next)
                Console.Write(node.data + " -> ");
            Console.WriteLine();
        }
    }
}