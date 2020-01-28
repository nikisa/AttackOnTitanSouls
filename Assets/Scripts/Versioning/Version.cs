using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Version : MonoBehaviour
{
    public Text gameVersion;

    private void Start()
    {
        gameVersion.text = "Build version: " + Application.version;
    }
}
