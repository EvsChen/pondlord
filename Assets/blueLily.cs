using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class blueLily : MonoBehaviour
{
    public ProgressBar Pb;
    public int progress = 0;
    public int state = 1; //seed = 1; leaf = 2; flower = 3;
    public Sprite flower;
    public Sprite leaf;
    public Image mImage;
    public int hp = 2;
    public GameObject bullet;
    public GameObject sunlight;
    bool functional = false;
    bool generateSun = false;

    // Start is called before the first frame update
    void Start()
    {
        functional = false;
        generateSun = false;
        progress = 0;
        state = 1;
        hp = 2;
        Pb = GetComponentInChildren<ProgressBar>();
        GameObject c = transform.GetChild(0).gameObject;
        mImage = c.GetComponent<Image>();
        StartCoroutine(Loop());
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
        
            if (this.progress == 50f)
            {
                state = 2;
                mImage.sprite = this.leaf;
                generateSun = true;

            }

            if (this.progress == 100f)
            {
                state = 3;
                mImage.sprite = this.flower;
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
                GameObject child = Instantiate(sunlight, new Vector3(transform.position.x, transform.position.y, transform.position.z), Quaternion.identity);
                child.transform.parent = this.gameObject.transform;
                child.transform.localPosition = new Vector3(0, 0, 0);
            }
        }
        if (functional){
            float t = Random.Range(0, 500);
            if (t < 1)
            {
                GameObject child = Instantiate(bullet, new Vector3(transform.position.x, transform.position.y, transform.position.z), Quaternion.identity);
                child.transform.parent = gameObject.transform;
               
            }
        }
        
       
    }

  

    
}
