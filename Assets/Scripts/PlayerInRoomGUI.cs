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
        if (playerNum == null)
        {
            playerNum = GameObject.Find("PlayerNum");
            numText = playerNum.GetComponent<TextMeshProUGUI>();
        }

        if (playerSign == null)
        {
            playerSign = GameObject.Find("PlayerIndicator");
        }

        if (otherplayerSign == null)
        {
            otherplayerSign = GameObject.Find("OtherPlayerIndicator");
        }        
    }

    private void Update()
    {
        if (PhotonNetwork.room == null) {
          return;
        }
        if (playerSign && !playerSign.activeSelf) { 
            if (PhotonNetwork.room.PlayerCount >= 1)
            {
                playerSign.SetActive(true);
                numText = playerNum.GetComponent<TextMeshProUGUI>();
                numText.text = "Number of players: 1";
            } 
        }
        
        if (otherplayerSign && !otherplayerSign.activeSelf)
        {
            if (PhotonNetwork.room.PlayerCount == 2)
            {
                otherplayerSign.SetActive(true);
                numText = playerNum.GetComponent<TextMeshProUGUI>();
                numText.text = "Number of players: 2";
            } 
        }
    }
}