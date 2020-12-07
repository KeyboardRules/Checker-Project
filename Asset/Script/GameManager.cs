using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public float time_to_prepare;
    public float time_between_turn;
    public UIManager ui;
    public BoardManager board;
    public BotLogic bot;

    enum GameState
    {
        Prepare,
        Pause,
        Playing,
        WaitBetweenTurn,
        Over,
    }
    bool player1_turn=false;
    bool ai_player = true;
    GameState gs;
    private void Start()
    {
        StartCoroutine(Prepare());
    }
    private void Update()
    {
        
    }
    IEnumerator Prepare()
    {
        if (BotLogic.difficulty == 0) ai_player = false;
        gs = GameState.Prepare;
        yield return new WaitForSeconds(time_to_prepare);
        gs = GameState.Playing;
        ui.SetUpPlayerTitle(player1_turn);
        NewTurn();
    }
    public void NexTurn()
    {
        gs = GameState.WaitBetweenTurn;
        if (!CheckerPiece.is_capture)
        {
            if(CheckerPiece.chosen_piece) CheckerPiece.chosen_piece.NewTurn();
        }
        StartCoroutine(WaitForNextTurn());
    }
    IEnumerator WaitForNextTurn()
    {
        yield return new WaitForSeconds(time_between_turn);
        NewTurn();
    }
    public void NewTurn()
    {
        if (!CheckerPiece.is_capture)
        {
            player1_turn = !player1_turn;
        }
        if (board.CountLegalMove(player1_turn) == 0)
        {
            GameOver();
        }
        if (gs!=GameState.Over)
        {
            gs = GameState.Playing;
            ui.SetUpPlayerTitle(player1_turn);
        }
        if (!player1_turn && ai_player && IsPlaying())
        {
            StartCoroutine(MoveBot(player1_turn));
        }
    }
    IEnumerator MoveBot(bool player_turn)
    {
        ui.BotThinking(true);
        yield return new WaitForSeconds(0f);
        Tuple<int, int, int, int> move;
        if (CheckerPiece.is_capture)
        {
            move = bot.GetNextMove(board.GetBoardPosition(), board.GetPlayerChecker(!player_turn), board.GetPlayerChecker(player_turn), CheckerPiece.is_capture, CheckerPiece.chosen_piece.GetPositionX(), CheckerPiece.chosen_piece.GetPositionY());
        }
        else
        {
            move = bot.GetNextMove(board.GetBoardPosition(), board.GetPlayerChecker(!player_turn), board.GetPlayerChecker(player_turn), CheckerPiece.is_capture);
        }
        yield return new WaitForSeconds(0f);
        ui.BotThinking(false);
        board.BotChosePiece(move.Item1, move.Item2);
        board.Move(move.Item1, move.Item2, move.Item3, move.Item4);
    }
    public bool AiPlayer()
    {
        return ai_player;
    }
    void GameOver()
    {
        int player1 = board.CountNumberChecker(true);
        int player2 = board.CountNumberChecker(false);
        if (player1 > player2 && ai_player ||!ai_player)
        {
            FindObjectOfType<AudioManager>().PlaySound("Win");
        }
        if(player1 < player2 && ai_player)
        {
            FindObjectOfType<AudioManager>().PlaySound("Lose");
        }
        gs = GameState.Over;
        ui.WinningSummarize(player1, player2);
    }
    public bool GetTurn()
    {
        return player1_turn;
    }
    public bool IsPlaying()
    {
        if (gs == GameState.Playing)
        {
            return true;
        }
        return false;
    }
    public void PauseGame()
    {
        gs = GameState.Pause;
    }
    public void Resume()
    {
        gs = GameState.Playing;
    }

}
