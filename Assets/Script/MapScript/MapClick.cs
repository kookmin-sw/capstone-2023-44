using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapClick : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            RaycastHit hit = new RaycastHit();

            if (true == (Physics.Raycast(ray.origin, ray.direction * 10, out hit)))   //마우스 근처에 오브젝트가 있는지 확인

            {

                LoadingSceneManager.LoadScene("GameScene");

            }

        }
    }
}
