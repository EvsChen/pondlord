using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScore : Photon.MonoBehaviour
{
    //score
    private int masterScore = 0;
    private int clientScore = 0;

    private PhotonView photonView;
    
    // Start is called before the first frame update
    void Start()
    {
        masterScore = 0;
        clientScore = 0;
        photonView = GetComponent<PhotonView>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    public void AddMasterScore()
    {
        photonView.RPC("AddMasterScorePun", PhotonTargets.All);
        Debug.Log("Master score: " + masterScore);
        Debug.Log("Client score: " + clientScore);
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
        Debug.Log("Master score: " + masterScore);
        Debug.Log("Client score: " + clientScore);
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
    }
    [PunRPC]
    public void ReduceClientScorePun()
    {
        clientScore--;
    }
}
