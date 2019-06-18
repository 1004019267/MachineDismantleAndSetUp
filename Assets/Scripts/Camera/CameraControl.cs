using UnityEngine;
using System.Collections;
/// <summary>
/// 相机围绕物体拖拽 拉进拉远 旋转
/// </summary>
public class CameraControl : MonoBehaviour
{
    public float xMinLimit = -360f;
    public float xMaxLimit = 360f;
    public float yMinLimit = 0f;
    public float yMaxLimit = 80f;
    public float xSpeed = 50.0f;
    public Transform target;
    public float distance = 10.0f;
    public float nearLimit = 1.0f;
    public float farLimit = 80.0f;
    public float movSpeedScroll = 5.0f;
    public float movSpeed = 0.005f;
    public float x;
    public float y;

    Quaternion rotation;
    Vector3 position;

    /// <summary>
    /// 是否被点击
    /// </summary>
    private bool bFstTouch;
    private Vector3 vCurPos;
    private Vector3 vOrgPos;
    private float yDis;
    private float xDis;
    private Touch touch1;
    private Touch touch2;
    private Touch touch3;
    private float touchDistance;
    private float lastTouchDistance;
    private float deltaDistance;
    // Update is called once per frame
    public void LateUpdate()
    {
        //如果当前平台是在Windows上的Unity编辑器中、在Windows上的播放器中、在Windows上的网页播放器中
        if (Application.platform == RuntimePlatform.WindowsEditor || Application.platform == RuntimePlatform.WindowsPlayer)
        {

            //右键按下
            if (Input.GetMouseButton(1))
            {
                x += Input.GetAxis("Mouse X") * xSpeed * 0.02f;
                x = ClampAngle(x, xMinLimit, xMaxLimit);
                y -= Input.GetAxis("Mouse Y") * xSpeed * 0.02f;
                y = ClampAngle(y, yMinLimit, yMaxLimit);
            }
            #region 拖拽 
            else if (Input.GetMouseButton(0))
            {

            }//中键按下
            else if (Input.GetMouseButton(2))
            {
                vCurPos = Input.mousePosition;
                if (!bFstTouch)
                {
                    vOrgPos = vCurPos;
                    bFstTouch = true;
                    return;
                }
                if (vOrgPos != vCurPos)
                {
                    yDis = (vCurPos.y - vOrgPos.y) * movSpeed;
                    xDis = (vCurPos.x - vOrgPos.x) * movSpeed;
                    vOrgPos = vCurPos;
                    //记录上一个鼠标位置             
                    //根据目标的旋转和鼠标移动的距离，更改目标的位置
                    Quaternion rot = Quaternion.Euler(y, x, 0);
                    Vector3 pos = rot * new Vector3(-xDis, -yDis, 0) + target.position;
                    target.position = pos;              
                }
            }
            if (Input.GetMouseButtonUp(2))
            {
                bFstTouch = false;
            }
            #endregion
            //鼠标滚轮拉近拉远 
            if (Input.GetAxis("Mouse ScrollWheel") != 0)
            {
                distance -= Input.GetAxis("Mouse ScrollWheel") * movSpeedScroll;
                distance = Mathf.Clamp(distance, nearLimit, farLimit);
            }
        
        }
        else
        {
            #region 触摸屏操作
            int iTouchCount = Input.touchCount;
            switch (iTouchCount)
            {
                case 1:
                    touch1 = Input.GetTouch(0);
                    if (touch1.phase == TouchPhase.Moved)
                    {
                        x += (Input.GetTouch(0).deltaPosition.x) * xSpeed * 0.02f;
                        x = ClampAngle(x, xMinLimit, xMaxLimit);
                        y -= (Input.GetTouch(0).deltaPosition.y) * xSpeed * 0.02f;
                        y = ClampAngle(y, yMinLimit, yMaxLimit);
                    }
                    break;
                case 2:
                    touch1 = Input.GetTouch(0);
                    touch2 = Input.GetTouch(1);
                    if (touch1.phase == TouchPhase.Moved || touch2.phase == TouchPhase.Moved)
                    {
                        touchDistance = (touch1.position - touch2.position).magnitude;
                        lastTouchDistance = ((touch1.position - touch1.deltaPosition) - (touch2.position - touch2.deltaPosition)).magnitude;
                        deltaDistance = lastTouchDistance - touchDistance;

                        distance += deltaDistance * movSpeedScroll;
                        distance = Mathf.Clamp(distance, nearLimit, farLimit);
                    }
                    break;
                case 3:
                    touch1 = Input.GetTouch(0);
                    touch2 = Input.GetTouch(1);
                    touch3 = Input.GetTouch(2);
                    if (touch1.phase == TouchPhase.Moved && touch1.phase == TouchPhase.Moved && touch3.phase == TouchPhase.Moved)
                    {
                        vCurPos = touch3.position;
                        if (!bFstTouch)
                        {
                            vOrgPos = vCurPos;
                            bFstTouch = true;
                            return;
                        }
                        if (vOrgPos != vCurPos)
                        {
                            yDis = (vCurPos.y - vOrgPos.y) * movSpeed;
                            xDis = (vCurPos.x - vOrgPos.x) * movSpeed;
                            vOrgPos = vCurPos;//Record last mouseposition
                            //Change target's position,acording' its rotation and the distance mouse moved
                            Quaternion rot = Quaternion.Euler(y, x, 0);
                            Vector3 pos = rot * new Vector3(-xDis, -yDis, 0) + target.position;
                            target.position = pos;

                        }
                    }
                    break;
                case 0:
                    bFstTouch = false;
                    break;
            }
            #endregion
        }
        //拖拽就不改变其他属性 不然会晃动
        if (!bFstTouch)
        {
            rotation = Quaternion.Euler(y, x, 0);
            //更平滑差值
            transform.rotation = Quaternion.Lerp(transform.rotation, rotation, Time.deltaTime * 10);
            position = rotation * new Vector3(0f, 0f, -distance) + target.position;
            transform.position = Vector3.Lerp(transform.position, position, Time.deltaTime * 10);
        }
    }

    /// <summary>
    /// 把角度限定在360度内
    /// </summary>
    /// <param name="angle"></param>
    /// <param name="min"></param>
    /// <param name="max"></param>
    /// <returns></returns>
    static float ClampAngle(float angle, float min, float max)
    {
        if (angle < -360)
            angle += 360;
        if (angle > 360)
            angle -= 360;
        return Mathf.Clamp(angle, min, max);
    }
}
