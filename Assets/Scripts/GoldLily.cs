﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoldLily : BaseLily
{
    // Start is called before the first frame update
    new void Start()
    {   
      base.Start();
    }

    // Update is called once per frame
    new void Update()
    {
      base.Update();
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