using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;
using UnityEngine;
using Unity.Barracuda;
using Debug = System.Diagnostics.Debug;
using System.Runtime.InteropServices;
using UnityEditor;




public class modelScript : MonoBehaviour
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
        public float[] predicted;
        
        public void SetPrediction(Tensor t)
        {
            predicted = t.AsFloats();
            predictedValue = Array.IndexOf(predicted, predicted.Max());
            UnityEngine.Debug.Log($"predicted {predictedValue}");
            
        }
    }

    public Prediction prediction;
    //public Prediction prediction;
    void Start()
    {   
        m_RuntimeModel = ModelLoader.Load(modelAsset);
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
            var channelCount = 3; //1=grayscale, 3 = color, 4 = color+alpha
            
            
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
}




