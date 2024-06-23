# Calcili-v1
A very novice chess bot that plays moves with simple material evaluation functions without opening books ad endgame tablebases. 

## How To Play?
The engine can be played at:
https://www.lichess.org/@/Calcili

Otherwise, you can download the executable (.exe) from the releases. As of now, not available for MacOS and Linux.

### UCI Protocol

Commands:
``` uci ``` : Details about itself and its author. 
``` isready ```: ``` readyok ```
``` ucinewgame ``` : Initializes a new game from the starting position.
``` position fen [fen] ``` : Initializes FEN
``` position startpos moves e2e4 e7e5 ```: Starts game as e2e4, e7e5.
``` quit ```: Terminates the process.

## Special thanks:
ChatGPT (I am new to C#, helped me a lot)
ChessProgramming.wiki
ChessDotNet Library

* Thank You! Do Leave a Star if you liked it! :) *
