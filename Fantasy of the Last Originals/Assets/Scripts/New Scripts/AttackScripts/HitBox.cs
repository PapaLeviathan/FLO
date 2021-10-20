using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class HitBox : MonoBehaviour
{
    public AttackDefinition AttackDefinition { get; set; }
    public Transform EffectPosition;

    private Transform _comboGravityPoint;

    private Rigidbody _targetRigidBody;
    private float _defaultKnockUpStrength = 1.2f;


    private HealthLogic _targetHealthLogic;
    private KnockBackDecelaration _knockBackDecelarationHandler;

    private Vector3 _knockBackPower;
    private Vector3 _comboPoint;
    private int _savedTargetID;
    private int _newTargetID;

    private Vector3 _attackDirection;
    private ImpactSoundHandler _targetSound;
    private NavMeshAgent _targetNavMesh;
    private StunHandler _targetStunHandler;
    private Player _playerLogic;
    private AttackDefinitionManager _attackDefinitionManager;


    void OnEnable()
    {
        GetHitBoxDefinition();
        DisableMeshRendererIfPresent();
        AddSwitchCameraComponent();
        FindComboGravityPoint();
        ActivateComboGravityPointIfLinkSkill();

        gameObject.transform.localScale = new Vector3(AttackDefinition.AttackRange + .1f,
            AttackDefinition.AttackRange + .1f,
            AttackDefinition.AttackRange + .1f);
        if (!GetComponent<TriggerStunAnimation>())
            AddProperHitstunComponent();
    }

    private void AddProperHitstunComponent()
    {
        if (AttackDefinition.StunType == StunType.HitStun)
            gameObject.AddComponent<TriggerHitStunAnimation>();

        if (AttackDefinition.StunType == StunType.KnockBack)
            gameObject.AddComponent<TriggerKnockBackAnimation>();
    }

    private void ActivateComboGravityPointIfLinkSkill()
    {
        if (AttackDefinition.SkillType == SkillType.LinkSkill && _comboGravityPoint != null)
            _comboGravityPoint.gameObject.SetActive(true);
    }

    private void FindComboGravityPoint()
    {
        _comboGravityPoint = transform.root.gameObject.transform.Find("Combo Gravity Point");
    }

    private void AddSwitchCameraComponent()
    {
        if (!GetComponent<SwitchCameraOnEvent>())
            gameObject.AddComponent<SwitchCameraOnEvent>();
    }

    private void DisableMeshRendererIfPresent()
    {
        if (GetComponent<MeshRenderer>())
            GetComponent<MeshRenderer>().enabled = false;
    }

    private void SetTriggerCollider()
    {
        if (GetComponent<Collider>())
            GetComponent<Collider>().isTrigger = true;
        else
        {
            gameObject.AddComponent<Collider>();
            GetComponent<Collider>().isTrigger = true;
        }
    }

    private void GetHitBoxDefinition()
    {
        _attackDefinitionManager = GetComponentInParent<AttackDefinitionManager>();
        AttackDefinition = _attackDefinitionManager.CurrentAttackDefinition;
    }

    private IEnumerator DeactivateSelf()
    {
        yield return new WaitForSeconds(AttackDefinition.HitBoxLinger);
        gameObject.SetActive(false);
    }

    void OnDisable()
    {
        if (GetComponent<TriggerStunAnimation>())
            RemoveStunTypeComponent();

        _savedTargetID = 0;

        if (_comboGravityPoint != null)
            if (_comboGravityPoint.gameObject.activeInHierarchy)
                _comboGravityPoint.gameObject.SetActive(false);
    }

    private void RemoveStunTypeComponent()
    {
        Destroy(GetComponent<TriggerStunAnimation>());
    }

    private void FixedUpdate()
    {
        Collider[] colliders =
            Physics.OverlapSphere(transform.position, AttackDefinition.AttackRange, AttackDefinition.LayerMask);

        if (colliders.Length == 0)
            return;


        foreach (Collider collider in colliders)
        {
            if (collider.isTrigger)
                return;


            _newTargetID = collider.GetInstanceID();

            //if the new enemy equals the saved enemy, return;
            if (_newTargetID == _savedTargetID)
            {
                return;
            }

            CacheTargetComponents(collider);
            GetComponent<TriggerStunAnimation>().TriggerAnimation(collider);
            _savedTargetID = _newTargetID;
            _targetStunHandler.DisableNavMesh();

            TransferInfoToTarget(collider);
        }
    }

    private void TransferInfoToTarget(Collider collider)
    {
        _attackDirection = collider.transform.position - transform.root.position;

        CheckSkillType();
        DoDamage();

        _playerLogic = collider.GetComponent<Player>();

        if (_playerLogic != null)
        {
            _playerLogic.enabled = false;
        }

        _targetSound.PlayHitStunSound();

        ChangeRigidBodySettings();

        HandleTargetKnockback(_targetStunHandler, _attackDirection);

        ApplyKnockBack(_targetStunHandler);
        ApplyKnockBackDeceleration();
    }


    private void OnDrawGizmosSelected()
    {
        if (AttackDefinition)
            Gizmos.DrawSphere(transform.position, AttackDefinition.AttackRange);
        Gizmos.color = Color.red;
    }

    private void HandleTargetKnockback(StunHandler targetStunHandler, Vector3 attackDirection)
    {
        if (targetStunHandler._groundCheck.UpdateIsGrounded())
        {
            attackDirection.y = AttackDefinition.KnockUpStrength;
            _knockBackPower = new Vector3(attackDirection.x,
                (attackDirection.y + AttackDefinition.KnockUpStrength) * AttackDefinition.KnockUpStrength,
                attackDirection.z * AttackDefinition.KnockBackStrength);
        }
        else
        {
            attackDirection.y = 0;
            _knockBackPower = new Vector3(attackDirection.x, attackDirection.y + AttackDefinition.AirBorneKnockUp,
                attackDirection.z * AttackDefinition.KnockBackStrength);
        }
    }


    private void ChangeRigidBodySettings()
    {
        _targetRigidBody.isKinematic = false;
        _targetRigidBody.useGravity = false;
        _targetRigidBody.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY
                                                                            | RigidbodyConstraints.FreezeRotationZ;
    }

    private void CheckSkillType()
    {
        if (AttackDefinition.SkillType == SkillType.LinkSkill)
            _comboPoint = new Vector3(_comboGravityPoint.transform.position.x,
                _comboGravityPoint.transform.position.y + AttackDefinition.LaunchLimiter,
                _comboGravityPoint.transform.position.z);
    }

    private void DoDamage()
    {
        _targetHealthLogic.TakeDamage(AttackDefinition.Damage);
    }

    private void ApplyKnockBack(StunHandler targetStun)
    {
        targetStun.AirBorneKnockUp = AttackDefinition.AirBorneKnockUp;
        targetStun.StunDuration = AttackDefinition.StunDuration;
        targetStun.ApplyKnockBack(AttackDefinition.HitStopDuration);
        targetStun.AllowKnockBackToApply(_knockBackPower);
        targetStun.SetAirStall(AttackDefinition.AirStallDuration);
        targetStun.SetContactPoint(AttackDefinition.SkillType, _comboPoint);
        targetStun.ResetDownForce();
    }

    void ApplyKnockBackDeceleration()
    {
        if (_knockBackDecelarationHandler != null)
        {
            _knockBackDecelarationHandler.SetKnockBackTrue(true);
            _knockBackDecelarationHandler.SetKnockBackDeceleration(AttackDefinition.KnockBackStrength);
            _knockBackDecelarationHandler.SetDecelerationDuration(AttackDefinition.DecelerationDuration);
        }
    }

    void CacheTargetComponents(Collider collider)
    {
        _knockBackDecelarationHandler = collider.GetComponent<KnockBackDecelaration>();
        _targetRigidBody = collider.GetComponent<Rigidbody>();
        _targetHealthLogic = collider.GetComponent<HealthLogic>();
        _targetSound = collider.GetComponent<ImpactSoundHandler>();
        _targetNavMesh = collider.GetComponent<NavMeshAgent>();
        _targetStunHandler = collider.GetComponent<StunHandler>();
    }
}