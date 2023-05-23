using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PhotoPopupFunction : MonoBehaviour
{
    [SerializeField]
    Image bg_Sample;
    [SerializeField]
    public List<Sprite> sampleImage_List = new List<Sprite>();
    private int sampleBG_Index;
    public Image change_BG;//¹Ù²Ü BGÁöÁ¤
    // Start is called before the first frame update
    void Start()
    {
        
    }
    void OnEnable()
    {
        sampleBG_Index = 0;
        sampleImage_List.AddRange(Resources.LoadAll<Sprite>("BackGrounds"));
        bg_Sample.sprite = sampleImage_List[sampleBG_Index];
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void LoadSampleBG()
    {
        
    }
    public void MoveRight()
    {
        if (sampleBG_Index < sampleImage_List.Count - 1)
        {
            sampleBG_Index++;
        }
        else
        {
            sampleBG_Index = 0;
        }
        bg_Sample.sprite = sampleImage_List[sampleBG_Index];
    }
    public void MoveLeft()
    {
        if (sampleBG_Index > 0)
        {
            sampleBG_Index--;
        }
        else
        {
            sampleBG_Index = sampleImage_List.Count - 1;
        }
        bg_Sample.sprite = sampleImage_List[sampleBG_Index];
    }
    public void ChangeBG()
    {
        change_BG.sprite = sampleImage_List[sampleBG_Index];
    }
}
