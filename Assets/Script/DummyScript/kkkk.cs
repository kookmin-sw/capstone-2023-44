using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class kkkk : MonoBehaviour
{
    [SerializeField]
    Image fadeImg;
    // Start is called before the first frame update
    void Start()
    {
        gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public IEnumerator open()
    {
        gameObject.SetActive(true);
        float time = 0.0f;
        while(time < 0.4f)
        {
            fadeImg.color = new Color(0, 0, 0, (float)(time * 2.5));
            time += Time.deltaTime;
            yield return new WaitForSeconds(0.005f);
        }
        StartCoroutine(close());
        yield return null;
        
    }
    public IEnumerator close()
    {
        float time = 0.0f;
        yield return new WaitForSeconds(1f);
        while (time < 0.4f)
        {
            fadeImg.color = new Color(0, 0, 0, 1 - (float)(time * 2.5));
            time += Time.deltaTime;
            yield return new WaitForSeconds(0.005f);
        }
        gameObject.SetActive(false);
        yield return null;
    }
}
