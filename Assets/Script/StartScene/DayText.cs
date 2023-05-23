using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class DayText : MonoBehaviour
{
    TextMeshProUGUI text;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void updateDayText()
    {
        text = gameObject.GetComponent<TextMeshProUGUI>();
        text.text = "Day " + DataManager.day_No;
    }
}
