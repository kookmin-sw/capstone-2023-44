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
            beardStatus = "수염있음";
        }
        else
        {
            beardStatus = "수염없음";
        }
        if (HumanOutput[0] > 0.9f)
        {
            humanStatus = "사람입니다";
        }
        else
        {
            humanStatus = "사람이 아닙니다";
        }
        if (GlassesOutput[0] > 0.9f)
        {
            glassesStatus = "안경있음";
        }
        else
        {
            glassesStatus = "안경없음";
        }
        
    }
    public void printHumanResult()
    {
        if (HumanOutput[0] > 0.9f)
        {
            resultText.text = "뭐.... 그래도 사람같이 생기긴 했네.";
        }
        else
        {
            resultText.text = "씻고 정돈하는법 정도는 알았으면 좋겠어 아직 사람으로 인식을 못하겠거든";
        }
    }
}
