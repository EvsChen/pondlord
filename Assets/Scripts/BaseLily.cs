﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class BaseLily : Photon.MonoBehaviour, IPunObservable
{
    GameObject mPbComponent;
    ProgressBar Pb;
    public int progress = 0, hp = 2;
    public int state = 1; //seed = 1; leaf = 2; flower = 3;
    public Sprite flower, leaf;
    public Image mImage;
    public GameObject sunlight;
    public bool generateSun = false;
    protected bool functional = false;
    public int mPlayerId; // Used to distinguish player
    public int parentID = -1;
    bool synced = false;
    Collider2D mCollider;
    public bool beeApplied = false;
    float updateTimer = 0.0f;
    public AudioSource bloom;

    float updateTimer2 = 0.0f;
    private float x;
    private Image imageScale;

    public void ResetLily() {
      progress = 0;
      state = 1;
      hp = 2; // Should not be reset here
    }

    public int viewid = -1;
    public void Start()
        
    {
        imageScale = this.GetComponentInChildren<Image>();
        //Debug.Log(photonView.viewID);
        beeApplied = false;
        viewid = photonView.viewID;
        ResetLily();
        mPlayerId = 0;
        mCollider = GetComponent<Collider2D>();
        for (int i = 0; i < transform.childCount; i++) {
          GameObject c = transform.GetChild(i).gameObject;
          if (c.name == "Image") {
            mImage = c.GetComponent<Image>();
          } else if (c.name == "ProgressBar") {
            mPbComponent = c;
            Pb = c.GetComponent<ProgressBar>();
          }
        }
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
      GameObject collideObj = collision.gameObject;
      PhotonView pv = collideObj.GetComponent<PhotonView>();
      if (!pv) {
        return;
      }
      bool isSamePlayer = photonView.ownerId == pv.ownerId;
      if (collideObj.CompareTag(GameConstants.Tags.bullet))
      {
        Debug.Log("Lily hit by " + collision.gameObject.name);
        bullet b = collideObj.GetComponent<bullet>();
        if (isSamePlayer) {
          //Debug.Log("Collide with same mPlayerId" + this.mPlayerId);
          Physics2D.IgnoreCollision(mCollider, collision.collider);
          return;
        }
        Debug.Log("Bullet collision and hp is " + hp);
        photonView.RPC("MinusHP", PhotonTargets.All);
      }
    }

    [PunRPC]
    public void MinusHP() {
      hp--;
    }


    // Update is called once per frame
    public void Update()
    {
        if(state != 3)
        {
            imageScale.transform.localScale = new Vector3((Mathf.Sin(Time.time * 1) * 1 + 5.5f) / 5.5f, (Mathf.Sin(Time.time * 1) * 1 + 5f) / 5.5f, 1);
        }
        
        if (beeApplied) {
            updateTimer2++;
            imageScale.transform.localScale = new Vector3((Mathf.Sin(Time.time * 10) * 1 + 6f) / 3, (Mathf.Sin(Time.time * 10) * 1 + 6f) / 3, 1);
            if(updateTimer2 > 120)
            {
                beeApplied = false;
                updateTimer2 = 0;
                if(imageScale.transform.localScale.x > 1.5)
                {
                    imageScale.transform.localScale = new Vector3(1, 1, 1);
                }
            }
                
        }
        

        if (!synced) {
          SyncLily();
        }

        if (photonView.isMine && hp <= 0) {
            //update score
            PlayerScore playerScore = GameObject.Find("CanvasGlobal").GetComponent<PlayerScore>();
            if (photonView.owner.IsMasterClient)
            {
                playerScore.ReduceMasterScore();
            }
            else
            {
                playerScore.ReduceClientScore();
            }
            
          PhotonNetwork.Destroy(gameObject);
        }
        
        if(this.progress < 100f)
        {
          updateTimer += Time.deltaTime;
          if (updateTimer > 1.0f) {
             if (state == 1 || state == 2)
              {
                  this.progress += 10;
              }
              Pb.BarValue = progress;
              if (this.progress == 50)
              {
                  state = 2;
                  mImage.sprite = this.leaf;
                  generateSun = true;
              }
              if (this.progress >= 100)
              {
                  state = 3;
                  mImage.sprite = this.flower;
                  generateSun = false;
                  functional = true;
                    bloom.Play();
                  Destroy(mPbComponent);
              }
              updateTimer = 0.0f;
          }
        }
        gameObject.GetComponentInChildren<Text>().text = hp + " hp";
        if (generateSun)
        {
            float t = Random.Range(0, 1000);
            if (t < 1)
            {
                //GameObject child = Instantiate(sunlight, new Vector3(transform.position.x, transform.position.y, transform.position.z), Quaternion.identity);

                if (photonView.isMine)
                {
                    GameObject child = Instantiate(sunlight, new Vector3(transform.position.x, transform.position.y, transform.position.z), Quaternion.identity);
                    //child.GetComponent<sunlight>().parentID = viewid;
                    child.transform.parent = this.gameObject.transform;
                    child.transform.localPosition = new Vector3(0, 0, 0);
                    child.transform.localScale = new Vector3(1, 1, 1);
                }
            }
        }
      
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log(this.gameObject.name);
        Debug.Log("baselily");
        bool beeEvolve = GameObject.Find("Global").GetComponent<Global>().beeEvolve;
        if (beeEvolve)
        {
            this.progress = 100;
            Cursor.SetCursor(default, Vector2.zero, CursorMode.Auto);
            beeEvolve = false;
        }
    }

    void SyncLily()
    {
        PhotonView parent = PhotonView.Find(parentID);
        if (!parent) {
          return;
        }
        synced = true;
        gameObject.transform.SetParent(parent.transform);
        gameObject.transform.localPosition = new Vector3(50, 50, 0);
        gameObject.transform.localScale = new Vector3(1, 1, 1);
        Cell cell = parent.GetComponent<Cell>();
        PhotonPlayer owner = photonView.owner;
        cell.mBgImg.color = owner.IsMasterClient
          ? cell.mMasterColor
          : cell.mClientColor;
    }


    private void OnDestroy() {
       GameObject parent = transform.parent.gameObject;
       if (!parent) {
         return;
       }
       Cell cell = parent.GetComponent<Cell>();
       PhotonPlayer owner = photonView.owner;
       cell.mBgImg.color = cell.mNormalColor;
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.isWriting)
        {
            stream.SendNext(parentID);
        }
        else
        {
            parentID = (int)stream.ReceiveNext();
        }
    }
}