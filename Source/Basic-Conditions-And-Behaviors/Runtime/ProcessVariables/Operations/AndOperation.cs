namespace VRBuilder.Core.ProcessUtils
{
    /// <summary>
    /// "And" boolean operation.
    /// </summary>
    public class AndOperation : IProcessVariableOperation<bool, bool>
    {
        /// <inheritdoc/>
        public bool Execute(bool leftOperand, bool rightOperand)
        {
            return leftOperand && rightOperand;
        }
    }
}