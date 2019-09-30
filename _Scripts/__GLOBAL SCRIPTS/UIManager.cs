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
    public Animator[] Loaders;
    public static Animator ActiveUIPanel;
    public static Animator ActiveUILoader;
    public Animator WarningPanel;
    public GameObject WarningText;
    public Image WarningBar;
    public Color WarningColor;
    public Color AlertColor;
    public static GameObject ActiveUIPanelButtons;
    public static GameObject ActiveUIPanelText;
    public int ActiveUIPanelIndex;
    public int ActiveUILoaderIndex;

    public static bool popup_open = false;
    public static bool loadingStop = true;
    public static bool enableClosing = true;
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
        ActiveUILoader = Loaders[ActiveUILoaderIndex];
        ConfirmEvent = DoNothing;
        CancelEvent = DoNothing;
        CloseEvent = DoNothing;
    }

    void OpenPopUp(int type = 0, int animationtype = 0) {
        StartCoroutine(Pop(type, animationtype));
    }

    void OpenWarning(string title, string content, bool alert = false) {
        if (alert) {
            WarningBar.color = AlertColor;
        } else {
            WarningBar.color = WarningColor;
        }
        var _title = WarningText.transform.GetChild(0);
        var _content = WarningText.transform.GetChild(1);
        _title.GetComponent<TextMeshProUGUI>().text = title;
        _content.GetComponent<TextMeshProUGUI>().text = content;
        StartCoroutine(Warn());
    }

    public void OpenLoader() {
        StartCoroutine(Loading());
    }
    public static void Alert(string content) {
        GameObject.Find("__UIManager").GetComponent<UIManager>().OpenWarning("alert", content, true);
    }
    public static void Warning(string content) {
        GameObject.Find("__UIManager").GetComponent<UIManager>().OpenWarning("warning", content, false);
    }

    public static void PopUp(string title, string content, bool showButtons = true, int type = 0, int animationtype = 0) {
        enableClosing = showButtons;

        if (showButtons) {
            var btn = ActiveUIPanelButtons.transform.GetChild(type);
            btn.gameObject.SetActive(true);
        }

        var _title = ActiveUIPanelText.transform.GetChild(0);
        var _content = ActiveUIPanelText.transform.GetChild(1);
        _title.GetComponent<TextMeshProUGUI>().text = title;
        _content.GetComponent<TextMeshProUGUI>().text = content;
        GameObject.Find("__UIManager").GetComponent<UIManager>().OpenPopUp(type, animationtype);
    }

    public static void DisplayLoading() {
        GameObject.Find("__UIManager").GetComponent<UIManager>().OpenLoader();
    }

    public static void HideLoading() {
        GameObject.Find("__UIManager").GetComponent<UIManager>().StopLoading();
    }

    public void ClosePopUp() {
        if (enableClosing) {
            popup_open = false;
        }
    }

    public void StopLoading() {
        loadingStop = true;
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
    IEnumerator Warn() {
        popup_open = true;
        WarningPanel.SetTrigger("Start");
        while (popup_open) {
            yield return null;
        }
        WarningPanel.SetTrigger("End");
    }

    IEnumerator Loading() {
        ActiveUILoader.SetTrigger("Start");
        loadingStop = false;
        while (!loadingStop) {
            yield return null;
        }
        ActiveUILoader.SetTrigger("End");
    }
}