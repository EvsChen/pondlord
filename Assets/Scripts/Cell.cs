using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;


public class Cell : Photon.MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    public Image mBgImg, mContentImg;
    public RectTransform mRectTransform;
    public Color mNormalColor,
                 mMasterColor = new Color(0.5f, 0.0f, 0.0f, 0.5f),
                 mClientColor = new Color(0.0f, 0.0f, 0.5f, 0.5f);
    public Board mBoard = null;
    public Global mGlobal;
    public int x;
    public int y;

    public GameObject blueLily, whiteLily, goldLily, pinkLily;
    public GameObject mFishPrefab;
    public GameObject mFrogPrefab;

    public int viewid = -1;
    public int parentID = -1;

    private float starttime;
    bool synced = false;

    public bool HasLily() {
      for (int i = 0; i < transform.childCount; i++) {
        GameObject c = transform.GetChild(i).gameObject;
        if (c.CompareTag(GameConstants.Tags.lily)) {
          return true;
        }
      }
      return false;
    }

    // Start is called before the first frame update
    void Start()
    {
      starttime = Time.time;
      mRectTransform = GetComponent<RectTransform>();
      mBgImg = GetComponent<Image>();
      GameObject c = transform.GetChild(0).gameObject;
      mContentImg = c.GetComponent<Image>();
      GameObject globalObj = GameObject.Find("GlobalObj");
      mGlobal = globalObj.GetComponent<Global>();
      mBgImg.color = mNormalColor;
      mContentImg.color = mNormalColor;
      
      if (photonView == null)
      {
        Debug.Log("cannot find photonview");
      } else {
        viewid = photonView.viewID;
      }
    }
    // Update is called once per frame
    void Update()
    {
      if (!synced) {
        SyncCell();
      }
    }

    public void SyncCell()
    {
      PhotonView parent = PhotonView.Find(parentID);
      if (!parent) {
        return;
      }
      synced = true;
      gameObject.transform.SetParent(parent.transform);
      gameObject.transform.localPosition = new Vector3(x * 100, y * 100, 0);
      gameObject.transform.localScale = new Vector3(1, 1, 1);
      RectTransform rectTransform = gameObject.GetComponent<RectTransform>();
      rectTransform.anchoredPosition = new Vector2(x * 100, y * 100);
      if ((x + y) % 2 == 0) {
        mNormalColor = new Color(0.9f, 0.85f, 0.72f, 0.1f);
      } else {
        mNormalColor = new Color(1.0f, 1.0f, 1.0f, 0.1f);
      }
      mBgImg.color = mNormalColor;
      mContentImg.color = mNormalColor; 
    }
    
    public void PlantNewLily(LilyType type)
    {
      GameObject child;
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
           if(mGlobal.mSelectedLilyType == LilyType.Gold && (GameConstants.sunlight-8) >= 0)
            {
                GameConstants.sunlight -= 8;
                PlantNewLily(mGlobal.mSelectedLilyType);
            }
            else if (mGlobal.mSelectedLilyType == LilyType.Blue && (GameConstants.sunlight - 3) >= 0)
           {
                GameConstants.sunlight -= 3;
                PlantNewLily(mGlobal.mSelectedLilyType);
            }
            else if (mGlobal.mSelectedLilyType == LilyType.Pink && (GameConstants.sunlight - 5) >= 0)
            {
                GameConstants.sunlight -= 5;
                PlantNewLily(mGlobal.mSelectedLilyType);
            }
            else if (mGlobal.mSelectedLilyType == LilyType.White && (GameConstants.sunlight - 2) >= 0)
            {
                GameConstants.sunlight -= 2;
                PlantNewLily(mGlobal.mSelectedLilyType);
            }

            GameObject.Find("sunlightText").GetComponent<Text>().text = "SunLight: " + GameConstants.sunlight;
        //photonView.RPC("PlantNewLily", PhotonTargets.All, mGlobal.mSelectedLilyType);
        
        mGlobal.mSelectedLilyType = LilyType.None;
      }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        Color newColor = mBgImg.color;
        newColor.a -= 0.2f;
        mBgImg.color = newColor;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        
        Color newColor = mBgImg.color;
        newColor.a += 0.2f;
        mBgImg.color = newColor;
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

    public bool HasFrog()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            GameObject c = transform.GetChild(i).gameObject;
            if (c.CompareTag(GameConstants.Tags.frog))
            {
                return true;
            }
        }
        return false;
    }

    public void ReorderComponent()
    {
        GameObject contentImg = null, lily = null, fish = null, frog = null;
        for (int i = 0; i < transform.childCount; i++)
        {
            GameObject c = transform.GetChild(i).gameObject;
            if (c.name == "contentImg")
            {
                contentImg = c;
            }
            else if (c.CompareTag(GameConstants.Tags.lily))
            {
                lily = c;
            }
            else if (c.CompareTag(GameConstants.Tags.fish))
            {
                fish = c;
            }
            else if (c.CompareTag(GameConstants.Tags.frog))
            {
                frog = c;
            }
            //contentImg.transform.SetAsLastSibling();
            if (lily)
            {
                lily.transform.SetAsLastSibling();
            }
            if (fish)
            {
                fish.transform.SetAsLastSibling();
            }
            if (frog)
            {
                fish.transform.SetAsLastSibling();
            }
        }
    }

    public void AddFish() {
      GameObject child = PhotonNetwork.Instantiate(mFishPrefab.name, Vector3.zero, quaternion.identity, 0);
      child.GetComponent<Fish>().parentID = viewid;
      ReorderComponent();
    }
    public void AddFrog()
    {
        GameObject child = PhotonNetwork.Instantiate(mFrogPrefab.name, Vector3.zero, quaternion.identity, 0);
        child.GetComponent<frog>().parentID = viewid;
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
