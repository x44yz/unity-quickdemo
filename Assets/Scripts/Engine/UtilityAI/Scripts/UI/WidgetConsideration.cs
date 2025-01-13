using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace AI.Utility
{
    public class WidgetConsideration : MonoBehaviour
    {
        public TMP_Text txtName;
        public TMP_Text txtScore;

        private Decision decision;
        private int conIdx;
        private DecisionDebug dbg;

        public string scoreFormat => UtilityAIMonitor.Inst.scoreFormat;

        public void Show(AgentAI agent, Decision act, int conIdx)
        {
            gameObject.SetActive(true);

            this.decision = act;
            this.conIdx = conIdx;

            dbg = agent.GetDecisionDebugInfo(act);

            var con = act.considerations[conIdx];
            txtName.text = con.name;
            txtScore.text = dbg.GetConsiderationScore(conIdx).ToString(scoreFormat);
        }

        public void Hide()
        {
            decision = null;
            gameObject.SetActive(false);
        }

        public void Refresh()
        {
            if (gameObject.activeSelf == false || decision == null)
                return;

            txtScore.text = dbg.GetConsiderationScore(conIdx).ToString(scoreFormat);
        }
    }
}
