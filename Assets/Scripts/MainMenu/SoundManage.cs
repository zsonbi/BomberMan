using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManage : MonoBehaviour
{
    /// <summary>
    /// Mute and unmute the background sound
    /// </summary>
    /// <param name="muted"></param>
    public void MuteSound(bool muted)
    {
        if (muted)
        {
            AudioListener.volume = 0;
        }
        else
        {
            AudioListener.volume = 0.2f;
        }
    }
}
