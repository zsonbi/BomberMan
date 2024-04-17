using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundManage : MonoBehaviour
{
    /// <summary>
    /// Set the toggle is on parameter
    /// </summary>
    public GameObject Toggle;

    private void Start()
    {
        Toggle.GetComponent<Toggle>().isOn = MainMenuConfig.SoundMuted;
    }
    /// <summary>
    /// Mute and unmute the background sound
    /// </summary>
    /// <param name="muted"></param>
    public void MuteSound(bool muted)
    {
        if (muted)
        {
            MainMenuConfig.SoundMuted = true;
            AudioListener.volume = 0;
        }
        else
        {
            MainMenuConfig.SoundMuted = false;
            AudioListener.volume = 0.2f;
        }
        Toggle.GetComponent<Toggle>().isOn = MainMenuConfig.SoundMuted;
    }
}
