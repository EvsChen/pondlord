using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sunlight : MonoBehaviour
{
    public int parentID = -1;

    private bool init = false;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!init && parentID != -1)
        {
            SyncSunlight();
            init = true;
        }
    }
    
    public void SyncSunlight()
    {
        gameObject.transform.SetParent(PhotonView.Find(parentID).transform);
        gameObject.transform.localPosition = new Vector3(0, 0, 0);
        gameObject.transform.localScale = new Vector3(1, 1, 1);
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
