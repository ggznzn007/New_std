using System;

namespace _Interface02
{
    interface ICar
    {
        void HasDoors();
        void HasAirbags();
        void HasEngine();
    }

    class ElectroCar : ICar
    {
        public void HasAirbags()
        {
            Console.WriteLine(GetType().Name + " has doors.");
        }

        public void HasDoor()
        {
            Console.WriteLine(GetType().Name + " has airbags.");
        }

        public void HasDoors()
        {
            throw new NotImplementedException();
        }

        public void HasEngine()
        {
            Console.WriteLine(GetType().Name + " has engine.");
        }
    }

    class GasolineCar : ICar
    {
        public void HasAirbags()
        {
            Console.WriteLine(GetType().Name + " has doors.");
        }

        public void HasDoor()
        {
            Console.WriteLine(GetType().Name + " has airbags.");
        }

        public void HasDoors()
        {
            throw new NotImplementedException();
        }

        public void HasEngine()
        {
            Console.WriteLine(GetType().Name + " has engine.");
        }
    }

    class Program
    {
        static void Main()
        {
            ICar electroCar = new ElectroCar();
            electroCar.HasEngine();
            electroCar.HasAirbags();
            electroCar.HasDoors();

            ICar gasolineCar = new GasolineCar();
            gasolineCar.HasEngine();
            gasolineCar.HasAirbags();
            gasolineCar.HasDoors();
        }
    }
}