
using System.Text;

public class GameObject {
    public int[] Position { get; set; }
}

public class Snake : GameObject {

}

public class Arena {
    public string arenaHBorder = "■ ";
    public string arenaVBorder = "■";

    public int[] arenaDimensions = new int[] { 61, 30 };
}

public class SnakeGame {

    private static string arenaHBorder = "■ ";
    private static string arenaVBorder = "■";
    private static string snakeHead = "☺";
    private static string snakeBody = "▣";
    private static string foodIcon = "◎";

    private static int[] arenaDimensions = new int[] { 61, 30 };

    private static float snakeSpeed = 1f;
    private static float maxSpeed = .05f;
    private static int initialSnakeSize = 5;

    private static int[] snakePosition;
    private static Direction currentDirection;
    private static Direction inputDirection;
    private static int snakeSize;

    private static List<int[]> snakeTrail = new List<int[]>();

    private enum Direction {
        UP,
        DOWN,
        LEFT,

        RIGHT
    }

    private static int maxFood = 5;
    private static List<int[]> spawnedFood = new List<int[]>();

    private static bool gameRunning = true;

    private static void InitializeGame() {
        Console.OutputEncoding = Encoding.UTF8;
        Console.CursorVisible = false;

        Console.SetWindowSize(arenaDimensions[0] * arenaHBorder.Length, arenaDimensions[1] + 2);

        IntroMessage();

        while (true) {
            ConsoleKeyInfo key = Console.ReadKey(true);
            if (key != null)
                break;
        }

        Thread inputTrd = new Thread(new ThreadStart(InputListener));
        inputTrd.IsBackground = true;
        inputTrd.Start();

        Thread foodTrd = new Thread(new ThreadStart(FoodSpawner));
        foodTrd.IsBackground = true;
        foodTrd.Start();

        Console.Clear();

        SpawnArena();

        currentDirection = Direction.LEFT;
        inputDirection = Direction.LEFT;
        snakePosition = [(arenaDimensions[0] * arenaHBorder.Length) / 2, (arenaDimensions[1] * arenaVBorder.Length) / 2];

        snakeSize = initialSnakeSize;

        Console.SetCursorPosition(snakePosition[0], snakePosition[1]);
        Console.Write($"{snakeHead}");
    }

    private static void SpawnArena() {
        string HBorders = "";
        string VBorders = "";

        for (int i = 0; i < arenaDimensions[0]; i++) {
            HBorders += $"{arenaHBorder}";
        }

        HBorders.Trim();

        for (int i = 0; i < arenaDimensions[1]; i++) {
            VBorders += arenaVBorder;
            for (int j = 0; j < HBorders.Length - 3; j++) {
                VBorders += " ";
            }
            VBorders += $"{arenaVBorder}\n";
        }

        string ArenaBorders = HBorders + "\n" + VBorders + HBorders;

        Console.Write($"\r{ArenaBorders}");
    }

    private static void IntroMessage() {
        string[] IntroMessage = {
                    $"+==========================+",
                    $"| {snakeBody} {snakeBody}  {snakeBody} {snakeBody}            Coru |",
                    $"| SNAKE  {snakeBody}                 |",
                    $"|    {snakeBody}{snakeBody}{snakeBody}{snakeBody}{snakeBody}                 |",
                    $"|    {snakeBody} Pulsa cualquier     |",
                    $"|  {snakeHead}{snakeBody}{snakeBody}  tecla para iniciar |",
                    $"+==========================+"
                };



        int startX = (arenaDimensions[0] * arenaHBorder.Length - IntroMessage[0].Length) / 2;
        int startY = (arenaDimensions[1] - IntroMessage.Length) / 2;

        for (int i = 0; i < IntroMessage.Length; i++) {
            Console.SetCursorPosition(startX, startY + i);
            Console.Write(IntroMessage[i]);
        }
    }
    
    private static void Main(string[] args) {

        InitializeGame();

        while (gameRunning) {

            if (spawnedFood.ToArray() != null)
                foreach (var pos in spawnedFood.ToArray()) {
                    Console.SetCursorPosition(pos[0], pos[1]);
                    Console.Write($"{foodIcon}");
                }

            snakeTrail.Add((int[])snakePosition.Clone());

            while (snakeTrail.Count > snakeSize) {
                Console.SetCursorPosition(snakeTrail.First()[0], snakeTrail.First()[1]);
                Console.Write($" ");
                snakeTrail.RemoveAt(0);
            }


            currentDirection = inputDirection;

            switch (currentDirection) {
                case Direction.UP:
                    snakePosition = [snakePosition[0], snakePosition[1] - 1];
                    break;

                case Direction.LEFT:
                    snakePosition = [snakePosition[0] - 1, snakePosition[1]];
                    break;

                case Direction.RIGHT:
                    snakePosition = [snakePosition[0] + 1, snakePosition[1]];
                    break;

                case Direction.DOWN:
                    snakePosition = [snakePosition[0], snakePosition[1] + 1];
                    break;
            }

            if (snakePosition[0] <= 0
                || snakePosition[0] >= arenaDimensions[0] * arenaHBorder.Length - arenaHBorder.Length
                || snakePosition[1] <= 0 || snakePosition[1] >= arenaDimensions[1] + 1
                || snakeTrail.Any(pos => pos.SequenceEqual(snakePosition))) {

                gameRunning = false;
                Console.SetCursorPosition(
                    (arenaDimensions[0] / 2) - 23,
                    (arenaDimensions[1] / 2) - 6
                );

                string totalPoints = (snakeSize - initialSnakeSize).ToString();

                string[] GameOverScreen = {
                    $"=======================",
                    $"          GAME         ",
                    $"          OVER         ",
                    $"=======================",
                    $"=======PUNTUACION======",
                    $"{totalPoints.PadLeft((23 + totalPoints.Length) /2).PadRight(23)}"
                };



                int startX = (arenaDimensions[0] * arenaHBorder.Length - GameOverScreen[0].Length) / 2;
                int startY = (arenaDimensions[1] - GameOverScreen.Length) / 2;

                for (int i = 0; i < GameOverScreen.Length; i++) {
                    Console.SetCursorPosition(startX, startY + i);
                    Console.Write(GameOverScreen[i]);
                }

                break;
            }

            int foodIndex = spawnedFood.FindIndex(
                pos => pos.SequenceEqual(snakePosition)
            );

            if (foodIndex != -1) {
                spawnedFood.RemoveAt(foodIndex);
                snakeSize++;
                if (snakeSpeed >= maxSpeed)
                    snakeSpeed -= 0.1f;
                if (snakeSpeed < maxSpeed)
                    snakeSpeed = maxSpeed;
            }

            if (snakeTrail != null)
                foreach (var pos in snakeTrail) {
                    Console.SetCursorPosition(pos[0], pos[1]);
                    Console.Write($"{snakeBody}");
                }

            Console.SetCursorPosition(snakePosition[0], snakePosition[1]);
            if (currentDirection.Equals(Direction.LEFT))
                Console.Write($"{snakeHead}");
            else if (currentDirection.Equals(Direction.RIGHT))
                Console.Write($"{snakeHead}");
            else
                Console.Write($"{snakeHead}");

            Thread.Sleep((int)(snakeSpeed * 1000));

        }

        while (true) ;
        Console.Write("New Game?");
    }

    private static void InputListener() {
        while (true) {
            ConsoleKeyInfo key = Console.ReadKey(true);
            switch (key.Key) {
                case ConsoleKey.UpArrow:
                case ConsoleKey.W:
                    if (currentDirection != Direction.DOWN)
                        inputDirection = Direction.UP;
                    break;

                case ConsoleKey.LeftArrow:
                case ConsoleKey.A:
                    if (currentDirection != Direction.RIGHT)
                        inputDirection = Direction.LEFT;
                    break;

                case ConsoleKey.RightArrow:
                case ConsoleKey.D:
                    if (currentDirection != Direction.LEFT)
                        inputDirection = Direction.RIGHT;
                    break;

                case ConsoleKey.DownArrow:
                case ConsoleKey.S:
                    if (currentDirection != Direction.UP)
                        inputDirection = Direction.DOWN;
                    break;
            }
        }
    }

    private static void FoodSpawner() {
        Random rtd = new Random();
        while (true) {
            if (spawnedFood.Count <= maxFood - 1) {
                int[] thisCoord = [0, 0];
                while (spawnedFood.Any(pos => pos.SequenceEqual(thisCoord)) || snakeTrail.Any(pos => pos.SequenceEqual(thisCoord)) || thisCoord.SequenceEqual([0, 0])) {
                    int xCoord = rtd.Next(1, arenaDimensions[0] * arenaHBorder.Length);
                    int yCoord = rtd.Next(1, arenaDimensions[1] * arenaVBorder.Length);
                    thisCoord = new int[] { xCoord, yCoord };
                }

                spawnedFood.Add(thisCoord);

                Thread.Sleep(rtd.Next(2000, 5000));
            }
        }
    }
}

