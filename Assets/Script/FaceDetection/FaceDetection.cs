using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;
using UnityEngine;
using Unity.Barracuda;
using Debug = System.Diagnostics.Debug;
using Google.Protobuf.Reflection;
using Unity.Burst.Intrinsics;
using System.Runtime.ExceptionServices;
using UnityEngine.UI;
using B83.Win32;
//using UnityEditor.IMGUI.Controls;
using System.IO;

public class FaceDetection : MonoBehaviour
{
    // Start is called before the first frame update

    public Texture2D texture;
    public Texture2D output;
    public NNModel modelAsset;
    public int classCount;
    public double confidence; // 객체가 있을 확률 기준
    public double minIou; //중복되는 박스 제거
    public float[] classOutputs;
    private Model m_RuntimeModel;
    private IWorker engine;

    //public Prediction prediction;
    void Start()
    {
        m_RuntimeModel = ModelLoader.Load(modelAsset);
        engine = WorkerFactory.CreateWorker(m_RuntimeModel, WorkerFactory.Device.GPU);
        classOutputs = new float[classCount];
    }
    public void FaceDetect()
    {
        //////////////////////////////
        string path = "C:/Users/myPC/Novel/Novel_Data/InputImage/"; // 있는 이미지의 확장자를 추출하는 과정.
        DirectoryInfo directory = new DirectoryInfo(path);
        FileInfo first_File = directory.GetFiles()[0];
        string ext = Path.GetExtension(first_File.Name);
        string dest_file = "C:/Users/myPC/Novel/Novel_Data/InputImage/input0" + ext; // Build된 파일경로에 저장
        byte[] byteTexture = System.IO.File.ReadAllBytes(dest_file);
        if (byteTexture.Length > 0)
        {
            texture = new Texture2D(0, 0);
            texture.LoadImage(byteTexture);
        }
        //////////////////////////////
        Array.Clear(classOutputs, 0, classOutputs.Length);
        int touchCount = Input.touchCount;
        List<float[]> IOU = new List<float[]>();
        List<float[]> boxes = new List<float[]>();
        Stopwatch watch = new Stopwatch();
        // making a tensor out of a grayscale texture
        var channelCount = 3; //1=grayscale, 3 = colr, 4 = color+alpha
        int originalWidth = texture.width;  // 가로
        int originalHeight = texture.height; //세로
        UnityEngine.Debug.Log(originalWidth);
        Texture2D originalImage = new Texture2D(texture.width, texture.height);
        originalImage.SetPixels(texture.GetPixels());
        originalImage.Apply();

        watch.Start();
        texture = Resize(texture, 640, 640);

        var inputX = new Tensor(texture, channelCount);
        Tensor outputY = engine.Execute(inputX).PeekOutput();
        //public int Index(int b, int d, int h, int w, int ch)
        inputX.Dispose();

        for (int i = 0; i < 25200; i++)
        {
            // x, y, width, height, confidence, id
            if (outputY[0, 0, 4, i] > confidence)
            {
                int index = 0;
                float prob = 0;
                boxes.Add(new float[5 + classCount]);
                for (int j = 0; j < 5 + classCount; j++)
                {
                    boxes[boxes.Count - 1][j] = outputY[0, 0, j, i];
                }
                for (int j = 0; j < classCount; j++)
                {
                    if (prob < outputY[0, 0, j + 5, i])
                    {
                        prob = outputY[0, 0, j + 5, i];
                        index = j;
                    }
                }

                classOutputs[index] = prob;

                if (IOU.Count == 0)
                {
                    IOU.Add(boxes[boxes.Count - 1]);
                }
            }
        }

        var ordered = boxes.OrderByDescending(y => y[4]);
        for (int i = 0; i < classCount; i++)
        {
            ordered = ordered.ThenBy(y => y[i + 4]);
        }

        boxes = ordered.ToList();

        for (int i = 0; i < boxes.Count; i++)
        {
            bool isunique = true;
            float IOU_result;
            for (int k = 0; k < IOU.Count; k++)
            {
                IOU_result = CalculateIOU(IOU[k], boxes[i]);
                if (IOU_result > minIou)
                {
                    isunique = false;
                }
            }
            if (isunique)
            {
                IOU.Add(boxes[i]);
            }
        }

        output = draw(IOU, originalWidth, originalHeight, originalImage, Color.red);

        watch.Stop();

        UnityEngine.Debug.Log(watch.ElapsedMilliseconds + " ms");
    }

    // Update is called once per frame
    void Update()
    {
        /*int touchCount = Input.touchCount;
        if (Input.GetKeyDown(KeyCode.Space) || touchCount != 0)
        {
            List<float[]> IOU = new List<float[]>();
            List<float[]> boxes = new List<float[]>();
            Stopwatch watch = new Stopwatch();

            // making a tensor out of a grayscale texture 
            var channelCount = 3; //1=grayscale, 3 = colr, 4 = color+alpha
            int originalWidth = texture.width;  // 가로
            int originalHeight = texture.height; //세로
            UnityEngine.Debug.Log(originalWidth);
            Texture2D originalImage = new Texture2D(texture.width, texture.height);
            originalImage.SetPixels(texture.GetPixels());
            originalImage.Apply();

            watch.Start();
            texture = Resize(texture, 640, 640);
            var inputX = new Tensor(texture, channelCount);
            Tensor outputY = engine.Execute(inputX).PeekOutput();
            //public int Index(int b, int d, int h, int w, int ch)
            inputX.Dispose();


            for (int i = 0; i < 25200; i++)
            {
                // x, y, width, height, confidence, id
                if (outputY[0, 0, 4, i] > confidence)
                {
                    int index = 0;
                    float prob = 0;
                    boxes.Add(new float[5+classCount]);
                    for (int j = 0; j < 5 + classCount; j++)
                    {
                        boxes[boxes.Count-1][j] = outputY[0, 0, j, i];
                    }
                    for (int j = 0; j < classCount; j++)
                    {
                        if(prob < outputY[0, 0, j + 5, i])
                        {
                            prob = outputY[0, 0, j + 5, i];
                            index = j;
                        }
                    }

                    classOutputs[index] = prob;

                    if (IOU.Count == 0)
                    {
                        IOU.Add(boxes[boxes.Count-1]);
                    }
                }
            }

            var ordered = boxes.OrderByDescending(y => y[4]);
            for(int i = 0; i < classCount; i++)
            {
                ordered = ordered.ThenBy(y => y[i + 4]);
            }
            
            boxes = ordered.ToList();

            for (int i = 0;i < boxes.Count;i++)
            {
                bool isunique = true;
                float IOU_result;
                for (int k = 0; k < IOU.Count; k++)
                {
                    IOU_result = CalculateIOU(IOU[k], boxes[i]);
                    if (IOU_result > minIou)
                    {
                        isunique = false;
                    }
                }
                if (isunique)
                {
                    IOU.Add(boxes[i]);
                }
            }

            output = draw(IOU, originalWidth, originalHeight, originalImage, Color.red);

            watch.Stop();

            UnityEngine.Debug.Log(watch.ElapsedMilliseconds + " ms");
        }*/

    }

    private void OnDestroy()
    {
        engine?.Dispose();

    }


    Texture2D Resize(Texture2D texture2D, int targetX, int targetY)
    {
        RenderTexture rt = new RenderTexture(targetX, targetY, 24);
        RenderTexture.active = rt;
        Graphics.Blit(texture2D, rt);
        Texture2D result = new Texture2D(targetX, targetY);
        result.ReadPixels(new Rect(0, 0, targetX, targetY), 0, 0);
        result.Apply();
        return result;
    }

    float CalculateIOU(float[] box1, float[] box2)
    {
        // x, y, width, height, confidence, id
        float left = (box1[0] - box1[2] / 2) - (box2[0] - box2[2] / 2);
        float upper = (box1[1] + box1[3] / 2) - (box2[1] + box2[3] / 2);
        float right = (box1[0] + box1[2] / 2) - (box2[0] + box2[2] / 2);
        float down = (box1[1] - box1[3] / 2) - (box2[1] - box2[3] / 2);

        if(left < 0) {left *= -1;}
        if(right < 0) {right *= -1;} 
        if(upper < 0) {upper *= -1;}
        if(down < 0) {down *= -1;}
        float intersection = (right - left) * (upper - down);
        if (intersection < 0) { intersection *= -1;}
        float union = (box1[2] * box1[3]) + (box2[2] * box2[3]) - intersection;
        return intersection / union;
    }

    Texture2D draw(List<float[]> IOU, int originalwidth, int originalheight, Texture2D image, Color color)
    {
        Color[] color_list = {Color.red, Color.blue, Color.green, Color.yellow, Color.magenta, Color.clear};
        float x_scale;
        float y_scale;
        y_scale = originalheight / 640;
        x_scale = originalwidth / 640;

        for(int i =0; i < IOU.Count; i++)
        {
            // x, y, width, height, confidence, id
            float x_left, x_right, y_upper, y_lower;
            int colorIndex = 0;
            float prob = 0;
            x_left = (IOU[i][0] - IOU[i][2] / 2) * x_scale;
            x_right = (IOU[i][0] + IOU[i][2] / 2) * x_scale;
            y_upper = (IOU[i][1] + IOU[i][3] / 2) * y_scale;
            y_lower= (IOU[i][1] - IOU[i][3]/2) * y_scale;
            for(int j = 0; j < classCount; j++)
            {
                if (IOU[i][j+5] > prob) 
                { 
                    prob = IOU[i][j+5];
                    colorIndex = j; 
                }
            }
            //string text = x_left+ " " + x_right + " " + y_lower + " " + y_upper;
            //UnityEngine.Debug.Log(text);
            for(int left = (int)x_left; left < (int)x_right; left++)
            {
                for (int pos = 0; pos < 3; pos++)
                {

                        image.SetPixel(left, originalheight- ((int)y_upper - pos), color_list[colorIndex]);

                        image.SetPixel(left, originalheight-((int)y_lower + pos), color_list[colorIndex]);

                }
                
            }

            for(int up = (int)y_lower; up < (int)y_upper; up++)
            {
                for (int pos = 0; pos < 3; pos++)
                {

                        image.SetPixel((int)x_left+pos, originalheight-up, color_list[colorIndex]);

                        image.SetPixel((int)x_right-pos, originalheight-up, color_list[colorIndex]);

                }
            }
        }


        image.Apply();
        return image;
    }

}
