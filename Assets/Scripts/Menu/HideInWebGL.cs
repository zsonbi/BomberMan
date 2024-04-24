using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A class for hiding the game object if the project is builded for WebGL
/// </summary>
public class HideInWebGL : MonoBehaviour
{
    /// <summary>
    /// Start is called before the first frame update
    /// </summary>
    void Start()
    {


#if UNITY_WEBGL
        this.gameObject.SetActive(false);

            
#endif

    }
}
