using System;
using TMPro;
using UnityEngine;

public class ModalWindow : MonoBehaviour
{
    private const int BASE_WIDTH = 310;
    private const int BASE_HEIGHT = 200;

    [SerializeField]
    private TMP_Text ModalTitleText;

    [SerializeField]
    private TMP_Text ModalContentText;

    [SerializeField]
    private TMP_Text ConfirmButtonText;

    private Action onOkAction;
    private Action onCancelAction;

    public void Show()
    {
        this.gameObject.SetActive(true);
    }

    public void Show(string title, string content, Action onOkAction = null)
    {
        this.ModalTitleText.text = title;
        this.ModalContentText.text = content;
        this.onOkAction = onOkAction;
        Show();
    }

    public void Show(string title, string content, Action onOkAction = null,Action onCancelAction=null)
    {
        this.ModalTitleText.text = title;
        this.ModalContentText.text = content;
        this.onOkAction = onOkAction;
        this.onCancelAction = onCancelAction;
        Show();
    }

    public void Show(string title, string content, Action onOkAction = null, Action onCancelAction = null,int width=BASE_WIDTH,int height=BASE_HEIGHT)
    {
        this.ModalTitleText.text = title;
        this.ModalContentText.text = content;
        this.onOkAction = onOkAction;
        this.onCancelAction = onCancelAction;
        
       RectTransform rectTrans= this.gameObject.GetComponent<RectTransform>();
        rectTrans.sizeDelta= new Vector2(width, height);
        Show();
    }

    public void Show(string title, string content, Action onOkAction = null, string confirmButtonLabel = "Ok")
    {
        this.ModalTitleText.text = title;
        this.ModalContentText.text = content;
        this.onOkAction = onOkAction;
        this.ConfirmButtonText.text = confirmButtonLabel;
        Show();
    }


    public void Hide()
    {
        this.gameObject.SetActive(false);
    }

    public void Cancel()
    {
        if (onCancelAction is not null)
        {
            onOkAction();
        }

        Hide();
    }

    public void ConfirmButtonClick()
    {
        if (onOkAction is not null)
        {
            onOkAction();
        }
        Hide();
    }
}