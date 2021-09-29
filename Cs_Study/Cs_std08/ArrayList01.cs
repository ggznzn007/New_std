using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cs_std08
{
    class ArrayList01
    {
        public static void Main()
        {
            // 생성
            ArrayList alist = new ArrayList();

            // 저장
            alist.Add(34);
            alist.Add("Albert");
            alist.Add(178.5);
            alist.Add(true);

            // 저장 02
            alist.Insert(0, 34);

            // 삭제 01
            alist.Remove(34);

            // 삭제 02
            alist.RemoveAt(2);

            // 삭제 03
            alist.Clear();

            // 정보조회
            alist.Contains(5);

            // 정보조회
            int size = alist.Count;

           


        }
    }
}
