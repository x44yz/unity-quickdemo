using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AI.FSM
{
    public class StateMachine<T> where T : IStateMachineOwner
    {
        public T owner;
        public State curState;
        public Dictionary<State, List<Transition>> transitions = new Dictionary<State, List<Transition>>();
        public Dictionary<Type, State> states = new Dictionary<Type, State>();

        public StateMachine(T owner)
        {
            this.owner = owner;
        }

        public void Register(State st)
        {
            var tp = st.GetType();
            if (states.ContainsKey(tp))
            {
                Debug.LogError($"[FSM]{owner.FSMDebugLogPrefix} failed Register because exist same state > {tp}");
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
                        if (owner.IsFSMDebug)
                        {
                            Debug.Log($"[FSM]{owner.FSMDebugLogPrefix} translate from {curState} to {ts.to}");
                        }

                        ts.OnTransition();
                        Translate(ts.to);
                        break;
                    }
                }
            }
        }

        public void Translate<TS>(params object[] data) where TS : FSM.State 
        {
            Translate(typeof(TS), data);
        }

        public void Translate(Type tp, params object[] data)
        {
            State st = null;
            if (!states.TryGetValue(tp, out st))
            {
                Debug.LogError($"[FSM]{owner.FSMDebugLogPrefix} failed Translate because cant find state > {tp}");
                return;
            }
            Translate(st, data);
        }

        public void Translate(State st, params object[] data)
        {
            State lastState = null;
            if (curState != null)
            {
                lastState = curState;
                curState.OnExit(st);
            }

            curState = st;
            curState.OnEnter(lastState, data);
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
    }
}