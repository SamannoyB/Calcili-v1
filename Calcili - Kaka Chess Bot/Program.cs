using System;
using System.Data;
using System.Collections.Generic;
using ChessDotNet;
using System.Linq;
using System.Diagnostics;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Reflection.Metadata.Ecma335;

namespace Kaka
{
    class Program
    {
        private string fen1;
        static int Evaluation(string fen)
        {

            int eval = 0;
            for (int i = 0; i < fen.Length; i++)
            {
                if (fen[i] == '/')
                {
                    continue;
                } 

                switch (fen[i])
                {
                    case 'K':
                        eval += 10;
                        // Console.WriteLine("White King");
                        break;
                    case 'Q':
                        eval += 9;
                        // Console.WriteLine("White Queen");
                        break;
                    case 'B':
                        eval += 3;
                        // Console.WriteLine("White Bishop");
                        break;
                    case 'N':
                        eval += 3;
                        // Console.WriteLine("White Knight");
                        break;
                    case 'R':
                        eval += 5;
                        // Console.WriteLine("White Rook");
                        break;
                    case 'P':
                        eval += 1;
                        //  Console.WriteLine("White Pawn");
                        break;
                    case 'k':
                        eval -= 10;
                        // Console.WriteLine("black king");
                        break;
                    case 'q':
                        eval -= 9;
                        // Console.WriteLine("black queen");
                        break;
                    case 'b':
                        eval -= 3;
                        // Console.WriteLine("black bishop");
                        break;
                    case 'n':
                        eval -= 3;
                        // Console.WriteLine("black knight");
                        break;
                    case 'r':
                        eval -= 5;
                        // Console.WriteLine("black rook");
                        break;
                    case 'p':
                        eval -= 1;
                        // Console.WriteLine("black pawn");
                        break;
                }
            }
            return eval;
        }
        public static int Minimax(int depth, bool isMaximizingPlayer, string fen)
        {

            if (depth == 0)
            {
                return Evaluation(fen);
            }

            int boardEval = 0;
            var game = new ChessGame(fen);

            if (isMaximizingPlayer)
            {
                //Console.WriteLine("Minimax in progress for white!");
                IEnumerable<Move> moves = game.GetValidMoves(Player.White);
                int maxEval = -9999;

                foreach (var move in moves)
                {
                    game.MakeMove(move, true);
                    string newFen = game.GetFen();
                    int eval = Minimax(depth - 1,
                                       false,
                                       newFen);
                    game = new ChessGame(fen);
                    maxEval = Math.Max(eval, maxEval);
                } boardEval = maxEval;
            } else
            {
                //Console.WriteLine("Minimax in progress for black!");
                IEnumerable<Move> moves = game.GetValidMoves(Player.Black);
                int minEval = 9999;

                foreach (var move in moves)
                {
                    game.MakeMove(move, true);
                    string newFen = game.GetFen();
                    int eval = Minimax(depth - 1,
                                       true,
                                       newFen);
                    game = new ChessGame(fen);
                    minEval = Math.Min(eval, minEval);
                } boardEval = minEval;
            }
            game = new ChessGame();
            return boardEval;
        }
        public static Move FindBestMove(int depth, bool isMaximizer, string fen)
        {
            var game = new ChessGame(fen);
            Move bestMoveFound = null;
            int bestEval;

            if (isMaximizer)
            {
                IEnumerable<Move> moves = game.GetValidMoves(Player.White);
                int maxEval = -9999;

                foreach (var move in moves)
                {
                    //Console.WriteLine("Find best move for White");
                    game.MakeMove(move, true);
                    string fen2 = game.GetFen();
                    int eval = Minimax(depth - 1,
                                       false,
                                       fen2);
                    if (eval > maxEval)
                    {
                        bestMoveFound = move;
                        maxEval = eval;
                    }
                    game = new ChessGame(fen);
                }
                bestEval = maxEval;
            }
            else
            {
                IEnumerable<Move> moves = game.GetValidMoves(Player.Black);
                int minEval = 9999;

                foreach (var move in moves)
                {
                    //Console.WriteLine("For Black best Move");
                    game.MakeMove(move, true);
                    string fen2 = game.GetFen();
                    int eval = Minimax(depth - 1,
                                       true,
                                       fen2);
                    if (eval < minEval)
                    {
                        bestMoveFound = move;
                        minEval = eval;
                    }
                    game = new ChessGame(fen);
                }
                bestEval = minEval;
            }
            game = new ChessGame();
            return bestMoveFound;
        }
        public void ProcessCommands(string[] input)
        {
            if (input[0] == "uci")
            {
                Console.WriteLine("id name Calcili");
                Console.WriteLine("id author SamannoyB");
                Console.WriteLine("uciok");
            }
            else if (input[0] == "isready")
            {
                Console.WriteLine("readyok");
            }
            else if (input[0] == "position")
            {
                if (input[1] == "startpos")
                {
                    fen1 = "rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1"; // standard starting position
                    if (input.Length > 2 && input[2] == "moves")
                    {
                        var game = new ChessGame(fen1);
                        for (int i = 3; i < input.Length; i++)
                        {
                            string moveStr = input[i];
                            string from = moveStr.Substring(0, 2);
                            string to = moveStr.Substring(2, 2);
                            game.MakeMove(new Move(from, to, game.WhoseTurn), true);
                            game = new ChessGame(game.GetFen());
                        }
                        fen1 = game.GetFen();
                    }
                }
                else
                {
                    fen1 = input[2] + " " + input[3] + " " + input[4] + " " + input[5] + " " + input[6] + " " + input[7];
                }
            }
            else if (input[0] == "ucinewgame")
            {
                fen1 = "rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1";
            }
            else if (input[0] == "go")
            {
                var game = new ChessGame(fen1);
                Player turn = game.WhoseTurn;
                game = new ChessGame();
                if (!string.IsNullOrEmpty(fen1))
                {

                    if (turn == Player.White)
                    {
                        Move bestMove = FindBestMove(3, true, fen1);
                        string[] thisMove = bestMove.ToString().Split('-');
                        Console.WriteLine($"bestmove {thisMove[0].ToLower()}{thisMove[1].ToLower()}");
                    }
                    else
                    {
                        Move bestMove = FindBestMove(3, false, fen1);
                        string[] thisMove = bestMove.ToString().Split('-');
                        Console.WriteLine($"bestmove {thisMove[0].ToLower()}{thisMove[1].ToLower()}");
                    }
                }
                else
                {
                    if (turn == Player.White)
                    {
                        Move bestMove = FindBestMove(3, true, "rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1");
                        string[] thisMove = bestMove.ToString().Split('-');
                        Console.WriteLine($"bestmove {thisMove[0].ToLower()}{thisMove[1].ToLower()}");
                    }
                    else
                    {
                        Move bestMove = FindBestMove(3, false, "rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1");
                        string[] thisMove = bestMove.ToString().Split('-');
                        Console.WriteLine($"bestmove {thisMove[0].ToLower()}{thisMove[1].ToLower()}");
                    }
                }
            }
            else Console.WriteLine(" ");
        }
        public static void Main(string[] args)
        {
            var engine = new Program();
            string inputLine;
            string[] input;

            while ((inputLine = Console.ReadLine()) != "quit")
            {
                input = inputLine.Split(' ');
                engine.ProcessCommands(input);
            }
        }  
    }
}

