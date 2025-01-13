using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace AI.Utility
{
    public class WidgetPrecondition : MonoBehaviour
    {
        public TMP_Text txtName;
        public TMP_Text txtBool;

        private int preIdx;
        private DecisionDebug dbg;

        public void Show(AgentAI agent, Decision decision, int preIdx)
        {
            gameObject.SetActive(true);

            this.preIdx = preIdx;
            dbg = agent.GetDecisionDebugInfo(decision);

            var pre = dbg.decision.preconditions[preIdx];
            txtName.text = pre.name;
            Refresh();
        }

        public void Hide()
        {
            dbg = null;
            gameObject.SetActive(false);
        }

        public void Refresh()
        {
            if (gameObject.activeSelf == false || dbg == null)
                return;

            bool? val = dbg.GetPreconditionBool(preIdx);
            if (val == null)
            {
                txtBool.text = "-";
                txtBool.color = Color.gray;
            }
            else
            {
                txtBool.text = val == true ? "T" : "F";
                txtBool.color = val == true ? Color.black : Color.red;
            }
        }
    }
}
