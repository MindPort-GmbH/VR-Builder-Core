namespace VRBuilder.Core.ProcessUtils
{
    /// <summary>
    /// Multiplies left by right.
    /// </summary>
    public class MultiplyOperation : IProcessVariableOperation<float, float>
    {
        /// <inheritdoc/>
        public float Execute(float leftOperand, float rightOperand)
        {
            return leftOperand * rightOperand;
        }
    }
}