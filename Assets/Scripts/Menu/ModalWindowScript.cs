using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Menu
{
    /// <summary>
    /// Modal window controller
    /// </summary>
    public class ModalWindow : MonoBehaviour
    {
        /// <summary>
        /// The window's default width
        /// </summary>
        private const int BASE_WIDTH = 310;
        /// <summary>
        /// The window's default height
        /// </summary>
        private const int BASE_HEIGHT = 200;

        /// <summary>
        /// The title of the modalwindow
        /// </summary>
        [SerializeField]
        private TMP_Text ModalTitleText;

        /// <summary>
        /// The content of the modalwindow
        /// </summary>
        [SerializeField]
        private TMP_Text ModalContentText;

        /// <summary>
        /// The confirm button's text
        /// </summary>
        [SerializeField]
        private TMP_Text ConfirmButtonText;

        /// <summary>
        /// The cancel button
        /// </summary>
        [SerializeField]
        private Button CancelButton;

        /// <summary>
        /// What to do when the ok button is pressed
        /// </summary>
        private Action onOkAction;
        /// <summary>
        /// What to do when the modal is closed
        /// </summary>
        private Action onCancelAction;
 


        /// <summary>
        /// Display the modalwindow without modifying anything
        /// </summary>
        public void Show()
        {


            this.gameObject.SetActive(true);
            this.CancelButton.gameObject.SetActive(true);

        }

        /// <summary>
        /// Displays the modalwindow which modified values
        /// Also this hides the cancel button
        /// </summary>
        /// <param name="title">The title of the modalwindow</param>
        /// <param name="content">The content of the modalwindow</param>
        /// <param name="onOkAction">What to do when the ok action is pressed</param>
        public void Show(string title, string content, Action onOkAction = null)
        {
            this.ModalTitleText.text = title;
            this.ModalContentText.text = content;
            this.onOkAction = onOkAction;
            Show();
            this.CancelButton.gameObject.SetActive(false);

        }

        /// <summary>
        /// Displays the modalwindow which modified values
        /// </summary>
        /// <param name="title">The title of the modalwindow</param>
        /// <param name="content">The content of the modalwindow</param>
        /// <param name="onOkAction">What to do when the ok action is pressed</param>
        /// <param name="onCancelAction">What to do when the cancel action is pressed</param>
        public void Show(string title, string content, Action onOkAction = null, Action onCancelAction = null)
        {
            this.ModalTitleText.text = title;
            this.ModalContentText.text = content;
            this.onOkAction = onOkAction;
            this.onCancelAction = onCancelAction;
            Show();
        }

        /// <summary>
        /// Displays the modalwindow which modified values
        /// </summary>
        /// <param name="title">The title of the modalwindow</param>
        /// <param name="content">The content of the modalwindow</param>
        /// <param name="onOkAction">What to do when the ok action is pressed</param>
        /// <param name="onCancelAction">What to do when the cancel action is pressed</param>
        /// <param name="width">The new width of the window</param>
        /// <param name="height">The new height of the window</param>
        public void Show(string title, string content, Action onOkAction = null, Action onCancelAction = null, int width = BASE_WIDTH, int height = BASE_HEIGHT)
        {
            this.ModalTitleText.text = title;
            this.ModalContentText.text = content;
            this.onOkAction = onOkAction;
            this.onCancelAction = onCancelAction;


            Show();
            this.gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(width, height);

        }

        /// <summary>
        /// Displays the modalwindow which modified values
        /// Also this hides the cancel button
        /// </summary>
        /// <param name="title">The title of the modalwindow</param>
        /// <param name="content">The content of the modalwindow</param>
        /// <param name="onOkAction">What to do when the ok action is pressed</param>
        /// <param name="confirmButtonLabel">Canges the confirm button's text</param>
        public void Show(string title, string content, Action onOkAction = null, string confirmButtonLabel = "Ok")
        {
            this.ModalTitleText.text = title;
            this.ModalContentText.text = content;
            this.onOkAction = onOkAction;
            this.ConfirmButtonText.text = confirmButtonLabel;

            RectTransform confButtonRectTrans = ConfirmButtonText.transform.parent.GetComponent<RectTransform>();
            confButtonRectTrans.sizeDelta = new Vector2(confirmButtonLabel.Length * 12 + 30, confButtonRectTrans.sizeDelta.y);
            Show();
        }

        /// <summary>
        /// Hides the modalwindow
        /// </summary>
        public void Hide()
        {
            this.gameObject.SetActive(false);
        }

        /// <summary>
        /// Cancels the modalwindow
        /// </summary>
        public void Cancel()
        {
            if (onCancelAction is not null)
            {
                onCancelAction();
            }

            Hide();
        }

        /// <summary>
        /// Confirm button event
        /// </summary>
        public void ConfirmButtonClick()
        {
            if (onOkAction is not null)
            {
                onOkAction();
            }
            Hide();
        }
    }
}