using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Board : MonoBehaviour
{
    public GameObject mCellPrefab;
    public Cell[,] mAllCells;
    public int mWidth, mHeight;

    // Start is called before the first frame update
    void Start()
    {
        mWidth = GameConstants.mBoardWidth;
        mHeight = GameConstants.mBoardHeight;
        mAllCells = new Cell[mWidth, mHeight];
        
        for (int y = 0; y < mHeight; y++) {
          for (int x = 0; x < mWidth; x++) {
            GameObject newCell = Instantiate(mCellPrefab, transform);
                newCell.GetComponent<Cell>().x = x;
                newCell.GetComponent<Cell>().y = y;
                RectTransform rectTransform = newCell.GetComponent<RectTransform>();
            rectTransform.anchoredPosition = new Vector2((x * 100) + 50, (y * 100) + 50);
            mAllCells[x, y] = newCell.GetComponent<Cell>();
            mAllCells[x, y].mBoard = this;
            if ((x + y) % 2 == 0) {
              mAllCells[x, y].mNormalColor = new Color(0.9f, 0.85f, 0.72f, 1.0f);
            } else {
              mAllCells[x, y].mNormalColor = new Color(1.0f, 1.0f, 1.0f, 1.0f);
            }
          }
        }

        for (int x = 0; x < 8; x += 2)
        {
            for (int y = 0; y < 8; y++)
            {
                int offset = (y % 2 != 0) ? 0 : 1;
                int finalX = x + offset;
                
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
