using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideInPlaying : MonoBehaviour
{
    public bool hide = true;

    void Awake()
    {
        if (hide)
        {
            gameObject.SetActive(false);
        }
    }
}
