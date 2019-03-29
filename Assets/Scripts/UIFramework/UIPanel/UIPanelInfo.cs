using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class UIPanelInfo : ISerializationCallbackReceiver {
    [NonSerialized]//uinty 自带json不能解析枚举类型
    public UIPanelType panelType;

    public string panelTypeStr;
  
    public string path;
    //反序列化，从文本信息到对象的过程
    public void OnBeforeSerialize()
    {
        UIPanelType type = (UIPanelType)System.Enum.Parse(typeof(UIPanelType), panelTypeStr);
        panelType = type;
        //Debug.Log(panelType);
    }

    public void OnAfterDeserialize()
    {
        throw new NotImplementedException();
    }
}
