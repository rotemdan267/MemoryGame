using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MemoryGame
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Which size do you want your board to be?");
            Console.WriteLine("Type \"2\" for 2X2");
            Console.WriteLine("Type \"4\" for 4X4");
            Console.WriteLine("Type \"8\" for 8X8");

            int boardSize = 0;
            while (boardSize == 0)
            {
                int choice = int.Parse(Console.ReadLine());
                if (choice == 2 || choice == 4 || choice == 8)
                {
                    boardSize = choice;
                }
                else
                {
                    Console.WriteLine();
                    Console.WriteLine("The number you typed is incorrect, please select 2, 4 or 8");
                }
            }

            Console.WriteLine();
            Console.WriteLine("How many players want to play?");
            int players = int.Parse(Console.ReadLine());

            int[] points = new int[players];

            int[,] colors = new int[boardSize, boardSize]; // הוספתי מערך מקביל ל"לוח", כאשר האותיות מתחלפות בערך מספרי, על מנת
                                                           // להדפיס אותם בצבע, כמו שייכתב בהמשך
            string[,] board = CreateLettersBoard(boardSize, colors);

            bool[,] printOrNot = new bool[boardSize, boardSize]; // מערך מקביל ל"לוח", כאשר הערך ברירת מחדל
                                                                 // הוא "שקר" אך אם צריך להדפיס תוכן ריבוע מהמערך "לוח", הוא יהיה "אמת"
            int turnsCounter = 0;

            while (PointsSum(points) < boardSize * boardSize / 2)
            {
                Turn(board, boardSize, points, printOrNot, turnsCounter % players, colors);
                turnsCounter++;
            }


            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine();

            int max = 0, index = 0;
            for (int i = 0; i < points.Length; i++)
            {
                if (points[i] > max)
                {
                    max = points[i];  // מוצא מי ה"מנצח"
                    index = i;        // ושומר איזה מס' שחקן ניצח
                }
            }
            int checkForTie = 0; // ...
            for (int i = index + 1; i < points.Length; i++)
            {
                if (points[i] == max)
                {
                    checkForTie++; // מגלה כמה פעמים מופיע הניקוד הגבוה ביותר, מלבד ה"מנצח" שכבר התגלה
                }
            }
            if (checkForTie == 0)
            {
                Console.WriteLine($"Congratulations Player {index + 1}, You are the Winner !!");
            }
            else
            {
                int[] indexes = new int[checkForTie]; 
                int count = 0;
                for (int i = index + 1; i < points.Length; i++)
                {
                    if (points[i] == max)
                    {
                        indexes[count] = i; // ישמור במערך את המס' של כל השחקנים בעלי הניקוד הגבוה, מלבד ה"מנצח"
                        count++;
                    }
                }
                Console.Write("It's a tie between players ");
                Console.Write($"{index + 1}");
                for (int i = 0; i < indexes.Length; i++)
                {
                    if (i == indexes.Length - 1)
                    {
                        Console.Write($" and {indexes[i] + 1}.");
                    }
                    else
                    {
                        Console.Write($", {indexes[i] + 1}");
                    }
                }
                Console.Write(" Maybe you should play again? :)");
            }
        }

        public static void Turn(string[,] board, int boardSize, int[] points, bool[,] printOrNot, int playerNum, int[,] colors)
        {
            int checkForAnotherTurn;
            do
            {
                Console.WriteLine($"Player {playerNum + 1}, press Enter to start your turn");
                string skip = Console.ReadLine();
                if (skip == "0") // אפשרות לדלג על התור. הוספתי בשביל שיהיה יותר נוח לשלוט בניקוד בזמן הרצה
                {                // כדי לבדוק מה קורה כשיש תיקו
                    break;
                }
                checkForAnotherTurn = points[playerNum]; // שומר את הניקוד בתחילת התור
                PrintLettersBoard(board, boardSize, printOrNot, colors);
                int rowFirstChoice, columnFirstChoice, checkForMistake = 0;
                do
                {
                    if (checkForMistake > 0) // יקרה רק אם הלולאה רצה יותר מפעם אחת
                    {
                        Console.WriteLine();
                        Console.WriteLine("You cannot choose this square, it has already been chosen.");
                        Console.WriteLine("Pick another:");
                    }
                    else // יקרה בריצה הראשונה של הלולאה
                    {
                        Console.WriteLine();
                        Console.WriteLine();
                        Console.WriteLine($"Player {playerNum + 1}, pick a square:");
                    }
                    Console.Write("Row: ");
                    rowFirstChoice = int.Parse(Console.ReadLine());
                    rowFirstChoice--;

                    Console.Write("Column: ");
                    columnFirstChoice = int.Parse(Console.ReadLine());
                    columnFirstChoice--;

                    checkForMistake++;
                }
                while (printOrNot[rowFirstChoice, columnFirstChoice]); // אם הערך הוא כבר "אמת" סימן שאותו ריבוע כבר גלוי וצריך לבחור אחר

                printOrNot[rowFirstChoice, columnFirstChoice] = true; // לאחר שהריבוע נבחר הוא יודפס גלוי, לפחות בתור הזה

                Console.WriteLine();
                Console.WriteLine();

                PrintLettersBoard(board, boardSize, printOrNot, colors);

                int rowSecondChoice, columnSecondChoice;
                checkForMistake = 0;
                do
                { 
                    if (checkForMistake > 0) // יקרה רק אם הלולאה רצה יותר מפעם אחת
                    {
                        Console.WriteLine();
                        Console.WriteLine("You cannot choose this square, it has already been chosen.");
                        Console.WriteLine("Pick another:");
                    }
                    else // יקרה בריצה הראשונה של הלולאה
                    {
                        Console.WriteLine();
                        Console.WriteLine();
                        Console.WriteLine("Pick a second square:");
                    }
                    Console.Write("Row: ");
                    rowSecondChoice = int.Parse(Console.ReadLine());
                    rowSecondChoice--;

                    Console.Write("Column: ");
                    columnSecondChoice = int.Parse(Console.ReadLine());
                    columnSecondChoice--;

                    checkForMistake++;
                }
                while (printOrNot[rowSecondChoice, columnSecondChoice]); // אם הערך הוא כבר "אמת" סימן שאותו ריבוע כבר גלוי, בין
                                                                         // אם הוא נבחר כבר בתור הנוכחי או שהתגלה בתורות קודמים וצריך לבחור אחר

                printOrNot[rowSecondChoice, columnSecondChoice] = true;  // לאחר שהריבוע נבחר הוא יודפס גלוי, לפחות בתור הזה

                Console.WriteLine();
                Console.WriteLine();
                PrintLettersBoard(board, boardSize, printOrNot, colors);

                if (board[rowFirstChoice, columnFirstChoice] != board[rowSecondChoice, columnSecondChoice])
                {                       // אם הריבועים לא זהים, צריך להסתיר אותם שוב מכאן והלאה
                    printOrNot[rowFirstChoice, columnFirstChoice] = false;
                    printOrNot[rowSecondChoice, columnSecondChoice] = false;
                }
                else
                {                       // אם הם זהים, הם יישארו "אמת", והשחקן יקבל נקודה
                    points[playerNum]++;
                    if ((PointsSum(points) < boardSize * boardSize / 2)) // נועד למנוע את המשפט הנ"ל בתור האחרון
                    {
                        Console.WriteLine();
                        Console.WriteLine("Well Done!! You earned another turn");
                    }
                }
                Console.WriteLine();
                Console.WriteLine("Points:");
                for (int i = 0; i < points.Length; i++)
                {
                    Console.WriteLine($"Player {i + 1}: {points[i]}");
                }
                Console.WriteLine();
            }
            while (points[playerNum] > checkForAnotherTurn && (PointsSum(points) < boardSize * boardSize / 2)); 
            // אם הניקוד יותר גבוה מהניקוד שהשחקן התחיל איתו את התור זה אומר שהוא צדק
            // בתור הזה ומגיע לו תור נוסף אא"כ כל הלוח התגלה
        }

        public static int PointsSum(int[] points)
        { // מחשב את סך הנקודות של כל השחקנים
            int sum = 0;
            for (int i = 0; i < points.Length; i++)
            {
                sum += points[i];
            }
            return sum;
        }

        public static string[,] CreateLettersBoard(int boardSize, int[,] colors)
        {
            string[,] board = new string[boardSize, boardSize];
            int numOfSymbols = boardSize * boardSize / 2;

            string[] symbols = new string[32]
            {
                "Aa", "Bb", "Cc", "Dd", "Ee", "Ff", "Gg", "Hh",
                "Ii", "Jj", "Kk", "Ll", "Mm", "Nn", "Oo", "Pp",
                "Qq", "Rr", "Ss", "Tt", "Uu", "Vv", "Ww", "Xx",
                "Yy", "Zz", "AB", "CD", "EF", "GH", "IJ", "KL",
            };

            for (int i = 0; i < numOfSymbols; i++)
            {
                PlaceVlaue(board, boardSize, symbols, colors, i);
                PlaceVlaue(board, boardSize, symbols, colors, i);
            }

            return board;
        }

        public static void PlaceVlaue(string[,] board, int boardSize, string[] symbols, int[,] colors, int i)
        {
            Random rand = new Random();
            int ILoc, JLoc; // Loc = Location
            bool flag = false;

            while (!flag) // כל עוד לא נמצא מקום פנוי ולא הוצב ערך חדש...
            {
                ILoc = rand.Next(boardSize);
                JLoc = rand.Next(boardSize);
                if (board[ILoc, JLoc] == null)
                {
                    board[ILoc, JLoc] = symbols[i];
                    colors[ILoc, JLoc] = i;
                    flag = true; // בודק האם היה מקום פנוי עבור הערך הנוכחי והוצב ערך חדש
                }
            }
        }

        public static void PrintLettersBoard(string[,] board, int boardSize, bool[,] printOrNot, int[,] colors)
        {
            int countI = 0, countJ = 0; // מכיוון שלולאות הפור רצות על מספרים גדולים יותר מגבולות
                                        // המערך, בשביל ליצור את הציור של הלוח, יצרתי משתנים ייחודיים במקום
                                        // "i" and "j"
                                        // שירוצו על המערך לפי הסדר
            int count = 1;   // מס' עמודה
            int count2 = 1;  // מס' שורה

            for (int i = 0; i < (boardSize * 3) + 2; i++)
            {
                if (i % 3 == 0 && i != 0)
                {
                    if (count2 < 10) // נכתב כדי לסדר רווחים במידה ומס' השורה הוא דו-ספרתי
                    {                // לא רלוונטי בסופו של דבר, כי שורה מקסימלית היא 8 אבל גם לא מזיק
                        Console.Write($"{count2}  ");
                    }
                    else
                    {
                        Console.Write($"{count2} ");
                    }
                    count2++;
                }
                else
                {
                    Console.Write("   ");
                }
                if (i == 1)
                {
                    Console.Write(" ");
                }
                if (i == 1)
                {// הקו העליון של הלוח יוצא קצר יותר מהקווים האמצעיים כי באמצעיים
                 // מפרידים התווים "|" באמצע הכתיבה של הקו "_" לכן בכתיבה של הקו
                 // העליון צריך להוסיף את מס' התווים האלה "|" בתור "_" ג
                    for (int j = 0; j < boardSize - 1; j++)
                    {
                        Console.Write("_");
                    }
                }
                for (int j = 0; j < boardSize * 6 + 1; j++)
                {
                    if (j % 6 == 0 && i > 1)
                    {
                        Console.Write("|");
                    }
                    if (i == 0 && j % 6 == 3)
                    {
                        if (count < 10) // נכתב כדי לסדר רווחים במידה ומס' העמודה הוא דו-ספרתי
                        {                // לא רלוונטי בסופו של דבר, כי עמודה מקסימלית היא 8 אבל גם לא מזיק
                            Console.Write(" " + count++);
                        }
                        else
                        {
                            Console.Write(count++);
                        }
                    }
                    else if (i % 3 == 1 && j != boardSize * 6)
                    {
                        Console.Write("_");
                    }
                    else if (i % 3 == 0 && j % 6 == 3 && i != 0)
                    {
                        if (printOrNot[countI, countJ]) // אם "אמת" צריך להדפיס בגלוי את ערך הריבוע
                        {
                            if (colors[countI, countJ] % 5 == 0) // כל 5 ערכים ("Aa","Bb","Cc","Dd","Ee"...)
                            {                                    // ייכתבו כל אחד בצבע אחר
                                Console.ForegroundColor = ConsoleColor.Red;
                            }
                            if (colors[countI, countJ] % 5 == 1)
                            {
                                Console.ForegroundColor = ConsoleColor.Green;
                            }
                            if (colors[countI, countJ] % 5 == 2)
                            {
                                Console.ForegroundColor = ConsoleColor.Blue;
                            }
                            if (colors[countI, countJ] % 5 == 3)
                            {
                                Console.ForegroundColor = ConsoleColor.DarkYellow;
                            }
                            if (colors[countI, countJ] % 5 == 4)
                            {
                                Console.ForegroundColor = ConsoleColor.Magenta;
                            }
                            Console.Write(board[countI, countJ]);

                            Console.ForegroundColor = ConsoleColor.Gray; // חזרה לאפור
                        }
                        else // אם "שקר" הריבוע יישאר מוסתר
                        {
                            Console.Write("??");
                        }
                        countJ++;
                        if (countJ == boardSize) // לאחר שסיימנו את כל העמודות בשורה
                        {
                            countI++;            // יורדים שורה
                            countJ = 0;          // ומתחילים מהעמודה הראשונה
                        }
                    }
                    else if (i % 3 == 0 && j % 6 == 1 && i != 0) // חלק מהסידור של הציור
                    { 
                    }
                    else
                    {
                        Console.Write(" ");
                    }
                }
                Console.WriteLine();
            }
        }

    }
}
