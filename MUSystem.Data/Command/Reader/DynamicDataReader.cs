using System;
using System.Collections.Generic;
using System.Dynamic;

namespace MUSystem.Data
{
    internal class DynamicDataReader : DynamicObject
    {
        private readonly System.Data.IDataReader _dataReader;

        internal DynamicDataReader(System.Data.IDataReader dataReader)
        {
            _dataReader = dataReader;
        }

        public override bool TryGetMember(GetMemberBinder binder, out object result)
        {
            result = _dataReader[binder.Name];
            if (result == DBNull.Value)
                result = null;

            return true;
        }
    }
}
