using System;
using System.Collections.Generic;
using _Scripts.Dialog_Ink;
using _Scripts.Enums.Places;
using _Scripts.NarrativeAndCutscenes;
using _Scripts.NarrativeAndCutscenes.Checkpoints;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Serialization;
using UnityEngine.Timeline;
using UnityEngine.UI;

namespace _Scripts.Managers
{
    /// <summary>
    /// This is narrative controller class. He follows the plot line and controls checkpoints.
    /// </summary>
    public class StoryProgressionManager : MonoBehaviour
    {
        
        public static StoryProgressionManager Instance;
        
        private Dictionary<int, Checkpoint> _checkpoints = new Dictionary<int, Checkpoint>();
        private int _currentCheckpointID = 0;
        
        
        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }

        }

        private void Start()
        {
            InitializeCheckpoints();
        }


        public void InitializeCheckpoints() {
            _checkpoints.Add(1,new Checkpoint1());
        }

        public void ActivateCheckpoint(int id) {
            if (_checkpoints.TryGetValue(id, out var checkpoint)) {
                checkpoint.Activate();
            }

            _currentCheckpointID = id;
            
            // disable dev npc
            // disable displaying not met quests (buttons in ui)
            EventManager.Instance.GameEvents.CheckpointActivated(_currentCheckpointID);
        }
        
    }
}