namespace TradeSphere.Domain.Exceptions;
public sealed class InvalidStateTransitionException(string entityName, string currentState, string attemptedState)
    : DomainException($"{entityName} cannot move from '{currentState}' to '{attemptedState}'.")
{
    public string EntityName { get; } = entityName;
    public string CurrentState { get; } = currentState;
    public string AttemptedState { get; } = attemptedState;
}