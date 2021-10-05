using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
public class TargetGroupSwitcher : MonoBehaviour
{
    [SerializeField] AutoTargetEnemy _targeter;
    CinemachineTargetGroup _targetGroup;
    // Start is called before the first frame update
    void Start()
    {
        _targetGroup = GetComponent<CinemachineTargetGroup>();
    }

    // Update is called once per frame
    void Update()
    {
        if (_targeter && _targeter.TargetedEnemy != null)
            SwitchTargets(_targeter.TargetedEnemy.transform);
    }

    public void SwitchTargets(Transform enemyTarget)
    {
        _targetGroup.m_Targets[1].target = enemyTarget;
    }
}
