namespace Facade
{
    class Program
    {
        static void Main(string[] args)
        {
            Picture picture = new Picture("언제나 휴일", 100, 100, 100);
            Picture picture2 = new Picture("언제나 휴일2", 100, 100, 100);
            SmartManager sm = new SmartManager();
            sm.AddPicture(picture);
            sm.AddPicture(picture2);
            sm.Change("언제나 휴일", 20, 20, -50);
            sm.View();
        }
    }
}