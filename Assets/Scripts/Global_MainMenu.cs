﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Global_MainMenu : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CompetitiveMode()
    {
        if (PhotonNetwork.room.PlayerCount > 0)
        {
            SceneManager.LoadScene("SampleScene");
        }
    }
    
    public void CooperativeMode()
    {
        if (PhotonNetwork.room.PlayerCount > 0)
        {
            SceneManager.LoadScene("SampleScene");
        }
    }

    public void RestartGame()
    {
        SceneManager.LoadScene("SampleScene");
    }
    public void QuitGame()
    {
        Application.Quit();
    }
}
