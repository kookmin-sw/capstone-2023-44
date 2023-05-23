using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using UnityEngine.SceneManagement;

public class ImageManager : MonoBehaviour
{   //그림들을 로딩하고 담는 List
    List<string> distinct_ImageList = new List<string>();
    [SerializeField]
    public static List<Texture2D> Image_List = new List<Texture2D>();
    //왼쪽 그림을 위한 셋
    public Image LeftCharacter;
    public RectTransform LeftRect;
    //오른쪽 그림을 위한 셋
    public Image RightCharacter;
    public RectTransform RightRect;
    //가운데 그림을 위한 셋
    public Image CenterCharacter;
    public RectTransform CenterRect;
    //배경을 위한 셋
    public Image BG_Image;
    public RectTransform BG_Rect;
    public ScriptManager SManager;
    Image target_Image;
    RectTransform TargetRect;
    [SerializeField]
    AudioManager Amanager;
    // Start is called before the first frame update
    public IEnumerator ImageEnum = null;
    void Start()
    {
        string activeSceneName = SceneManager.GetActiveScene().name;
       
        
    }
    public void LoadImage() // 이미지 메모리 상주문제떄문에 스크립트를 읽어서 있는 location만 Image_List Texture2D List에 저장
    {
        Image_List.Clear();
        foreach (string str in ScriptManager.Img_List)
        {
            distinct_ImageList.Add(str);
        }
        distinct_ImageList = distinct_ImageList.Distinct().ToList();
        distinct_ImageList.RemoveAll(string.IsNullOrEmpty);
        foreach (string str in distinct_ImageList)
        {
            Texture2D texture;
            Rect rect;
            byte[] byteTexture = System.IO.File.ReadAllBytes(str);
            if (byteTexture.Length > 0)
            {
                texture = new Texture2D(0, 0);
                texture.LoadImage(byteTexture);
                Image_List.Add(texture);
            }
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
    public void ActionImage(string dest_Img, string function, string img_Location, float time, int speedx, int speedy)
    {
        if(function == "")
        {
            return;
        }
        int img_Index = distinct_ImageList.IndexOf(img_Location);

        if (function == "flipping" && ImageEnum == null)
        {
            ImageEnum = ImageFlipping(dest_Img, time);
            StartCoroutine(ImageEnum);
        }
        else if (function == "move" && ImageEnum == null)
        {
            ImageEnum = ImageMove(dest_Img, speedx, speedy, time);
            StartCoroutine(ImageEnum);

        }
        else if (function == "change" && ImageEnum == null)
        {
            //ImageEnum = ImageChange(dest_Img, img_Index);
            ImageEnum = ImageNothing();
            ImageChange(dest_Img, img_Index);
            //StartCoroutine(ImageEnum);
            ClearEnumerator();

        }
        else if (function == "hide" && ImageEnum == null)
        {
            ImageEnum = ImageNothing();
            ImageHide(dest_Img, img_Index);
            //StartCoroutine(ImageEnum);
            ClearEnumerator();

        }
        else if (function == "out" && ImageEnum == null)
        {
            ImageEnum = ImageNothing();
            ImageOut(dest_Img);
            //StartCoroutine(ImageEnum);
            ClearEnumerator();

        }
        else if (function == "bright" && ImageEnum == null)
        {
            ImageEnum = ImageBright(dest_Img);
            StartCoroutine(ImageEnum);
            

        }
        else if (function == "evade" && ImageEnum == null)
        {
            ImageEnum = ImageEvade(dest_Img);
            StartCoroutine(ImageEnum);

        }
        else if (function == "evadeandbright" && ImageEnum == null)
        {
            ImageEnum = ImageEvadeAndBright(dest_Img, img_Index);
            StartCoroutine(ImageEnum);

        }
        else if (function == "show" && ImageEnum == null)
        {
            ImageEnum = ImageNothing();
            ImageShowUp(dest_Img);
            //StartCoroutine(ImageEnum);
            ClearEnumerator();

        }
    }
    IEnumerator ImageFlipping(string dest_img, float time)
    {
        float flippingTime = 0.0f;
        while(flippingTime < time)
        {
            LeftRect.localRotation = Quaternion.Euler(0, flippingTime * (360/time), 0);
            flippingTime += Time.deltaTime;
            yield return new WaitForSeconds(0.02f);
            
        }
        LeftRect.localRotation = Quaternion.Euler(0, 0, 0);

    }
    IEnumerator ImageMove(string dest_img, int speedx, int speedy, float time) // 이미지 움직임
    {
        float moveTime = 0.0f;
        while (moveTime < time)
        {
            LeftRect.anchoredPosition = LeftRect.anchoredPosition + new Vector2(speedx, speedy);
            moveTime += Time.deltaTime;
            yield return new WaitForSeconds(0.02f);

        }



    }
    public void ImageChange(string dest_Img, int img_Index)
    {   
        target_Image = GameObject.Find(dest_Img).GetComponent<Image>();
        TargetRect = GameObject.Find(dest_Img).GetComponent<RectTransform>();
        Texture2D texture = Image_List[img_Index];
        Rect rect;
        if (true)
        {
            //texture = new Texture2D(0, 0);
            //texture.LoadImage(byteTexture);
            rect = new Rect(0, 0, texture.width, texture.height);
            target_Image.sprite = Sprite.Create(texture, rect, new Vector2(0.5f, 0.5f));
            
        }
        Debug.Log("changed");
        return;
    }
    public void ImageHide(string dest_Img, int img_Index) // 투명화된 상태로 타겟 이미지 교체하는 함수
    {
        target_Image = GameObject.Find(dest_Img).GetComponent<Image>();
        TargetRect = GameObject.Find(dest_Img).GetComponent<RectTransform>();
        Texture2D texture = Image_List[img_Index];
        Rect rect;
        if (true)
        {
            //texture = new Texture2D(0, 0);
            //texture.LoadImage(byteTexture);
            rect = new Rect(0, 0, texture.width, texture.height);
            target_Image.sprite = Sprite.Create(texture, rect, new Vector2(0.5f, 0.5f));

        }
        target_Image.color = new Color(0, 0, 0, 1);
        return;
    }
    IEnumerator ImageBright(string dest_Img) // 보이게 만드는기능
    {

        target_Image = GameObject.Find(dest_Img).GetComponent<Image>();
        TargetRect = GameObject.Find(dest_Img).GetComponent<RectTransform>();
        float moveTime = 0.0f;
        while (moveTime < 0.3f)
        {
            target_Image.color = new Color(5*moveTime, 5 * moveTime, 5 * moveTime, 1);
            moveTime += Time.deltaTime;
            yield return new WaitForSeconds(0.02f);

        }
        target_Image.color = new Color(1, 1, 1, 1);
        yield return new WaitForSeconds(0.3f);
        ClearEnumerator();

    }
    IEnumerator ImageEvade(string dest_Img) // 투명화
    {

        target_Image = GameObject.Find(dest_Img).GetComponent<Image>();
        TargetRect = GameObject.Find(dest_Img).GetComponent<RectTransform>();
        float moveTime = 0.0f;
        while (moveTime < 0.3f)
        {
            target_Image.color = new Color(1-(5 * moveTime), 1 - (5 * moveTime), 1 - (5 * moveTime), 1 - (5 * moveTime));
            moveTime += Time.deltaTime;
            yield return new WaitForSeconds(0.02f);

        }
        target_Image.color = new Color(0, 0, 0, 0);
        yield return new WaitForSeconds(0.4f);
        ClearEnumerator();

    }
    IEnumerator ImageEvadeAndBright(string dest_Img, int img_Index) // 검은이미지후에 이미지교체 그리고 다시 보이게끔
    {

        target_Image = GameObject.Find(dest_Img).GetComponent<Image>();
        TargetRect = GameObject.Find(dest_Img).GetComponent<RectTransform>();
        float moveTime = 0.0f;
        while (moveTime < 0.3f)
        {
            target_Image.color = new Color(1 - (5 * moveTime), 1 - (5 * moveTime), 1 - (5 * moveTime), 1);
            moveTime += Time.deltaTime;
            yield return new WaitForSeconds(0.02f);

        }
        target_Image.color = new Color(0, 0, 0, 1);
        ImageHide(dest_Img, img_Index);
        yield return new WaitForSeconds(0.2f);
        moveTime = 0.0f;
        while (moveTime < 0.3f)
        {
            target_Image.color = new Color(5 * moveTime, 5 * moveTime, 5 * moveTime, 1);
            moveTime += Time.deltaTime;
            yield return new WaitForSeconds(0.02f);

        }
        target_Image.color = new Color(1, 1, 1, 1);
        yield return new WaitForSeconds(0.3f);
        ClearEnumerator();

    }
    public void ImageOut(string dest_Img)
    {
        target_Image = GameObject.Find(dest_Img).GetComponent<Image>();
        TargetRect = GameObject.Find(dest_Img).GetComponent<RectTransform>();
        //
        target_Image.color = new Color(0, 0, 0, 0);
        
        return;
    }/*
    IEnumerator ImageShowUp(string dest_Img)
    {
        target_Image = GameObject.Find(dest_Img).GetComponent<Image>();
        TargetRect = GameObject.Find(dest_Img).GetComponent<RectTransform>();
        target_Image.color = new Color(1, 1, 1, 1);
        yield return new WaitForSeconds(0.4f);
        ImageEnum = null;
        yield return null;
    }*/
    public void ImageShowUp(string dest_Img)
    {
        target_Image = GameObject.Find(dest_Img).GetComponent<Image>();
        TargetRect = GameObject.Find(dest_Img).GetComponent<RectTransform>();
        target_Image.color = new Color(1, 1, 1, 1);
        
    }
    IEnumerator ImageNothing()
    {
        yield return new WaitForSeconds(0.4f);
        yield return null;
    }
    void ClearEnumerator()
    {
        ImageEnum = null;
    }
}
