using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class HitBox : MonoBehaviour
{
    public HitBoxDefinition HitBoxDefinition { get; set; }

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
    private KnockBackHandler _targetKnockBackHandler;
    private Player _playerLogic;
    private HitBoxManager _hitBoxManager;


    void OnEnable()
    {
        GetHitBoxDefinition();
        SetTriggerCollider();
        DisableMeshRendererIfPresent();
        AddSwitchCameraComponent();
        FindComboGravityPoint();
        ActivateComboGravityPointIfLinkSkill();

        if (!GetComponent<TriggerStunAnimation>())
            AddProperHitstunComponent();
    }

    private void AddProperHitstunComponent()
    {
        if (HitBoxDefinition.StunType == StunType.HitStun)
            gameObject.AddComponent<TriggerHitStunAnimation>();

        if (HitBoxDefinition.StunType == StunType.KnockBack)
            gameObject.AddComponent<TriggerKnockBackAnimation>();
    }

    private void ActivateComboGravityPointIfLinkSkill()
    {
        if (HitBoxDefinition.SkillType == SkillType.LinkSkill && _comboGravityPoint != null)
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
        _hitBoxManager = GetComponentInParent<HitBoxManager>();
        HitBoxDefinition = _hitBoxManager.CurrentHitBoxDefinition;
    }

    private IEnumerator DeactivateSelf()
    {
        yield return new WaitForSeconds(HitBoxDefinition.HitBoxLinger);
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

    private void Update()
    {
        Collider[] colliders =
            Physics.OverlapSphere(transform.position, HitBoxDefinition.AttackRange, HitBoxDefinition.LayerMask);

        if (colliders.Length == 0)
            return;


        foreach (Collider collider in colliders)
        {
            CacheTargetComponents(collider);

            _newTargetID = collider.GetInstanceID();

            //if the new enemy equals the saved enemy, return;
            if (_newTargetID == _savedTargetID)
            {
                return;
            }

            _savedTargetID = _newTargetID;

            _attackDirection = collider.transform.position - transform.root.position;

           CheckSkillType();
            DoDamage();

            _playerLogic = collider.GetComponent<Player>();

            if (_playerLogic != null)
            {
                _playerLogic.enabled = false;
            }

            if (_targetNavMesh != null)
            {
                _targetNavMesh.enabled = false;
            }

            _targetSound.PlayHitStunSound();

            ChangeRigidBodySettings();

            HandleTargetKnockback(_targetKnockBackHandler, _attackDirection);

            ApplyKnockBack(_targetKnockBackHandler);
            ApplyKnockBackDeceleration();
        }
    }


    private void OnDrawGizmosSelected()
    {
        if (HitBoxDefinition)
            Gizmos.DrawSphere(transform.position, HitBoxDefinition.AttackRange);
        Gizmos.color = Color.red;
    }

    private void HandleTargetKnockback(KnockBackHandler targetKnockBackHandler, Vector3 attackDirection)
    {
        if (targetKnockBackHandler._groundCheck.UpdateIsGrounded())
        {
            attackDirection.y = HitBoxDefinition.KnockUpStrength;
            _knockBackPower = new Vector3(attackDirection.x,
                (attackDirection.y + HitBoxDefinition.KnockUpStrength) * HitBoxDefinition.KnockUpStrength,
                attackDirection.z * HitBoxDefinition.KnockBackStrength);
        }
        else
        {
            attackDirection.y = 0;
            _knockBackPower = new Vector3(attackDirection.x, attackDirection.y + HitBoxDefinition.AirBorneKnockUp,
                attackDirection.z * HitBoxDefinition.KnockBackStrength);
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
        if (HitBoxDefinition.SkillType == SkillType.LinkSkill)
            _comboPoint = new Vector3(_comboGravityPoint.transform.position.x,
                _comboGravityPoint.transform.position.y + HitBoxDefinition.LaunchLimiter,
                _comboGravityPoint.transform.position.z);
    }

    private void DoDamage()
    {
        _targetHealthLogic.TakeDamage(HitBoxDefinition.Damage);
    }

    private void ApplyKnockBack(KnockBackHandler targetKnockBack)
    {
        targetKnockBack.SetKnockBackPower(_knockBackPower);
        targetKnockBack.ResetDownForce();
        targetKnockBack.SetAirStall(HitBoxDefinition.AirStallDuration);
        targetKnockBack.ApplyKnockBack(HitBoxDefinition.HitStopDuration);
        targetKnockBack.SetAirBorneKnockUp(HitBoxDefinition.AirBorneKnockUp);
        targetKnockBack.SetContactPoint(HitBoxDefinition.SkillType, _comboPoint);
    }

    void ApplyKnockBackDeceleration()
    {
        if (_knockBackDecelarationHandler != null)
        {
            _knockBackDecelarationHandler.SetKnockBackTrue(true);
            _knockBackDecelarationHandler.SetKnockBackDeceleration(HitBoxDefinition.KnockBackStrength);
            _knockBackDecelarationHandler.SetDecelerationDuration(HitBoxDefinition.DecelerationDuration);
        }
    }

    void CacheTargetComponents(Collider collider)
    {
        _knockBackDecelarationHandler = collider.GetComponent<KnockBackDecelaration>();
        _targetRigidBody = collider.GetComponent<Rigidbody>();
        _targetHealthLogic = collider.GetComponent<HealthLogic>();
        _targetSound = collider.GetComponent<ImpactSoundHandler>();
        _targetNavMesh = collider.GetComponent<NavMeshAgent>();
        _targetKnockBackHandler = collider.GetComponent<KnockBackHandler>();
    }
}