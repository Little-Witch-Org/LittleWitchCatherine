using UnityEngine;

public class TransparentTriggerComponentFromOpaque : TransparentTriggerComponent
{
    [SerializeField] private AnimationClip clip;

    public void ToLaunchTrigger()
    {
        animator.Play(clip.name);
    }
}
