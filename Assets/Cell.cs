using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Cell : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    public Image mBgImg, mContentImg;
    public RectTransform mRectTransform;
    public Color mNormalColor;
    public Board mBoard = null;
    public Global mGlobal;
    public int x;
    public int y;

    public GameObject blueLily, whiteLily, goldLily, pinkLily;
 
    // Start is called before the first frame update
    void Start()
    {
      
        mRectTransform = GetComponent<RectTransform>();
        mBgImg = GetComponent<Image>();
        GameObject c = transform.GetChild(0).gameObject;
        mContentImg = c.GetComponent<Image>();
        GameObject globalObj = GameObject.Find("GlobalObj");
        mGlobal = globalObj.GetComponent<Global>();
        mBgImg.color = mNormalColor;
        mContentImg.color = mNormalColor;
    }
    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnPointerClick(PointerEventData eventData) {
      Debug.Log("Cell click");
      if (GameConstants.sunlight <= 0)
      {
        GameConstants.enablePlant = false;
      }
      if (mGlobal.mSelectedLilyType != LilyType.None && GameConstants.enablePlant) {
        Debug.Log("Cell click " + mGlobal.mSelectedLilyType.ToString("g"));
        GameObject child;
        switch (mGlobal.mSelectedLilyType) {
            case LilyType.Gold: 
              child = PhotonNetwork.Instantiate(goldLily.name, Vector3.zero, quaternion.identity, 0); // Satisfy compiler
              break;
            case LilyType.Blue:
              child = PhotonNetwork.Instantiate(blueLily.name, Vector3.zero, quaternion.identity, 0);;
              break;
            case LilyType.Pink:
              child = PhotonNetwork.Instantiate(pinkLily.name, Vector3.zero, quaternion.identity, 0);;
              break;
            case LilyType.White:
              child = PhotonNetwork.Instantiate(whiteLily.name, Vector3.zero, quaternion.identity, 0);;
              break;
            default:
              child = PhotonNetwork.Instantiate(whiteLily.name, Vector3.zero, quaternion.identity, 0); // Satisfy compiler
              break;
        }
        child.transform.SetParent(gameObject.transform);
        child.transform.localPosition = new Vector3(50, 50, 0);
        child.transform.localScale = new Vector3(1, 1, 1);
        child.transform.localRotation = Quaternion.identity;
<<<<<<< Updated upstream
=======
    }

    public void OnPointerClick(PointerEventData eventData) {
      Debug.Log("Number of players: " + PhotonNetwork.room.PlayerCount);
      if (!PhotonNetwork.inRoom /*|| PhotonNetwork.room.PlayerCount != 2*/)
      {
        // only use PhotonNetwork.Instantiate while in a room.
        return;
      }
      Debug.Log("Cell click");
      if (GameConstants.sunlight <= 0)
      {
        GameConstants.enablePlant = false;
      }
      if (mGlobal.mSelectedLilyType != LilyType.None && GameConstants.enablePlant) {
        Debug.Log("Cell click " + mGlobal.mSelectedLilyType.ToString("g"));
        GameConstants.sunlight--;
        GameObject.Find("sunlightText").GetComponent<Text>().text = "SunLight: " + GameConstants.sunlight;
        PlantNewLily(mGlobal.mSelectedLilyType);
        mGlobal.mSelectedLilyType = LilyType.None;
>>>>>>> Stashed changes
      }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        Debug.Log("The cursor entered the selectable UI element.");
        Color newColor = mNormalColor;
        newColor.a = 0.8f;
        mBgImg.color = newColor;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
      Debug.Log("The cursor exited the selectable UI element.");
      mBgImg.color = mNormalColor;
    }

}
