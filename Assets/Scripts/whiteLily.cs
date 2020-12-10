using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class whiteLily : BaseLily
{
    public GameObject mProtector;

    // Start is called before the first frame update
    new void Start()
    {

      base.Start();
        hp = 5;
    }

    // Update is called once per frame
    new void Update()
    {
      base.Update();  
      if (functional && photonView.isMine)
      {
          GameObject c = PhotonNetwork.Instantiate(mProtector.name, new Vector3(transform.position.x, transform.position.y, transform.position.z), Quaternion.identity, 0);

          //GameObject c = Instantiate(mProtector, new Vector3(transform.position.x, transform.position.y, transform.position.z), Quaternion.identity);
          protector p = c.GetComponent<protector>();
          p.mPlayerId = mPlayerId;
          p.parentID = viewid;
          c.transform.parent = transform;
          functional = false;
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
