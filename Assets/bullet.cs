using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bullet : MonoBehaviour
{
    public float speed = 3000;
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
        transform.localPosition = new Vector3(transform.localPosition.x + direction.x * 40, transform.localPosition.y + direction.y * 40, transform.localPosition.z - 10);

    }

    // Update is called once per frame
    void Update()
    {
        
        transform.localPosition +=  direction * speed * Time.deltaTime;
    }
}
