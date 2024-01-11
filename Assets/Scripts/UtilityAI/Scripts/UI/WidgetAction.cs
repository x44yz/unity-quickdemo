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

        private ActionObj action;
        public System.Action<WidgetAction, ActionObj> onWidgetClick;
        public System.Action<WidgetAction> onWidgetRefresh;

        public string scoreFormat => UtilityAIMonitor.Inst.scoreFormat;

        private void Start()
        {
            btn.onClick.AddListener(() =>
            {
                onWidgetClick?.Invoke(this, action);
            });
        }

        public void Show(ActionObj act)
        {
            gameObject.SetActive(true);

            this.action = act;
            act.onScoreChanged += OnActionScoreChanged;

            txtName.text = act.dbgName;
            RefreshScore(act);
        }

        public void Hide()
        {
            if (action != null)
            {
                action.onScoreChanged -= OnActionScoreChanged;
                action = null;
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

        private void OnActionScoreChanged(ActionObj act)
        {
            RefreshScore(act);
            onWidgetRefresh?.Invoke(this);
        }

        private void RefreshScore(ActionObj act)
        {
            string strScore = act.curScore.ToString(scoreFormat);
            if (act.IsPrecondtionsValid() == false)
                strScore += "[P]";
            if (act.isCooldown)
                strScore += "[C]";
            txtScore.text = strScore;
        }
    }
}