using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LilyButton : MonoBehaviour
{
    // Start is called before the first frame update
    public LilyType mLilyType = LilyType.None;
    public Button mButton;
    public Image mBtnImg;
    Global mGlobal;
    private bool isSelected = false; // Custom state for button press
    private List<LilyButton> mAllBtns = new List<LilyButton>();
    void Start()
    {
        mButton = GetComponent<Button>();
        mBtnImg = mButton.GetComponent<Image>();
        mButton.onClick.AddListener(OnButtonClick);
        GameObject globalObj = GameObject.Find("GlobalObj");
        mGlobal = globalObj.GetComponent<Global>();
        GameObject[] allBtns = GameObject.FindGameObjectsWithTag("LilyButton");
        foreach (GameObject obj in allBtns) {
          mAllBtns.Add(obj.GetComponent<LilyButton>());
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void BtnSelect() {
      ColorBlock cBlock = mButton.colors;
      mBtnImg.color = cBlock.pressedColor;
      isSelected = true;
    }

    public void BtnDeselect() {
      ColorBlock cBlock = mButton.colors;
      mBtnImg.color = cBlock.normalColor;
      isSelected = false;
    }

    void OnButtonClick() {
      if (!isSelected) {
        BtnSelect();
        mGlobal.OnLilySelect(mLilyType);
        foreach (LilyButton lBtn in mAllBtns) {
          if (lBtn != this) {
            lBtn.BtnDeselect();
          }
        }
      } else {
        BtnDeselect();
        mGlobal.OnLilyDeselect();
      }
    }
}
