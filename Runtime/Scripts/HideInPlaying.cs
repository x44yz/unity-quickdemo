using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace QuickDemo
{
    public class HideInPlaying : MonoBehaviour
    {
        void Awake()
        {
            gameObject.SetActive(false);
        }
    }
}
