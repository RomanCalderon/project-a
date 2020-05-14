﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameUIManager : MonoBehaviour
{
    [SerializeField]
    private Image m_playerLivesMask = null;
    [SerializeField]
    private Text m_playerScoreText = null;
    [SerializeField]
    private Text m_playerLevelTimeText = null;

    private void OnEnable ()
    {
        GameManager.onPlayerStatsChanged += UpdatePlayerStatsUI;
    }

    private void OnDisable ()
    {
        GameManager.onPlayerStatsChanged -= UpdatePlayerStatsUI;
    }

    // Start is called before the first frame update
    void Awake ()
    {
        Debug.Assert ( m_playerLivesMask != null );
        Debug.Assert ( m_playerScoreText != null );
        Debug.Assert ( m_playerLevelTimeText != null );
    }

    private void UpdatePlayerStatsUI ( PlayerStats playerStats )
    {
        m_playerLivesMask.fillAmount = playerStats.Lives / 3f;
        m_playerScoreText.text = playerStats.ScoreLast.ToString ();
        m_playerLevelTimeText.text = playerStats.LevelTime.ToString () + "s";
    }
}
