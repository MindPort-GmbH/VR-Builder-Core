namespace VRBuilder.Core.ProcessUtils
{
    /// <summary>
    /// "Exclusive or" boolean operation.
    /// </summary>
    public class XorOperation : IProcessVariableOperation<bool, bool>
    {
        /// <inheritdoc/>
        public bool Execute(bool leftOperand, bool rightOperand)
        {
            return leftOperand ^ rightOperand;
        }
    }
}