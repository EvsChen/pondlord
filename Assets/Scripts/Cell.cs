using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;


public class Cell : Photon.MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler, IPunObservable
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
    public AudioSource click;
    public AudioSource evolve;

    public GameObject blueLily, whiteLily, goldLily, pinkLily;
    public GameObject evolveParticle;
    public GameObject mFishPrefab;
    public GameObject mFrogPrefab;

    public int viewid = -1;
    public int parentID = -1;

    private float starttime;
    bool synced = false;

    private GameObject thislily;
    public int thislilyViewId = -1;

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

        if (PhotonView.Find(thislilyViewId) != null)
        {
            return;
        }
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

      thislily = child;
      thislilyViewId = thislily.GetComponent<PhotonView>().viewID;
      
      //update score
          PlayerScore playerScore = GameObject.Find("CanvasGlobal").GetComponent<PlayerScore>();
          if (child.GetPhotonView().owner.IsMasterClient)
          {
              playerScore.AddMasterScore();
          }
          else
          {
              playerScore.AddClientScore();
          }
      

    }


    public void OnPointerClick(PointerEventData eventData) {
        if (GameObject.Find("GlobalObj").GetComponent<Global>().beeEvolve  && PhotonView.Find(thislilyViewId) != null) 
        {
            evolve.Play();
            thislily.GetComponent<BaseLily>().progress = 90;
            thislily.GetComponent<BaseLily>().state = 2;
            thislily.GetComponent<BaseLily>().beeApplied = true;
            GameObject.Find("GlobalObj").GetComponent<Global>().beeEvolve = false;
            GameObject child = PhotonNetwork.Instantiate(evolveParticle.name, Vector3.zero, quaternion.identity, 0);
            child.transform.SetParent(gameObject.transform);
            child.transform.localPosition = new Vector3(50, 50, 1);
            child.transform.localScale = new Vector3(5, 5, 1);
            child.transform.localRotation = Quaternion.identity;
            ReorderComponent();
        }
      if (!PhotonNetwork.inRoom /*|| PhotonNetwork.room.PlayerCount != 2*/)
      {
        return;
      }
      
      if (mGlobal.sunlightNum <= 0)
      {
        GameConstants.enablePlant = false;
      }
      if (mGlobal.mSelectedLilyType != LilyType.None && GameConstants.enablePlant) {
            click.Play();
            if (mGlobal.mSelectedLilyType == LilyType.Gold && (mGlobal.sunlightNum-8) >= 0)
            {
                mGlobal.sunlightNum -= 8;
                PlantNewLily(mGlobal.mSelectedLilyType);
            }
            else if (mGlobal.mSelectedLilyType == LilyType.Blue && (mGlobal.sunlightNum - 3) >= 0)
           {
                mGlobal.sunlightNum -= 3;
                PlantNewLily(mGlobal.mSelectedLilyType);
            }
            else if (mGlobal.mSelectedLilyType == LilyType.Pink && (mGlobal.sunlightNum - 5) >= 0)
            {
                mGlobal.sunlightNum -= 5;
                PlantNewLily(mGlobal.mSelectedLilyType);
            }
            else if (mGlobal.mSelectedLilyType == LilyType.White && (mGlobal.sunlightNum - 2) >= 0)
            {
                mGlobal.sunlightNum -= 2;
                PlantNewLily(mGlobal.mSelectedLilyType);
            }

            GameObject.Find("sunlightText").GetComponent<Text>().text = "SunLight: " + mGlobal.sunlightNum;
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

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
      if (stream.isWriting)
      {
        stream.SendNext(parentID);
        stream.SendNext(x);
        stream.SendNext(y);
        stream.SendNext(thislilyViewId);
      }
      else
      {
        parentID = (int)stream.ReceiveNext();
        x = (int)stream.ReceiveNext();
        y = (int)stream.ReceiveNext();
        thislilyViewId = (int)stream.ReceiveNext();
      }
    }

}
