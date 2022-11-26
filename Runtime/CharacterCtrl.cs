using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace QuickDemo
{
    [AddComponentMenu("QuickDemo/CharacterCtrl")]
    public class CharacterCtrl : MonoBehaviour
    {
        [Header("DEFINE")]
        public float walkSpeed = 1;
        public float runSpeed = 2;

        [Header("RUNTIME")]
        public Animator animator;

        private void Start()
        {
            animator = GetComponent<Animator>();
        }

        private void Update()
        {
            Vector3 moveDir = Vector3.zero;
            if (Input.GetKey(KeyCode.W)) { moveDir.z = 1f; }
            if (Input.GetKey(KeyCode.S)) { moveDir.z = -1; }
            if (Input.GetKey(KeyCode.A)) { moveDir.x = -1f;}
            if (Input.GetKey(KeyCode.D)) { moveDir.x = 1f; }
        }
    }
}
