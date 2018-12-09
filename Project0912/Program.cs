using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotNet5._12._18
{
	class Program
	{
		static void Main(string[] args)
		{
            bool check = true;
            while (check)
            {
                Console.WriteLine("Enter the size of the board: ");
                int size = int.Parse(Console.ReadLine());
                char[,] board = generateBoard(size);
                populateBoard(board);
                printBoard(board);
                play(board);
                Console.WriteLine("Want to play again? y/n");
                char answer = Console.ReadKey().KeyChar;
                if (answer != 'y' | answer!= 'y')
                {
                    check = false;
                }
                Console.WriteLine();
            }

		}
        
        //creates the game board, fills it with $ sign
		static char[,] generateBoard(int size)
		{
			if (size < 0 | size % 2 != 0 | size > 8)
			{
				return null;
			}
			char[,] board = new char[size, size];
            for (int i = 0; i < board.GetLength(0); i++)
            {
                for (int j = 0; j < board.GetLength(0); j++)
                {
                    board[i, j] = '$';
                }
            }
            return board;
		}
        //end game board creation

        //puts values into the game board
		static void populateBoard(char[,] board)
		{
			string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
			int numberOfPairs = board.GetLength(0)*board.GetLength(0)/2;
			char[] charsToUse = new char[numberOfPairs];

			//Set up
			for(int i = 0; i < charsToUse.Length; i++)
			{
				charsToUse[i] = '$';
			}
			//End Set up

			//Choose the chars to put on the board
			Random rand = new Random();
			for(int i = 0; i < charsToUse.Length; i++)
			{
				bool check = false;
				while (!check)
				{
					char tmp = chars[rand.Next(0, 26)];
					if (!charArrayContains(charsToUse, tmp))
					{
						charsToUse[i] = tmp;
						check = true;
					}
				}
			}
			//End Choose the chars to put on the board

			//Place the chars on the board
			for (int i = 0; i < numberOfPairs; i++)
			{
				for(int j = 0; j < 2; j++)
				{
					bool check = false;
					while (!check)
					{
						int x = rand.Next(0, board.GetLength(0));
						int y = rand.Next(0, board.GetLength(0));
						if(board[y,x] == '$')
						{
							board[y, x] = charsToUse[i];
							check = true;
						}
					}
				}
			}
			//End Place the chars on the board
		}
        //end assigning values to the board

        //sub function of populateBoards - check to see if we already placed this kind of char in the board
		static bool charArrayContains(char[] charsToUse,char charToCheck) {
			for(int i = 0; i < charsToUse.Length; i++)
			{
				if(charsToUse[i] == charToCheck)
				{
					return true;
				}
			}
			return false;
		}
        //end sub function

        //prints the board to the screen - Also if we need to print the board with 2 cards open
        static void printBoard(char[,] board,int[] coordA, int []coordB)
		{
			for(int i = 0; i < board.GetLength(0); i++)
			{
				for(int j = 0; j < board.GetLength(1); j++)
				{
                    //prints the char on this place of the board if it was chosen by the player
					if(((coordA[0]!=-1&& coordA[0] == i) & (coordA[1] != -1 && coordA[1] == j)) |
						((coordB[0] != -1 && coordB[0] == i) & (coordB[1] != -1 && coordB[1] == j)))
					{
						Console.Write(board[i,j]+" ");
					}
                    //prints a $ if this card pair was already found
					else if(board[i,j]=='$')
					{
						Console.Write("$ ");
					}
                    //prints * if this card wasnt found or was asked for
					else
					{
						Console.Write("* ");
					}
				}
				Console.WriteLine();
			}
		}
        //if we need to print the board with 1 card open
		static void printBoard(char[,] board, int[] coordA)
		{
			printBoard(board, coordA, new int[] { -1, -1 });
		}
        //if we need a fresh print board
		static void printBoard(char[,] board)
		{
			printBoard(board,new int[] { -1, -1},new int[] { -1, -1 });
		}

        //actual gameplay
		static void play(char[,] board) 
		{
			int playerOnePoints = 0;
			int playerTwoPoints = 0;

			int turn = 1;

			int numberOfPairs = board.GetLength(0) * board.GetLength(0) / 2;
            //game loop
			while (numberOfPairs > 0)
			{
				Console.WriteLine("Player " + turn + " Choose a card");
				if (!Turn(board))
				{
					Console.WriteLine("not a match");
					turn = 3 - turn;
				}
				else
				{
					Console.WriteLine("found a match!");
                    if(turn == 1)
                    {
                        playerOnePoints++;
                    }
                    else
                    {
                        playerTwoPoints++;
                    }
					numberOfPairs--;
				}
			}
			if (playerOnePoints > playerTwoPoints)
			{
				Console.WriteLine("Game is finished, the winner is player number 1!");
			}
			else if (playerOnePoints < playerTwoPoints) {
				Console.WriteLine("Game is finished, the winner is player number 2!");
			}
			else
			{
				Console.WriteLine("Game is finished, this is a draw!");
			}
			
		}
        //the turn of a player
		static bool Turn(char[,] board)
		{
			int[] firstPick = new int[2];
			for(int i = 0; i < 2; i++)
			{
				Console.WriteLine("Enter x: ");
				int x = int.Parse(Console.ReadLine());
				Console.WriteLine("Enter y: ");
				int y = int.Parse(Console.ReadLine());

                //checks if the coords are legal
                if (!indexInRange(board.GetLength(0), y, x))
                {
                    Console.WriteLine("invalid choice, index is out of range");
                    i--;
                }
                
                //checks if it is the first pick
				else if (i == 0)
				{
					firstPick[0] = y;
					firstPick[1] = x;

                    //checks to see if this card was already found in a pair
					if(board[firstPick[0], firstPick[1]] == '$')
					{
						Console.WriteLine("invalid choice, card is out");
						i--;
					}
					else
					{
						printBoard(board, firstPick);
					}
				}
                
                //if it is the 2nd pick
				else
				{
                    //checks to see if this card was already found in a pair or is the card from the first choice
                    if (board[y, x] == '$' | (firstPick[0] == y & firstPick[1] == x))
					{
						Console.WriteLine("invalid choice, card is out");
						i--;
					}
					else
					{
						printBoard(board, firstPick, new int[] { y, x });
						Console.WriteLine(" first x =" + firstPick[1] + " first y =" + firstPick[0] + " 2nd x =" + x + " 2nd y =" + y);
						Console.WriteLine("values " + board[firstPick[0], firstPick[1]] + "," + board[y, x]);
						if (board[firstPick[0], firstPick[1]] == board[y, x])
						{
							board[firstPick[0], firstPick[1]] = '$';
							board[y, x] = '$';
							return true;
						}
						else
						{
							return false;
						}
					}
				}
				
			}
			return false;
		}
        //checks to see if the chosen coords are legal
        static bool indexInRange(int size,int indexY,int indexX)
        {
            if (indexY >= size|indexX>=size)
            {
                return false;
            }
            return true;
        }
	}
}
