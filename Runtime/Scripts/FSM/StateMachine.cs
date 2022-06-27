using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace QuickDemo.FSM
{
    public class StateMachine<T> where T : IStateMachineOwner
    {
        public T owner;
        public State curState;
        public Dictionary<State, List<Transition>> transitions = new Dictionary<State, List<Transition>>();
        public Dictionary<Type, State> states = new Dictionary<Type, State>();

        public bool isDebugLog = false;
        public string debugLogPrefix = "";

        public StateMachine(T owner)
        {
            this.owner = owner;
        }

        public void Register(State st)
        {
            var tp = st.GetType();
            if (states.ContainsKey(tp))
            {
                Debug.LogError("[FSM]failed Register because exist same state > " + tp);
                return;
            }
            states.Add(tp, st);
        }

        public void Update(float dt)
        {
            if (curState == null)
                return;

            curState.OnUpdate(dt);

            if (transitions.TryGetValue(curState, out List<Transition> tsList))
            {
                foreach (var ts in tsList)
                {
                    if (ts.IsValid())
                    {
                        if (isDebugLog)
                        {
                            Debug.Log($"[FSM]{debugLogPrefix}translate from {curState} to {ts.to}");
                        }

                        ts.OnTransition();
                        Translate(ts.to);
                        break;
                    }
                }
            }
        }

        public void Translate(Type tp)
        {
            State st = null;
            if (!states.TryGetValue(tp, out st))
            {
                Debug.LogError("[FSM]failed Translate because cant find state > " + tp);
                return;
            }
            Translate(st);
        }

        public void Translate(State st)
        {
            State lastState = null;
            if (curState != null)
            {
                lastState = curState;
                curState.OnExit(st);
            }

            curState = st;
            curState.OnEnter(lastState);
        }

        public void AddTransition(Transition ts)
        {
            List<Transition> tsList;
            if (!transitions.TryGetValue(ts.from, out tsList))
            {
                tsList = new List<Transition>();
                transitions.Add(ts.from, tsList);
            }
            tsList.Add(ts);
        }

        public void SetDebug(bool isDebugLog, string debugLogPrefix = null)
        {
            this.isDebugLog = isDebugLog;
            if (isDebugLog && debugLogPrefix != null)
            {
                this.debugLogPrefix = debugLogPrefix;
            }
        }
    }
}