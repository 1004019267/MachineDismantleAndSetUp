using UnityEngine;
using System.Collections;
/// <summary>
/// 设置一块3D的背景在3D物体的一定距离 脚本挂在相机上 Quad放在相机子节点下
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
        //顶点坐标
        mesh.vertices = CalcVerts();
        //uv坐标 因为设置了相机顶点所以会100%看见 如果想让别的相机也看见可以设置uv
        //mesh.uv = new Vector2[] { new Vector2(0, 0), new Vector2(1, 0), new Vector2(0, 1), new Vector2(1, 1) };
        //三角形索引
        mesh.triangles = new int[] { 1, 0, 3, 3, 0, 2 };

    }

    void Update()
    {
        //大于限制尝试移动 / 旋转，则无需重新计算任何内容：
        if (distance == prevDistance &&
           backdrop.transform.position == prevPosition &&
           backdrop.transform.localEulerAngles == prevRotation
           ) return;

        //尝试在相机远剪辑平面之外放置背景时出错
        if (distance > thisCamera.farClipPlane)
        {
            Debug.LogError("Backdrop's distance is further than the camera's far clip plane. Extend the camera's far clip plane or reduce the billboard's distance.");
            return;
        }

        //在相机的近剪辑平面之前尝试背景放置时出错
        if (distance < thisCamera.nearClipPlane)
        {
            Debug.LogError("Backdrop's distance is closer than the camera's near clip plane. Extend the distance or reduce the camera's near clip plane.");
            return;
        }


        //设置背景的位置
        backdrop.transform.position = transform.forward * distance;

        //计算网格
        mesh.vertices = CalcVerts();
        mesh.RecalculateNormals();

        //重新调整值
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