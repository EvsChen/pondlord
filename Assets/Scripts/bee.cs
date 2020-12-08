using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bee : MonoBehaviour
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
}
