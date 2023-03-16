using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace QuickDemo
{
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
}
