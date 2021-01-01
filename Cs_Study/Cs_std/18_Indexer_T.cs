using System;

namespace Indexer_T
{
    class MyCollection<T>
    {
        private T[] array = new T[100];

        public T this[int i]// 인덱스 정의
        {
            get { return array[i]; }
            set { array[i] = value; }
        }
    }
}