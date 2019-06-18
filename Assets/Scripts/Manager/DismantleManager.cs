using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using DG.Tweening;
public class DismantleManager : MonoBehaviour
{
    Dictionary<int, DismantleStep> stepConfig;
    Dictionary<int, Dictionary<int, DismantleAni>> allAniConfig;

    Dictionary<int, DismantleAniState> aniSta;
    Transform canvas;
    Transform toolsSV;
    Transform btnTools;
    Transform skipSV;
    Transform btnSkip;
    Transform left;

    Button lastBtn;
    /// <summary>
    /// 现在的步骤
    /// </summary>
    int nowStep = 0;
    /// <summary>
    /// 机器
    /// </summary>
    Transform machine;
    void Start()
    {
        ConfigManager<int, DismantleStep>.Instance.AddXml(EXML.StepXml, "Step", NodePathManager.Instance.GetDismantleStepPath("duobilongtou", "Chai"), "ID");
        stepConfig = ConfigManager<int, DismantleStep>.Instance.GetXml(EXML.StepXml);
        ConfigManager<int, DismantleTools>.Instance.AddXml(EXML.StepToolsXml, "Step", NodePathManager.Instance.GetDismantleStepToolsPath("duobilongtou", "Chai"), "id");
        Dictionary<int, DismantleTools> toolsConfig = ConfigManager<int, DismantleTools>.Instance.GetXml(EXML.StepToolsXml);
        ResourcesManager<Sprite>.Instance.ResLoadAll(ELoad.Tools, "Texture/UI/Tools");
        Sprite[] toolsSprite = ResourcesManager<Sprite>.Instance.ResGetAll(ELoad.Tools);

        ConfigManager<int, DismantleAniState>.Instance.AddXml(EXML.StepAniStaXml, "Step", NodePathManager.Instance.GetDismantleStepAllAnimationStatePath("duobilongtou", "Chai"), "ID");
        for (int i = 0; i < stepConfig.Count; i++)
        {
            ConfigManager<int, DismantleAni>.Instance.AddXml(EXML.StepAniXml, "Step", NodePathManager.Instance.GetDismantleStepOneAnimationPath("duobilongtou", "Chai", "0"), "ID");
        }
        allAniConfig = ConfigManager<int, DismantleAni>.Instance.GetAllAniXml();

        aniSta = ConfigManager<int, DismantleAniState>.Instance.GetXml(EXML.StepAniStaXml);
        canvas = GameObject.Find("Canvas").transform;

        LoadTools(toolsSprite, toolsConfig);
        //SkipWnd(stepConfig);
        LeftBtnInit();

        machine = GameObject.Find("jianganzhiji").transform;

        SetTips(stepConfig[nowStep].Info);
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            //打开这两个层
            int layer = (1 << LayerMask.NameToLayer("Machine")) | (1 << LayerMask.NameToLayer("Step"));
            RaycastHit[] hit = Physics.RaycastAll(ray, 100, layer);
            if (hit.Length > 0)
            {
                if (hit.Length == 1)
                {
                    Debug.Log("位置错误");
                    return;
                }
                for (int i = 0; i < hit.Length; i++)
                {
                    if (hit[i].transform.parent.name == "Step" + nowStep)
                    {
                        Dismantle();
                        return;
                    }
                }

                Debug.Log("步骤错误");
                return;
            }
        }
    }
    /// <summary>
    /// 设置拆的一步
    /// </summary>
    public void Dismantle(bool isActive = false, bool isForward = true)
    {
        Transform parent = machine.GetChild(nowStep);

        HightLigthManager.Instance.SetLight(parent.gameObject, Color.blue, Color.cyan, 2.5f);

        AnimationManager.Instance.PlayAniGroup(allAniConfig[nowStep], parent,
            aniSta[nowStep].isEndActiveTogether, aniSta[nowStep].canForward, isForward);
        AnimationManager.Instance.func = () =>
        {
            parent.gameObject.SetActive(isActive);

            HightLigthManager.Instance.SetLightActive(parent.gameObject, false);
        };
    }

    /// <summary>
    /// 加载下方工具
    /// </summary>
    public void LoadTools(Sprite[] toolsSprite, Dictionary<int, DismantleTools> toolsConfig)
    {
        toolsSV = canvas.Find("DismantleWnd/Down/toolsSV/Viewport/Content");
        btnTools = canvas.Find("DismantleWnd/Down/toolsSV/Viewport/Button");
        for (int i = 0; i < toolsSprite.Length; i += 2)
        {
            Transform go = Instantiate(btnTools);
            go.localScale = Vector3.one;

            go.GetComponent<Image>().sprite = toolsSprite[i];
            SpriteState ss = new SpriteState();
            ss.disabledSprite = toolsSprite[i];
            ss.pressedSprite = toolsSprite[i + 1];
            var btn = go.GetComponent<Button>();
            btn.spriteState = ss;

            btn.onClick.AddListener(() =>
            {
                btn.GetComponent<Image>().sprite = btn.spriteState.pressedSprite;
                //上一个btn登记 把之前Btn置回去
                if (lastBtn)
                    lastBtn.GetComponent<Image>().sprite = lastBtn.spriteState.disabledSprite;

                lastBtn = btn;
            });

            int index = int.Parse(toolsSprite[i].name.Substring(0, 1));
            go.Find("Text").GetComponent<Text>().text = toolsConfig[index].name;
            go.name = toolsConfig[index].id.ToString();

            go.SetParent(toolsSV, false);
            go.gameObject.SetActive(true);
        }
    }

    /// <summary>
    /// 刷新下方工具为手
    /// </summary>
    public void RefreshDownBtn()
    {
        Button btn = toolsSV.GetComponentInChildren<Button>();
        btn.GetComponent<Image>().sprite = btn.spriteState.pressedSprite;

        if (lastBtn)
            lastBtn.GetComponent<Image>().sprite = lastBtn.spriteState.disabledSprite;

    }

    ///// <summary>
    ///// 加载跳步骤中间的图
    ///// </summary>
    //public void SkipWnd(Dictionary<int, DismantleStep> stepConfig)
    //{
    //    Transform skipWnd = canvas.Find("DismantleWnd/Mid/skipWnd");
    //    skipSV = skipWnd.Find("Scroll View/Viewport/Content");
    //    btnSkip = skipWnd.Find("Scroll View/Viewport/Button");
    //    for (int i = 0; i < stepConfig.Count; i++)
    //    {
    //        Transform go = Instantiate(btnSkip);
    //        go.localScale = Vector3.one;
    //        go.Find("TextNum").GetComponent<Text>().text = stepConfig[i].ID.ToString();
    //        go.Find("TextStep").GetComponent<Text>().text = stepConfig[i].Info;

    //        go.SetParent(skipSV, false);
    //        go.gameObject.SetActive(true);
    //    }

    //    UISVMove svM = new UISVMove(skipSV, 9);
    //    skipWnd.Find("btn/btnClose").GetComponent<Button>().onClick.AddListener(() =>
    //    {
    //        skipWnd.gameObject.SetActive(false);
    //        svM.ClearTimes();
    //    });

    //    skipWnd.Find("btn/btnPrevious").GetComponent<Button>().onClick.AddListener(() =>
    //    {
    //        svM.Up();
    //    });

    //    skipWnd.Find("btn/btnNext").GetComponent<Button>().onClick.AddListener(() =>
    //    {
    //        svM.Down();
    //    });
    //}
    /// <summary>
    /// 设置左边Tips提示
    /// </summary>
    /// <param name="message"></param>
    public void SetTips(string message)
    {
        left.Find("Tips/Text").GetComponent<Text>().text = message;
    }
    /// <summary>
    /// 左边Btn初始化
    /// </summary>
    public void LeftBtnInit()
    {
        left = canvas.Find("DismantleWnd/Left");
        Transform tips = left.Find("Tips");

        ChangeBtnShowName(tips, left.Find("btnShow/Text").GetComponent<Text>());

        left.Find("Tips/btnSkip").GetComponent<Button>().onClick.AddListener(() =>
        {

        });
        left.Find("btnShow").GetComponent<Button>().onClick.AddListener(() =>
        {
            left.Find("Tips").gameObject.SetActive(!tips.gameObject.activeInHierarchy);
            ChangeBtnShowName(tips, left.Find("btnShow/Text").GetComponent<Text>());
        });
        left.Find("btnSubmit").GetComponent<Button>().onClick.AddListener(() =>
        {

        });
        left.Find("btnPreviousStep").GetComponent<Button>().onClick.AddListener(() =>
        {

        });
    }

    /// <summary>
    /// 根据Btn激活失活改变名字Text
    /// </summary>
    /// <param name="btn"></param>
    /// <param name="text"></param>
    public void ChangeBtnShowName(Transform btn, Text text)
    {
        //判断是否激活失活
        if (btn.gameObject.activeInHierarchy)
            text.text = "隐   藏";
        else
            text.text = "显   示";
    }
    /// <summary>
    /// 设置当前步数
    /// </summary>
    /// <param name="num"></param>
    public void ChangeNowStep(int num)
    {
        nowStep += num;
        nowStep = nowStep <= 0 ? 0 : nowStep;
    }
}
