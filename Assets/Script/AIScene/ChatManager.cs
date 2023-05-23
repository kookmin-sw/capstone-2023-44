using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ChatManager : MonoBehaviour
{
    [SerializeField]
    ScriptManager SManager;
    public GameObject YellowArea, WhiteArea;
    public GameObject monologueScript;
    public RectTransform ContentRect;
    public Scrollbar scrollBar;
    public ImageManager IManager;
    [SerializeField]
    TextMeshProUGUI monologue_text;
    [SerializeField]
    TextMeshProUGUI AITutorial_text;
    MessageScript LastArea;
    IEnumerator writingText = null;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    //독백일경우 script창을 띄우고 script창에 표시하는 함수
    public void monologue(string content)
    {
        
        if (writingText != null)
        {
            StopCoroutine(writingText);
            monologue_text.text = "";
            monologue_text.text = content;
            if (SManager.script_Status == "story")
            {
                SManager.storyIndex++;
            }
            else if (SManager.script_Status == "move")
            {
                SManager.moveIndex++;
            }
            else if (SManager.script_Status == "store")
            {
                SManager.storeIndex++;
            }
            //moveList(script_Num);
            writingText = null;
        }
        else if (writingText == null /*&& Imanager.ImageEnum == null*/)
        {
            writingText = printScriptEnum(content);
            //playAudio(script_Num);
            //ImageChange(script_Num);
            //monologue_text.text = name_List[script_Num];
            StartCoroutine(writingText);
        }
    }
    //AITutorial일경우 script창을 띄우고 script창에 표시하는 함수

    IEnumerator printScriptEnum(string content)
    {
        int presentSize = 0; //현재 표시될 stringSize
        int textSize = content.Length;
        bool isClicked = false;
        string presentedText = "";
        yield return new WaitForSeconds(0.15f);
        while (presentSize < textSize && isClicked == false)
        {
            //text.text = script_List[DataManager.script_No].Substring(0, presentSize);
            presentedText += content[presentSize];
            monologue_text.text = presentedText;
            yield return new WaitForSeconds(0.07f);
            presentSize++;
        }
        monologue_text.text = content;
        if (SManager.script_Status == "story")
        {
            SManager.storyIndex++;
        }
        else if (SManager.script_Status == "move")
        {
            SManager.moveIndex++;
        }
        else if (SManager.script_Status == "store")
        {
            SManager.storeIndex++;
        }
        //moveList(script_Num);
        writingText = null;
    }
    public void AIPrintScript()
    {
        int script_Num = -1; // Script가 출력될 number
        if (SManager.script_Status == "story") // script상태에 따라서 바꿔주는 함수.
        {
            script_Num = SManager.storyIndex;
        }
        else if (SManager.script_Status == "move")
        {
            script_Num = SManager.moveIndex;
        }
        else if (SManager.script_Status == "store")
        {
            script_Num = SManager.storeIndex;
        }
        if (script_Num == ScriptManager.script_List.Count)
        {
            return;
        }
        string text = ScriptManager.script_List[script_Num];
        string character = ScriptManager.name_List[script_Num];
        string move = ScriptManager.move_List[script_Num];
        string dest_Img = ScriptManager.Img_Dest_List[script_Num];
        string function = ScriptManager.Img_Effect_List[script_Num];
        string img_Location = ScriptManager.Img_List[script_Num];
        bool isSend;
        IManager.ActionImage(dest_Img, function, img_Location, 0, 0, 0);
        
        ////////////////////////////////////////////////////////////
        if(move == "Monologue")
        {
            monologue_text.text = "";
            monologueScript.SetActive(true);
            SManager.moveList(script_Num);
            return;

        }
        else if (move != "")
        {
            monologueScript.SetActive(false);
            SManager.moveList(script_Num);
            return;
        }
        monologueScript.SetActive(false);
        if (character == "주인공")
        {
            isSend = true;
            
        }
        else
        {
            isSend = false;
        }
        Chat(isSend, text, character, null);
        if (SManager.script_Status == "story")
        {
            SManager.storyIndex++;
        }
        else if (SManager.script_Status == "move")
        {
            SManager.moveIndex++;
        }
        else if (SManager.script_Status == "store")
        {
            SManager.storeIndex++;
        }

    }
    public void Chat(bool isSend, string text, string user, Texture picture)
    {
        if (text.Trim() == "") return;
        
        bool isBottom = scrollBar.value <= 0.1f;
        MessageScript Area = Instantiate(isSend ? YellowArea : WhiteArea).GetComponent<MessageScript>();
        Area.transform.SetParent(ContentRect.transform, false);
        Area.BoxRect.sizeDelta = new Vector2(600, Area.BoxRect.sizeDelta.y);
        Area.TextRect.GetComponent<TextMeshProUGUI>().text = text;
        Fit(Area.BoxRect);
        float X = Area.TextRect.sizeDelta.x + 42;
        float Y = Area.TextRect.sizeDelta.y;
        if (Y > 49)
        {
            for (int i = 0; i < 200; i++)
            {
                Area.BoxRect.sizeDelta = new Vector2(X - i * 2, Area.BoxRect.sizeDelta.y);
                Fit(Area.BoxRect);

                if (Y != Area.TextRect.sizeDelta.y) { Area.BoxRect.sizeDelta = new Vector2(X - (i * 2) + 2, Y);  break; }
            }

        }

        else Area.BoxRect.sizeDelta = new Vector2(X, Y);
        // 이전 것과 같으면 이전 시간, 꼬리 없애기
        Area.User = user;
        bool isSame = LastArea != null && LastArea.User == Area.User;
        Area.Tail.SetActive(!isSame);

        if (!isSend)
        {
            Area.UserImage.gameObject.SetActive(!isSame);
            Area.UserText.gameObject.SetActive(!isSame);
            Area.UserText.text = Area.User;
        }
        Fit(Area.BoxRect);
        Fit(Area.AreaRect);
        Fit(ContentRect);
        LastArea = Area;

        if (!isBottom) return;
        Invoke("ScrollDelay", 0.03f);
    }
    void Fit(RectTransform Rect) => LayoutRebuilder.ForceRebuildLayoutImmediate(Rect);
    void ScrollDelay() => scrollBar.value = 0;
}
