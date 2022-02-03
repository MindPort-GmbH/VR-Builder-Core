using System;

namespace VRBuilder.Core.ProcessUtils
{
    /// <summary>
    /// Divides left by right.
    /// </summary>
    public class DivideOperation : IProcessVariableOperation<float, float>
    {
        /// <inheritdoc/>
        public float Execute(float leftOperand, float rightOperand)
        {
            if(rightOperand == 0)
            {
                throw new DivideByZeroException("Process data operation attempted to divide by zero.");
            }
            
            return leftOperand / rightOperand;
        }
    }
}