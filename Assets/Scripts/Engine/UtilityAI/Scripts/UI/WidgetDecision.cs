using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

namespace AI.Utility
{
    public class WidgetDecision : MonoBehaviour
    {
        public TMP_Text txtName;
        public TMP_Text txtScore;
        public Button btn;
        public Image imgBG;
        public Color selectedBgColor;
        public Color normalBgColor;

        private AgentAI agent;
        private Decision decision;
        public System.Action<WidgetDecision, AgentAI, Decision> onWidgetClick;
        public System.Action<WidgetDecision> onWidgetRefresh;

        public string scoreFormat => UtilityAIMonitor.Inst.scoreFormat;

        private void Start()
        {
            btn.onClick.AddListener(() =>
            {
                onWidgetClick?.Invoke(this, agent, decision);
            });
        }

        public void Show(AgentAI agent, Decision act)
        {
            gameObject.SetActive(true);

            this.agent = agent;
            this.decision = act;
            agent.onScoreChanged += OnDecisionScoreChanged;

            txtName.text = act.name;
            RefreshScore(agent, act);
        }

        public void Hide()
        {
            if (agent != null)
            {
                agent.onScoreChanged -= OnDecisionScoreChanged;
                agent = null;
            }

            gameObject.SetActive(false);
        }

        public void Select()
        {
            imgBG.color = selectedBgColor;
        }

        public void Deselect()
        {
            imgBG.color = normalBgColor;
        }

        private void OnDecisionScoreChanged(Decision act)
        {
            if (decision != act)
                return;

            RefreshScore(agent, act);
            onWidgetRefresh?.Invoke(this);
        }

        private void RefreshScore(AgentAI agent, Decision act)
        {
            var dbgInfo = agent.GetDecisionDebugInfo(act);

            string strScore = dbgInfo.curScore.ToString(scoreFormat);
            if (dbgInfo.IsPrecondtionsValid() == false)
                strScore += "[P]";
            if (dbgInfo.isInCooldown)
                strScore += "[C]";
            txtScore.text = strScore;
        }
    }
}