using UnityEngine;

public class ToggleDistanceColliders : MonoBehaviour
{
    [SerializeField] GameObject _enemyDistancingColliders;

    public void ToggleDistanceCollidersFalse()
    {
        _enemyDistancingColliders.SetActive(false);
    }

    public void ToggleDistanceCollidersTrue()
    {
        _enemyDistancingColliders.SetActive(true);
    }
}