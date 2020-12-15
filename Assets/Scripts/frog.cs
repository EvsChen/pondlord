using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class frog : Photon.MonoBehaviour
{
    public int parentID = -1;

    private bool init = false;
    private float timer = 0.0f;
    private int pause;
    private Image imageScale;
    public AudioSource audioData;
    // Start is called before the first frame update
    void Start()
    {
        //  pause = PhotonView.Find(parentID).GetComponent<BaseLily>().progress;
        // Debug.Log(pause);
        imageScale = this.GetComponentInChildren<Image>();
        audioData = GetComponent<AudioSource>();
        audioData.Play(0);
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(pause + "123");
        // PhotonView.Find(parentID).GetComponent<BaseLily>().state = 3;
        imageScale.transform.localScale = new Vector3((Mathf.Sin(Time.time * 5) * 1 + 5f) / 7f+0.3f, (Mathf.Sin(Time.time * 5) * 1 + 5f) / 7f+0.3f, 1);
       
        if (!init && parentID != -1)
        {
            SyncFrog();
            init = true;
        }
        timer += Time.deltaTime;
        if(timer > 3.0f)
        {
            Destroy(this.gameObject);
            this.gameObject.transform.parent.GetComponentInChildren<BaseLily>().generateSun = true;
            this.gameObject.transform.parent.GetComponentInChildren<BaseLily>().state = 2;
            
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
