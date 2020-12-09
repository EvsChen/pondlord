using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class sunlight : MonoBehaviour, IPointerClickHandler
{
    public int parentID = -1;
    private bool init = false;
    GameObject mSunlightText;
    private float timer;
    
    // Start is called before the first frame update
    void Start()
    {
        mSunlightText = GameObject.Find("sunlightText");
    }

    // Update is called once per frame
    void Update()
    {
        // if (!init && parentID != -1)
        // {
        //     SyncSunlight();
        //     init = true;
        // }

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

    public void OnPointerClick(PointerEventData eventData) {
      GameConstants.sunlight++;
      if (mSunlightText) {
        mSunlightText.GetComponent<Text>().text = "SunLight: " + GameConstants.sunlight;
      }
      if (GameConstants.sunlight > 0)
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
