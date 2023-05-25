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
using UnityEditor.IMGUI.Controls;
using System.Threading;

public class YOLOScriptcam : MonoBehaviour
{
    // Start is called before the first frame update


    public Renderer outputDisplay;
    public Texture2D outputTexture;
    public NNModel modelAsset;

    public int classCount;
    public double confidenceThreshold; // 객체가 있을 확률 기준
    public double IOUThreshold; //중복되는 박스 제거
    public float[] classOutputs;
    

    private Model m_RuntimeModel;
    private IWorker engine;


    WebCamTexture camTexture;
    private int currentIndex = 0;


    private Texture2D cam;


    private float time = 0;


    //public Prediction prediction;
    void Start()
    {
        // Setup WebCam
        if (camTexture != null)
        {
            camTexture.Stop();
            camTexture = null;
        }
        WebCamDevice device = WebCamTexture.devices[currentIndex];
        camTexture = new WebCamTexture(device.name);
        camTexture.Play();

        // set ai model
        m_RuntimeModel = ModelLoader.Load(modelAsset);
        engine = WorkerFactory.CreateWorker(m_RuntimeModel, WorkerFactory.Device.GPU);

        classOutputs = new float[classCount];

        cam = new Texture2D(camTexture.width, camTexture.height);
    }

    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;
        
        if(time > 0.1f)
        {
            time = 0;
            run(camTexture.width, camTexture.height);
            outputDisplay.material.mainTexture = outputTexture;
        }

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

    List<float[]> NMS(List<float[]> box, double threshold)
    {
        for(int i = 0; i < box.Count; i++)
        {
            for(int j = i+1; j < box.Count; j++)
            {

                float leftPoint;
                float rightPoint;
                float upperPoint;
                float lowerPoint;
                float areaA = box[i][2] * box[i][3]; // 박스 a의 넓이
                float areaB = box[j][3] * box[j][3]; // 박스 b의 넓이
                if (box[i][0] - box[i][2] / 2 < box[j][0] - box[j][2] / 2)
                {
                    leftPoint = box[i][0] - box[i][2] / 2;
                }
                else
                {
                    leftPoint = box[j][0] - box[j][2] / 2;
                }

                if (box[i][0] + box[i][2] / 2 > box[j][0] + box[j][2] / 2)
                {
                    rightPoint = box[i][0] - box[i][2] / 2;
                }
                else
                {
                    rightPoint = box[j][0] - box[j][2] / 2;
                }


                if (box[i][1] - box[i][3] / 2 < box[j][1] - box[j][3] / 2)
                {
                    lowerPoint = box[i][1] - box[i][3] / 2;
                }
                else
                {
                    lowerPoint = box[j][1] - box[j][3] / 2;
                }

                if (box[i][1] + box[i][3] / 2 > box[j][1] + box[j][3] / 2)
                {
                    upperPoint = box[i][1] + box[i][3] / 2;
                }
                else
                {
                    upperPoint = box[j][1] + box[j][3] / 2;
                }

                float intersection = (rightPoint - leftPoint) * (upperPoint - lowerPoint); //a, b박스의 겹치는 넓이
                float union = areaA + areaB - intersection;
                if (union > threshold)
                {
                    if (box[i][4] > box[j][4])
                    {
                        box.RemoveAt(j);
                        j--;
                    }
                    else
                    {
                        box.RemoveAt(i);
                        i--;
                    }
                }
            }
        }
        return box;
    }

    Texture2D draw(List<float[]> IOU, int originalwidth, int originalheight, int thickness, Texture2D inputImage)
    {
        Color[] color_list = { Color.red, Color.blue, Color.green, Color.yellow, Color.magenta, Color.clear };

        Texture2D image = new Texture2D(inputImage.width, inputImage.height);
        image.SetPixels32(inputImage.GetPixels32());
        image.Apply();

        double x_scale;
        double y_scale;
        y_scale = (double)originalheight / (double)640;
        x_scale = (double)originalwidth / (double)640;

        for (int i = 0; i < IOU.Count; i++)
        {
            // x, y, width, height, confidence, id
            double x_left, x_right, y_upper, y_lower;
            int colorIndex = 0;
            float prob = 0;
            x_left = (IOU[i][0] - IOU[i][2] / 2) * x_scale;
            x_right = (IOU[i][0] + IOU[i][2] / 2) * x_scale;
            y_upper = (IOU[i][1] + IOU[i][3] / 2) * y_scale;
            y_lower = (IOU[i][1] - IOU[i][3] / 2) * y_scale;

            for (int j = 0; j < classCount; j++)
            {
                if (IOU[i][j + 5] > prob)
                {
                    prob = IOU[i][j + 5];
                    colorIndex = j;
                }
            }

            for (int left = (int)x_left; left < (int)x_right; left++)
            {
                for (int pos = 0; pos < thickness; pos++)
                {

                    image.SetPixel(left, originalheight - ((int)y_upper - pos), color_list[colorIndex]);

                    image.SetPixel(left, originalheight - ((int)y_lower + pos), color_list[colorIndex]);

                }

            }

            for (int up = (int)y_lower; up < (int)y_upper; up++)
            {
                for (int pos = 0; pos < thickness; pos++)
                {

                    image.SetPixel((int)x_left + pos, originalheight - up, color_list[colorIndex]);

                    image.SetPixel((int)x_right - pos, originalheight - up, color_list[colorIndex]);

                }
            }
        }

        image.Apply();
        return image;
    }

    void run(int width, int height)
    {
        List<float[]> boxes = new List<float[]>();
        Stopwatch watch = new Stopwatch();

        // making a tensor out of a grayscale texture 
        var channelCount = 3; //1=grayscale, 3 = colr, 4 = color+alpha
        cam.SetPixels32(camTexture.GetPixels32());
        cam.Apply();

        watch.Start();
        var inputX = new Tensor(Resize(cam, 640, 640), channelCount);
        Tensor outputY = engine.Execute(inputX).PeekOutput();
        //public int Index(int b, int d, int h, int w, int ch)
        inputX.Dispose();


        for (int i = 0; i < 25200; i++)
        {
            // x, y, width, height, confidence, id
            if (outputY[0, 0, 4, i] > confidenceThreshold)
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
            }
        }

        var ordered = boxes.OrderByDescending(y => y[4]);

        boxes = ordered.ToList();
        boxes = NMS(boxes, IOUThreshold);

        outputTexture = draw(boxes, width, height, 2, cam);
        watch.Stop();

        //UnityEngine.Debug.Log(watch.ElapsedMilliseconds + " ms");
    }

}
