using UnityEngine;
using System.Collections;
/// <summary>
/// ����һ��3D�ı�����3D�����һ������ �ű���������� Quad��������ӽڵ���
/// </summary>
public class Backdrop : MonoBehaviour
{
    Camera thisCamera;
    public float distance = 500;
    GameObject backdrop;
    Mesh mesh;
    float prevDistance;
    Vector3 prevRotation, prevPosition;

    void Start()
    {
        backdrop = transform.Find("BackDrop").gameObject;
        thisCamera = transform.GetComponent<Camera>();

        mesh = backdrop.GetComponent<MeshFilter>().mesh;
        //��������
        mesh.vertices = CalcVerts();
        //uv���� ��Ϊ����������������Ի�100%���� ������ñ�����Ҳ������������uv
        //mesh.uv = new Vector2[] { new Vector2(0, 0), new Vector2(1, 0), new Vector2(0, 1), new Vector2(1, 1) };
        //����������
        mesh.triangles = new int[] { 1, 0, 3, 3, 0, 2 };

    }

    void Update()
    {
        //�������Ƴ����ƶ� / ��ת�����������¼����κ����ݣ�
        if (distance == prevDistance &&
           backdrop.transform.position == prevPosition &&
           backdrop.transform.localEulerAngles == prevRotation
           ) return;

        //���������Զ����ƽ��֮����ñ���ʱ����
        if (distance > thisCamera.farClipPlane)
        {
            Debug.LogError("Backdrop's distance is further than the camera's far clip plane. Extend the camera's far clip plane or reduce the billboard's distance.");
            return;
        }

        //������Ľ�����ƽ��֮ǰ���Ա�������ʱ����
        if (distance < thisCamera.nearClipPlane)
        {
            Debug.LogError("Backdrop's distance is closer than the camera's near clip plane. Extend the distance or reduce the camera's near clip plane.");
            return;
        }


        //���ñ�����λ��
        backdrop.transform.position = transform.forward * distance;

        //��������
        mesh.vertices = CalcVerts();
        mesh.RecalculateNormals();

        //���µ���ֵ
        prevDistance = distance;
        prevPosition = backdrop.transform.position;
        prevRotation = backdrop.transform.localEulerAngles;
    }

    Vector3[] CalcVerts()
    {
        return new Vector3[] {
            backdrop.transform.InverseTransformPoint(thisCamera.ScreenToWorldPoint(new Vector3(0,0,distance))),
            backdrop.transform.InverseTransformPoint(thisCamera.ScreenToWorldPoint(new Vector3(Screen.width,0,distance))),
            backdrop.transform.InverseTransformPoint(thisCamera.ScreenToWorldPoint(new Vector3(0,Screen.height,distance))),
            backdrop.transform.InverseTransformPoint(thisCamera.ScreenToWorldPoint(new Vector3(Screen.width,Screen.height,distance)))
        };
    }
}