using System;
using System.Collections.Generic;

namespace MUSystem.Data
{
    internal interface IQueryTypeHandler<TEntity>
    {
        bool IterateDataReader { get; }
        object HandleType(Action<TEntity, IDataReader> customMapperReader, Action<TEntity, dynamic> customMapperDynamic);
    }
}
