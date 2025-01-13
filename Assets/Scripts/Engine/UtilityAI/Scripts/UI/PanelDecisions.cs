using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace AI.Utility
{
    public class PanelDecisions : MonoBehaviour
    {
        public WidgetDecision tmpWidgetDecision;
        public Button btnClose;

        private UtilityAIMonitor monitor;
        private WidgetDecision selectedWidgetDecision;

        private void Start()
        {
            btnClose.onClick.AddListener(()=>{
                Hide();
                monitor.panelAgents.DeselectWidgetAgent();
            });
        }

        public void Init(UtilityAIMonitor monitor)
        {
            this.monitor = monitor;
            selectedWidgetDecision = null;
            tmpWidgetDecision.Hide();
        }

        public void Show(AgentAI agent, List<Decision> decisions)
        {
            gameObject.SetActive(true);

            UIUtils.HandleListAllWidgets<WidgetDecision>(tmpWidgetDecision, (wgt) =>
            {
                wgt.Hide();
            });

            if (decisions == null)
                return;

            for (int i = 0; i < decisions.Count; ++i)
            {
                var act = decisions[i];
                var wgt = UIUtils.GetListValidWidget<WidgetDecision>(i, tmpWidgetDecision);
                wgt.Show(agent, act);
                wgt.onWidgetClick = OnWidgetDecisionClick;
                wgt.onWidgetRefresh = OnWidgetDecisionRefresh;
            }
        }

        public void Hide()
        {
            monitor.panelPreconditions.Hide();
            monitor.panelConsiderations.Hide();
            UIUtils.HandleListAllWidgets<WidgetDecision>(tmpWidgetDecision, (wgt) =>
            {
                wgt.Hide();
            });

            DeselectWidgetDecision();
            gameObject.SetActive(false);
        }

        private void OnWidgetDecisionClick(WidgetDecision wgt, AgentAI agent, Decision act)
        {
            DeselectWidgetDecision();
            selectedWidgetDecision = wgt;
            selectedWidgetDecision.Select();
            monitor.panelPreconditions.Show(agent, act);
            monitor.panelConsiderations.Show(agent, act);
        }

        private void OnWidgetDecisionRefresh(WidgetDecision wgt)
        {
            if (wgt != selectedWidgetDecision)
                return;
            monitor.panelConsiderations.Refresh();
            monitor.panelConsiderations.Refresh();
        }

        public void DeselectWidgetDecision()
        {
            if (selectedWidgetDecision != null)
                selectedWidgetDecision.Deselect();
            selectedWidgetDecision = null;
        }
    }
}
