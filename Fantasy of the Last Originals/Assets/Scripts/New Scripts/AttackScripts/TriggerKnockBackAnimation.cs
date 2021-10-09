using UnityEngine;

public class TriggerKnockBackAnimation : TriggerStunAnimation
{
    protected override void TriggerAnimation(Collider collider)
    {
            collider.GetComponent<Animator>().SetTrigger("Knock Back");
    }
}