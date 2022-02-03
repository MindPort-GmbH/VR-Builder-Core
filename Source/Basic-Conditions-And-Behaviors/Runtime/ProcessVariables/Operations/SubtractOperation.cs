namespace VRBuilder.Core.ProcessUtils
{
    /// <summary>
    /// Subtracts right from left.
    /// </summary>
    public class SubtractOperation : IProcessVariableOperation<float, float>
    {
        /// <inheritdoc/>
        public float Execute(float leftOperand, float rightOperand)
        {
            return leftOperand - rightOperand;
        }
    }
}