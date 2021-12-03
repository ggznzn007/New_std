using System;

namespace Abstract_Virtual01
{
    public abstract class BaseCharacter
    {
        public virtual void Attack()
        {

        }

        public virtual void Stop()
        {

        }

        public abstract void Skill_Q();
        public abstract void Skill_W();
        public abstract void Skill_E();
        public abstract void Skill_R();

    }

    
}