using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[DisallowMultipleComponent]
[RequireComponent(typeof(TMP_Text))]
public class UITMPLocal : MonoBehaviour
{
    public static LocalSystem sLocal => GameMgr.Inst.sLocal;

    public string key;

    private TMP_Text tmpText;

    private void Start() 
    {
        tmpText = GetComponent<TMP_Text>();
        if (tmpText == null)
            Debug.Log($"{Utils.GetHierarchyPath(transform)} cant find tmp text");

        RefreshText();
        sLocal.onLangChanged += OnLangChanged;
    }

    private void OnDestroy() 
    {
        sLocal.onLangChanged -= OnLangChanged;
    }

    private bool OnLangChanged()
    {
        RefreshText();
        return true;
    }

    private void RefreshText()
    {
        if (tmpText == null)
            return;

        tmpText.font = sLocal.font;
        if (string.IsNullOrEmpty(key) == false)
            tmpText.text = sLocal.Get(key);
    }
}
