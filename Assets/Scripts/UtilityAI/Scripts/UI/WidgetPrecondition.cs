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

        private ActionObj action;
        private int preIdx;

        public void Show(ActionObj act, int preIdx)
        {
            gameObject.SetActive(true);

            this.action = act;
            this.preIdx = preIdx;

            var pre = act.preconditions[preIdx];
            txtName.text = pre.name;
            Refresh();
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

            bool? val = action.GetPreconditionBool(preIdx);
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
