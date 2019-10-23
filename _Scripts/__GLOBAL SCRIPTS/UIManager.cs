using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public enum Notification {
    Success = 1, Info = 2, Warning, Danger, Primary, Secondary, Dark, Light
}

public struct NotificationObject {
    public Notification type;
    public string message;
    public bool show_at_top;

}
public class UIManager : MonoBehaviour {
    // Start is called before the first frame update
    public Animator[] UIPanels;
    public GameObject[] UIPanelText;
    public GameObject[] UIPanelButtons;
    public Animator[] Loaders;
    public static Animator ActiveUIPanel;
    public static Animator ActiveUILoader;
    public Animator WarningPanel;
    public Animator BootstrapAlert;
    public Image BootstrapAlertPanel;
    public TextMeshProUGUI BootstrapAlertText;
    public GameObject WarningText;
    public Image WarningBar;
    public Button WarningCloseButton;
    public Color[] AlertColors;
    public Color[] AlertFontColors;
    public static GameObject ActiveUIPanelButtons;
    public static GameObject ActiveUIPanelText;
    public int ActiveUIPanelIndex;
    public int ActiveUILoaderIndex;

    public static bool popup_open = false;
    public static bool notif_open = false;
    public static bool loadingStop = true;
    public static bool enableClosing = true;
    public delegate void ConfirmEventHandler();
    public delegate void CancelEventHandler();
    public delegate void CloseEventHandler();
    public static event ConfirmEventHandler ConfirmEvent;
    public static event CancelEventHandler CancelEvent;
    public static event CloseEventHandler CloseEvent;
    void Awake() {
        Initiate();
    }

    void Initiate() {
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

    void OpenWarning(string title, string content, Notification alert = Notification.Info) {
        WarningBar.color = AlertColors[(int) alert];
        var _title = WarningText.transform.GetChild(0);
        var _content = WarningText.transform.GetChild(1);
        _title.GetComponent<TextMeshProUGUI>().text = title;
        _content.GetComponent<TextMeshProUGUI>().text = content;
        if (!enableClosing) {
            WarningCloseButton.interactable = false;
            WarningCloseButton.gameObject.SetActive(false);
        } else {
            WarningCloseButton.gameObject.SetActive(true);
            WarningCloseButton.interactable = true;
        }
        StartCoroutine(Warn());
    }

    void ShowNotification(Notification type, string content, bool fromtop = true) {
        BootstrapAlertPanel.color = AlertColors[(int) type];
        BootstrapAlertText.color = AlertFontColors[(int) type];
        BootstrapAlertText.text = content;
        StartCoroutine(Notify(fromtop));
    }

    public void OpenLoader() {
        StartCoroutine(Loading());
    }
    public static void Danger(string content) {
        GameObject.Find("__UIManager").GetComponent<UIManager>().OpenWarning("alert", content, Notification.Danger);
    }

    public static void AlertBox(Notification type, string content, bool _enableClosing = true) {
        enableClosing = _enableClosing;
        GameObject.Find("__UIManager").GetComponent<UIManager>().OpenWarning(type.ToString(), content, type);
    }
    public static void Warning(string content) {
        GameObject.Find("__UIManager").GetComponent<UIManager>().OpenWarning("warning", content, Notification.Warning);
    }

    public static void Notify(Notification alert, string content, bool fromtop = true) {
        GameObject.Find("__UIManager").GetComponent<UIManager>().ShowNotification(alert, content, fromtop);
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
    public void NotifClose() {
        notif_open = false;
    }

    public void DoNothing() {
        //nothing
    }

    IEnumerator Pop(int type, int animationtype) {
        Initiate();
        popup_open = true;
        ActiveUIPanel.SetTrigger("Start");
        ActiveUIPanel.SetInteger("Type", animationtype);
        while (popup_open) {
            yield return null;
        }
        ActiveUIPanel.SetTrigger("End");
    }
    IEnumerator Warn() {
        Initiate();
        popup_open = true;
        WarningPanel.SetTrigger("Start");
        while (popup_open) {
            yield return null;
        }
        WarningPanel.SetTrigger("End");
    }

    IEnumerator Loading() {
        Initiate();
        ActiveUILoader.SetTrigger("Start");
        loadingStop = false;
        while (!loadingStop) {
            yield return null;
        }
        ActiveUILoader.SetTrigger("End");
    }

    IEnumerator Notify(bool fromtop) {
        notif_open = true;
        BootstrapAlert.SetBool("Top", fromtop);
        BootstrapAlert.SetTrigger("Start");
        while (notif_open) {
            yield return null;
        }
        BootstrapAlert.SetTrigger("End");
    }
}