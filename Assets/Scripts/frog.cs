using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class frog : MonoBehaviour
{
    public int parentID = -1;

    private bool init = false;
    private float timer = 0.0f;
    private int pause;
    // Start is called before the first frame update
    void Start()
    {
     //  pause = PhotonView.Find(parentID).GetComponent<BaseLily>().progress;
       // Debug.Log(pause);
        
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(pause + "123");
       // PhotonView.Find(parentID).GetComponent<BaseLily>().state = 3;
        if (!init && parentID != -1)
        {
            SyncFrog();
            init = true;
        }
        timer += Time.deltaTime;
        if(timer > 3.0f)
        {
            this.gameObject.transform.parent.GetComponentInChildren<BaseLily>().state = 2;
            Destroy(this.gameObject);
        }

    }

    public void SyncFrog()
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
