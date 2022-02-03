namespace VRBuilder.Core.ProcessUtils
{
    /// <summary>
    /// Sums left and right.
    /// </summary>
    public class SumOperation : IProcessVariableOperation<float, float>
    {
        /// <inheritdoc/>
        public float Execute(float leftOperand, float rightOperand)
        {
            return leftOperand + rightOperand;
        }
    }
}