using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MUSystem.Data
{
    public interface IInsertUpdateBuilder
    {
        BuilderData Data { get; }
        IInsertUpdateBuilder Column(string columnName, object value, DataTypes parameterType = DataTypes.Object, int size = 0);
    }
}
