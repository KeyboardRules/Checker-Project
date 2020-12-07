using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public Button pause_button;
    public GameObject pause_menu;
    public GameObject player1_title;
    public GameObject player2_title;
    public WinningSummarize winning_summarize;
    public GameObject bot_think;

    public void SetUpPlayerTitle(bool player1)
    {
        if (player1)
        {
            player1_title.SetActive(true);
            player2_title.SetActive(false);
        }
        else
        {
            player1_title.SetActive(false);
            player2_title.SetActive(true);
        }
    }
    public void BotThinking(bool bot)
    {
        if (!bot) bot_think.SetActive(false);
        if (bot) bot_think.SetActive(true);
    }
    public void WinningSummarize(int player1_checker,int player2_checker)
    {
        winning_summarize.gameObject.SetActive(true);
        winning_summarize.SetSummarize(player1_checker, player2_checker);
    }
}
