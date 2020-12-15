using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CountDownTimer : Photon.MonoBehaviour
{
    private float totalTime;
    private float currentTime = 0;

    private TextMeshProUGUI minuteText;
    private TextMeshProUGUI secondText;

    public GameObject minute;
    public GameObject second;
    
    // Start is called before the first frame update
    void Start()
    {
        totalTime = GameConstants.countDownTime;
        minuteText = minute.GetComponent<TextMeshProUGUI>();
        secondText = second.GetComponent<TextMeshProUGUI>();
        
        currentTime = totalTime;

    }

    // Update is called once per frame
    void Update()
    {
        if (!PhotonNetwork.inRoom || PhotonNetwork.room.PlayerCount != 2)
        {
            return;
        }
        
        currentTime -= Time.deltaTime;
        
        //
        float minutef = Mathf.FloorToInt(currentTime / 60);
        float secondf = Mathf.FloorToInt(currentTime % 60);
        
        minuteText.text = minutef.ToString("0");
        secondText.text = secondf.ToString("0");

        if (currentTime <= 0)
        {
            currentTime = 0;
            
            PlayerScore playerScore = GameObject.Find("CanvasGlobal").GetComponent<PlayerScore>();
            playerScore.EndGame();
            Debug.Log("Time ends!");
        }
    }
}
