using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum LilyType {
  Gold,
  Blue,
  Pink,
  White,
  None
}

public static class GameConstants {
  public static int mBoardWidth = 8;
  public static int mBoardHeight = 8;
    public static int sunlight = 5;
    public static bool enablePlant = true;
}

public class Global : MonoBehaviour
{
    // Start is called before the first frame update
    public LilyType mSelectedLilyType = LilyType.None;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {


            RaycastHit2D hit = Physics2D.GetRayIntersection(Camera.main.ScreenPointToRay(Input.mousePosition)); //edit in your raycast settings
            if (hit)
            {
                GameObject selectedObject = hit.collider.gameObject;
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
            }
        }
    }

    public void OnLilySelect(LilyType newType) {
      mSelectedLilyType = newType;
    }

    public void OnLilyDeselect() {
      mSelectedLilyType = LilyType.None;
    }
}
