using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;
using UnityEngine.UI;
//StartScene�� static data��� ���þ��� �Լ����� ������� ex)ȭ����ȯ ȿ�������
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
        DataManager.saveFile_No = 0; //���۽ÿ� ���� ó��Index�� �о�´�.
        readSaveFile(0); // ���۽ÿ� ���� saveFile�� �о���Եȴ�.
        dayText.updateDayText();
        //StartCoroutine(Blink_BG());


    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.RightArrow)) // ȭ����ȯ ȿ�� ������ ������ ���� ����Ű�� �޾Ƽ� �Լ�ȣ��
        {
            slide_SaveFile("Right");
        }
        else if (Input.GetKeyUp(KeyCode.LeftArrow))
        {
            slide_SaveFile("Left");
        }
        if (Input.GetKeyDown(KeyCode.B))
        {
            //textAnalysis.EmotionAnalysis("�ȳ� ���� ���� �ؽ�Ʈ��.");
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
    public void slide_SaveFile(string direction)//�����̳� �����̳Ŀ� ���� �Լ� ȣ�� 
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
            }//Save���� ������ 3����� �����Ҷ� ���������� �����϶� ���� �ֱ��� save������ ��� 0���� �ʱ�ȭ���ְ� ������ ��쿣 +1�� ���ְ�
            readSaveFile(DataManager.saveFile_No);
            //�ö� Save���� �ѹ��� ���� �´� Save������ �о�ͼ� DataManager�� ������
            //StartCoroutine(slide_Right());//���������� �����̵��ϴ� Animation�� �����ص�
            StartCoroutine(Img.open());
            dayText.updateDayText();
            
            //UIâ�� text�� slide_bar�� �ٲ��ش�.
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
            }//Save���� ������ 3����� �����Ҷ� �������� �����϶� index�� 0�ϰ�� 2�� �ٲ��ְ� �������� ��쿡�� -1�� ���ش�
            readSaveFile(DataManager.saveFile_No);
            //������ Save���� �ѹ��� ���� �´� Save������ �о�ͼ� DataManager�� ������
            StartCoroutine(slide_Left());//�������� �����̵��ϴ� Animation�� �����ص�
            dayText.updateDayText();
            //UIâ�� text�� slide_bar�� �ٲ��ش�.
        }

    }
    IEnumerator slide_Right()
    {
        float time = 0.0f;
        while (time < 0.25f) // �ð��� ����
        {
            camera.position = new Vector3(time * 820, 1, -10); // time�ִ밪�� �������� ������ �� 205�� �ǵ���
            yield return new WaitForSeconds(0.01f);
            time += Time.deltaTime;
        }
        camera.position = new Vector3(0, 1, -10); // �������� ���ؼ� ������ ���ʿ� ����UI�� �־����� �����δ� �߾�UI�� ���� ������Ʈ �ѵ� �ǵ��ƿ��� �Լ�
    }
    IEnumerator slide_Left()
    {
        float time = 0.0f;
        while (time < 0.25f)
        {
            camera.position = new Vector3(time * -820, 1, -10); //time�ִ밪�� �������� ������ �� 205�� �ǵ���
            yield return new WaitForSeconds(0.01f);
            time += Time.deltaTime;
        }
        camera.position = new Vector3(0, 1, -10);// �������� ���ؼ� ������ ���ʿ� ����UI�� �־����� �����δ� �߾�UI�� ���� ������Ʈ �ѵ� �ǵ��ƿ��� �Լ�
    }
    public void readSaveFile(int saveFileIndex) // ��ο� index�� ���� ����� saveFileIndex�� �о DataManager�� static ������ ����
    {
        XmlDocument save = new XmlDocument();
        //save.Load(Application.dataPath + "/TextScript/DummySaveFile/Save"+saveFileIndex.ToString()+".xml");save.Load(Application.dataPath + "/TextScript/DummySaveFile/Save"+saveFileIndex.ToString()+".xml");
        save.Load(Application.persistentDataPath + "/TextScript/DummySaveFile/Save" + saveFileIndex.ToString() + ".xml");
        XmlNodeList nodes = save["Save"].ChildNodes;
        DataManager.day_No = int.Parse(nodes[0].InnerText);
        DataManager.day_Turn = int.Parse(nodes[1].InnerText);
        DataManager.script_No = int.Parse(nodes[2].InnerText);

    }
    //���߿� ���� >> ������ ����̸����� �д°��� �ƴ� ���ϸ���Ʈ ������ ���� �ڵ������� ���ϰ����� �̸����� ������ List�� ���� ����� ����
}
