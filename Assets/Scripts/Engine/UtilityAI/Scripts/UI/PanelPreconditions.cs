using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace AI.Utility
{
    public class PanelPreconditions : MonoBehaviour
    {
        public WidgetPrecondition tmpWidgetPrecondition;
        public Button btnClose;

        private UtilityAIMonitor monitor;
        private List<WidgetPrecondition> widgets = new List<WidgetPrecondition>();

        private void Start()
        {
            btnClose.onClick.AddListener(()=>{
                Hide();
                monitor.panelDecisions.DeselectWidgetDecision();
            });
        }

        public void Init(UtilityAIMonitor monitor)
        {
            this.monitor = monitor;
            tmpWidgetPrecondition.Hide();
        }

        public void Show(AgentAI agent, Decision act)
        {
            gameObject.SetActive(true);

            UIUtils.HandleListAllWidgets<WidgetPrecondition>(tmpWidgetPrecondition, (wgt) =>
            {
                wgt.Hide();
            });

            if (act.preconditions == null)
                return;

            widgets.Clear();
            for (int i = 0; i < act.preconditions.Length; ++i)
            {
                var wgt = UIUtils.GetListValidWidget<WidgetPrecondition>(i, tmpWidgetPrecondition);
                widgets.Add(wgt);
                wgt.Show(agent, act, i);
            }
        }

        public void Hide()
        {
            UIUtils.HandleListAllWidgets<WidgetPrecondition>(tmpWidgetPrecondition, (wgt) =>
            {
                wgt.Hide();
            });

            gameObject.SetActive(false);
        }

        public void Refresh()
        {
            foreach (var wgt in widgets)
            {
                wgt.Refresh();
            }
        }
    }
}