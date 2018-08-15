using System;

namespace MUSystem.Data
{
    public class EntityFactory : IEntityFactory
    {
        public virtual object Create(Type type)
        {
            return Activator.CreateInstance(type);
        }
    }
}
