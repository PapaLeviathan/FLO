using UnityEngine;

public class TriggerKnockBackAnimation : TriggerStunAnimation
{
    protected override void TriggerAnimation(Collider collider)
    {
        if(transform.root.gameObject.GetComponent<Animator>())
        collider.GetComponent<Animator>().SetTrigger("Knock Back");
    }
}