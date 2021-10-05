using UnityEngine;

[CreateAssetMenu(menuName = "Hit Box Definition")]
public class HitBoxDefinition : ScriptableObject
{
    public LayerMask LayerMask;
    public SkillType SkillType = SkillType.LinkSkill;
    public StunType StunType = StunType.HitStun;

    public float Damage = 10f;
    public float AttackRange = 1f;
    public float KnockBackStrength = 6f;
    public float KnockUpStrength = 2f;
    public float AirBorneKnockUp = 1;
    public float LaunchLimiter = 1f;

    public float DecelerationDuration = 5f;
    public float HitStopDuration = .2f;
    public float AirStallDuration = .8f;

    public float HitBoxLinger = .2f;
}