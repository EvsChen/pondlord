using System.Collections;
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
    public Board mBoard;
    public float fishTimer = 0.0f, fishTimerMax = 10.0f; // Refresh fish every fishTimerMax secs
    public bool beeEvolve = false;
    public Canvas canvas;
    public Texture2D beeMouse;
    bool init = false;
    public int sunlightNum;
    

    void Start()
    {
        sunlightNum = GameConstants.sunlight;
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

    void InitBoard() {
      GameObject boardObj = GameObject.Find("PF_board");
      if (!boardObj) {
        return;
      }
      init = true;
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

    // Update is called once per frame
    void Update()
    {
        if (!init) {
          InitBoard(); // Try finding board if we didn't find one already.
        }
        if (beeEvolve)
        {
            //Debug.Log("bee true");
            //Cursor.visible = false;
            //Cursor.SetCursor(beeMouse, Vector2.zero, CursorMode.Auto);
        }
        else
        {
            // Debug.Log("bee false");
            //Cursor.visible = true;
            //Cursor.SetCursor(default, Vector2.zero, CursorMode.Auto);
        }
        /*
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

         


                if (beeEvolve && selectedObject.name.Contains("Seed"))
                {
                    selectedObject.GetComponent<BaseLily>().progress = 100;
                    Cursor.SetCursor(default, Vector2.zero, CursorMode.Auto);
                    beeEvolve = false;
                }
            }
            }
        */
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
