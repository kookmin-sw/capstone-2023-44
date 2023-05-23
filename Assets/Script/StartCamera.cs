using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartCamera : MonoBehaviour
{

    public float ZoomSpeed;         // �� ���ǵ�.
    public float Distance;          // ī�޶���� �Ÿ�.

    private Vector3 AxisVec;        // ���� ����.
    private Transform MainCamera;   // ī�޶� ������Ʈ.

    void Start()
    {
        MainCamera = Camera.main.transform;
    }

    void Update()
    {
        Zoom();
    }

    // ī�޶� ��.
    void Zoom()
    {
        Distance += Input.GetAxis("Mouse ScrollWheel") * ZoomSpeed * -1;
        Distance = Mathf.Clamp(Distance, 5f, 20f);

        AxisVec = transform.forward * -1;
        AxisVec *= Distance;
        MainCamera.position = transform.position + AxisVec;
    }
}
