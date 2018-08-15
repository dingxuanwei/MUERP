using System;

namespace MUSystem.Data
{
    public interface IEntityFactory
    {
        object Create(Type type);
    }
}
