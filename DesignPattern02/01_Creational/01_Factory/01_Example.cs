using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FactoryMethod
{
    class Program
    {
        public static void Main(string[] args)
        {
            //메소드 호출
            File[] files = new File[2];
            files[0] = new Furniture();// 가구
            files[1] = new Crokery();// 그릇

            foreach (File File in files)
            {
                Console.WriteLine("\n" + File.GetType().Name + "--");
                foreach (Items Items in File.Itemss)
                {
                    Console.WriteLine(" " + Items.GetType().Name);
                }
            }
            Console.ReadKey();
        }
    }
    // 생산상품 추상 클래스 
    abstract class Items
    { }
    // ConcreteProduct classes
    class SofasetItems : Items
    { }
    class PlatesItems : Items
    { }
    class BedItems : Items
    { }
    class GlassesItems : Items
    { }
    class TeasetItems : Items
    { }
    class BowlsItems : Items
    { }
    class DiningItems : Items
    { }
    abstract class File
    {
        private List<Items> _Itemss = new List<Items>();
        // Constructor calls abstract Factory method
        public File()
        {
            this.CreateItemss();
        }
        public List<Items> Itemss
        {
            get { return _Itemss; }
        }
        // Factory Method
        public abstract void CreateItemss();
    }
    class Furniture : File
    {
        // Factory Method implementation
        public override void CreateItemss()
        {
            Itemss.Add(new SofasetItems());
            Itemss.Add(new BedItems());
            Itemss.Add(new DiningItems());
        }
    }
    class Crokery : File
    {
        public override void CreateItemss()
        {
            Itemss.Add(new PlatesItems());
            Itemss.Add(new GlassesItems());
            Itemss.Add(new TeasetItems());
            Itemss.Add(new BowlsItems());
        }
    }
}