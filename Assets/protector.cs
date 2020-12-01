using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class protector : MonoBehaviour
{

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
        if (collision.gameObject.tag == "bullet")
        {

            Destroy(collision.gameObject);
            hp--;
            if (hp <= 0)
            {

                Destroy(this.gameObject);

            }
        }

    }
}
