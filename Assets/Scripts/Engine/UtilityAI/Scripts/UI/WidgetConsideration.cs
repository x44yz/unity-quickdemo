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

        private Action action;
        private int conIdx;
        private ActionDebug dbg;

        public string scoreFormat => UtilityAIMonitor.Inst.scoreFormat;

        public void Show(AgentAI agent, Action act, int conIdx)
        {
            gameObject.SetActive(true);

            this.action = act;
            this.conIdx = conIdx;

            dbg = agent.GetActionDebugInfo(act);

            var con = act.considerations[conIdx];
            txtName.text = con.name;
            txtScore.text = dbg.GetConsiderationScore(conIdx).ToString(scoreFormat);
        }

        public void Hide()
        {
            action = null;
            gameObject.SetActive(false);
        }

        public void Refresh()
        {
            if (gameObject.activeSelf == false || action == null)
                return;

            txtScore.text = dbg.GetConsiderationScore(conIdx).ToString(scoreFormat);
        }
    }
}
