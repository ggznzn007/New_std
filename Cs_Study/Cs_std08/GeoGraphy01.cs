using System;
using System.Collections.Generic;
using System.Data.Entity.Spatial;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cs_std08
{
    class GeoGraphy01
    {
        static void Main(string[] args)
        {
            // 서울 (경도: 126.9784, 위도: 37.5667)
            DbGeography seoul = DbGeography.FromText("POINT(126.9784 37.5667)");

            // 부산 (경도: 129.0403, 위도: 35.1028)
            DbGeography busan = DbGeography.FromText("POINT(129.0403 35.1028)");

            // 거리 (미터로 계산됨)
            double? meters = seoul.Distance(busan);

            // 출력
            int km = (int)(meters.Value / 1000);
            Console.WriteLine("서울-부산 거리 = {0} km", km);
        }
    }
}
