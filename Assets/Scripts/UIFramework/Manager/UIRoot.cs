using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIRoot : MonoBehaviour {

    private void Start()
    {
        UIManager.Instance.PushPanel(UIPanelType.MainMenu);
        //UIManager.Instance.Test();
    }
}
