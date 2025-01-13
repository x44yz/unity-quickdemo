using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

namespace AI.Utility
{
    public class WidgetAgent : MonoBehaviour
    {
        public TMP_Text txtName;
        public TMP_Text txtDecision;
        public Button btn;
        public Image imgBG;
        public Color selectedBgColor;
        public Color normalBgColor;

        public AgentAI agent;
        public System.Action<WidgetAgent, AgentAI> onWidgetClick;

        private void Start()
        {
            btn.onClick.AddListener(() =>
            {
                onWidgetClick?.Invoke(this, agent);
            });
        }

        public void Show(AgentAI agent)
        {
            gameObject.SetActive(true);

            this.agent = agent;
            agent.debugScore = true;

            agent.onDecisionChanged += OnDecisionChanged;

            txtName.text = agent.name;
            SetDecision(agent.curDecision);
        }

        public void Hide()
        {
            if (agent != null)
            {
                agent.debugScore = false;
                agent.onDecisionChanged -= OnDecisionChanged;
                agent = null;
            }

            gameObject.SetActive(false);
        }

        private void OnDecisionChanged(Decision act)
        {
            SetDecision(act);
        }

        public void Select()
        {
            imgBG.color = selectedBgColor;
        }

        public void Deselect()
        {
            imgBG.color = normalBgColor;
        }

        private void SetDecision(Decision decision)
        {
            if (decision != null)
                txtDecision.text = decision.name;
            else
                txtDecision.text = "null";
        }
    }
}