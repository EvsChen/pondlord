using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class whiteLily : BaseLily
{
    public GameObject protector;

    // Start is called before the first frame update
    new void Start()
    {
      base.Start();
    }

    // Update is called once per frame
    new void Update()
    {
      base.Update();  
      if (functional)
      {
          Instantiate(protector, new Vector3(transform.position.x, transform.position.y, transform.position.z), Quaternion.identity);
          functional = false;
      }
    }
}
