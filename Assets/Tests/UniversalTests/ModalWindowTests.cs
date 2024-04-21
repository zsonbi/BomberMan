using Bomberman;
using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TestTools;
using Menu;
public class ModalWindowTests : MonoBehaviour
{
    private GameObject modalWindowPrefab = Resources.Load<GameObject>("Prefabs/ModalWindowPrefab");

    private ModalWindow modalWindow;

    private bool eventHappened=false;

    private bool cancelEventHappened=false;

    [SetUp]
    public void Init()
    {
        this.modalWindow = GameObject.Instantiate(modalWindowPrefab).GetComponent<ModalWindow>();

    }

    [TearDown]
    public void Shutdown()
    {
        eventHappened=false;
        cancelEventHappened = false;

        if (modalWindow is not null)
            GameObject.Destroy(this.modalWindow.gameObject);
    }

    private void ConfirmEvent()
    {
        eventHappened = true;
    }

    private void CancelEvent()
    {
        cancelEventHappened=true;
    }

    [UnityTest]
    public IEnumerator ModalShowAndConfirmTest()
    {
        modalWindow.Show("xd","content",ConfirmEvent);

        modalWindow.ConfirmButtonClick();
        yield return new WaitForFixedUpdate();

        Assert.IsTrue(eventHappened);
        Assert.IsFalse(cancelEventHappened);

    }

    [UnityTest]
    public IEnumerator ModalShowAndCancelTest()
    {
        modalWindow.Show("xd", "content", ConfirmEvent);

        modalWindow.Cancel();
        yield return new WaitForFixedUpdate();

        Assert.IsFalse(eventHappened);
        Assert.IsFalse (cancelEventHappened);
    }

    [UnityTest]
    public IEnumerator ModalShowAndCancelWithEventTest()
    {
        modalWindow.Show("xd", "content", ConfirmEvent,CancelEvent);

        modalWindow.Cancel();
        yield return new WaitForFixedUpdate();

        Assert.IsFalse(eventHappened);
        Assert.IsTrue(cancelEventHappened);
    }

    [UnityTest]
    public IEnumerator ResizedModal()
    {
        modalWindow.Show("xd", "content", ConfirmEvent, CancelEvent,100,150);

        modalWindow.Cancel();
        yield return new WaitForFixedUpdate();

        Assert.IsFalse(eventHappened);
        Assert.IsTrue(cancelEventHappened);
        RectTransform rectTrans=modalWindow.gameObject.GetComponent<RectTransform>();  
       Assert.AreEqual(100,rectTrans.sizeDelta.x);
       Assert.AreEqual(150,rectTrans.sizeDelta.y);
    }

}
