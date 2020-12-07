using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.EventSystems;

public class CheckerPiece : MonoBehaviour
{
    public Color default_color;
    public Color chosen_color;
    public Sprite player1_sprite;
    public Sprite player1_king_sprite;
    public Sprite player2_sprite;
    public Sprite player2_king_sprite;
    public float moving_speed=5;
    public static CheckerPiece chosen_piece;
    public static bool is_capture;

    Vector2 local_position;
    int index_positionX;
    int index_positionY;
    int score;
    bool player1;
    private void Start()
    {
        if (score > 0)
        {
            player1 = true;
        }
        if(score<0)
        {
            player1 = false;
        }
    }
    private void Update()
    {
        if ((Vector2)transform.localPosition != local_position)
        {
            transform.localPosition = Vector2.MoveTowards(transform.localPosition, local_position, Time.deltaTime * moving_speed);
            if((Vector2)transform.localPosition == local_position)
            {
                FindObjectOfType<AudioManager>().PlaySound("Checker");
            }
        }
    }
    void UpgradeChecker()
    {
        if (score == 1)
        {
            score++;
            GetComponent<SpriteRenderer>().sprite = player1_king_sprite;
        }
        if(score == -1)
        {
            score--;
            GetComponent<SpriteRenderer>().sprite = player2_king_sprite;
        }
        FindObjectOfType<AudioManager>().PlaySound("Promotion");
    }
    public void SetUpChecker(bool player1,bool upgrade)
    {
        if (player1)
        {
            if (!upgrade)
            {
                GetComponent<SpriteRenderer>().sprite = player1_sprite;
                score = 1;
            }
            else
            {
                GetComponent<SpriteRenderer>().sprite = player1_king_sprite;
                score = 2;
            }
        }
        else
        {
            if (!upgrade)
            {
                GetComponent<SpriteRenderer>().sprite = player2_sprite;
                score = -1;
            }
            else
            {
                GetComponent<SpriteRenderer>().sprite = player2_king_sprite;
                score = -2;
            }
        }
    }
    public void SetIndexPosition(int index_positionX,int index_positionY,Vector2 position)
    {
        local_position = position;
        this.index_positionX = index_positionX;
        this.index_positionY = index_positionY;
    }
    private void OnMouseDown()
    {
        if (player1== FindObjectOfType<GameManager>().GetTurn() && FindObjectOfType<GameManager>().IsPlaying() && !is_capture && !FindObjectOfType<GameManager>().AiPlayer()
            || player1 && FindObjectOfType<GameManager>().GetTurn() && FindObjectOfType<GameManager>().AiPlayer() && FindObjectOfType<GameManager>().IsPlaying() && !is_capture)
        {
            DestroyAllAvaliableMove();
            if (chosen_piece == this)
            {
                GetComponent<SpriteRenderer>().color = default_color;
                chosen_piece = null;
            }
            else
            {
                if (chosen_piece) chosen_piece.GetComponent<SpriteRenderer>().color = default_color;
                GetComponent<SpriteRenderer>().color = chosen_color;
                chosen_piece = this;
                FindObjectOfType<BoardManager>().SetupMove(index_positionX, index_positionY, score);
            }
        }
    }
    public void MoveChecker(Vector2 new_position,int new_indexX,int new_indexY)
    {
        DestroyAllAvaliableMove();
        local_position = new_position;
        index_positionX = new_indexX;
        index_positionY = new_indexY;
        if ((index_positionY == 7 && score == 1)||(index_positionY==0 && score == -1))
        {
            UpgradeChecker();
        }
    }
    public int GetScore()
    {
        return score;
    }
    void DestroyAllAvaliableMove()
    {
        AvaliableMove[] avaliableMoves = FindObjectsOfType<AvaliableMove>();
        foreach(AvaliableMove av in avaliableMoves)
        {
            Destroy(av.gameObject);
        }
    }
    public int GetPositionX()
    {
        return index_positionX;
    }
    public int GetPositionY()
    {
        return index_positionY;
    }
    public void NewTurn()
    {
        if (chosen_piece) chosen_piece.GetComponent<SpriteRenderer>().color = default_color;
       // DestroyAllAvaliableMove();
        chosen_piece = null;
    }
    public bool GetPlayer()
    {
        return player1;
    }
}
