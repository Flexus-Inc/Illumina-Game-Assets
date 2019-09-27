using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {
    // Start is called before the first frame update
    public Animator[] UIPanels;
    public GameObject[] UIPanelText;
    public GameObject[] UIPanelButtons;

    public static Animator ActiveUIPanel;
    public static GameObject ActiveUIPanelButtons;
    public static GameObject ActiveUIPanelText;
    public int ActiveUIPanelIndex;
    public static bool popup_open = false;
    public delegate void ConfirmEventHandler();
    public delegate void CancelEventHandler();
    public delegate void CloseEventHandler();
    public static event ConfirmEventHandler ConfirmEvent;
    public static event CancelEventHandler CancelEvent;
    public static event CloseEventHandler CloseEvent;
    void Awake() {
        ActiveUIPanel = UIPanels[ActiveUIPanelIndex];
        ActiveUIPanelButtons = UIPanelButtons[ActiveUIPanelIndex];
        ActiveUIPanelText = UIPanelText[ActiveUIPanelIndex];
        ConfirmEvent = DoNothing;
        CancelEvent = DoNothing;
        CloseEvent = DoNothing;
    }

    void OpenPopUp(int type = 0, int animationtype = 0) {
        StartCoroutine(Pop(type, animationtype));
    }

    public static void PopUp(string title, string content, int type = 0, int animationtype = 0) {
        var btn = ActiveUIPanelButtons.transform.GetChild(type);
        btn.gameObject.SetActive(true);
        var _title = ActiveUIPanelText.transform.GetChild(0);
        var _content = ActiveUIPanelText.transform.GetChild(1);
        _title.GetComponent<TextMeshProUGUI>().text = title;
        _content.GetComponent<TextMeshProUGUI>().text = content;
        GameObject.Find("__UIManager").GetComponent<UIManager>().OpenPopUp(type, animationtype);
    }

    public void ClosePopUp() {
        popup_open = false;
    }

    public void PopUpConfirm() {
        ConfirmEvent();
        ConfirmEvent = DoNothing;
    }
    public void PopUpCancel() {
        CancelEvent();
        CancelEvent = DoNothing;
    }
    public void PopUpClose() {
        CloseEvent();
        CloseEvent = DoNothing;
    }

    public void DoNothing() {
        //nothing
    }

    IEnumerator Pop(int type, int animationtype) {
        popup_open = true;
        ActiveUIPanel.SetTrigger("Start");
        ActiveUIPanel.SetInteger("Type", animationtype);
        while (popup_open) {
            yield return null;
        }
        ActiveUIPanel.SetTrigger("End");
    }
}