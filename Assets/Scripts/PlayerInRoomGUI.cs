using System;
using System.Collections;
using System.Collections.Generic;
using ExitGames.UtilityScripts;
using Photon;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using MonoBehaviour = Photon.MonoBehaviour;

public class PlayerInRoomGUI : MonoBehaviour
{
    public GameObject playerNum;
    private TextMeshProUGUI numText;
    public GameObject playerSign;
    public GameObject otherplayerSign;
    
    private void Start()
    {
        numText = playerNum.GetComponent<TextMeshProUGUI>();
    }

    private void Update()
    {
        if (!playerSign.activeSelf)
        {
            if (PhotonNetwork.room.PlayerCount >= 1)
            {
                playerSign.SetActive(true);
                numText.text = "Number of players: 1";
            } 
        }
        
        if (!otherplayerSign.activeSelf)
        {
            if (PhotonNetwork.room.PlayerCount == 2)
            {
                otherplayerSign.SetActive(true);
                numText.text = "Number of players: 2";
            } 
        }
    }
}