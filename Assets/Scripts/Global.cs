﻿using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public enum LilyType {
  Gold,
  Blue,
  Pink,
  White,
  None
}
public class Global : MonoBehaviour
{
    // Start is called before the first frame update
    public LilyType mSelectedLilyType = LilyType.None;
    public Texture beeMouse;
    public Board mBoard;
    public float fishTimer = 0.0f, fishTimerMax = 10.0f; // Refresh fish every fishTimerMax secs

    public Canvas canvas;
    void Start()
    {
        // if (PhotonNetwork.player.IsMasterClient)
        // {
        //     PhotonNetwork.Instantiate(canvas.name, Vector3.zero, quaternion.identity, 0);
        //     
        // }
        GameObject boardObj = GameObject.Find("PF_board");
        mBoard = boardObj.GetComponent<Board>();
        //Set gamemode
        GameObject GamemodeObj = GameObject.Find("Gamemode");
        Text GamemodeText = GamemodeObj.GetComponent<Text>();
        if (PlayerPrefs.GetInt("Gamemode") == 0)
        {
            GamemodeText.text = "Cooperative Mode";
        } else if (PlayerPrefs.GetInt("Gamemode") == 1)
        {
            GamemodeText.text = "Competitive Mode";
        }
    }

    void AddFish() {
      bool added = false;
      while (!added) {
        int rX = Random.Range(0, GameConstants.mBoardWidth);
        int rY = Random.Range(0, GameConstants.mBoardHeight);
        Cell cell = mBoard.mAllCells[rX, rY];
        if (cell.HasFish()) {
          continue;
        }
        added = true;
        cell.AddFish();
      }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            RaycastHit2D hit = Physics2D.Raycast(this.gameObject.transform.position, Input.mousePosition);
          //edit in your raycast settings
            Debug.Log(hit.collider);
            if (hit.collider!=null)
            {
                Debug.Log("select");
                GameObject selectedObject = hit.collider.gameObject;
                Debug.Log(selectedObject);
                if (selectedObject.name.Contains("Sunlight"))
                {
                    GameConstants.sunlight++;
                    GameObject.Find("sunlightText").GetComponent<Text>().text = "SunLight: " + GameConstants.sunlight;
                    Destroy(selectedObject);

                    if (GameConstants.sunlight > 0)
                    {
                        GameConstants.enablePlant = true;
                    }
                }

                if (selectedObject.name.Contains("bee"))
                {
                    Cursor.visible = false;
                    Vector3 vector3 = Input.mousePosition;
                    GUI.DrawTexture(new Rect(vector3.x - beeMouse.width / 2, (Screen.height - vector3.y) - beeMouse.height / 2, beeMouse.width, beeMouse.height), beeMouse);
                }
            }
            }
        
        fishTimer += Time.deltaTime;
        if (fishTimer > fishTimerMax) {
          //AddFish();
          fishTimer = 0.0f;
        }
    }

    public void OnLilySelect(LilyType newType) {
      mSelectedLilyType = newType;
    }

    public void OnLilyDeselect() {
      mSelectedLilyType = LilyType.None;
    }
}
