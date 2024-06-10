using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScenesManager : MonoBehaviour
{

    public GameObject pauseMenu;
    public GameObject gamemanager;
    public bool isSinglePlayer;

    
    // Start is called before the first frame update
    void Start()
    {
        isSinglePlayer =false;
    }

    
}