using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Global_MainMenu : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        PhotonNetwork.automaticallySyncScene = true; 
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CompetitiveMode()
    {
      PhotonNetwork.LoadLevel("RoomSelection");
      //SceneManager.LoadScene("SampleScene");
      PlayerPrefs.SetInt("Gamemode", 1);
    }
    
    public void CooperativeMode()
    {
        PhotonNetwork.LoadLevel("RoomSelection");
        //SceneManager.LoadScene("SampleScene");
        PlayerPrefs.SetInt("Gamemode", 0);
    }

    public void RestartGame()
    {
        SceneManager.LoadScene("StartMenu");
    }
    public void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit ();
#endif
    }
}
