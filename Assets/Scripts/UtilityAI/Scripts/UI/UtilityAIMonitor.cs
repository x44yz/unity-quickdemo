using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace AI.Utility
{
    public class UtilityAIMonitor : MonoBehaviour
    {
        public static UtilityAIMonitor Inst;

        public PanelAgents panelAgents;
        public PanelActions panelActions;
        public PanelPreconditions panelPreconditions;
        public PanelConsiderations panelConsiderations;
        public Button btnMonitor;
        public bool autoShowAgents;
        public bool showHideAgents;
        public string scoreFormat;

        private void Awake()
        {
            if (Inst != null)
            {
                Debug.LogError($"[UTILITY_AI]you cant add more than one monitor.");
                return;
            }

            Inst = this;

            // check event system
#if UNITY_EDITOR
            var eventSystem = GameObject.FindObjectOfType<EventSystem>();
            if (eventSystem == null)
            {
                EditorUtility.DisplayDialog("ERROR", "AIMonitor cant work, because no any EventSystem", "OK");
            }
#endif
        }

        private void Start()
        {
            panelAgents.Init(this);
            panelActions.Init(this);
            panelPreconditions.Init(this);
            panelConsiderations.Init(this);

            panelAgents.Hide();
            panelActions.Hide();
            panelPreconditions.Hide();
            panelConsiderations.Hide();

            btnMonitor.onClick.AddListener(()=>{
                if (panelAgents.gameObject.activeSelf)
                    panelAgents.Hide();
                else
                    ShowPanelAgents();
            });

            if (autoShowAgents)
                ShowPanelAgents();

            AgentAI.onAgentCreate += OnAgentCreate;
            AgentAI.onAgentDestroy += OnAgentDestroy;
        }

        private void OnDestroy()
        {
            AgentAI.onAgentCreate -= OnAgentCreate;
            AgentAI.onAgentDestroy -= OnAgentDestroy;
        }

        private void ShowPanelAgents()
        {
            var agents = GameObject.FindObjectsOfType<AgentAI>(showHideAgents);
            panelAgents.Show(agents);
        }

        private void OnAgentCreate(AgentAI agent)
        {
            panelAgents.AddAgent(agent);
        }

        private void OnAgentDestroy(AgentAI agent)
        {
            panelAgents.RemoveAgent(agent);
        }
    }
}
