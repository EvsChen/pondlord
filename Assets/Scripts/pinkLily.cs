    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.UI;

    public class pinkLily : BaseLily
    {
        public Sprite mSeedSprite;
        public GameObject mPinkSeedPrefab;
        public Board mBoard;
        
        // Start is called before the first frame update
        new void Start()
        {
            base.Start();
            mBoard = GameObject.Find("PF_board").GetComponent<Board>();
        }

        // Update is called once per frame
        new void Update()
        {
            base.Update();
            if (functional)
            {
                Cell[,] mAllCells = mBoard.mAllCells;
                if (!this.transform.parent) {
                  return;
                }
                Cell cell = this.transform.parent.GetComponent<Cell>();
                int newX = cell.x;
                int newY = cell.y;
                int mWidth = mBoard.mWidth;
                int mHeight = mBoard.mHeight;
                int t = Random.Range(0, 4);
                // Here we cannot instantiate the pink lily within the same pink lily.
                // So move the instantiation method to cell instead.
                switch (t)
                {
                 
                    case 0:
                    if (newX + 1 < mWidth && mAllCells[newX + 1, newY].transform.childCount == 1)
                        {
                            Cell c = mAllCells[newX + 1, newY];
                            c.PlantNewLily(LilyType.Pink);
                            functional = false;
                        }
                        break;
                    case 1:
                    if (newX - 1 >= 0 && mAllCells[newX - 1, newY].transform.childCount == 1)
                        {
                            Cell c = mAllCells[newX - 1, newY];
                            c.PlantNewLily(LilyType.Pink);
                            functional = false;
                        }
                        break;
                    case 2:
                    if (newY + 1 < mHeight && mAllCells[newX, newY + 1].transform.childCount == 1)
                        {
                            Cell c = mAllCells[newX, newY + 1];
                            c.PlantNewLily(LilyType.Pink);
                            functional = false;
                        }
                        break;
                    case 3:
                    if (newY - 1 >= 0 && mAllCells[newX, newY - 1].transform.childCount == 1)
                        {
                            Cell c = mAllCells[newX, newY - 1];
                            c.PlantNewLily(LilyType.Pink);
                            functional = false;
                        }
                        break;
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
    




