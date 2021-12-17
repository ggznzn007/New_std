using System;

namespace Builder01
{
    public abstract class Builder
    {
        protected House house;

        public void createHouse()
        {
            house = new House();
        }

        public abstract void buildWalls();
        public abstract void buildDoors();
        public abstract void buildRoof();
        public abstract void buildWindows();
        public abstract void getHouse();
    }
}