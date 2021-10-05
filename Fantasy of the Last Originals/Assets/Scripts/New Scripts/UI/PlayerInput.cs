using UnityEngine;

public class PlayerInput : MonoBehaviour, IPlayerInput
{
    public static IPlayerInput Instance { get; set; }
    public Vector2 MousePosition => Input.mousePosition;
    
    private void Awake() => Instance = this;
}