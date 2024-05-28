using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

namespace AI.Utility
{
    public class WidgetAction : MonoBehaviour
    {
        public TMP_Text txtName;
        public TMP_Text txtScore;
        public Button btn;
        public Image imgBG;
        public Color selectedBgColor;
        public Color normalBgColor;

        private AgentAI agent;
        private Action action;
        public System.Action<WidgetAction, AgentAI, Action> onWidgetClick;
        public System.Action<WidgetAction> onWidgetRefresh;

        public string scoreFormat => UtilityAIMonitor.Inst.scoreFormat;

        private void Start()
        {
            btn.onClick.AddListener(() =>
            {
                onWidgetClick?.Invoke(this, agent, action);
            });
        }

        public void Show(AgentAI agent, Action act)
        {
            gameObject.SetActive(true);

            this.agent = agent;
            this.action = act;
            agent.onScoreChanged += OnActionScoreChanged;

            txtName.text = act.name;
            RefreshScore(agent, act);
        }

        public void Hide()
        {
            if (agent != null)
            {
                agent.onScoreChanged -= OnActionScoreChanged;
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

        private void OnActionScoreChanged(Action act)
        {
            if (action != act)
                return;

            RefreshScore(agent, act);
            onWidgetRefresh?.Invoke(this);
        }

        private void RefreshScore(AgentAI agent, Action act)
        {
            var dbgInfo = agent.GetActionDebugInfo(act);

            string strScore = dbgInfo.curScore.ToString(scoreFormat);
            if (dbgInfo.IsPrecondtionsValid() == false)
                strScore += "[P]";
            if (dbgInfo.isInCooldown)
                strScore += "[C]";
            txtScore.text = strScore;
        }
    }
}