using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bullet : MonoBehaviour
{
    public float speed = 10099;
    public Vector3 direction;
    // Start is called before the first frame update
    void Start()
    {
        float t = Random.Range(0, 4);
        switch (t)
        {
            case 1:
                direction = new Vector3(1, 0, 0);
                break;
            case 2:
                direction = new Vector3(0, 1, 0);
                break;
            case 3:
                direction = new Vector3(-1, 0, 0);
                break;
            case 4:
                direction = new Vector3(0, -1, 0);
                break;
        }
        transform.position = new Vector3(transform.position.x + direction.x * 60, transform.position.y + direction.y * 60, transform.position.z - 10);

    }

    // Update is called once per frame
    void Update()
    {
        
        transform.position +=  direction * speed * Time.deltaTime;
    }
}
