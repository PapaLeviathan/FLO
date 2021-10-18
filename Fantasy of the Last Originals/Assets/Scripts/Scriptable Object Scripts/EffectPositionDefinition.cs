using UnityEngine;

[CreateAssetMenu(menuName = "EffectPositionDefinition")]
public class EffectPositionDefinition : ScriptableObject
{
    public Vector3 EffectPosition;
    public Quaternion EffectRotation;
    public Vector3 EffectScale;
}