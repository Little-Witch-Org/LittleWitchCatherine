using System;
using UnityEngine;


/// <summary>
/// Contains triggers for different animator types
/// </summary>
//todo refactor this? to leave one universal animation ?
public class TransparentTriggerComponent : MonoBehaviour
{
    protected Animator animator;

    protected const string TransitionTriggerCycle = "TransitionCycle";
    protected const string TransitionTriggerFadeIn = "TransitionFadeIn";
    protected const string TransitionTriggerFadeOut = "TransitionFadeOut";
    protected void Awake()
    {
        var overlappingImage = GameObject.Find("TransitionManager"); 
        animator = overlappingImage.GetComponent<Animator>();
    }
    public void ToLaunchTriggerCycle()
    {
        animator.SetTrigger(TransitionTriggerCycle);
    }
    public void ToLaunchTriggerFadeIn()
    {
        animator.SetTrigger(TransitionTriggerFadeIn);
    }
    public void ToLaunchTriggerFadeOut()
    {
        animator.SetTrigger(TransitionTriggerFadeOut);
    }
}
