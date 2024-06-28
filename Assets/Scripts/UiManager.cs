using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UiManager : MonoBehaviour
{
    public static UiManager instance;
    public TextMeshProUGUI maxText;

    private void Awake()
    {
        instance = this;
    }
}
