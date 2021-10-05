using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    [SerializeField] private string _sceneToLoad = "Level1";
    
    public void LoadScene()
    {
        SceneManager.LoadScene(_sceneToLoad);
        SceneManager.LoadScene("UI", LoadSceneMode.Additive);
    }
}
