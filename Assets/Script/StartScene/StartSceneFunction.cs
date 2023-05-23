using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;
using UnityEngine.UI;
//StartScene에 static data들과 관련없는 함수들이 들어있음 ex)화면전환 효과라던가
public class StartSceneFunction : MonoBehaviour
{
    [SerializeField]
    Transform camera;
    [SerializeField]
    DayText dayText;
    [SerializeField]
    kkkk Img;
    [SerializeField]
    Image BackGround;
    public TextAnalysis textAnalysis;
    public VGGScript imageDetection;
    // Start is called before the first frame update
    void Start()
    {
        DataManager.saveFile_No = 0; //시작시에 가장 처음Index를 읽어온다.
        readSaveFile(0); // 시작시에 먼저 saveFile을 읽어오게된다.
        dayText.updateDayText();
        //StartCoroutine(Blink_BG());


    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.RightArrow)) // 화면전환 효과 시작점 오른쪽 왼쪽 방향키를 받아서 함수호출
        {
            slide_SaveFile("Right");
        }
        else if (Input.GetKeyUp(KeyCode.LeftArrow))
        {
            slide_SaveFile("Left");
        }
        if (Input.GetKeyDown(KeyCode.B))
        {
            //textAnalysis.EmotionAnalysis("안녕 나는 샘플 텍스트야.");
            imageDetection.ImageDetection();
        }
    }
    public IEnumerator Blink_BG()
    {
        float backGround_Time = 0.0f;
        while (true)
        {
            BackGround.color = new Color(0.35f + backGround_Time * 0.5f, 0.35f + backGround_Time * 0.5f, 0.35f + backGround_Time * 0.5f, 1);
            backGround_Time += Time.deltaTime;
            yield return new WaitForSeconds(0.03f);
        }
    }
    public void slide_SaveFile(string direction)//우측이냐 좌측이냐에 따라서 함수 호출 
    {
        if (direction == "Right")
        {
            if(DataManager.saveFile_No == 2)
            {
                DataManager.saveFile_No = 0;
            }
            else
            {
                DataManager.saveFile_No++;
            }//Save파일 개수가 3개라고 가정할때 오른쪽으로 움직일때 가장 최근의 save파일일 경우 0으로 초기화해주고 나머지 경우엔 +1을 해주고
            readSaveFile(DataManager.saveFile_No);
            //올라간 Save파일 넘버에 의한 맞는 Save파일을 읽어와서 DataManager에 덮어씌우고
            //StartCoroutine(slide_Right());//오른쪽으로 슬라이딩하는 Animation을 보여준뒤
            StartCoroutine(Img.open());
            dayText.updateDayText();
            
            //UI창의 text와 slide_bar를 바꿔준다.
        }
        if (direction == "Left")
        {
            if (DataManager.saveFile_No == 0)
            {
                DataManager.saveFile_No = 2;
            }
            else
            {
                DataManager.saveFile_No--;
            }//Save파일 개수가 3개라고 가정할때 왼쪽으로 움직일때 index가 0일경우 2로 바꿔주고 나머지의 경우에는 -1을 해준다
            readSaveFile(DataManager.saveFile_No);
            //내려간 Save파일 넘버에 의한 맞는 Save파일을 읽어와서 DataManager에 덮어씌우고
            StartCoroutine(slide_Left());//왼쪽으로 슬라이딩하는 Animation을 보여준뒤
            dayText.updateDayText();
            //UI창의 text와 slide_bar를 바꿔준다.
        }

    }
    IEnumerator slide_Right()
    {
        float time = 0.0f;
        while (time < 0.25f) // 시간초 조절
        {
            camera.position = new Vector3(time * 820, 1, -10); // time최대값과 곱셈값의 곱셈이 이 205가 되도록
            yield return new WaitForSeconds(0.01f);
            time += Time.deltaTime;
        }
        camera.position = new Vector3(0, 1, -10); // 움직임을 위해서 오른쪽 왼쪽에 더미UI를 넣었지만 실제로는 중앙UI에 값을 업데이트 한뒤 되돌아오는 함수
    }
    IEnumerator slide_Left()
    {
        float time = 0.0f;
        while (time < 0.25f)
        {
            camera.position = new Vector3(time * -820, 1, -10); //time최대값과 곱셈값의 곱셈이 이 205가 되도록
            yield return new WaitForSeconds(0.01f);
            time += Time.deltaTime;
        }
        camera.position = new Vector3(0, 1, -10);// 움직임을 위해서 오른쪽 왼쪽에 더미UI를 넣었지만 실제로는 중앙UI에 값을 업데이트 한뒤 되돌아오는 함수
    }
    public void readSaveFile(int saveFileIndex) // 경로와 index로 부터 저장된 saveFileIndex를 읽어서 DataManager속 static 변수에 저장
    {
        XmlDocument save = new XmlDocument();
        //save.Load(Application.dataPath + "/TextScript/DummySaveFile/Save"+saveFileIndex.ToString()+".xml");save.Load(Application.dataPath + "/TextScript/DummySaveFile/Save"+saveFileIndex.ToString()+".xml");
        save.Load(Application.persistentDataPath + "/TextScript/DummySaveFile/Save" + saveFileIndex.ToString() + ".xml");
        XmlNodeList nodes = save["Save"].ChildNodes;
        DataManager.day_No = int.Parse(nodes[0].InnerText);
        DataManager.day_Turn = int.Parse(nodes[1].InnerText);
        DataManager.script_No = int.Parse(nodes[2].InnerText);

    }
    //나중에 할일 >> 파일을 경로이름으로 읽는것이 아닌 파일리스트 모음을 만들어서 자동적으로 파일개수와 이름으로 구성된 List를 만들어서 사용할 생각
}
