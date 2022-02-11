namespace VRBuilder.Core.ProcessUtils
{
    /// <summary>
    /// An operation on process variables.
    /// </summary>
    /// <typeparam name="TOperand">Type of the operands.</typeparam>
    /// <typeparam name="TResult">Type of the returned result.</typeparam>
    public interface IProcessVariableOperation<TOperand, TResult>
    {
        /// <summary>
        /// Executes the operation on the provided operands.
        /// </summary>
        TResult Execute(TOperand leftOperand, TOperand rightOperand);
    }
}