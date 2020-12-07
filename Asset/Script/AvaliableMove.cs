using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AvaliableMove : MonoBehaviour
{
    public Sprite player1;
    public Sprite player2;
    int fromX;
    int fromY;
    int toX;
    int toY;
    public void SetPosition(int fromX,int fromY,int toX,int toY,int player)
    {
        this.fromX = fromX;
        this.fromY = fromY;
        this.toX = toX;
        this.toY = toY;
        if (player > 0)
        {
            GetComponent<SpriteRenderer>().sprite = player1;
        }
        if (player < 0)
        {
            GetComponent<SpriteRenderer>().sprite = player2;
        }
    }
    private void OnMouseDown()
    {
        GameManager gm = FindObjectOfType<GameManager>();
        if (gm.IsPlaying() && (gm.AiPlayer() && gm.GetTurn() || !gm.AiPlayer()))
        {
            FindObjectOfType<BoardManager>().Move(fromX, fromY, toX, toY);
        }
    }
}
