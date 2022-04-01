using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UpdateUIText : MonoBehaviour
{
    public TextMeshProUGUI TimeText;
    public TextMeshProUGUI PlayerTurnText;
    public TextMeshProUGUI IsGameStartedText;
    public TextMeshProUGUI ObjectLookedAtText;

    void Start()
    {
        PlayerTurnText.SetText("PlayerTurn: White");
        IsGameStartedText.SetText("IsGameStarted: False");
        ObjectLookedAtText.SetText("ObjectLookedAt: None");
    }

    // Update is called once per frame
    void Update()
    {
        TimeText.SetText("Time: " + DateTime.Now.ToString());
    }
}
