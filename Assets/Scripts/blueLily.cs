using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class blueLily : BaseLily
{
    public GameObject bullet;

    // Start is called before the first frame update
    new void Start()
    {
        base.Start();
        hp = 3;
    }

    // Update is called once per frame
    new void Update()
    {
        base.Update();
        if (functional){
            float t = Random.Range(0, 100);
            if (t < 1 && photonView.isMine)
            {
                GameObject child = PhotonNetwork.Instantiate(bullet.name,
                    new Vector3(transform.position.x, transform.position.y, transform.position.z), Quaternion.identity, 0);
                //GameObject child = Instantiate(bullet, new Vector3(transform.position.x, transform.position.y, transform.position.z), Quaternion.identity);
                //child.transform.parent = gameObject.transform;
                bullet b = child.GetComponent<bullet>();
                b.mPlayerId = mPlayerId;
                b.parentID = viewid;
            }
        }
    }
}
