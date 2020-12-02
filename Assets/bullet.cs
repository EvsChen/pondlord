using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bullet : MonoBehaviour
{
    public float speed = 3000;
    public Vector3 direction;
    public int mPlayerId;
    Collider2D mCollider;
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
        Vector3 movement = direction * speed * Time.deltaTime;
        transform.localPosition +=  movement;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
      if (collision.gameObject.CompareTag("cell")) {
        Physics2D.IgnoreCollision(mCollider, collision.collider);
        return;
      } 
      if (collision.gameObject.CompareTag("lily")) {
        BaseLily bl = collision.gameObject.GetComponent<BaseLily>();
        if (bl.mPlayerId == mPlayerId) {
          Physics2D.IgnoreCollision(mCollider, collision.collider);
          return;
        }
      }
      Destroy(gameObject);
    }
}
