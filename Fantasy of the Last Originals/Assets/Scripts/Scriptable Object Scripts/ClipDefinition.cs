using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Attack Animation Definition")]
public class ClipDefinition : ScriptableObject
{
    [SerializeField] private AnimationClip _clip;
    [SerializeField] private AttackDefinition attackDefinition;
}