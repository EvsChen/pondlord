using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class protector : MonoBehaviour
{
    public int mPlayerId;
    public int hp = 5;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag(GameConstants.Tags.bullet))
        {
            bullet b = collision.gameObject.GetComponent<bullet>();
            if (b.mPlayerId == mPlayerId) {
              return;
            }
            hp--;
            if (hp <= 0)
            {
                Destroy(this.gameObject);
            }
        }

    }
}
