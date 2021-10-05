using UnityEngine;

public class TriggerHitStunAnimation : TriggerStunAnimation
{
    protected override void TriggerAnimation(Collider collider)
    {
        if (transform.root.gameObject.GetComponent<Animator>())
            collider.GetComponent<Animator>().SetTrigger("Hit Stun");
    }
}