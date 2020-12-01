using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pinkLily : MonoBehaviour
{
    public ProgressBar Pb;
    public int progress = 0;
    public int state = 1; //seed = 1; leaf = 2; flower = 3;
    public Sprite flower;
    public Sprite leaf;
    public Sprite Seed;
    public int hp = 2;
    public GameObject seed;
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
        this.GetComponent<SpriteRenderer>().sprite = this.Seed;
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

            if (this.progress < 100f)
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
                this.GetComponent<SpriteRenderer>().sprite = this.leaf;
                generateSun = true;

            }

            if (this.progress == 100f)
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
                    GameObject child = Instantiate(sunlight, new Vector3(transform.position.x, transform.position.y, transform.position.z), Quaternion.identity);
                child.transform.parent = this.gameObject.transform;
                child.transform.localPosition = new Vector3(0,0,0);
            }
            }
            if (functional)
            {

                Board b = GameObject.Find("PF_board").GetComponent<Board>();
                Cell[,] mAllCells = b.mAllCells;
                Cell cell = this.transform.parent.GetComponent<Cell>();
                int newX = cell.x;
                int newY = cell.y;
                int mWidth = b.mWidth;
                int mHeight = b.mHeight;
                float t = Random.Range(0, 5);
                GameObject child;

            
                switch (t)
                {
                    case 1:
                        if (newX + 1 < mWidth && mAllCells[newX + 1, newY].transform.childCount == 1)
                        {
                            Cell c = mAllCells[newX + 1, newY];
                            child = Instantiate(seed, new Vector3(c.transform.position.x, c.transform.position.y, c.transform.position.z), Quaternion.identity);
                        child.transform.localScale = new Vector3(1,1,1);
                        child.transform.parent = c.gameObject.transform;
                        Debug.Log(c.x + " "+c.y);
                        functional = false;

                        }

                        break;
                    case 2:
                        if (newX - 1 > 0 && mAllCells[newX - 1, newY].transform.childCount == 1)
                        {
                            Cell c = mAllCells[newX - 1, newY];
                            child = Instantiate(seed, new Vector3(c.transform.position.x, c.transform.position.y, c.transform.position.z), Quaternion.identity);
                        child.transform.localScale = new Vector3(1, 1, 1);
                        child.transform.parent = c.gameObject.transform;
                        Debug.Log(c.x + " " + c.y);
                        functional = false;
                        }

                        break;
                    case 3:
                        if (newY + 1 < mHeight && mAllCells[newX, newY + 1].transform.childCount == 1)
                        {
                            Cell c = mAllCells[newX, newY + 1];
                            child = Instantiate(seed, new Vector3(c.transform.position.x, c.transform.position.y, c.transform.position.z), Quaternion.identity);
                        child.transform.localScale = new Vector3(1, 1, 1);
                        child.transform.parent = c.gameObject.transform;
                        Debug.Log(c.x + " " + c.y);
                        functional = false;
                        }
                        break;
                    case 4:
                        if (newY - 1 > 0 && mAllCells[newX, newY - 1].transform.childCount == 1)
                        {
                            Cell c = mAllCells[newX, newY - 1];
                            child = Instantiate(seed, new Vector3(c.transform.position.x, c.transform.position.y, c.transform.position.z), Quaternion.identity);
                        child.transform.localScale = new Vector3(1, 1, 1);
                        child.transform.parent = c.gameObject.transform;
                        Debug.Log(c.x + " " + c.y);
                        functional = false;
                        }
                        break;
                }


            }


        }
    }




