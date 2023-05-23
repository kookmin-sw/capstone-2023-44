using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;
using UnityEngine;
using Unity.Barracuda;
using Debug = System.Diagnostics.Debug;
using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;
using UnityEditor;
using UnityEngine.Networking;
using TMPro;

public class Text
{
    public string sentence;
}
public class TextAnalysis : MonoBehaviour
{
    // Start is called before the first frame update
    public String text;
    public String textIdx;
    public NNModel modelAsset;
    private Model m_RuntimeModel;
    private IWorker engine;
    public TMP_InputField Contents;
    [Serializable]
    public struct Prediction
    {
        public int predictedValue;
        public float[] predicted;
        
        public void SetPrediction(Tensor t)
        {
            predicted = t.AsFloats();
            predictedValue = Array.IndexOf(predicted, predicted.Max());
            UnityEngine.Debug.Log($"Text predicted index: {predictedValue}");
            
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
        /*int touchCount = Input.touchCount;
        if (Input.GetKeyDown(KeyCode.Space) || touchCount != 0)
        {
            Stopwatch watch = new Stopwatch();
            watch.Start();
            StartCoroutine(Upload());
            IEnumerator Upload()
            {
                Text SendData = new Text
                {
                    sentence = text
                };
                string json = JsonUtility.ToJson(SendData);
                UnityWebRequest www = UnityWebRequest.Post("http://tiglrs.iptime.org:5000/", json);
                byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(json);
                www.uploadHandler = new UploadHandlerRaw(jsonToSend);
                www.downloadHandler = (DownloadHandler) new DownloadHandlerBuffer();
                www.SetRequestHeader("Content-Type", "application/json");
                
                yield return www.SendWebRequest();
                
                if (www.result != UnityWebRequest.Result.Success)
                {
                    UnityEngine.Debug.Log(www.error);
                }
                else
                {
                    textIdx = www.downloadHandler.text;
                }
            }
            textIdx = new List<string>(textIdx.Split(":"))[1];
            textIdx = textIdx.Substring(2, textIdx.Length-6);
            List<String> splitText = new List<string>(textIdx.Split(", "));
            float[] t = Enumerable.Repeat<float>(0, 128).ToArray<float>();
            for (int i = 0; i < splitText.Count; i++)
            {
                t[128 - splitText.Count + i] = Convert.ToInt32(splitText[i]);
            }

            TensorShape shape = new TensorShape(1, 1, 128, 1);
            var inputX = new Tensor(shape, t);
            Tensor outputY = engine.Execute(inputX).PeekOutput();
            inputX.Dispose();
            prediction.SetPrediction(outputY);
            watch.Stop();
            UnityEngine.Debug.Log(watch.ElapsedMilliseconds + " ms");
        }*/
        
    }
    public void EmotionAnalysis(string sentences)
    {

        int touchCount = Input.touchCount;
        Stopwatch watch = new Stopwatch();
        watch.Start();
        StartCoroutine(Upload());
        IEnumerator Upload()
        {

            Text SendData = new Text
            {
                sentence = sentences
            };
            string json = JsonUtility.ToJson(SendData);
            UnityWebRequest www = UnityWebRequest.Post("http://3.105.236.30:5000/", json);
            byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(json);
            www.uploadHandler.Dispose();
            www.uploadHandler = new UploadHandlerRaw(jsonToSend);
            www.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
            www.SetRequestHeader("Content-Type", "application/json");

            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                UnityEngine.Debug.Log(www.error);
            }
            else
            {
                textIdx = www.downloadHandler.text;
            }


            if (www.isDone)
            {
                UnityEngine.Debug.Log(textIdx);
                String[] splitText = textIdx.Substring(15, textIdx.Length - 20).Split(',');

                float[] t = Enumerable.Repeat<float>(0, 128).ToArray<float>();
                for (int i = 0; i < splitText.Length; i++)
                {
                    t[128 - splitText.Length + i] = Convert.ToInt32(splitText[i]);
                }

                TensorShape shape = new TensorShape(1, 1, 128, 1);
                var inputX = new Tensor(shape, t);
                Tensor outputY = engine.Execute(inputX).PeekOutput();
                inputX.Dispose();
                prediction.SetPrediction(outputY);
                watch.Stop();
                UnityEngine.Debug.Log(watch.ElapsedMilliseconds + " ms");
            }
        }
    }
        
        
    
    public void getAnalaysis()
    {
        string content = Contents.text;
        UnityEngine.Debug.Log(content);
        EmotionAnalysis(content);
    }
    private void OnDestroy()
    {
        engine?.Dispose();
        
    }
    
}




