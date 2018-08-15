using System;

namespace MUSystem.Data
{
   public interface IParameterValue
    {
       TParameterType ParameterValue<TParameterType>(string outputParameterName);        
    }
}
