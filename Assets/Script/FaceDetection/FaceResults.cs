using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class FaceResults : MonoBehaviour
{
    public FaceDetection BeardDetection;
    public FaceDetection HumanDetection;
    public FaceDetection GlassesDetection;
    public FaceDetection HeadPhoneDetection;
    public TextMeshProUGUI resultText;

    public float[] beardOutput;
    public float[] HumanOutput;
    public float[] GlassesOutput;
    public float[] HeadPhoneOutput;

    public string beardStatus;
    public string humanStatus;
    public string glassesStatus;
    public string headPhoneStatus;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void getOutputs()
    {
        beardOutput = BeardDetection.classOutputs;
        HumanOutput = HumanDetection.classOutputs;
        GlassesOutput = GlassesDetection.classOutputs;
        HeadPhoneOutput = HeadPhoneDetection.classOutputs;
        resultText.text = HumanOutput[0].ToString();
        setOutputs();
        printHumanResult();
    }
    void setOutputs()
    {
        if (beardOutput[0] > 0.9f)
        {
            beardStatus = "��������";
        }
        else
        {
            beardStatus = "��������";
        }
        if (HumanOutput[0] > 0.9f)
        {
            humanStatus = "����Դϴ�";
        }
        else
        {
            humanStatus = "����� �ƴմϴ�";
        }
        if (GlassesOutput[0] > 0.9f)
        {
            glassesStatus = "�Ȱ�����";
        }
        else
        {
            glassesStatus = "�Ȱ����";
        }
        
    }
    public void printHumanResult()
    {
        if (HumanOutput[0] > 0.9f)
        {
            resultText.text = "��.... �׷��� ������� ����� �߳�.";
        }
        else
        {
            resultText.text = "�İ� �����ϴ¹� ������ �˾����� ���ھ� ���� ������� �ν��� ���ϰڰŵ�";
        }
    }
}
