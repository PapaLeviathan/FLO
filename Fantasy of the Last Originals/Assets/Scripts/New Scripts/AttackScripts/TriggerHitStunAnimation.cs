using UnityEngine;

public class TriggerHitStunAnimation : TriggerStunAnimation
{
    public override void TriggerAnimation(Collider collider)
    {
            collider.GetComponent<Animator>().SetTrigger("Hit Stun");
    }
}