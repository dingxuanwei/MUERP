using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MUSystem.Utils.EPPlus.Utils
{
    public interface IValidationResult
    {
        void IsTrue();
        void IsFalse();
    }
}
