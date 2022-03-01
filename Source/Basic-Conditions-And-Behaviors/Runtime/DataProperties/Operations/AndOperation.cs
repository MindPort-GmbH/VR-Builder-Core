namespace VRBuilder.Core.ProcessUtils
{
    /// <summary>
    /// "And" boolean operation.
    /// </summary>
    public class AndOperation : IOperationCommand<bool, bool>
    {
        /// <inheritdoc/>
        public bool Execute(bool leftOperand, bool rightOperand)
        {
            return leftOperand && rightOperand;
        }
    }
}