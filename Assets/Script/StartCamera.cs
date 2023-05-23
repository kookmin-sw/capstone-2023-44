using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartCamera : MonoBehaviour
{

    public float ZoomSpeed;         // 줌 스피드.
    public float Distance;          // 카메라와의 거리.

    private Vector3 AxisVec;        // 축의 벡터.
    private Transform MainCamera;   // 카메라 컴포넌트.

    void Start()
    {
        MainCamera = Camera.main.transform;
    }

    void Update()
    {
        Zoom();
    }

    // 카메라 줌.
    void Zoom()
    {
        Distance += Input.GetAxis("Mouse ScrollWheel") * ZoomSpeed * -1;
        Distance = Mathf.Clamp(Distance, 5f, 20f);

        AxisVec = transform.forward * -1;
        AxisVec *= Distance;
        MainCamera.position = transform.position + AxisVec;
    }
}
