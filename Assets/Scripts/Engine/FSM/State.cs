using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AI.FSM
{
    public class State
    {
        public virtual void OnEnter(State from, params object[] data) {}
        public virtual void OnExit(State to) {}
        public virtual void OnUpdate(float dt) {}
    }
}