using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class sunlight : MonoBehaviour, IPointerClickHandler
{
    public int parentID = -1;
    GameObject mSunlightText;
    private float timer;
    public AudioSource sunGen;
    public AudioSource sunCollect;

    // Start is called before the first frame update
    void Start()
    {
        mSunlightText = GameObject.Find("sunlightText");
        sunGen.Play();
    }

    // Update is called once per frame
    void Update()
    {
        // if (!init && parentID != -1)
        // {
        //     SyncSunlight();
        //     init = true;
        // }
        transform.Rotate(0, 0, 0.5f, Space.Self);
        timer += Time.deltaTime;
        if (timer > 5.0f) { 
            Destroy(this.gameObject);
        }


    }
    
    // public void SyncSunlight()
    // {
    //     gameObject.transform.SetParent(PhotonView.Find(parentID).transform);
    //     gameObject.transform.localPosition = new Vector3(0, 0, 0);
    //     gameObject.transform.localScale = new Vector3(1, 1, 1);
    // }

    public void OnPointerClick(PointerEventData eventData)
    {
        Global global = GameObject.Find("GlobalObj").GetComponent<Global>();
        global.sunlightNum++; //GameConstants.sunlight++;
        sunCollect.Play(0);
      
      if (mSunlightText) {
        mSunlightText.GetComponent<Text>().text = "SunLight: " + global.sunlightNum;
      }
      if (global.sunlightNum > 0)
      {
          GameConstants.enablePlant = true;
      }
      Destroy(gameObject);
    }

    // void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    // {
    //     if (stream.isWriting)
    //     {
    //         stream.SendNext(parentID);
    //     }
    //     else
    //     {
    //         parentID = (int)stream.ReceiveNext();
    //     }
    // }
}
