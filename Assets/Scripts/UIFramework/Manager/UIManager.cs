using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using LitJson;

public class UIManager  {
    private Dictionary<UIPanelType,string> panelPathDist;//存储所有面板路径
    private Dictionary<UIPanelType, BasePanel> panelDist;//保存所有实例化面板的游戏物体身上的BasePanel组件
    private Stack<BasePanel> panelStack;//栈 先进后出
    /// <summary>
    /// 单例模式
    /// </summary>
    private static UIManager _instance;
    public static UIManager Instance{
        get
        {
            if (_instance == null)
            {
                _instance = new UIManager();
            }
            return _instance;
        }
    }

    private UIManager()
    {
        ParseUIPanelTypeJson();
    }

    private Transform canvasTransform;
    private Transform CanvasTransform
    {
        get
        {
            if (canvasTransform == null)
            {
                canvasTransform = GameObject.Find("Canvas").transform;
            }
            return canvasTransform;
        }
    }

    /// <summary>
    /// 进栈，把页面展示到界面上
    /// </summary>
    /// <param name="panelType"></param>
    public void PushPanel(UIPanelType panelType)
    {
        if (panelStack == null)
        {
            panelStack = new Stack<BasePanel>();
        }

        if(panelStack.Count > 0)
        {
            BasePanel topPanel = panelStack.Peek();
            topPanel.OnPause();
        }

        BasePanel panel = GetPanel(panelType);
        panel.OnEnter();
        panelStack.Push(panel);
    }
    /// <summary>
    /// 出栈，把页面从界面上影藏
    /// </summary>
    /// <param name="panelType"></param>
    public void PopPanel(UIPanelType panelType)
    {
        if (panelStack == null)
        {
            panelStack = new Stack<BasePanel>();
        }
        if (panelStack.Count <= 0)
            return;
        //最上面页面关闭
        BasePanel topPanel = panelStack.Pop();
        topPanel.OnExit();
        if (panelStack.Count <= 0)
            return;
        //继续之前界面
        BasePanel topPanel2 = panelStack.Peek();
        topPanel2.OnResume();
    }

    private BasePanel GetPanel(UIPanelType panelType)
    {
        if (panelDist == null)
        {
            panelDist = new Dictionary<UIPanelType, BasePanel>();
        }

        BasePanel panel;
        //panelDist.TryGetValue(panelType,out panel);
        panel = panelDist.TryGet(panelType);

        if (panel == null)
        {
            string path = panelPathDist.TryGet(panelType);
            GameObject instPanel = GameObject.Instantiate(Resources.Load(path)) as GameObject;
            instPanel.transform.SetParent(CanvasTransform, false);
            panelDist.Add(panelType, instPanel.GetComponent<BasePanel>());
            return instPanel.GetComponent<BasePanel>();
        }
        else
        {
            return panel;
        }
    } 


    private void ParseUIPanelTypeJson()
    {
        Debug.Log("ParseUIPanelTypeJson");
        panelPathDist = new Dictionary<UIPanelType, string>();
        TextAsset ta = Resources.Load<TextAsset>("UIPanelType");

        JsonData jsonObject = JsonMapper.ToObject(ta.text);

        foreach (JsonData info in jsonObject["infoList"])
        {
            //将字符串转成枚举
            UIPanelType type = (UIPanelType)System.Enum.Parse(typeof(UIPanelType), info["panelType"].ToString());

            panelPathDist.Add (type, info["path"].ToString());
        }
      
    }

    

    public void Test()
    {
        string path;
        panelPathDist.TryGetValue(UIPanelType.MainMenu, out path);
        Debug.Log("path  =  "+path);
    }

}
