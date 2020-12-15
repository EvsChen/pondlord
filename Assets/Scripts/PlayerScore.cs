using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerScore : Photon.MonoBehaviour
{
    public GameObject wincanvas;

    public GameObject losecanvas;
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
    }
    [PunRPC]
    public void AddMasterScorePun()
    {
        masterScore++;
        GameObject.Find("MasterScore").GetComponent<Text>().text = "Red: " + masterScore;
    }
    //
    public void ReduceMasterScore()
    {
        photonView.RPC("ReduceMasterScorePun", PhotonTargets.All);
    }
    [PunRPC]
    public void ReduceMasterScorePun()
    {
        masterScore--;
        GameObject.Find("MasterScore").GetComponent<Text>().text = "Red: " + masterScore;
    }
    //
    public void AddClientScore()
    {
        photonView.RPC("AddClientScorePun", PhotonTargets.All);
    }
    [PunRPC]
    public void AddClientScorePun()
    {
        clientScore++;
        GameObject.Find("ClientScore").GetComponent<Text>().text = "Blue: " + clientScore;
        
    }
    //
    public void ReduceClientScore()
    {
        photonView.RPC("ReduceClientScorePun", PhotonTargets.All);
    }
    [PunRPC]
    public void ReduceClientScorePun()
    {
        clientScore--;
        GameObject.Find("ClientScore").GetComponent<Text>().text = "Blue: " + clientScore;
    }

    public void EndGame()
    {
        //Cooperative
        if (PlayerPrefs.GetInt("Gamemode") == 0)
        {
            if (masterScore + clientScore == GameConstants.maxScore)
            {
                Instantiate(wincanvas);
                //SceneManager.LoadScene("WinScene");
            }
            else
            {
                Instantiate(losecanvas);
                //GameObject.Find("Canvas").SetActive(false);
                GameObject.Find("LoseCanvas").SetActive(true);
                //SceneManager.LoadScene("LoseScene");
            }
        }
        // Competitive
        else if (PlayerPrefs.GetInt("Gamemode") == 1)
        {
            if (PhotonNetwork.player.IsMasterClient)
            {
                if (masterScore < clientScore)
                {
                    Instantiate(losecanvas);
                }
                else
                {
                    Instantiate(wincanvas);
                }
            }
            else
            {
                Debug.Log("client lose");
                PhotonNetwork.DestroyPlayerObjects(PhotonNetwork.player);
                PhotonNetwork.LeaveRoom();
                if (clientScore < masterScore)
                {
                    Instantiate(losecanvas);
                }
                else
                {
                    Instantiate(wincanvas);
                }
            }

        }

    }

}
