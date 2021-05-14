using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class LineStart : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler
{
    /// <summary>
    /// 按下
    /// </summary>
    /// <param name="eventData"></param>
    public void OnPointerDown(PointerEventData eventData)
    {//创建一个3D物体给他添加
        line = new GameObject("LineObject").AddComponent<LineRenderer>();
        line.material = new Material(Shader.Find("Sprites/Default"));
        Property();
        index = 0;
        linelist.Add(line.gameObject);

    }
    /// <summary>
    /// 进入
    /// </summary>
    /// <param name="eventData"></param>
    public void OnPointerEnter(PointerEventData eventData)
    {
        Issmear = true;
    }
    /// <summary>
    /// 退出
    /// </summary>
    /// <param name="eventData"></param>
    public void OnPointerExit(PointerEventData eventData)
    {
        Issmear = false;
    }
    //抬起
    public void OnPointerUp(PointerEventData eventData)
    {
        line = null;
    }
    /// <summary>
    /// 画线组件
    /// </summary>
    LineRenderer line;
    /// <summary>
    /// 顶点数索引
    /// </summary>
    int index = 0;
    /// <summary>
    /// 每次线的位置
    /// </summary>
    Vector3 dir;
    /// <summary>
    /// 是否可以画线，控制是否在画板内部
    /// </summary>
    bool Issmear = false;
    public Button green, red, black, cc, back, backall,quit;
    public Image greenm, redm, blackm;
    public InputField cct;
    Color a = Color.yellow;
    float b = 0.01f;//默认尺寸
    public List<GameObject> linelist = new List<GameObject>();
    Vector3 startpos, endpos;
    int count = 0;

    void Start()
    {
       
        green.onClick.AddListener(() =>
        {
            a = greenm.color;
        });
        red.onClick.AddListener(() =>
        {
            a = redm.color;
        });
        black.onClick.AddListener(() =>
        {
            a = blackm.color;
        });
        cc.onClick.AddListener(() =>
        {
            b = float.Parse(cct.text);
        });
        back.onClick.AddListener(() =>
        {

            //从最后一个清除
            if (linelist.Count > 0)
            {
                Destroy(linelist[linelist.Count - 1]);
                linelist.RemoveAt(linelist.Count - 1);
                print(linelist.Count);
                count = 0;
            }
        });
        backall.onClick.AddListener(() =>
        {

            //全部清除
            if (linelist.Count > 0)
            {
                foreach (var item in linelist)
                {
                    Destroy(item);
                }
                linelist.Clear();
                count = 0;
            }
        });
        quit.onClick.AddListener(() => {
            Application.Quit();
        });
    }
    /// <summary>
    /// 设置属性
    /// </summary>
    void Property()
    {//可以做成 提供选择的方式进行设置
        line.startColor = a;
        line.endColor = a;
        line.startWidth = b;
        line.endWidth = b;
    }

    void Update()
    {//判断画线的游戏对象存在，并且，在画板内，然后进行设置画线的位置，将画线转化为屏幕坐标
        if (line != null && Issmear)
        {
            index++;
            //其实画出来的的线是通过一个个点绘制出来的
            line.positionCount = index;
            //获取画笔在画板里绘制的位置(顶点)的坐标
            dir = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 1));
            line.SetPosition(index - 1, dir);

            startpos = dir;
        }


    }

}
