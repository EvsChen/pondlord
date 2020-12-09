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
        int t = Random.Range(0, 4);
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
        
        mCollider = GetComponent<Collider2D>();
        transform.localPosition = new Vector3(
          transform.localPosition.x + direction.x * 51,
          transform.localPosition.y + direction.y * 51,
          0
        );
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
        transform.localPosition += movement;
        float bx = transform.position.x,
              by = transform.position.y;
        if (bx < 0 || bx > Screen.width || by < 0 || by > Screen.height) {
          if (photonView.isMine) {
            PhotonNetwork.Destroy(gameObject);
          }
        }
    }

    private void SyncBullet()
    {
        gameObject.transform.SetParent(PhotonView.Find(parentID).transform);
        // Translate the bullet a little more than the current cell bondary (50)
        // transform.localPosition = new Vector3(
        //   transform.localPosition.x + direction.x * 51,
        //   transform.localPosition.y + direction.y * 51,
        //   0
        // );
        gameObject.transform.localRotation = Quaternion.identity;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
      if (collision.gameObject.CompareTag(GameConstants.Tags.cell)) {
        Debug.Log("Bullet hit by " + collision.gameObject.name);
        Physics2D.IgnoreCollision(mCollider, collision.collider);
        return;
      } 
      GameObject collideObj = collision.gameObject;
      PhotonView pv = collideObj.GetComponent<PhotonView>();
      if (!pv) {
        Debug.Log("Bullet hit by " + collision.gameObject.name);
        return;
      }
      bool isSamePlayer = pv.ownerId == photonView.ownerId;
      if (collision.gameObject.CompareTag(GameConstants.Tags.lily)) {
        Debug.Log("Bullet hit by lily" + collision.gameObject.name);
        BaseLily bl = collision.gameObject.GetComponent<BaseLily>();
        if (isSamePlayer) {
          Physics2D.IgnoreCollision(mCollider, collision.collider);
          return;
        }
      } else if (collision.gameObject.CompareTag(GameConstants.Tags.protector)) {
        Debug.Log("Bullet hit by protector" + collision.gameObject.name);
        protector p = collision.gameObject.GetComponent<protector>();
        if (isSamePlayer) {
          Physics2D.IgnoreCollision(mCollider, collision.collider);
          return;
        } 
      } 
      // if (!photonView.isMine) {
      //   photonView.RequestOwnership();
      // }
      if (photonView.isMine) {
        PhotonNetwork.Destroy(gameObject);
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
