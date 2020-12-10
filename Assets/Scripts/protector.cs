using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class protector : Photon.MonoBehaviour
{
    public int mPlayerId;
    public int hp = 5;
    public int parentID = -1;
    bool init = false;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (!init && parentID != -1)
        {
            SyncProtector();
            init = true;
        }
    }

    void SyncProtector()
    {
        gameObject.transform.SetParent(PhotonView.Find(parentID).transform);
        gameObject.transform.localPosition = new Vector3(0, 0, 0);
        gameObject.transform.localRotation = Quaternion.identity;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        GameObject collideObj = collision.gameObject;
        PhotonView pv = collideObj.GetComponent<PhotonView>();
        if (!pv) {
          return;
        }
        bool isSamePlayer = photonView.ownerId == pv.ownerId;
        
        if (collideObj.CompareTag(GameConstants.Tags.bullet))
        {
            bullet b = collision.gameObject.GetComponent<bullet>();
            if (isSamePlayer) {
              return;
            }
            hp--;
            if (hp <= 0)
            {
                Destroy(this.gameObject);
                if (photonView.isMine) {
                PhotonNetwork.Destroy(gameObject);
                 
              }
            }
        }

    }
    
    void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
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
