using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class BaseLily : Photon.PunBehaviour
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
    float updateTimer = 0.0f;

    public void ResetLily() {
      progress = 0;
      state = 1;
      hp = 2; // Should not be reset here
    }

    public int viewid = -1;
    public void Start()
    {
        //Debug.Log(photonView.viewID);
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
        hp--;
        if (hp <= 0) {
          if (photonView.isMine) {
            PhotonNetwork.Destroy(gameObject);
          }
        }
      }
    }

    // Update is called once per frame
    public void Update()
    {
        if (!synced) {
          SyncLily();
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
              if (this.progress == 100)
              {
                  state = 3;
                  mImage.sprite = this.flower;
                  generateSun = false;
                  functional = true;
                  Destroy(mPbComponent);
              }
              updateTimer = 0.0f;
          }
        }
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
}