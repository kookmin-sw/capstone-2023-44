using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class BlinkIcon : MonoBehaviour
{
    [SerializeField]
    SpriteRenderer icon;
    [SerializeField]
    float Blinktime;
    [SerializeField]
    float EndTime;
    [SerializeField]
    float speedY;
    [SerializeField]
    float magnitutdeX;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Blink());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public IEnumerator Blink()
    {
        while (true)
        {
            icon.color = new Color(1, 1, 1, 1);
            yield return new WaitForSeconds(Blinktime);
            icon.color = new Color(1, 1, 1, 0);
            yield return new WaitForSeconds(EndTime);
        }
    }
    
}
