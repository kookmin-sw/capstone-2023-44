using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;
using UnityEngine;
using Unity.Barracuda;
using Debug = System.Diagnostics.Debug;


public class YOLOScript : MonoBehaviour
{
// Start is called before the first frame update
    
    public Texture2D texture;
    public NNModel modelAsset;
    private Model m_RuntimeModel;
    private IWorker engine;

    [Serializable]
    public struct Prediction
    {
        public int predictedValue;

        public void SetPrediction(Tensor t)
        {
            predictedValue = Array.IndexOf(t.AsFloats(), t.AsFloats().Max());
            UnityEngine.Debug.Log($"predicted {predictedValue}");
            
        }
    }

    public Prediction prediction;
    //public Prediction prediction;
    void Start()
    {   
        m_RuntimeModel = ModelLoader.Load(modelAsset);

        UnityEngine.Debug.Log("==============");
        engine = WorkerFactory.CreateWorker(m_RuntimeModel, WorkerFactory.Device.GPU);
        prediction = new Prediction();
    }    
    
    // Update is called once per frame
    void Update()
    {
        int touchCount = Input.touchCount;
        if (Input.GetKeyDown(KeyCode.Space) || touchCount != 0)
        {
            Stopwatch watch = new Stopwatch();
            watch.Start();
            // making a tensor out of a grayscale texture 
            var channelCount = 3; //1=grayscale, 3 = colr, 4 = color+alpha
            texture = Resize(texture, 416, 416);
            
            var inputX = new Tensor(texture, channelCount);
            
            
            Tensor outputY = engine.Execute(inputX).PeekOutput();
            inputX.Dispose();
            prediction.SetPrediction(outputY);
            watch.Stop();
            UnityEngine.Debug.Log(watch.ElapsedMilliseconds + " ms");
        }
        
    }

    private void OnDestroy()
    {
        engine?.Dispose();
        
    }
    
    
    Texture2D Resize(Texture2D texture2D,int targetX,int targetY)
    {
        RenderTexture rt=new RenderTexture(targetX, targetY,24);
        RenderTexture.active = rt;
        Graphics.Blit(texture2D,rt);
        Texture2D result=new Texture2D(targetX,targetY);
        result.ReadPixels(new Rect(0,0,targetX,targetY),0,0);
        result.Apply();
        return result;
    }
    
}
