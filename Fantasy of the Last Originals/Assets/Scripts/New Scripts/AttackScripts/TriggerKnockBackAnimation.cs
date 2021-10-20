using UnityEngine;

public class TriggerKnockBackAnimation : TriggerStunAnimation
{
    public override void TriggerAnimation(Collider collider)
    {
            collider.GetComponent<Animator>().SetTrigger("Knock Back");
    }
}