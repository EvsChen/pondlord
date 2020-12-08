using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class bee : MonoBehaviour, IPointerClickHandler
{

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
        Vector3 movement = direction *100* Time.deltaTime;
        transform.position += movement;

    }

    public void OnPointerClick(PointerEventData eventData)
    {
        GameObject.Find("GlobalObj").GetComponent<Global>().beeEvolve = true;
        PhotonNetwork.Destroy(gameObject);
    }
}
