using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Data.Entity.Spatial;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cs_std08
{
    class Program
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
    #region EF 셋업 (Code-First)
    public class StoreDbContext : DbContext
    {
        public StoreDbContext() : base("DefaultConnection")
        {
            Database.SetInitializer<StoreDbContext>(null);
        }
        public DbSet<Store> Stores { get; set; }
    }

    [Table("Store")]
    public class Store
    {
        public int Id { get; set; }
        public string Name { get; set; }
        // 위치 속성 정의
        public DbGeography Location { get; set; }
    }
    #endregion


    public class StoreManager
    {
        // 현재위치에서 반경 x km 이내의 상점 찾기
        public static void FindStores(double currLat, double currLng, int withinKm = 5)
        {
            // 현재위치 DbGeography 객체
            string point = string.Format("POINT({0} {1})", currLng, currLat);
            DbGeography currentLocation = DbGeography.FromText(point);

            double distance = withinKm * 1000;

            var db = new StoreDbContext();

            // LINQ 쿼리 : 반경 5km 이내 상점
            var stores = from s in db.Stores
                         where s.Location.Distance(currentLocation) <= distance
                         select new
                         {
                             Id = s.Id,
                             Name = s.Name,
                             Latitude = s.Location.Latitude,  //위도 속성
                             Longitude = s.Location.Longitude //경도
                         };

            // 코드 단순화를 위해 그대로 출력함
            foreach (var s in stores)
            {
                Console.WriteLine("{0}: {1},{2}", s.Name, s.Latitude, s.Longitude);
            }
        }
    }
}
