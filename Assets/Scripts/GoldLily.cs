using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoldLily : BaseLily
{
    public Board mBoard;
    public GameObject bee;
    public GameObject frog;
    // Start is called before the first frame update
    new void Start()
    {   
      base.Start();
        hp = 5;
        GameObject boardObj = GameObject.Find("PF_board");
        mBoard = boardObj.GetComponent<Board>();
    }

    void AddFrog()
    {
        Cell cell = this.transform.parent.GetComponent<Cell>();
        int newX = cell.x;
        int newY = cell.y;
        int rX = Random.Range(((newX - 2)< 0 ? 0 : newX - 2), ((newX + 3) > GameConstants.mBoardWidth ? GameConstants.mBoardWidth : (newX + 3)));
        int rY = Random.Range(((newY - 2) < 0 ? 0 : newY - 2), ((newY + 3) > GameConstants.mBoardHeight ? GameConstants.mBoardHeight : (newY + 3)));
        Cell spawnCell = mBoard.mAllCells[rX, rY];
       // Debug.Log(spawnCell.GetComponentInChildren<BaseLily>());
        if (!spawnCell.HasFrog() && spawnCell.GetComponentInChildren<BaseLily>() != null && spawnCell.GetComponentInChildren<BaseLily>().generateSun)
        {
            spawnCell.GetComponentInChildren<BaseLily>().state = 3;
            spawnCell.GetComponentInChildren<BaseLily>().generateSun = false;
            spawnCell.AddFrog();
        }
    }


    // Update is called once per frame
    new void Update()
    {
        base.Update();
        if (functional)
        {
            float t = Random.Range(0, 3000);
            if (t < 1)
            {
                GameObject child = PhotonNetwork.Instantiate(bee.name, new Vector3(transform.position.x, transform.position.y, transform.position.z), Quaternion.identity, 0);

                //GameObject child = Instantiate(bee, new Vector3(transform.position.x, transform.position.y, transform.position.z), Quaternion.identity);
                
                bee b = child.GetComponent<bee>();
                b.mPlayerId = mPlayerId;
                b.parentID = viewid;
            }

            float t2 = Random.Range(0, 30);
            if (t2 < 1)
            {
                AddFrog();
            }
        }
    }
}
