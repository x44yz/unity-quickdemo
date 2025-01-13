
namespace AI.Utility
{
    public interface IContext
    {
        float GetDecisionCooldownTS();
        bool IsDebugLog();
        string DebugLogId();
    }
}