
using UnityEngine;
using UnityEngine.UI;

namespace Menu
{
    /// <summary>
    /// Manages the game's sound settings
    /// </summary>
    public class SoundManage : MonoBehaviour
    {
        /// <summary>
        /// Set the toggle is on parameter
        /// </summary>
        public GameObject Toggle;

        /// <summary>
        /// Runs on the first frame
        /// </summary>
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
            //Update the button's state
            Toggle.GetComponent<Toggle>().isOn = MainMenuConfig.SoundMuted;
        }
    }
}