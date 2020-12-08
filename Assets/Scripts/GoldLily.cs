using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoldLily : BaseLily
{
    public Board mBoard;
    public GameObject bee;
    // Start is called before the first frame update
    new void Start()
    {   
      base.Start();
    }

    void AddFrog()
    {
        bool added = false;
        while (!added)
        {
            int rX = Random.Range(0, GameConstants.mBoardWidth);
            int rY = Random.Range(0, GameConstants.mBoardHeight);
            Cell cell = mBoard.mAllCells[rX, rY];
            if (cell.HasFish())
            {
                continue;
            }
            added = true;
            cell.AddFish();
        }
    }


    // Update is called once per frame
    new void Update()
    {
        base.Update();
        if (functional)
        {
            float t = Random.Range(0, 500);
            if (t < 1)
            {
                GameObject child = Instantiate(bee, new Vector3(transform.position.x, transform.position.y, transform.position.z), Quaternion.identity);
                child.transform.parent = gameObject.transform;
                bee b = child.GetComponent<bee>();
                b.mPlayerId = mPlayerId;
            }
        }
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
