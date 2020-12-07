using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BoardManager : MonoBehaviour
{
    public CheckerPiece checker_piece;
    public AvaliableMove avaliable_move;
    Position[,] position_on_board;
    int player1;
    int player2;

    private void Start()
    {
        SetupPosition();
        SetupPlayer();
       // TestSetup4();
        CountPlayerChecker();

    }
    void CountPlayerChecker()
    {
        CheckerPiece[] checkers = FindObjectsOfType<CheckerPiece>();
        foreach (CheckerPiece checker in checkers)
        {
            if (checker.GetScore() > 0) player1++;
            if (checker.GetScore() < 0) player2++;
        }
    }
    public int[,] GetBoardPosition()
    {
        int[,] board = new int[8, 8];
        for (int j = 0; j < 8; j++)
        {
            for (int i = 0; i < 8; i++)
            {
                if (position_on_board[i, j].current_piece != null) board[i, j] = position_on_board[i, j].current_piece.GetScore();
                else board[i, j] = 0;
            }
        }
        return board;
    }
    void TestSetup1()
    {
        CheckerSetup(3, 3, true, false);
        CheckerSetup(3, 5, false, false);
        
    }
    void TestSetup2()
    {
        CheckerPiece checker = Instantiate(checker_piece, transform);
        checker.GetComponent<Transform>().SetParent(gameObject.transform);
        checker.SetIndexPosition(3, 3, position_on_board[3, 3].GetPosition());
        checker.SetUpChecker(true,false);
        position_on_board[3, 3].current_piece = checker;
        CheckerPiece checker5 = Instantiate(this.checker_piece, transform);
        checker5.GetComponent<Transform>().SetParent(gameObject.transform);
        checker5.SetIndexPosition(5,3, position_on_board[5, 3].GetPosition());
        checker5.SetUpChecker(true,false);
        position_on_board[5, 3].current_piece = checker5;
        CheckerPiece checker2 = Instantiate(checker_piece, transform);
        checker2.GetComponent<Transform>().SetParent(gameObject.transform);
        checker2.SetIndexPosition(5, 5, position_on_board[5, 5].GetPosition());
        checker2.SetUpChecker(false,false);
        position_on_board[5, 5].current_piece = checker2;
        CheckerPiece checker3 = Instantiate(checker_piece, transform);
        checker3.GetComponent<Transform>().SetParent(gameObject.transform);
        checker3.SetIndexPosition(3, 5, position_on_board[3, 5].GetPosition());
        checker3.SetUpChecker(false,false);
        position_on_board[3, 5].current_piece = checker3;
    }
    void TestSetup3()
    {
        CheckerSetup(6, 2, true, false);
        CheckerSetup(4, 4, false, true);
        CheckerSetup(3, 5, true, true);
        CheckerSetup(7, 7, false, true);
    }
    void TestSetup4()
    {
        CheckerSetup(1, 1, true, false);
        CheckerSetup(2,2, false, false);
        CheckerSetup(2,4, false, false);
        CheckerSetup(2,6, false, false);
        CheckerSetup(7,7, false, false);
    }
    void SetupPosition()
    {
        position_on_board = new Position[8, 8];
        for (int j = 0; j < 8; j++)
        {
            for (int i = 0; i < 8; i ++)
            {
                position_on_board[i, j] = new Position(new Vector2(-5.5f + 1.62f * i, -5.5f + 1.62f * j), null);
            }
        }
    }
    void SetupPlayer()
    {
        for (int j = 0; j < 3; j++)
        {
            for (int i = 0; i < 8; i++)
            {
                if ((j % 2 == 0 && i % 2 == 0) || (j % 2 != 0 && i % 2 != 0))
                {
                    CheckerSetup(i, j, true,false);
                }
            }
        }
        for(int j = 7; j > 4; j--)
        {
            for(int i = 0; i < 8; i++)
            {
                if ((j % 2 == 0 && i % 2 == 0) || (j % 2 != 0 && i % 2 != 0))
                {
                    CheckerSetup(i, j, false,false);
                }
            }
        }
    }
    void CheckerSetup(int X,int Y,bool player1,bool upgrade)
    {
        CheckerPiece checker = Instantiate(checker_piece, transform);
        checker.GetComponent<Transform>().SetParent(gameObject.transform);
        checker.SetIndexPosition(X, Y, position_on_board[X, Y].GetPosition());
        checker.SetUpChecker(player1,upgrade);
        position_on_board[X, Y].current_piece = checker;
    }
    public void Move(int fromX,int fromY,int toX,int toY)
    {
        position_on_board[toX, toY].current_piece = position_on_board[fromX, fromY].current_piece;
        position_on_board[fromX, fromY].current_piece = null;
        if(CheckerPiece.chosen_piece) CheckerPiece.chosen_piece.MoveChecker(position_on_board[toX, toY].GetPosition(),toX,toY);
        if (Math.Abs(fromX - toX) == 2 && Math.Abs(fromY - toY) == 2)
        {
            Capture((fromX + toX) / 2, (fromY + toY) / 2);
            if (GetCaptureMove(toX, toY).Count > 0)
            {
                SetupMove(toX, toY, position_on_board[toX, toY].current_piece.GetScore(), true);
                CheckerPiece.is_capture = true;
            }
            else
            {
                CheckerPiece.is_capture = false;
            }
        }
        else
        {
            //CheckerPiece.chosen_piece.NewTurn();
        }
        FindObjectOfType<GameManager>().NexTurn();
    }
    public void DestroyAllAvaliableMove()
    {
        AvaliableMove[] av_on_board = FindObjectsOfType<AvaliableMove>();
        foreach(AvaliableMove move in av_on_board)
        {
            Destroy(move);
        }
    }
    public void BotChosePiece(int X,int Y)
    {
        CheckerPiece[] checkers = FindObjectsOfType<CheckerPiece>();
        foreach(CheckerPiece checker in checkers)
        {
            if(checker.GetPositionX()==X && checker.GetPositionY() == Y)
            {
                CheckerPiece.chosen_piece = checker;
                return;
            }
        }
    }
    void Capture(int X,int Y)
    {
        if (position_on_board[X, Y].current_piece)
        {
            if (position_on_board[X, Y].current_piece.GetScore() > 0)
            {
                player1--;
            }
            else 
            {
                player2--;
            }
            Destroy(position_on_board[X, Y].current_piece.gameObject);
            position_on_board[X, Y].current_piece = null;
        }
    }
    ArrayList GetLegalMove(int X,int Y)
    {
        CheckerPiece current_piece = position_on_board[X, Y].current_piece;
        ArrayList all_move = new ArrayList();
        if (current_piece != null)
        {
            if (current_piece.GetScore() == 1 || current_piece.GetScore() == 2 || current_piece.GetScore() == -2)
            {
                if (X + 1 < 8 && Y + 1 < 8 && !position_on_board[X + 1, Y + 1].current_piece)
                {
                    all_move.Add(new int[] { 1, 1 });
                }
                if (X - 1 >= 0 && Y + 1 < 8 && !position_on_board[X - 1, Y + 1].current_piece)
                {
                    all_move.Add(new int[] { -1, 1 });
                }
                if (X + 2 < 8 && Y + 2 < 8 && !position_on_board[X + 2, Y + 2].current_piece && position_on_board[X + 1, Y + 1].current_piece && position_on_board[X + 1, Y + 1].current_piece.GetScore() * position_on_board[X, Y].current_piece.GetScore() < 0)
                {
                    all_move.Add(new int[] { 2, 2 });
                }
                if (X - 2 >= 0 && Y + 2 < 8 && !position_on_board[X - 2, Y + 2].current_piece && position_on_board[X - 1, Y + 1].current_piece && position_on_board[X - 1, Y + 1].current_piece.GetScore() * position_on_board[X, Y].current_piece.GetScore() < 0)
                {
                    all_move.Add(new int[] { -2, 2 });
                }
            }
            if (current_piece.GetScore() == -1 || current_piece.GetScore() == 2 || current_piece.GetScore() == -2)
            {
                if (X + 1 < 8 && Y - 1 >= 0 && !position_on_board[X + 1, Y - 1].current_piece)
                {
                    all_move.Add(new int[] { 1, -1 });
                }
                if (X - 1 >= 0 && Y - 1 >= 0 && !position_on_board[X - 1, Y - 1].current_piece)
                {
                    all_move.Add(new int[] { -1, -1 });
                }
                if (X + 2 < 8 && Y - 2 >= 0 && !position_on_board[X + 2, Y - 2].current_piece && position_on_board[X + 1, Y - 1].current_piece && position_on_board[X + 1, Y - 1].current_piece.GetScore() * position_on_board[X, Y].current_piece.GetScore() < 0)
                {
                    all_move.Add(new int[] { 2, -2 });
                }
                if (X - 2 >= 0 && Y - 2 >= 0 && !position_on_board[X - 2, Y - 2].current_piece && position_on_board[X - 1, Y - 1].current_piece && position_on_board[X - 1, Y - 1].current_piece.GetScore() * position_on_board[X, Y].current_piece.GetScore() < 0)
                {
                    all_move.Add(new int[] { -2, -2 });
                }
            }
        }
        return all_move;
       // Debug.Log(all_move.Count);
    }
    ArrayList GetCaptureMove(int X, int Y)
    {
        CheckerPiece current_piece = position_on_board[X, Y].current_piece;
        ArrayList all_move = new ArrayList();
        if (current_piece != null)
        {
            if (current_piece.GetScore() == 1 || current_piece.GetScore() == 2 || current_piece.GetScore() == -2)
            {
                if (X + 2 < 8 && Y + 2 < 8 && !position_on_board[X + 2, Y + 2].current_piece && position_on_board[X + 1, Y + 1].current_piece && position_on_board[X + 1, Y + 1].current_piece.GetScore() * position_on_board[X, Y].current_piece.GetScore() < 0)
                {
                    all_move.Add(new int[] { 2, 2 });
                }
                if (X - 2 >= 0 && Y + 2 < 8 && !position_on_board[X - 2, Y + 2].current_piece && position_on_board[X - 1, Y + 1].current_piece && position_on_board[X - 1, Y + 1].current_piece.GetScore() * position_on_board[X, Y].current_piece.GetScore() < 0)
                {
                    all_move.Add(new int[] { -2, 2 });
                }
            }
            if (current_piece.GetScore() == -1 || current_piece.GetScore() == 2 || current_piece.GetScore() == -2)
            {
                if (X + 2 < 8 && Y - 2 >= 0 && !position_on_board[X + 2, Y - 2].current_piece && position_on_board[X + 1, Y - 1].current_piece && position_on_board[X + 1, Y - 1].current_piece.GetScore() * position_on_board[X, Y].current_piece.GetScore() < 0)
                {
                    all_move.Add(new int[] { 2, -2 });
                }
                if (X - 2 >= 0 && Y - 2 >= 0 && !position_on_board[X - 2, Y - 2].current_piece && position_on_board[X - 1, Y - 1].current_piece && position_on_board[X - 1, Y - 1].current_piece.GetScore() * position_on_board[X, Y].current_piece.GetScore() < 0)
                {
                    all_move.Add(new int[] { -2, -2 });
                }
            }
        }
        return all_move;
    }
    public int CountLegalMove(bool player1)
    {
        int count=0;
        CheckerPiece[] checkers=FindObjectsOfType<CheckerPiece>();
        foreach(CheckerPiece checker in checkers)
        {
            if((!checker.GetPlayer() && !player1) || (checker.GetPlayer() && player1))
            {
                count += GetLegalMove(checker.GetPositionX(), checker.GetPositionY()).Count;
            }
        }
        return count;
    }
    public void SetupMove(int X, int Y, int player, bool capture = false)
    {
        ArrayList all_move;
        if (!capture) all_move = GetLegalMove(X, Y);
        else all_move = GetCaptureMove(X, Y);
        foreach (int[] move in all_move)
        {
            AvaliableMove avMove= Instantiate(avaliable_move, transform);
            avMove.GetComponent<Transform>().SetParent(transform);
            avMove.gameObject.transform.localPosition = position_on_board[X+move[0], Y + move[1]].GetPosition();
            avMove.SetPosition(X, Y, X + move[0], Y + move[1], player);
        }
    }
    public int CountNumberChecker(bool player1)
    {
        if (player1) return this.player1;
        else return player2;
    }
    public List<int[]> GetPlayerChecker(bool player1)
    {
        List<int[]> all_checker = new List<int[]>();
        CheckerPiece[] checkers = FindObjectsOfType<CheckerPiece>();
        foreach(CheckerPiece checker in checkers)
        {
            if (player1 == checker.GetPlayer())
            {
                all_checker.Add(new int[] { checker.GetPositionX(), checker.GetPositionY() });
            }
        }
        return all_checker;
    }
    [Serializable]
    class Position
    {
        Vector2 position;
        public CheckerPiece current_piece;
        public Position(Vector2 position,CheckerPiece current_piece)
        {
            this.position = position;
            this.current_piece = current_piece;
        }
        public Vector2 GetPosition()
        {
            return position;
        }
    }
}
