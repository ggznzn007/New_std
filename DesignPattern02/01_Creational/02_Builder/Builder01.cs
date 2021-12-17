using System;

namespace EquipmentSystem
{
    class Equipment
    {
        string hp;
        string mp;
        int attack;
        int defence;

        public Equipment() { }

        public void setHp(string d) { hp = d; }
        public void setMp(string s) { hp = s; }

        public void setAttack(int a) { attack = a; }

        public void setDefence(int de) { defence = de; }
    }

    abstract class CreateSlot
    {
        protected Equipment equipment;

        public CreateSlot() { }
        public Equipment getEquipment() { return equipment; }
        public void EquipmentCreate() { equipment = new Equipment(); }

        public abstract void BuildHp();
        public abstract void BuildMp();
        public abstract void BuildOption();
    }

    class WeaponCreateSlot : CreateSlot
    {
        public override void BuildHp()
        {
            equipment.setHp("HP Potion");
        }

        public override void BuildMp()
        {
            equipment.setMp("MP Potion");
        }

        public override void BuildOption()
        {
            equipment.setAttack(100);
        }
    }

    class SheildCreateSlot : CreateSlot
    {
        public override void BuildHp()
        {
            equipment.setHp("HP Potion2");
        }

        public override void BuildMp()
        {
            equipment.setMp("MP Potion2");
        }

        public override void BuildOption()
        {
            equipment.setDefence(200);
        }

    }

    class BlackSmith
    {
        private CreateSlot selectedCreatSlot;

        public void SetCreatSlot(CreateSlot slot) { selectedCreatSlot = slot; }
        public Equipment GetEquipment() { return selectedCreatSlot.getEquipment(); }

        public void ConstructEquipment()
        {
            selectedCreatSlot.EquipmentCreate();
            selectedCreatSlot.BuildHp();
            selectedCreatSlot.BuildMp();
            selectedCreatSlot.BuildOption();
        }

    }

    class App
    {
        static void Main(string[] args)
        {
            BlackSmith albert = new BlackSmith();
            CreateSlot[] createSlots = { new WeaponCreateSlot(), new SheildCreateSlot() };

            
                albert.SetCreatSlot(createSlots[2]);
                albert.ConstructEquipment();               
            

            Equipment producedEquipment = albert.GetEquipment();

        }
    }
}