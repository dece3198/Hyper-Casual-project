using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public bool isFizzCupMachine = false;
    public int level = 1;
    [SerializeField] private TextMeshProUGUI levelText;

    private void Awake()
    {
        instance = this;
    }

    private void Update()
    {
        levelText.text = level.ToString();
    }
}
