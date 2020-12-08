using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ColorEachPlayer : Photon.MonoBehaviour
{

    public TextMeshProUGUI playerText;
    public TextMeshProUGUI otherPlayerText;

    public Image playerImg;
    public Image otherPlayerImg;

    public Color masterColor;
    public Color clientColor;
    
    private bool init = false;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (PhotonNetwork.room.PlayerCount == 2 && !init)
        {
            if (PhotonNetwork.player.IsMasterClient)
            {
                playerText.color = masterColor;
                playerImg.color = masterColor;

                otherPlayerText.color = clientColor;
                otherPlayerImg.color = clientColor;
            }
            else
            {
                playerText.color = clientColor;
                playerImg.color = clientColor;

                otherPlayerText.color = masterColor;
                otherPlayerImg.color = masterColor;
            }

            init = true;
        }
    }
}
