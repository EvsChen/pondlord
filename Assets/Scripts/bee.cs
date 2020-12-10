using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class bee : Photon.MonoBehaviour, IPointerClickHandler
{
    public int parentID = -1;

    private bool init = false;
    public int mPlayerId;
    public Vector3 direction;
    // Start is called before the first frame update
    void Start()
    {
        float a = Random.Range(-1, 1);
        float b = Random.Range(-1, 1);
        direction = new Vector3(a, b, 0.0f);
    }

    // Update is called once per frame
    void Update()
    {
        if (!init && parentID != -1)
        {
            SyncBee();
            init = true;
        }
        Vector3 movement = direction *100* Time.deltaTime;
        transform.position += movement;

    }

    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log("bee");
        GameObject.Find("GlobalObj").GetComponent<Global>().beeEvolve = true;
        PhotonNetwork.Destroy(this.gameObject);
        Destroy(this.gameObject);
    }
    
    public void SyncBee()
    {
        gameObject.transform.SetParent(PhotonView.Find(parentID).transform);
        //gameObject.transform.localPosition = new Vector3(0, 0, 0);
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
