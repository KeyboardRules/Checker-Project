using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class WinningSummarize : MonoBehaviour
{
    public TextMeshProUGUI winner;
    public TextMeshProUGUI player1_score;
    public TextMeshProUGUI player2_score;
    
    public void SetSummarize(int player1_score,int player2_score)
    {
        this.player1_score.text = player1_score.ToString();
        this.player2_score.text = player2_score.ToString();
        if (player1_score > player2_score)
        {
            winner.text = "Player 1 win";
        }
        else if (player1_score < player2_score)
        {
            winner.text = "Player 2 win";
        }
        else
        {
            winner.text = "Draw !";
        }
    }
}
