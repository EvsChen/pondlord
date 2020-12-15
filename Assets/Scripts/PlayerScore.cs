using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerScore : Photon.MonoBehaviour
{
    //score
    private int masterScore = 0;
    private int clientScore = 0;
    // Start is called before the first frame update
    void Start()
    {
        masterScore = 0;
        clientScore = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (masterScore + clientScore == GameConstants.maxScore)
        {
            EndGame();
        
        }
    }
    
    public void AddMasterScore()
    {
        photonView.RPC("AddMasterScorePun", PhotonTargets.All);
        GameObject.Find("MasterScore").GetComponent<Text>().text = "Red: " + masterScore;
    }
    [PunRPC]
    public void AddMasterScorePun()
    {
        masterScore++;
    }
    //
    public void ReduceMasterScore()
    {
        photonView.RPC("ReduceMasterScorePun", PhotonTargets.All);
        GameObject.Find("MasterScore").GetComponent<Text>().text = "Red: " + masterScore;
    }
    [PunRPC]
    public void ReduceMasterScorePun()
    {
        masterScore--;
    }
    //
    public void AddClientScore()
    {
        photonView.RPC("AddClientScorePun", PhotonTargets.All);
        GameObject.Find("ClientScore").GetComponent<Text>().text = "Blue: " + clientScore;
    }
    [PunRPC]
    public void AddClientScorePun()
    {
        clientScore++;
        
    }
    //
    public void ReduceClientScore()
    {
        photonView.RPC("ReduceClientScorePun", PhotonTargets.All);
        GameObject.Find("ClientScore").GetComponent<Text>().text = "Blue: " + clientScore;
    }
    [PunRPC]
    public void ReduceClientScorePun()
    {
        clientScore--;
    }

    public void EndGame()
    {
        //Cooperative
        if (PlayerPrefs.GetInt("Gamemode") == 0)
        {
            if (masterScore + clientScore == GameConstants.maxScore)
            {
                PhotonNetwork.DestroyPlayerObjects(PhotonNetwork.player);
                PhotonNetwork.LeaveRoom();
                SceneManager.LoadScene("WinScene");
            }
            else
            {
                PhotonNetwork.DestroyPlayerObjects(PhotonNetwork.player);
                PhotonNetwork.LeaveRoom();
                SceneManager.LoadScene("LoseScene");
            }
        }
        // Competitive
        else if (PlayerPrefs.GetInt("Gamemode") == 1)
        {
            if (PhotonNetwork.player.IsMasterClient)
            {
                PhotonNetwork.DestroyPlayerObjects(PhotonNetwork.player);
                PhotonNetwork.LeaveRoom();
                if (masterScore < clientScore)
                {
                    SceneManager.LoadScene("LoseScene");
                }
                else
                {
                    SceneManager.LoadScene("WinScene");
                }
            }
            else
            {
                PhotonNetwork.DestroyPlayerObjects(PhotonNetwork.player);
                PhotonNetwork.LeaveRoom();
                if (clientScore < masterScore)
                {
                    SceneManager.LoadScene("LoseScene");
                }
                else
                {
                    SceneManager.LoadScene("WinScene");
                }
            }

        }

    }


}
