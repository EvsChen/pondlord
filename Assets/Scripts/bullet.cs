using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bullet : Photon.MonoBehaviour
{
    public float speed = 3000;
    public Vector3 direction;
    public int mPlayerId;
    Collider2D mCollider;
    
    public int parentID = -1;

    public bool init = false;
    // Start is called before the first frame update
    void Start()
    {
        float t = Random.Range(0, 4);
        switch (t)
        {
            case 0:
                direction = new Vector3(1, 0, 0);
                break;
            case 1:
                direction = new Vector3(0, 1, 0);
                break;
            case 2:
                direction = new Vector3(-1, 0, 0);
                break;
            case 3:
                direction = new Vector3(0, -1, 0);
                break;
        }
        transform.localPosition = new Vector3(transform.localPosition.x + direction.x * 40, transform.localPosition.y + direction.y * 40, transform.localPosition.z - 10);
        mCollider = GetComponent<Collider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!init && parentID != -1)
        {
            SyncBullet();
            init = true;
        }
        Vector3 movement = direction * speed * Time.deltaTime;
        transform.localPosition +=  movement;
    }

    private void SyncBullet()
    {
        gameObject.transform.SetParent(PhotonView.Find(parentID).transform);
        gameObject.transform.localPosition = new Vector3(0, 0, 0);
        gameObject.transform.localRotation = Quaternion.identity;
    }
    void OnCollisionEnter2D(Collision2D collision)
    {
      if (collision.gameObject.CompareTag(GameConstants.Tags.cell)) {
        Physics2D.IgnoreCollision(mCollider, collision.collider);
        return;
      } 
      if (collision.gameObject.CompareTag(GameConstants.Tags.lily)) {
        BaseLily bl = collision.gameObject.GetComponent<BaseLily>();
        if (bl.mPlayerId == mPlayerId) {
          Physics2D.IgnoreCollision(mCollider, collision.collider);
          return;
        }
      } else if (collision.gameObject.CompareTag(GameConstants.Tags.protector)) {
        protector p = collision.gameObject.GetComponent<protector>();
        if (p.mPlayerId == mPlayerId) {
          Physics2D.IgnoreCollision(mCollider, collision.collider);
          return;
        } 
      }
      PhotonNetwork.Destroy(gameObject);
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
