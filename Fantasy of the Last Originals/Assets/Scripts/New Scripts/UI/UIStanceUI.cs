using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIStanceUI : MonoBehaviour
{
    //Stance Icon variables
    [SerializeField]
    Sprite m_stanceImage1, m_stanceImage2, m_stanceImage3, m_stanceImage4;
    [SerializeField]
    Image m_mainStanceImage;

    public GameObject m_controlPanel;
    private Player _player;
    private CombatManagerScript _combatManager;

    // Start is called before the first frame update
    void Start()
    {
        _player = FindObjectOfType<Player>();
        _combatManager = _player.GetComponent<CombatManagerScript>();
        m_mainStanceImage.sprite = m_stanceImage1;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            if (!m_controlPanel.activeInHierarchy)
            {
                m_controlPanel.SetActive(true);
            }
            else if (m_controlPanel.activeInHierarchy)
            {
                m_controlPanel.SetActive(false);
            }
        }
        if (_combatManager._player.Stance == PlayerStance.Stance1)
        {
            m_mainStanceImage.sprite = m_stanceImage1;
        }
        if (_combatManager._player.Stance == PlayerStance.Stance2)
        {
            m_mainStanceImage.sprite = m_stanceImage2;
        }
        if (_combatManager._player.Stance == PlayerStance.Stance3)
        {
            m_mainStanceImage.sprite = m_stanceImage3;
        }
        if (_combatManager._player.Stance == PlayerStance.Stance4)
        {
            m_mainStanceImage.sprite = m_stanceImage4;
        }

    }
}
