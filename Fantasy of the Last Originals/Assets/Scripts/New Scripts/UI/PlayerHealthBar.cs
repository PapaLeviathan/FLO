using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthBar : MonoBehaviour
{
    [SerializeField] private Image _healthBarImage;
    private Player _player;

    // Start is called before the first frame update
    void Start()
    {
        _player = FindObjectOfType<Player>();
        _healthBarImage.fillAmount = _player.Health.CurrentHealthValue / _player.Health.MAXHealthValue;
        _player.Health.OnHealthUpdate += HandleUpdateHealth;
    }

    private void HandleUpdateHealth()
    {
        _healthBarImage.fillAmount = _player.Health.CurrentHealthValue / _player.Health.MAXHealthValue;
    }


}
