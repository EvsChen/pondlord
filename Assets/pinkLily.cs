﻿    using System.Collections;
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
                Cell cell = this.transform.parent.GetComponent<Cell>();
                int newX = cell.x;
                int newY = cell.y;
                int mWidth = mBoard.mWidth;
                int mHeight = mBoard.mHeight;
                float t = Random.Range(0, 4);
                switch (t)
                {
                    case 0:
                        if (newX + 1 < mWidth && mAllCells[newX + 1, newY].transform.childCount == 1)
                        {
                            Cell c = mAllCells[newX + 1, newY];
                            c.PlantNewLily(LilyType.Pink);
                        }
                        break;
                    case 1:
                        if (newX - 1 > 0 && mAllCells[newX - 1, newY].transform.childCount == 1)
                        {
                            Cell c = mAllCells[newX - 1, newY];
                            c.PlantNewLily(LilyType.Pink);
                        }
                        break;
                    case 2:
                        if (newY + 1 < mHeight && mAllCells[newX, newY + 1].transform.childCount == 1)
                        {
                            Cell c = mAllCells[newX, newY + 1];
                            c.PlantNewLily(LilyType.Pink);
                        }
                        break;
                    case 3:
                        if (newY - 1 > 0 && mAllCells[newX, newY - 1].transform.childCount == 1)
                        {
                            Cell c = mAllCells[newX, newY - 1];
                            c.PlantNewLily(LilyType.Pink);
                        }
                        break;
                }
                functional = false;
                
            }


        }
    }




