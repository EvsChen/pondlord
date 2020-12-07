﻿using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Cell : Photon.MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    public Image mBgImg, mContentImg;
    public RectTransform mRectTransform;
    public Color mNormalColor;
    public Board mBoard = null;
    public Global mGlobal;
    public int x;
    public int y;

    public GameObject blueLily, whiteLily, goldLily, pinkLily;
    public GameObject mFishPrefab;

    private GameObject lilyChild;
    private PhotonView photonView;

    public int viewid = -1;
    public int parentID = -1;
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
        
        photonView = GetComponent<PhotonView>();
        if (photonView == null)
        {
          Debug.Log("cannot find photonview");
        }

        viewid = photonView.viewID;
    }
    // Update is called once per frame
    void Update()
    {
        gameObject.transform.SetParent(PhotonView.Find(parentID).transform);
        gameObject.transform.localPosition = new Vector3(x * 100, y * 100, 0);
        gameObject.transform.localScale = new Vector3(1, 1, 1);
    }


    
    public void PlantNewLily(LilyType type)
    {
      GameObject child;
      //  switch (type) {
      //    case LilyType.Gold: 
      //      child = Instantiate(goldLily); // Satisfy compiler
      //      break;
      //    case LilyType.Blue:
      //      child = Instantiate(blueLily);
      //      break;
      //    case LilyType.Pink:
      //      child = Instantiate(pinkLily);
      //      break;
      //    case LilyType.White:
      //      child = Instantiate(whiteLily);
      //      break;
      //    default:
      //      child = Instantiate(whiteLily); // Satisfy compiler
      //      break;
      // }
      switch (type) {
            case LilyType.Gold: 
              child = PhotonNetwork.Instantiate(goldLily.name, Vector3.zero, quaternion.identity, 0); // Satisfy compiler
              break;
            case LilyType.Blue:
              child = PhotonNetwork.Instantiate(blueLily.name, Vector3.zero, quaternion.identity, 0);
              break;
            case LilyType.Pink:
              child = PhotonNetwork.Instantiate(pinkLily.name, Vector3.zero, quaternion.identity, 0);
              break;
            case LilyType.White:
              child = PhotonNetwork.Instantiate(whiteLily.name, Vector3.zero, quaternion.identity, 0);
              break;
            default:
              child = PhotonNetwork.Instantiate(whiteLily.name, Vector3.zero, quaternion.identity, 0); // Satisfy compiler
              break;
        }

      child.GetComponent<BaseLily>().parentID = viewid;
        child.transform.SetParent(gameObject.transform);
        child.transform.localPosition = new Vector3(50, 50, 0);
        child.transform.localScale = new Vector3(1, 1, 1);
        child.transform.localRotation = Quaternion.identity;
        ReorderComponent();
    }


    public void OnPointerClick(PointerEventData eventData) {
      if (!PhotonNetwork.inRoom /*|| PhotonNetwork.room.PlayerCount != 2*/)
      {
        return;
      }
      
      if (GameConstants.sunlight <= 0)
      {
        GameConstants.enablePlant = false;
      }
      if (mGlobal.mSelectedLilyType != LilyType.None && GameConstants.enablePlant) {
        GameConstants.sunlight--;
        GameObject.Find("sunlightText").GetComponent<Text>().text = "SunLight: " + GameConstants.sunlight;
        
        //photonView.RPC("PlantNewLily", PhotonTargets.All, mGlobal.mSelectedLilyType);
        PlantNewLily(mGlobal.mSelectedLilyType);
        
        mGlobal.mSelectedLilyType = LilyType.None;
      }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        Color newColor = mNormalColor;
        newColor.a = 0.8f;
        mBgImg.color = newColor;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
      mBgImg.color = mNormalColor;
    }

    public bool HasFish() {
      for (int i = 0; i < transform.childCount; i++) {
        GameObject c = transform.GetChild(i).gameObject;
        if (c.CompareTag(GameConstants.Tags.fish)) {
          return true;
        }
      }
      return false;
    }

    public void ReorderComponent() {
      GameObject contentImg = null, lily = null, fish = null;
      for (int i = 0; i < transform.childCount; i++) {
        GameObject c = transform.GetChild(i).gameObject;
        if (c.name == "contentImg") {
          contentImg = c;
        } else if (c.CompareTag(GameConstants.Tags.lily)) {
          lily = c;
        } else if (c.CompareTag(GameConstants.Tags.fish)) {
          fish = c;
        }
      }
      contentImg.transform.SetAsLastSibling();
      if (lily) {
        lily.transform.SetAsLastSibling();
      }
      if (fish) {
        fish.transform.SetAsLastSibling();
      }
    }

    public void AddFish() {
      GameObject child = PhotonNetwork.Instantiate(mFishPrefab.name, Vector3.zero, quaternion.identity, 0);
      child.GetComponent<Fish>().parentID = viewid;
      ReorderComponent();
    }
    
    void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
      if (stream.isWriting)
      {
        stream.SendNext(parentID);
        stream.SendNext(x);
        stream.SendNext(y);
      }
      else
      {
        parentID = (int)stream.ReceiveNext();
        x = (int)stream.ReceiveNext();
        y = (int)stream.ReceiveNext();
      }
    }

}