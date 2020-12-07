using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Runtime.CompilerServices;
using UnityEngine;

public class BotLogic : MonoBehaviour
{
    public static int difficulty=3;
    public static void SettingDifficulty(int difficulty)
    {
        BotLogic.difficulty = difficulty;
    }
    public Tuple<int, int, int, int> GetNextMove(int[,] board, List<int[]> player1, List<int[]> player2, bool capture = false,int X=0,int Y = 0)
    {
        if (difficulty == 1)
        {
            return GetEasyMove(board, player2, capture, X, Y);
        }
        else if (difficulty == 2)
        {
            return GetNormalMove(board, player1, player2, capture, X, Y);
        }
        else
        {
            return GetHardMove(board, player1, player2, capture, X, Y);
        }
    }
    Tuple<int, int, int, int> GetEasyMove(int[,] board, List<int[]> player2, bool capture, int X, int Y)
    {
        System.Random rd = new System.Random();
        List<int[]> move_list;
        if (!capture) move_list = GetAllLegalMove(board, player2);
        else move_list = GetCaptureMove(X, Y, board);
        int index = rd.Next(move_list.Count);
        int[] var = (int[])move_list[index];
        return Tuple.Create(var[0], var[1], var[2], var[3]);
    }
    Tuple<int,int,int,int> GetNormalMove(int[,] board, List<int[]> player1, List<int[]> player2, bool capture, int X, int Y)
    {
        Tuple<int, int, int, int, int> best_move=AlphaBeta(board, player1, player2, 3 , int.MinValue, int.MaxValue, true, capture, X, Y);
        //Debug.Log("best move:");
        //Debug.Log(best_move.Item1);
        return Tuple.Create(best_move.Item2, best_move.Item3, best_move.Item4, best_move.Item5);
    }
    Tuple<int,int,int,int> GetHardMove(int[,] board, List<int[]> player1, List<int[]> player2, bool capture, int X, int Y)
    {
        int depthLimit = 15;
        Tuple<int, int, int, int, int> best_move = AlphaBeta(board, player1, player2, 7 , int.MinValue, int.MaxValue, true, capture, X, Y);
        //Debug.Log(best_move.Item1);
        return Tuple.Create(best_move.Item2, best_move.Item3, best_move.Item4, best_move.Item5);
    }
    Tuple<int, int, int, int, int> AlphaBeta(int[,] board,List<int[]> player1, List<int[]> aIplayer, int depth, int alpha, int beta, bool maximazingPlayer, bool capture = false, int X = 0, int Y = 0)
    {
        Tuple<int, int, int, int> best_move = Tuple.Create(0, 0, 0, 0);
        if (maximazingPlayer)
        {
            List<int[]> all_move;
            if (!capture) all_move = GetAllLegalMove(board, aIplayer);
            else all_move = GetCaptureMove(X, Y, board);
            if (all_move.Count == 0 && !capture)
            {
                //Debug.Log(computeUtilityValue(player1, aIplayer));
                return Tuple.Create(computeUtilityValue(player1, aIplayer), 0, 0, 0, 0);
            }
            else if (depth == 0)
            {
               // Debug.Log(computeHeuristic(board, player1, aIplayer));
                return Tuple.Create(computeHeuristic(board, player1, aIplayer), 0, 0, 0, 0);
            }
            
            int maxEval = int.MinValue;
            foreach(int[] move in all_move)
            {
                int eval = 0;
                Tuple<bool, int, int,int,int> turn = VirtualMove(move[0], move[1], move[2], move[3], board,player1,aIplayer);
                if (turn.Item1 == true)
                {
                    eval = AlphaBeta(board, player1, aIplayer, depth, alpha, beta, true,true,turn.Item2,turn.Item3).Item1;
                }
                else
                {
                    eval = AlphaBeta(board, player1, aIplayer, depth - 1, alpha, beta, false,false , turn.Item2, turn.Item3).Item1;
                }
                if (eval > maxEval)
                {
                    best_move = Tuple.Create(move[0], move[1], move[2], move[3]);
                }
                ReverseVirtualMove(move[0], move[1], move[2], move[3], board, player1, aIplayer, turn.Item4, turn.Item5);
                maxEval = Math.Max(maxEval, eval);
                alpha = Math.Max(alpha, eval);
                if (beta <= alpha) break;
            }
            return Tuple.Create(maxEval, best_move.Item1, best_move.Item2, best_move.Item3, best_move.Item4); 
        }
        else
        {
            List<int[]> all_move;
            if (!capture) all_move = GetAllLegalMove(board, player1);
            else all_move = GetCaptureMove(X, Y, board);
            if (all_move.Count == 0 && !capture)
            {
                //Debug.Log(computeUtilityValue(player1, aIplayer));
                return Tuple.Create(computeUtilityValue(player1, aIplayer), 0, 0, 0, 0);
            }
            else if (depth == 0)
            {
                //Debug.Log(computeHeuristic(board, player1, aIplayer));
                return Tuple.Create(computeHeuristic(board, player1, aIplayer), 0, 0, 0, 0);
            }
            
            int minEval = int.MaxValue;
            foreach (int[] move in all_move)
            {
                int eval=0;
                Tuple<bool, int, int,int,int> turn = VirtualMove(move[0], move[1], move[2], move[3], board, player1, aIplayer);
                if (turn.Item1 == true)
                {
                    eval = AlphaBeta(board, player1, aIplayer, depth, alpha, beta, false, true, turn.Item2, turn.Item3).Item1;
                }
                else
                {
                    eval = AlphaBeta(board, player1, aIplayer, depth - 1, alpha, beta, true,false, turn.Item2, turn.Item3).Item1;
                }
                if (eval < minEval)
                {
                    best_move = Tuple.Create(move[0], move[1], move[2], move[3]);
                }
                ReverseVirtualMove(move[0], move[1], move[2], move[3], board, player1, aIplayer, turn.Item4, turn.Item5);
                minEval = Math.Min(minEval, eval);
                beta = Math.Min(beta, eval);
                if (beta <= alpha) break;
            }
            return Tuple.Create(minEval, best_move.Item1, best_move.Item2, best_move.Item3, best_move.Item4);
        }
    }
     Tuple<bool,int,int,int,int> VirtualMove(int fromX, int fromY, int toX, int toY,int[,] board,List<int[]> player1,List<int[]> aIplayer)
     {
        int capture = 0;
        int previousValues = board[fromX, fromY];
        /*Debug.Log("fromX "+fromX);
        Debug.Log("fromY "+fromY);
        Debug.Log("toX "+toX);
        Debug.Log("toY "+toY);*/
        board[toX, toY] = board[fromX, fromY];
        board[fromX, fromY] = 0;
        if (board[toX, toY] > 0) UpdateChecker(player1, fromX, fromY, toX, toY);
        if (board[toX, toY] < 0) UpdateChecker(aIplayer, fromX, fromY, toX, toY);
        for (int i = 0; i < 8; i++)
        {
            if (board[i,7] == 1) board[i,7]++;
            if (board[i, 0] == -1) board[i, 0]--;
        }
        if (Math.Abs(fromX - toX) == 2 && Math.Abs(fromY - toY) == 2)
        {
            capture=Capture((fromX + toX) / 2, (fromY + toY) / 2, board, player1, aIplayer);
            if (GetCaptureMove(toX, toY, board).Count > 0)
            {
                return Tuple.Create(true,toX,toY,previousValues,capture);
            }
            else
            {
                return Tuple.Create(false,toX,toX, previousValues, capture);
            }
        }
        else
        {
            return Tuple.Create(false,toX,toX, previousValues, capture);
        }
    }
    int Capture(int X, int Y,int[,] board, List<int[]> player1, List<int[]> aIplayer)
    {
        int values=0;
        if (board[X, Y]!=0)
        {
            if (board[X, Y] > 0) RemoveChecker(player1, X, Y);
            else RemoveChecker(aIplayer, X, Y);
            values = board[X, Y];
            board[X, Y] = 0;
        }
        return values;
    }
    void ReverseVirtualMove(int fromX,int fromY,int toX,int toY,int[,] board, List<int[]> player1, List<int[]> aIplayer, int previousValues, int captureValue=0)
    {
        board[fromX, fromY] = board[toX, toY];
        board[toX, toY] = 0;
        board[fromX, fromY] = previousValues;
        if (board[fromX, fromY] > 0) UpdateChecker(player1, toX, toY, fromX, fromY);
        if (board[fromX, fromY] < 0) UpdateChecker(aIplayer, toX, toY, fromX, fromY);
        if (Math.Abs(fromX - toX) == 2 && Math.Abs(fromY - toY) == 2)
        {
            if (captureValue > 0) AddChecker(player1, (fromX + toX) / 2, (fromY + toY) / 2);
            if (captureValue < 0) AddChecker(aIplayer, (fromX + toX) / 2, (fromY + toY) / 2);
            board[(fromX + toX) / 2, (fromY + toY) / 2] = captureValue;
        }
    }
    List<int[]> GetLegalMove(int X, int Y,int[,] board)
    {
        List<int[]> all_move = new List<int[]>();
        if (board[X,Y] != 0)
        {
            if (board[X, Y] == 1 || board[X, Y] == 2 || board[X, Y] == -2)
            {
                if (X + 1 < 8 && Y + 1 < 8 && board[X + 1, Y + 1]==0)
                {
                    all_move.Add(new int[] { X, Y ,X + 1, Y + 1 });
                }
                if (X - 1 >= 0 && Y + 1 < 8 && board[X - 1, Y + 1]==0)
                {
                    all_move.Add(new int[] { X, Y , X - 1 ,Y + 1 });
                }
                if (X + 2 < 8 && Y + 2 < 8 && board[X + 2, Y + 2]==0 && board[X + 1, Y + 1] * board[X, Y] < 0)
                {
                    all_move.Add(new int[] { X, Y , X + 2 , Y + 2 });
                }
                if (X - 2 >= 0 && Y + 2 < 8 && board[X - 2, Y + 2] == 0 &&  board[X - 1, Y + 1] * board[X, Y] < 0)
                {
                    all_move.Add(new int[] { X, Y, X - 2, Y + 2 });
                }
            }
            if (board[X, Y] == -1 || board[X, Y] == 2 || board[X, Y] == -2)
            {
                if (X + 1 < 8 && Y - 1 >= 0 && board[X + 1, Y - 1]==0)
                {
                    all_move.Add(new int[] { X, Y, X + 1,  Y-1 });
                }
                if (X - 1 >= 0 && Y - 1 >= 0 && board[X - 1, Y - 1]==0)
                {
                    all_move.Add(new int[] { X, Y, X -1, Y-1 });
                }
                if (X + 2 < 8 && Y - 2 >= 0 && board[X + 2, Y - 2]==0 &&  board[X + 1, Y - 1] * board[X, Y] < 0)
                {
                    all_move.Add(new int[] { X, Y, X + 2, Y-2 });
                }
                if (X - 2 >= 0 && Y - 2 >= 0 && board[X - 2, Y - 2]==0 &&  board[X - 1, Y - 1] * board[X, Y] < 0)
                {
                    all_move.Add(new int[] { X, Y, X  -2, Y-2 });
                }
            }
        }
        return all_move;
    }
    List<int[]> GetCaptureMove(int X, int Y,int[,] board)
    {
        List<int[]> all_move = new List<int[]>();
        if (board[X,Y] != 0)
        {
            if (board[X,Y] == 1 || board[X, Y] == 2 || board[X, Y] == -2)
            {
                if (X + 2 < 8 && Y + 2 < 8 && board[X + 2, Y + 2] == 0 && board[X + 1, Y + 1] * board[X, Y] < 0)
                {
                    all_move.Add(new int[] { X, Y, X + 2, Y + 2 });
                }
                if (X - 2 >= 0 && Y + 2 < 8 && board[X - 2, Y + 2] == 0 && board[X - 1, Y + 1] * board[X, Y] < 0)
                {
                    all_move.Add(new int[] { X, Y, X - 2, Y + 2 });
                }
            }
            if (board[X, Y] == -1 || board[X, Y] == 2 || board[X, Y] == -2)
            {
                if (X + 2 < 8 && Y - 2 >= 0 && board[X + 2, Y - 2] == 0 && board[X + 1, Y - 1] * board[X, Y] < 0)
                {
                    all_move.Add(new int[] { X, Y, X + 2, Y - 2 });
                }
                if (X - 2 >= 0 && Y - 2 >= 0 && board[X - 2, Y - 2] == 0 && board[X - 1, Y - 1] * board[X, Y] < 0)
                {
                    all_move.Add(new int[] { X, Y, X - 2, Y - 2 });
                }
            }
        }
        return all_move;
    }
    List<int[]> GetAllLegalMove(int[,] board, List<int[]> checkers)
    {
        List<int[]> all_move =new List<int[]>();
        foreach (int[] checker in checkers.ToArray())
        {
            all_move.AddRange(GetLegalMove(checker[0], checker[1], board));
        }
        return all_move;
    }
    void RemoveChecker(List<int[]> checkers,int X,int Y)
    {
        foreach(int[] checker in checkers.ToArray())
        {
            if(checker[0]==X && checker[1] == Y)
            {
                checkers.Remove(checker);
            }
        }
    }
    void AddChecker(List<int[]> checkers, int X, int Y)
    {
        checkers.Add(new int[] { X, Y });
    }
    void UpdateChecker(List<int[]> checkers, int fromX, int fromY, int toX, int toY)
    {
        foreach (int[] checker in checkers.ToArray())
        {
            if (checker[0] == fromX && checker[1] == fromY)
            {
                checker[0] = toX;
                checker[1] = toY;
            }
        }
    }
    int CountCheckerSafeAIChecker(int[,] board, List<int[]> ai_checker)
    {
        int count = 0;
        foreach(int[] checker in ai_checker.ToArray())
        {
            if(checker[0]==0|| checker[0] == 7|| checker[1] == 7 || checker[1] == 0)
            {
                count++;
            }
            else if((board[checker[0] - 1, checker[1] - 1] == 0 && board[checker[0] + 1, checker[1] + 1] != 2 || board[checker[0] - 1, checker[1] - 1] * board[checker[0], checker[1]] < 0 && board[checker[0] + 1, checker[1] + 1] != 0 || board[checker[0] + 1, checker[1] + 1]==2 && board[checker[0] - 1, checker[1] - 1] != 0 || board[checker[0] - 1, checker[1] - 1] < 0) &&
                (board[checker[0] + 1, checker[1] - 1] == 0 && board[checker[0] - 1, checker[1] + 1] != 2 || board[checker[0] + 1, checker[1] - 1] * board[checker[0], checker[1]] < 0 && board[checker[0] - 1, checker[1] + 1] != 0 || board[checker[0] - 1, checker[1] + 1]==2 && board[checker[0] + 1, checker[1] - 1] != 0 || board[checker[0] + 1, checker[1] - 1] < 0))
            {
                count++;
            }
        }
        return count;
    }
    int computeUtilityValue(List<int[]> player1, List<int[]> ai_checker)
    {
        int utility = (ai_checker.Count - player1.Count) * 500;
        return utility;
    }
    int computeHeuristic(int[,] board, List<int[]> player1, List<int[]> ai_checker)
    {
        int heurisitc = (ai_checker.Count - player1.Count) * 50 + CountCheckerSafeAIChecker(board, ai_checker) * 10 + ai_checker.Count;
        return heurisitc;
    }
}
