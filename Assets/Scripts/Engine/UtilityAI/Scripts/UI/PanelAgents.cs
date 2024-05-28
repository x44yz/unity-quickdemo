using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace AI.Utility
{
    public class PanelAgents : MonoBehaviour
    {
        public WidgetAgent tmpWidgetAgent;
        public Button btnClose;

        private UtilityAIMonitor monitor;
        private WidgetAgent selectedWidgetAgent;
        private int widgetAgentIdx = 0;

        public void Init(UtilityAIMonitor monitor)
        {
            btnClose.onClick.AddListener(()=>{
                Hide();
            });

            this.monitor = monitor;
            selectedWidgetAgent = null;
            tmpWidgetAgent.Hide();
        }

        public void Show(AgentAI[] agents)
        {
            gameObject.SetActive(true);

            UIUtils.HandleListAllWidgets<WidgetAgent>(tmpWidgetAgent, (wgt) =>
            {
                wgt.Hide();
            });

            if (agents == null)
                return;

            widgetAgentIdx = 0;
            for (int i = 0; i < agents.Length; ++i)
            {
                var agent = agents[i];
                AddAgent(agent);
            }
        }

        public void Hide()
        {
            monitor.panelActions.Hide();
            UIUtils.HandleListAllWidgets<WidgetAgent>(tmpWidgetAgent, (wgt) =>
            {
                wgt.Hide();
            });

            DeselectWidgetAgent();
            gameObject.SetActive(false);
            widgetAgentIdx = 0;
        }

        private void OnWidgetAgentClick(WidgetAgent wgt, AgentAI agent)
        {
            DeselectWidgetAgent();
            selectedWidgetAgent = wgt;
            selectedWidgetAgent.Select();
            monitor.panelActions.Show(agent, agent.actions);
        }

        public void DeselectWidgetAgent()
        {
            if (selectedWidgetAgent != null)
                selectedWidgetAgent.Deselect();
            selectedWidgetAgent = null;
        }

        public void AddAgent(AgentAI agent)
        {
            var wgt = UIUtils.GetListValidWidget<WidgetAgent>(widgetAgentIdx++, tmpWidgetAgent);
            wgt.Show(agent);
            wgt.onWidgetClick = OnWidgetAgentClick;
        }

        public void RemoveAgent(AgentAI agent)
        {
            var wgts = UIUtils.GetListAllWidgets<WidgetAgent>(tmpWidgetAgent);
            foreach (var wgt in wgts)
            {
                if (wgt.gameObject.activeSelf == false)
                    continue;
                if (wgt.agent != agent)
                    continue;
                wgt.Hide();
                if (selectedWidgetAgent == wgt)
                {
                    DeselectWidgetAgent();
                }
            }
        }
    }
}
