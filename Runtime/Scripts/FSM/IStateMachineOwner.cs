
namespace QuickDemo.FSM
{
    public interface IStateMachineOwner
    {
        bool IsFSMDebug { get; }
        string FSMDebugLogPrefix { get; }
    }
}