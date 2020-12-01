using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class blueLily : MonoBehaviour
{
    public ProgressBar Pb;
    public int progress = 0;
    public int state = 1; //seed = 1; leaf = 2; flower = 3;
    public Sprite flower;
    public Sprite leaf;
    public int hp = 2;
    public GameObject bullet;
    public GameObject sunlight;
    bool functional = false;
    bool generateSun = false;

    // Start is called before the first frame update
    void Start()
    {
        Pb = GetComponentInChildren<ProgressBar>();
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "bullet")
        {

            Destroy(collision.gameObject);
            hp--;
            if (hp <= 0)
            {

                Destroy(this.gameObject);

            }
        }

    }
    void Awake()
    {
        // Uncommenting this will cause framerate to drop to 10 frames per second.
        // This will mean that FixedUpdate is called more often than Update.
        //Application.targetFrameRate = 10;
        StartCoroutine(Loop());
    }

    IEnumerator Loop()
    {
        while (true)
        {

            
        if(this.progress < 100f)
        {
                if (state == 1)
                {
                    yield return new WaitForSeconds(1);
                    this.progress = progress + 10;
                }

                if (state == 2)
                {

                    yield return new WaitForSeconds(1);
                    this.progress = progress + 10;
                }
                
            }
        
            if (this.progress == 30f)
            {
                state = 2;
                this.GetComponent<SpriteRenderer>().sprite = this.leaf;
                generateSun = true;

            }

            if (this.progress == 60f)
            {
                state = 3;
                this.GetComponent<SpriteRenderer>().sprite = this.flower;
                generateSun = false;
                functional = true;
                yield break;
            }

        }

    }

    // Update is called once per frame
    void Update()
    {
        Pb.BarValue = progress;
        if (generateSun)
        {
            float t = Random.Range(0, 1000);
            if (t < 1)
            {
                GameObject child = Instantiate(sunlight, new Vector3(transform.position.x, transform.position.y, transform.position.z-100), Quaternion.identity);
                
            }
        }
        if (functional){
            float t = Random.Range(0, 500);
            if (t < 1)
            {
                GameObject child = Instantiate(bullet, new Vector3(transform.position.x, transform.position.y, transform.position.z+10), Quaternion.identity);
                child.transform.parent = gameObject.transform;
            }
        }
        
       
    }

  

    
}
