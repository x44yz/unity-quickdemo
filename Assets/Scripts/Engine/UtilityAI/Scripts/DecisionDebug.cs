
namespace AI.Utility
{
    public class DecisionDebug
    {
        public Decision decision { get; set; }
        public float curScore { get; set; }
        public float[] conScores = null;
        public bool[] preBools = null;
        public int preBreakIdx = 0;
        public bool isInCooldown { get; set; }
        // public Precondition[] preconditions => action.preconditions;
        // public ConsiderationDeco[] considerations => action.considerations;

        public void Init(Decision decision)
        {
            this.decision = decision;

            conScores = new float[decision.considerations.Length];
            preBools = new bool[decision.preconditions.Length];
            // conTotalWeight = 0f;
            // foreach (var con in considerations)
            // {
            //     conTotalWeight += con.weight;
            // }
        }

        public bool? GetPreconditionBool(int idx)
        {
            if (preBools == null || idx < 0 || idx >= preBools.Length)
                return null;
            if (idx > preBreakIdx)
                return null;
            return preBools[idx];
        }

        public float GetConsiderationScore(int idx)
        {
            if (conScores == null || idx < 0 || idx >= conScores.Length)
                return 0f;
            return conScores[idx];
        }

        public bool IsPrecondtionsValid()
        {
            if (preBools == null || preBools.Length == 0)
                return true;
            return preBreakIdx > preBools.Length;
        }
    }
}