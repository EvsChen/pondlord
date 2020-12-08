using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BaseLily : Photon.MonoBehaviour
{
    GameObject mPbComponent;
    ProgressBar Pb;
    public int progress = 0, hp = 2;
    public int state = 1; //seed = 1; leaf = 2; flower = 3;
    public Sprite flower, leaf;
    public Image mImage;
    public GameObject sunlight;
    Collider2D mCollider;
    float updateTimer = 0.0f;
    protected bool generateSun = false;
    protected bool functional = false;
    public int mPlayerId; // Used to distinguish player
    public int parentID = -1;
    public void ResetLily() {
      progress = 0;
      state = 1;
      hp = 2; // Should not be reset here
    }

    public PhotonView photonView;
    public int viewid = -1;
    public void Start()
    {
        photonView = new PhotonView();
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
      if (collision.gameObject.CompareTag(GameConstants.Tags.bullet))
      {
        bullet b = collision.gameObject.GetComponent<bullet>();
        if (b.mPlayerId == this.mPlayerId) {
          //Debug.Log("Collide with same mPlayerId" + this.mPlayerId);
          Physics2D.IgnoreCollision(mCollider, collision.collider);
          return;
        }
        Debug.Log("Bullet collision and hp is " + hp);
        hp--;
        if (hp <= 0) {
          Destroy(gameObject);
        }
      }
    }

    // Update is called once per frame
    public void Update()
    {
        SyncLily();
        
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
                GameObject child = Instantiate(sunlight, new Vector3(transform.position.x, transform.position.y, transform.position.z), Quaternion.identity);

                //GameObject child = PhotonNetwork.Instantiate(sunlight.name, new Vector3(transform.position.x, transform.position.y, transform.position.z), Quaternion.identity, 0);
                //child.GetComponent<sunlight>().parentID = viewid;
                child.transform.parent = this.gameObject.transform;
                child.transform.localPosition = new Vector3(0, 0, 0);
            }
        }
    }

    public void SyncLily()
    {
        gameObject.transform.SetParent(PhotonView.Find(parentID).transform);
        gameObject.transform.localPosition = new Vector3(50, 50, 0);
        gameObject.transform.localScale = new Vector3(1, 1, 1);
    }
}
