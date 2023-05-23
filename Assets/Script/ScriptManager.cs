using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;
using TMPro;
using System;
using UnityEngine.SceneManagement;

public class ScriptManager : MonoBehaviour
{
    public static XmlDocument doc = new XmlDocument();
    public GlitchEffect MainCamera;
    public GameObject BlockCanvas;
    public GameObject AIBlockCanvas;


    public GameObject MapButton;
    public GameObject MapCanvas;
    public GameObject AICanvas;
    
    public GameObject ScriptPanel;
    public GameObject CellPhoneButton;
    public GameObject CellPhone;
    public GameObject CellPhoneMessage;
    public GameObject UITutorialPanel;
    public TextMeshProUGUI UITutorial_text;

    public GameObject AIScriptPanel;
    public TextMeshProUGUI AIScriptText;
    public GameObject AITutorialPanel;
    public GameObject Research_Popup;
    public GameObject ChatPopup;
    public TextMeshProUGUI AITutorial_text;
    public GameObject Research_WritePanel;
    public GameObject DecisionPanel_GameObject;
    public DecisionPanel DecisionPanel;

    public GameObject PictureDataButton;
    public GameObject AI_PicManager;
    public VGGScript Image_Detection;

    public GameObject BlackJack_Popup;
    public GameObject BlackJack_Real;

    public static List<string> script_List = new List<string>();
    public static List<string> Img_List = new List<string>();
    public static List<string> Img_Dest_List = new List<string>();
    public static List<string> Img_Effect_List = new List<string>();
    public static List<string> move_List = new List<string>();
    public static List<string> sound_List = new List<string>();
    public static List<string> name_List = new List<string>();
    public void clearScript() // Script를 다른거 불러올때 초기화해주기 위한 함수.
    {
        script_List.Clear();
        Img_List.Clear();
        Img_Dest_List.Clear();
        Img_Effect_List.Clear();
        move_List.Clear();
        sound_List.Clear();
        name_List.Clear();
    }
    bool isRunning = false;
    [SerializeField]
    ImageManager Imanager;
    [SerializeField]
    AudioManager Amanager;
    //텍스트 출력을 위한 컴포넌트
    [SerializeField]
    public TextMeshProUGUI text;
    [SerializeField]
    TextMeshProUGUI name_Text;
    public ChatManager CManager;
    IEnumerator writingText = null;
    //스토리 진행할때 모드 구분을 위한 인자들.Script_Status >> 현재 작동되고 있는 Scene이름 인덱스들은 Script상 어디까지 진행됐는지 저장하는 함수
    //mode Status는 같은 게임씬이라도 ai씬 작동중인지 아닌지 확인하는 함수
    public string script_Status = "";
    string mode_Status = "Normal";
    public int storyIndex;
    public int storeIndex = 0;
    public int moveIndex = 0;
    // Start is called before the first frame update
    void Start()
    {
        
    }
    void OnEnable()
    {
        // 씬 매니저의 sceneLoaded에 체인을 건다.
        storyIndex = DataManager.script_No;
        SceneManager.sceneLoaded += OnSceneLoaded;

    }
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        LoadSources();
    }
    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log("KKKKK");
            AI_PicManager.SetActive(false);
        }
    }
    public void LoadSources()
    {
        string activeSceneName = SceneManager.GetActiveScene().name;
        if (activeSceneName == "GameScene")
        {
            script_Status = "story";
            LoadScript(DataManager.day_No, DataManager.day_Turn);
        }
        else if (activeSceneName == "StoreScene")
        {
            script_Status = "store";
            StoreScript();
        }
        else if (activeSceneName == "AIScene")
        {
            Debug.Log("AI");
            script_Status = "story";
            LoadScript(DataManager.day_No, DataManager.day_Turn);
        }
        //text = GameObject.Find("ScriptText").GetComponent<TextMeshProUGUI>();
        name_Text = GameObject.Find("NameText").GetComponent<TextMeshProUGUI>();
        Imanager = GameObject.Find("ImageManager").GetComponent<ImageManager>();
        Amanager = GameObject.Find("AudioManager").GetComponent<AudioManager>();
        Imanager.LoadImage();
        Amanager.LoadSound();
        if(storyIndex == 0)
        {
            printScript();
        }
    }
    public void LoadScript(int day, int turn)
    {
        string activeSceneName = SceneManager.GetActiveScene().name;
        script_Status = "story";
        doc.Load(Application.dataPath + "/TextScript/"+ activeSceneName +"/"+ activeSceneName+"day"+day.ToString()+ "_"+turn.ToString()+".xml");
        XmlElement nodes = doc["day"+day.ToString()];
        clearScript();
        foreach (XmlElement node in nodes.ChildNodes)
        {
            script_List.Add(node.GetAttribute("Comm"));
            Img_List.Add(node.GetAttribute("Img_Location"));
            move_List.Add(node.GetAttribute("Move"));
            Img_Dest_List.Add(node.GetAttribute("Img_Dest"));
            Img_Effect_List.Add(node.GetAttribute("Img_Effect"));
            sound_List.Add(node.GetAttribute("BGM"));
            name_List.Add(node.GetAttribute("Name"));

        }
        Debug.Log(script_List.Count);
    }
    
    public void printScript()
    {
        string activeSceneName = SceneManager.GetActiveScene().name;
        Debug.Log(storyIndex);
        /*if(storyIndex == script_List.Count)
        {
            DataManager.day_No++;
            DataManager.script_No = 0;
            DataManager.save();
            LoadingSceneManager.LoadScene("StartScene");
            return;
        }*/
        int script_Num = -1; // Script가 출력될 number
        if (script_Status == "story")
        {
            script_Num = storyIndex;
        }
        else if (script_Status == "move")
        {
            script_Num = moveIndex;
        }
        else if (script_Status == "store")
        {
            script_Num = storeIndex;
        }
        if(script_Num == script_List.Count)
        {
            return;
        }
        if (move_List[script_Num] == "Research" || move_List[script_Num] == "Research_Write" || move_List[script_Num] == "Research_End")
        {
            text.text = "";
            text.text = script_List[script_Num];
            moveList(script_Num);
            return;
        }
        if (move_List[script_Num] == "BlackJack" || move_List[script_Num] == "BlackJack_Start" || move_List[script_Num] == "BlackJack_End")
        {
            text.text = "";
            text.text = script_List[script_Num];
            moveList(script_Num);
            return;
        }
        if (move_List[script_Num] == "CellPhoneMessage" | move_List[script_Num] == "CellPhone" | move_List[script_Num] == "CellPhoneEnd")
        {
            text.text = "";
            text.text = script_List[script_Num];
            moveList(script_Num);
            return;
        }
        if (activeSceneName == "GameScene")
        {
            if(script_Status == "story") // story일때 
            {
                if (writingText != null)
                {
                    StopCoroutine(writingText);
                    text.text = "";
                    name_Text.text = name_List[script_Num];
                    int textSize = script_List[script_Num].Length;
                    if (textSize == 0)
                    {
                        text.fontSize = 40;
                    }
                    else if (script_List[script_Num][0] == "<".ToCharArray()[0])
                    {
                        int size = Int32.Parse(script_List[script_Num].Substring(1, 2));
                        script_List[script_Num] = script_List[script_Num].Substring(5, textSize - 5);
                        text.fontSize = 0.4f * size;
                        textSize = script_List[script_Num].Length;
                    }
                    else
                    {
                        text.fontSize = 40;
                    }
                    text.text = script_List[script_Num];
                    moveList(script_Num);
                    storyIndex++;
                    writingText = null;
                }
                else if (writingText == null && Imanager.ImageEnum == null)
                {
                    StopAudio(script_Num);
                    playAudio(script_Num);
                    writingText = printScriptEnum(script_Num);
                    
                    ImageChange(script_Num);
                    StartCoroutine(writingText);

                }
            }
            else if(script_Status == "move") // move일때
            {
                if (writingText != null)
                {
                    StopCoroutine(writingText);
                    text.text = "";
                    name_Text.text = name_List[script_Num];
                    text.text = script_List[script_Num];
                    moveList(script_Num);
                    moveIndex++;
                    writingText = null;
                }
                else if (writingText == null && Imanager.ImageEnum == null)
                {
                    StopAudio(script_Num);
                    writingText = printScriptEnum(script_Num);
                    playAudio(script_Num);
                    ImageChange(script_Num);
                    StartCoroutine(writingText);

                }
            }
            
        }
        else if (activeSceneName == "AIScene") // AIScene일때 출력하는 왜따로 만들었냐? >> AI콘솔이 On되면 메시지를 아래 스크립트창이 아닌 kakaotalk같은 형식으로 출력해야 하기 때문.
        {
            if (script_Status == "story") // story일때 
            {
                if (writingText != null)
                {
                    StopCoroutine(writingText);
                    text.text = "";
                    name_Text.text = name_List[script_Num];
                    int textSize = script_List[script_Num].Length;
                    if (textSize == 0)
                    {
                        text.fontSize = 40;
                    }
                    else if (script_List[script_Num][0] == "<".ToCharArray()[0])
                    {
                        int size = Int32.Parse(script_List[script_Num].Substring(1, 2));
                        script_List[script_Num] = script_List[script_Num].Substring(5, textSize - 5);
                        text.fontSize = 0.4f * size;
                        textSize = script_List[script_Num].Length;
                    }
                    else
                    {
                        text.fontSize = 40;
                    }
                    text.text = script_List[script_Num];
                    storyIndex++;
                    moveList(script_Num);
                    writingText = null;
                }
                else if (writingText == null && Imanager.ImageEnum == null)
                {
                    StopAudio(script_Num);
                    playAudio(script_Num);
                    writingText = printScriptEnum(script_Num);
                    
                    ImageChange(script_Num);
                    StartCoroutine(writingText);

                }
            }
            else if (script_Status == "move") // move일때
            {
                if (writingText != null)
                {
                    StopCoroutine(writingText);
                    text.text = "";
                    name_Text.text = name_List[script_Num];
                    text.text = script_List[script_Num];
                    moveIndex++;
                    moveList(script_Num);
                    writingText = null;
                }
                else if (writingText == null && Imanager.ImageEnum == null)
                {
                    StopAudio(script_Num);
                    writingText = printScriptEnum(script_Num);
                    playAudio(script_Num);
                    ImageChange(script_Num);
                    StartCoroutine(writingText);

                }
            }

        }
        else if(activeSceneName == "StoreScene")
        {
            if (writingText != null)
            {
                StopCoroutine(writingText);
                text.text = script_List[storeIndex];
                storeIndex++;
                writingText = null;
            }
            else
            {
                ImageChange(script_Num);
                writingText = printScriptEnum(script_Num);
                StartCoroutine(writingText);

            }
        }
        
    }

    IEnumerator printScriptEnum(int script_Num)
    {
        int presentSize = 0; //현재 표시될 stringSize
        int textSize = script_List[script_Num].Length;
        bool isClicked = false;
        string presentedText = "";
        yield return new WaitForSeconds(0.15f);
        name_Text.text = name_List[script_Num];
        if(textSize == 0)
        {

        }
        else if(script_List[script_Num][0] == "<".ToCharArray()[0])
        {
            int size = Int32.Parse(script_List[script_Num].Substring(1, 2));
            script_List[script_Num] = script_List[script_Num].Substring(5, textSize - 5);
            text.fontSize = 0.4f * size;
            textSize = script_List[script_Num].Length;
        }
        else
        {
            text.fontSize = 40;
        }
        while ( presentSize < textSize && isClicked == false)
        {
            //text.text = script_List[DataManager.script_No].Substring(0, presentSize);
            presentedText += script_List[script_Num][presentSize];
            text.text = presentedText;
            yield return new WaitForSeconds(0.04f);
            presentSize++;
        }
        text.text = script_List[script_Num];
        if(script_Status == "story")
        {
            storyIndex++;
        }
        else if(script_Status == "move")
        {
            moveIndex++;
        }
        moveList(script_Num);
        writingText = null;
        

    }
    public void UITutorial()
    {

        int script_Num = -1; // Script가 출력될 number
        if (script_Status == "story")
        {
            script_Num = storyIndex;
        }
        else if (script_Status == "move")
        {
            script_Num = moveIndex;
        }
        else if (script_Status == "store")
        {
            script_Num = storeIndex;
        }
        if (move_List[script_Num] == "CellPhoneMessage" | move_List[script_Num] == "CellPhone" | move_List[script_Num] == "CellPhoneEnd")
        {
            UITutorial_text.text = "";
            UITutorial_text.text = script_List[script_Num];
            moveList(script_Num);
            return;
        }
        Debug.Log("KK");
        if (writingText != null)
        {
            StopCoroutine(writingText);
            UITutorial_text.text = "";
            UITutorial_text.text = script_List[script_Num];
            if (script_Status == "story")
            {
                storyIndex++;
            }
            else if (script_Status == "move")
            {
                moveIndex++;
            }
            else if (script_Status == "store")
            {
                storeIndex++;
            }
            moveList(script_Num);
            writingText = null;
        }
        else if (writingText == null /*&& Imanager.ImageEnum == null*/)
        {
            writingText = UITutorialPrintScriptEnum(script_Num);
            //playAudio(script_Num);
            //ImageChange(script_Num);
            //monologue_text.text = name_List[script_Num];
            StartCoroutine(writingText);
        }
    }
    IEnumerator UITutorialPrintScriptEnum(int script_Num)
    {
        int presentSize = 0; //현재 표시될 stringSize
        int textSize = script_List[script_Num].Length;
        bool isClicked = false;
        string presentedText = "";
        yield return new WaitForSeconds(0.15f);
        while (presentSize < textSize && isClicked == false)
        {
            //text.text = script_List[DataManager.script_No].Substring(0, presentSize);
            presentedText += script_List[script_Num][presentSize];
            UITutorial_text.text = presentedText;
            yield return new WaitForSeconds(0.07f);
            presentSize++;
        }
        UITutorial_text.text = script_List[script_Num];
        if (script_Status == "story")
        {
            storyIndex++;
        }
        else if (script_Status == "move")
        {
            moveIndex++;
        }
        else if (script_Status == "store")
        {
            storeIndex++;
        }
        moveList(script_Num);
        writingText = null;
    }
    public void AITutorial()
    {
        int script_Num = -1; // Script가 출력될 number
        if (script_Status == "story")
        {
            script_Num = storyIndex;
        }
        else if (script_Status == "move")
        {
            script_Num = moveIndex;
        }
        else if (script_Status == "store")
        {
            script_Num = storeIndex;
        }
        if(move_List[script_Num] == "Research" || move_List[script_Num] == "Research_Write" || move_List[script_Num] == "Research_End")
        {
            AITutorial_text.text = "";
            AITutorial_text.text = script_List[script_Num];
            moveList(script_Num);
            return;
        }
        if (move_List[script_Num] == "BlackJack" || move_List[script_Num] == "BlackJack_Start" || move_List[script_Num] == "BlackJack_End")
        {
            AITutorial_text.text = "";
            AITutorial_text.text = script_List[script_Num];
            moveList(script_Num);
            return;
        }
        if (writingText != null)
        {
            StopCoroutine(writingText);
            AITutorial_text.text = "";
            AITutorial_text.text = script_List[script_Num];
            if (script_Status == "story")
            {
                storyIndex++;
            }
            else if (script_Status == "move")
            {
                moveIndex++;
            }
            else if (script_Status == "store")
            {
                storeIndex++;
            }
            moveList(script_Num);
            writingText = null;
        }
        else if (writingText == null /*&& Imanager.ImageEnum == null*/)
        {
            writingText = AITutorialPrintScriptEnum(script_Num);
            //playAudio(script_Num);
            //ImageChange(script_Num);
            //monologue_text.text = name_List[script_Num];
            StartCoroutine(writingText);
        }
    }
    IEnumerator AITutorialPrintScriptEnum(int script_Num)
    {
        int presentSize = 0; //현재 표시될 stringSize
        int textSize = script_List[script_Num].Length;
        bool isClicked = false;
        string presentedText = "";
        yield return new WaitForSeconds(0.15f);
        while (presentSize < textSize && isClicked == false)
        {
            //text.text = script_List[DataManager.script_No].Substring(0, presentSize);
            presentedText += script_List[script_Num][presentSize];
            AITutorial_text.text = presentedText;
            yield return new WaitForSeconds(0.07f);
            presentSize++;
        }
        AITutorial_text.text = script_List[script_Num];
        if (script_Status == "story")
        {
            storyIndex++;
        }
        else if (script_Status == "move")
        {
            moveIndex++;
        }
        else if (script_Status == "store")
        {
            storeIndex++;
        }
        moveList(script_Num);
        writingText = null;
    }
    public void StoreScript()
    {
        doc.Load(Application.dataPath + "/TextScript/StoreSample.xml");
        XmlElement nodes = doc["Store"];
        clearScript();
        foreach (XmlElement node in nodes.ChildNodes)
        {
            script_List.Add(node.GetAttribute("Comm"));
            Img_List.Add(node.GetAttribute("Img"));
            move_List.Add(node.GetAttribute("Move"));

        }
    }

    IEnumerator printStoreScriptEnum()
    {
        int presentSize = 0; //현재 표시될 stringSize
        int textSize = script_List[storeIndex].Length;
        bool isClicked = false;
        while (presentSize < textSize && isClicked == false)
        {
            text.text = script_List[storeIndex].Substring(0, presentSize);
            yield return new WaitForSeconds(0.03f);
            presentSize++;
        }
        text.text = script_List[storeIndex];
        storeIndex++;
        yield return new WaitForSeconds(0.2f);
        writingText = null;
    }
    IEnumerator mapLoading()
    {
        MapButton.SetActive(true);
        bool isClicked = false;
        while (!isClicked)
        {
            if (MapCanvas.activeSelf == true)
            {
                isClicked = true;
            }
            yield return null;
        }
        writingText = null;
        printScript();
    }
    IEnumerator photoLoading()
    {
        PictureDataButton.SetActive(true);
        bool isClicked = false;
        while (!isClicked)
        {
            if (AI_PicManager.activeSelf == true)
            {
                isClicked = true;
            }
            yield return null;
        }
        writingText = null;
        printScript();
    }
    IEnumerator photo_EndLoading()
    {
        bool isClicked = false;
        while (!isClicked)
        {
            if (AI_PicManager.activeSelf == false)
            {
                isClicked = true;
            }
            yield return null;
        }
        writingText = null;
        Image_Detection.ImageDetection();
        printScript();
    }
    IEnumerator cellPhoneLoading()
    {
        CellPhoneButton.SetActive(true);
        bool isClicked = false;
        while (!isClicked)
        {
            if (CellPhone.activeSelf == true)
            {
                isClicked = true;
            }
            yield return null;
        }
        writingText = null;
        moveIndex++;
        printScript();
    }
    IEnumerator cellPhoneMessageLoading()
    {
        bool isClicked = false;
        while (!isClicked)
        {
            if (CellPhoneMessage.activeSelf == true)
            {
                isClicked = true;
            }
            yield return null;
        }
        
        moveIndex = 6;
        writingText = null;
        printScript();
    }
    IEnumerator researchLoading()
    {
        bool isClicked = false;
        while (!isClicked)
        {
            if (Research_Popup.activeSelf == true)
            {
                isClicked = true;
            }
            yield return null;
        }
        writingText = null;
        AITutorial();
    }
    IEnumerator makeDelay(float delayTime)
    {
        float time = 0.0f;
        while(time < delayTime)
        {
            time += Time.deltaTime;
            yield return new WaitForSeconds(0.05f);
        }
        writingText = null;
        printScript();
    }

    public void ImageChange(int script_Num) // Image매니저 내의 image를 출력하는 함수
    {
        string activeSceneName = SceneManager.GetActiveScene().name;
        if (activeSceneName == "GameScene" || activeSceneName == "AIScene")
        {
            
            if (Img_Effect_List[script_Num] != "stay")
            {
                Imanager.ActionImage(Img_Dest_List[script_Num], Img_Effect_List[script_Num], Img_List[script_Num], 0, 0, 0);
            }
        }
        else if (activeSceneName == "StoreScene")
        {
           
        }
        
        
    }
    public void playAudio(int script_Num) // Amanager로 bgm이름 보내주는 함수
    {
        string activeSceneName = SceneManager.GetActiveScene().name;
        if (activeSceneName == "GameScene" || activeSceneName == "AIScene")
        {
          
            if (sound_List[script_Num] != "")
            {
                Amanager.get_SFXName(sound_List[script_Num]);
            }
           

        }
        else if (activeSceneName == "StoreScene")
        {

        }

    }
    public void StopAudio(int script_Num)
    {
        string activeSceneName = SceneManager.GetActiveScene().name;
        if (activeSceneName == "GameScene" || activeSceneName == "AIScene")
        {

            if (sound_List[script_Num] == "Stop")
            {
                Amanager.StopSFX();
            }


        }
    }
    public void ChangeScript()
    {
        int presentIndex = 0;
        if (script_Status == "story")
        {
            presentIndex = storyIndex;
        }
        else if (script_Status == "move")
        {
            presentIndex = moveIndex;
        }
        else if (script_Status == "store")
        {
            //storyIndex = storyIndex + 3;
        }
        while(move_List[presentIndex] == "Choice_Result")
        {
            string splited = script_List[presentIndex];
            string[] splited_Array = splited.Split('|');
            script_List[presentIndex] = splited_Array[DecisionPanel.choice_Index];
            presentIndex++;
        }
    }
    public void moveList(int script_Num) // moveList  ex)Tutorial 등등 이 있으면 List에 맞게 출력하고 Script 죽이기. 
    {
        string activeSceneName = SceneManager.GetActiveScene().name;
        if (activeSceneName == "GameScene" || activeSceneName == "AIScene")
        {
            if (move_List[script_Num] == "Tutorial_Lab")
            {
                script_Status = "move";
                moveIndex = 0;
                doc.Load(Application.dataPath + "/TextScript/Tutorial_Lab.xml");
                XmlElement nodes = doc["Tutorial_Lab"];
                clearScript();
                foreach (XmlElement node in nodes.ChildNodes)
                {
                    script_List.Add(node.GetAttribute("Comm"));
                    Img_List.Add(node.GetAttribute("Img_Location"));
                    move_List.Add(node.GetAttribute("Move"));
                    Img_Dest_List.Add(node.GetAttribute("Img_Dest"));
                    Img_Effect_List.Add(node.GetAttribute("Img_Effect"));
                    sound_List.Add(node.GetAttribute("BGM"));
                    name_List.Add(node.GetAttribute("Name"));
                }
                Imanager.LoadImage();
                Amanager.LoadSound();
                printScript();
            }
            else if (move_List[script_Num] == "Tutorial_CellPhone")
            {
                script_Status = "move";
                moveIndex = 0;
                doc.Load(Application.dataPath + "/TextScript/Tutorial_CellPhone.xml");
                XmlElement nodes = doc["Tutorial_CellPhone"];
                clearScript();
                foreach (XmlElement node in nodes.ChildNodes)
                {
                    script_List.Add(node.GetAttribute("Comm"));
                    Img_List.Add(node.GetAttribute("Img_Location"));
                    move_List.Add(node.GetAttribute("Move"));
                    Img_Dest_List.Add(node.GetAttribute("Img_Dest"));
                    Img_Effect_List.Add(node.GetAttribute("Img_Effect"));
                    sound_List.Add(node.GetAttribute("BGM"));
                    name_List.Add(node.GetAttribute("Name"));
                }
                Imanager.LoadImage();
                Amanager.LoadSound();

                printScript();
            }
            else if (move_List[script_Num] == "Day2_Choice")
            {
                if(DecisionPanel.choice_Index == 0)
                {
                    storyIndex = 0;
                    activeSceneName = SceneManager.GetActiveScene().name;
                    script_Status = "story";
                    doc.Load(Application.dataPath + "/TextScript/" + activeSceneName + "/" + activeSceneName + "day2_2"+"drink" +".xml");
                    XmlElement nodes = doc["day2"];
                    clearScript();
                    foreach (XmlElement node in nodes.ChildNodes)
                    {
                        script_List.Add(node.GetAttribute("Comm"));
                        Img_List.Add(node.GetAttribute("Img_Location"));
                        move_List.Add(node.GetAttribute("Move"));
                        Img_Dest_List.Add(node.GetAttribute("Img_Dest"));
                        Img_Effect_List.Add(node.GetAttribute("Img_Effect"));
                        sound_List.Add(node.GetAttribute("BGM"));
                        name_List.Add(node.GetAttribute("Name"));
                    }
                    Imanager.LoadImage();
                    Amanager.LoadSound();
                    
                    printScript();
                }
                else
                {

                }
                
            }
            else if (move_List[script_Num] == "Tutorial_Home")
            {
                script_Status = "move";
                moveIndex = 0;
                doc.Load(Application.dataPath + "/TextScript/Tutorial_Home.xml");
                XmlElement nodes = doc["Tutorial_Home"];
                clearScript();
                foreach (XmlElement node in nodes.ChildNodes)
                {
                    script_List.Add(node.GetAttribute("Comm"));
                    Img_List.Add(node.GetAttribute("Img_Location"));
                    move_List.Add(node.GetAttribute("Move"));
                    Img_Dest_List.Add(node.GetAttribute("Img_Dest"));
                    Img_Effect_List.Add(node.GetAttribute("Img_Effect"));
                    sound_List.Add(node.GetAttribute("BGM"));
                    name_List.Add(node.GetAttribute("Name"));
                }
                Imanager.LoadImage();
                Amanager.LoadSound();
                printScript();
            }
            else if (move_List[script_Num] == "Tutorial_Emotion")
            {
                script_Status = "move";
                moveIndex = 0;
                doc.Load(Application.dataPath + "/TextScript/Tutorial_Emotion.xml");
                XmlElement nodes = doc["Tutorial_Emotion"];
                clearScript();
                foreach (XmlElement node in nodes.ChildNodes)
                {
                    script_List.Add(node.GetAttribute("Comm"));
                    Img_List.Add(node.GetAttribute("Img_Location"));
                    move_List.Add(node.GetAttribute("Move"));
                    Img_Dest_List.Add(node.GetAttribute("Img_Dest"));
                    Img_Effect_List.Add(node.GetAttribute("Img_Effect"));
                    sound_List.Add(node.GetAttribute("BGM"));
                    name_List.Add(node.GetAttribute("Name"));
                }
                Imanager.LoadImage();
                Amanager.LoadSound();
                ChatPopup.SetActive(false);
                printScript();
                //CManager.monologue(script_List[moveIndex]);
                //AITutorialPanel.SetActive(true);
            }
            else if (move_List[script_Num] == "Tutorial_BlackJack")
            {
                script_Status = "move";
                moveIndex = 0;
                doc.Load(Application.dataPath + "/TextScript/Tutorial_BlackJack.xml");
                XmlElement nodes = doc["Tutorial_BlackJack"];
                clearScript();
                foreach (XmlElement node in nodes.ChildNodes)
                {
                    script_List.Add(node.GetAttribute("Comm"));
                    Img_List.Add(node.GetAttribute("Img_Location"));
                    move_List.Add(node.GetAttribute("Move"));
                    Img_Dest_List.Add(node.GetAttribute("Img_Dest"));
                    Img_Effect_List.Add(node.GetAttribute("Img_Effect"));
                    sound_List.Add(node.GetAttribute("BGM"));
                    name_List.Add(node.GetAttribute("Name"));
                }
                Imanager.LoadImage();
                Amanager.LoadSound();
                ChatPopup.SetActive(false);
                //AITutorialPanel.SetActive(true);
            }
            else if (move_List[script_Num] == "Tutorial_Photo")
            {
                script_Status = "move";
                moveIndex = 0;
                doc.Load(Application.dataPath + "/TextScript/Tutorial_Photo.xml");
                XmlElement nodes = doc["Tutorial_Photo"];
                clearScript();
                foreach (XmlElement node in nodes.ChildNodes)
                {
                    script_List.Add(node.GetAttribute("Comm"));
                    Img_List.Add(node.GetAttribute("Img_Location"));
                    move_List.Add(node.GetAttribute("Move"));
                    Img_Dest_List.Add(node.GetAttribute("Img_Dest"));
                    Img_Effect_List.Add(node.GetAttribute("Img_Effect"));
                    sound_List.Add(node.GetAttribute("BGM"));
                    name_List.Add(node.GetAttribute("Name"));
                }
                Imanager.LoadImage();
                Amanager.LoadSound();
            }
            else if (move_List[script_Num] == "Tutorial_Map")
            {
                script_Status = "move";
                moveIndex = 0;
                doc.Load(Application.dataPath + "/TextScript/Tutorial_Map.xml");
                XmlElement nodes = doc["Tutorial_Map"];
                clearScript();
                foreach (XmlElement node in nodes.ChildNodes)
                {
                    script_List.Add(node.GetAttribute("Comm"));
                    Img_List.Add(node.GetAttribute("Img_Location"));
                    move_List.Add(node.GetAttribute("Move"));
                    Img_Dest_List.Add(node.GetAttribute("Img_Dest"));
                    Img_Effect_List.Add(node.GetAttribute("Img_Effect"));
                    sound_List.Add(node.GetAttribute("BGM"));
                    name_List.Add(node.GetAttribute("Name"));
                }
                Imanager.LoadImage();
                Amanager.LoadSound();
            }
            else if (move_List[script_Num] == "Block")
            {
                BlockCanvas.SetActive(true);
                writingText = null;
                printScript();
            }
            else if (move_List[script_Num] == "AIBlock")
            {
                //AIBlockCanvas.SetActive(true);
                //writingText = null;

                //CManager.AITutorial(script_List[script_Num]);
            }
            else if (move_List[script_Num] == "Keepgoing")
            {
                StartCoroutine(makeDelay(0.1f));
            }
            else if (move_List[script_Num] == "Map")
            {
                StartCoroutine(mapLoading());
                
            }
            else if (move_List[script_Num] == "CellPhone")
            {
                
                CellPhoneButton.SetActive(true);
                StartCoroutine(cellPhoneLoading());
                
                


            }
            else if (move_List[script_Num] == "CellPhoneMessage")
            {
                if(writingText == null)
                {
                    writingText = cellPhoneMessageLoading();
                    StartCoroutine(writingText);
                }
                else
                {
                    return;
                }
               

            }
            else if (move_List[script_Num] == "Research")
            {
                if (Research_Popup.activeSelf == true)
                {
                    moveIndex++;
                    printScript();
                }
                
            }
            else if (move_List[script_Num] == "Research_Write")
            {

                if (Research_WritePanel.activeSelf == true)
                {
                    moveIndex++;
                    printScript();
                }


            }
            else if (move_List[script_Num] == "Research_End")
            {

                if (Research_WritePanel.activeSelf == false)
                {
                    moveIndex++;
                    printScript();
                }


            }
            else if (move_List[script_Num] == "BlackJack")
            {
                if (BlackJack_Popup.activeSelf == true)
                {
                    moveIndex++;
                    printScript();
                }

            }
            else if (move_List[script_Num] == "BlackJack_Start")
            {
                if (BlackJack_Real.activeSelf == true)
                {
                    moveIndex++;
                    text.text = "";
                    ScriptPanel.SetActive(false);
                                 
                }

            }
            else if (move_List[script_Num] == "BlackJack_End")
            {

                

            }
            else if (move_List[script_Num] == "MapIsOpened")
            {

            }
            else if (move_List[script_Num] == "AISceneOn")
            {
                AIBlockCanvas.SetActive(true);
                AICanvas.SetActive(true);
                //ScriptPanel.SetActive(false);
                //AIScriptPanel.SetActive(true);
                mode_Status = "AI";
            }
            else if (move_List[script_Num] == "AISceneOnWithOutScript")
            {
                AIBlockCanvas.SetActive(true);
                AICanvas.SetActive(true);
                ScriptPanel.SetActive(false);
                AIScriptPanel.SetActive(false);
                mode_Status = "AI";
            }
            else if (move_List[script_Num] == "AISceneOff")
            {
                Debug.Log("Off");
                text.text = "";
                AIBlockCanvas.SetActive(false);
                AICanvas.SetActive(false);
                ScriptPanel.SetActive(true);
                mode_Status = "story";
            }
            else if (move_List[script_Num] == "Turnup")
            {
                Debug.Log("TU");
                DataManager.day_Turn++;
                printScript();
                
            }
            else if (move_List[script_Num] == "Dayend")
            {
                DataManager.day_Turn = 0;
                DataManager.day_No++;
                DataManager.save();
                LoadingSceneManager.LoadScene("StartScene");
            }
            else if (move_List[script_Num] == "Day1end")
            {
                DataManager.day_Turn = 0;
                DataManager.day_No++;
                DataManager.save();
                LoadingSceneManager.LoadScene("GameScene");
            }
            else if (move_List[script_Num] == "Tutorial_End")
            {
                script_Status = "story";
                writingText = null;
                storyIndex++;
                LoadSources();
            }
            else if (move_List[script_Num] == "Photo_Tutorial_End")
            {
                BlockCanvas.SetActive(false);
                ScriptPanel.SetActive(true);
                PictureDataButton.SetActive(false);
                script_Status = "story";
                writingText = null;
                storyIndex++;
                LoadSources();
            }
            else if (move_List[script_Num] == "Tutorial_End")
            {
                script_Status = "story";
                writingText = null;
                storyIndex++;
                LoadSources();
            }
            else if (move_List[script_Num] == "Tutorial_CellPhone_End")
            {
                UITutorialPanel.SetActive(false);
                ScriptPanel.SetActive(true);
                CellPhone.SetActive(false);
                CellPhoneButton.SetActive(false);
                BlockCanvas.SetActive(false);
                text.text = "";
                script_Status = "story";
                writingText = null;
                storyIndex++;
                LoadSources();
            }
            else if (move_List[script_Num] == "Tutorial_Emotion_End")
            {
                script_Status = "story";
                AIScriptText.text = "";
                writingText = null;
                Research_Popup.SetActive(false);
                storyIndex++;
                AITutorialPanel.SetActive(false);
                //AIScriptPanel.SetActive(true);
                LoadSources();
            }
            else if (move_List[script_Num] == "Monologue")
            {
                CManager.monologue(script_List[script_Num]);
                return;
            }
            else if (move_List[script_Num] == "AITutorial")
            {
                return;
            }
            else if (move_List[script_Num] == "just_glitch")
            {
                MainCamera.intensity = 1;
            }
            else if (move_List[script_Num] == "glitch_end")
            {
                MainCamera.intensity = 0;
            }
            else if (move_List[script_Num] == "Photo")
            {
                StartCoroutine(photoLoading());

            }
            else if (move_List[script_Num] == "Photo_End")
            {
                StartCoroutine(photo_EndLoading());

            }
            else if (move_List[script_Num] == "MoveToHome")
            {
                DataManager.save();
                LoadingSceneManager.LoadScene("GameScene");

            }
            else if (move_List[script_Num] == "Skip")
            {
                printScript();

            }
            else if (move_List[script_Num] == "smalltext")
            {
                

            }
            else if (move_List[script_Num] == "SFX")
            {
                StopAudio(script_Num);
                playAudio(script_Num);
            }
            else if (move_List[script_Num] == "BlackJack_TutorialChoice") // 선택창을 여는 함수
            {
                ScriptPanel.SetActive(false); // 일단 끄고
                DecisionPanel_GameObject.SetActive(true);
                DecisionPanel.firstText.text = script_List[script_Num + 1];
                DecisionPanel.secondText.text = script_List[script_Num + 2];
                DecisionPanel.WaitForChoice(); // DecisionPanel의 함수를 부르고 기다리게 됨.
                if (script_Status == "story")
                {
                    storyIndex = storyIndex + 2;

                }
                else if (script_Status == "move")
                {
                    moveIndex = moveIndex + 2;

                }
                else if (script_Status == "store")
                {
                    //storyIndex = storyIndex + 3;
                }

            }
            else if (move_List[script_Num] == "BlackJack_TutorialChoiceResult") // 선택창을 여는 함수
            {
                ScriptPanel.SetActive(true); // 일단 열고

                if (DecisionPanel.choice_Index == 0)
                {
                    return;
                }
                else
                {
                    if (script_Status == "story")
                    {
                        storyIndex = storyIndex + 4;

                    }
                    else if (script_Status == "move")
                    {
                        moveIndex = moveIndex + 4;

                    }
                    else if (script_Status == "store")
                    {
                        //storyIndex = storyIndex + 3;
                    }
                }
                

            }
            else if (move_List[script_Num] == "Choice") // 선택창을 여는 함수
            {
                ScriptPanel.SetActive(false); // 일단 끄고
                DecisionPanel_GameObject.SetActive(true);
                DecisionPanel.firstText.text = script_List[script_Num+1];
                DecisionPanel.secondText.text = script_List[script_Num+2];
                DecisionPanel.WaitForChoice(); // DecisionPanel의 함수를 부르고 기다리게 됨.
                if (script_Status == "story")
                {
                    storyIndex = storyIndex + 2;

                }
                else if (script_Status == "move")
                {
                    moveIndex = moveIndex + 2;

                }
                else if (script_Status == "store")
                {
                    //storyIndex = storyIndex + 3;
                }

            }
            else if (move_List[script_Num] == "Choice_Result")
            {
                /*Debug.Log("KKK");
                move_List[script_Num] = "";
                script_List[script_Num] = DecisionPanel.choice_Result;
                if (script_Status == "story")
                {
                    storyIndex = storyIndex - 1;
                }
                else if (script_Status == "move")
                {
                    moveIndex = moveIndex - 1;
                }
                else if (script_Status == "store")
                {
                    //storyIndex = storyIndex + 3;
                }
                //DecisionPanel.choice_Result = "";
                printScript();*/

            }
        }
    }
}
