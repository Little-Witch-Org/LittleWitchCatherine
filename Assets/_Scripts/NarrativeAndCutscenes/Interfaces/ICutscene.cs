using System.Collections.Generic;
using _Scripts.Managers;
using UnityEngine;
using UnityEngine.Playables;

namespace _Scripts.NarrativeAndCutscenes.Interfaces
{
    public interface ICutscene
    {
        void LaunchCutscene(PlayableDirector director);
    }
}