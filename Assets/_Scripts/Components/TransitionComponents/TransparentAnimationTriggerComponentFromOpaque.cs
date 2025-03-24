using UnityEngine;

public class TransparentAnimationTriggerComponentFromOpaque : TransparentAnimationTriggerComponent
{
    [SerializeField] private AnimationClip clip;

    public void ToLaunchTrigger()
    {
        animator.Play(clip.name);
    }
}
