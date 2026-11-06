using System.Linq;
using System.Numerics;

public class Player {
    public string Name { get; set; }
    public List<Card> Hand { get; set; }

    public Player(string Name) {
        this.Name = Name;
        this.Hand = new List<Card>();
    }

    public void TakeCards(int amount) {
        Card[] cardsDealt = Deck.Instance.DealCards(amount);
        foreach (var card in cardsDealt) {
            this.Hand.Add(card);
        }
    }

    public void GiveCard(Card card) {
        if (Hand.Contains(card)) {
            Hand.Remove(card);
        }
    }

    public void ClearHand() {
        Hand.Clear();
    }
}

public class BlackJackPlayer : Player {

    public int CurrentPoints { get; set; }

    public bool isStaying = false;
    public bool hasWon = false;

    public BlackJackPlayer(string Name) : base(Name) {
        this.Name = Name;
    }

    public Card[] AcesInHand() {
        List<Card> acesInHand = new List<Card>();
        foreach (var card in Hand) {
            if (card.GetCardValue().Length > 1) {
                acesInHand.Add(card);
            }
        }
        return acesInHand.ToArray();
    }

    public void CalculateCurrentPoints() {
        int auxValue = 0;
        foreach (var card in Hand) {
            if (card.GetCardValue().Length == 1) {
                foreach (var value in card.GetCardValue()) {
                    auxValue += value;
                }
            }
        }

        int acesInHand = AcesInHand().Length;

        for (int i = 0; i < acesInHand; i++) {
            if (auxValue + 11 + 1 * (acesInHand - (i + 1)) > 21) {
                auxValue += 1;
            }
            else {
                auxValue += 11;
            }
        }

        this.CurrentPoints = auxValue;
    }

    private bool CalculateBlackjack(int points) {
        if (points > 21) { return true; }
        return false;
    }
}

public class Card {
    public enum suit {
        HEARTS,
        CLUBS,
        SPADES,
        DIAMONDS
    }

    public enum rank {
        ACE,
        TWO,
        THREE,
        FOUR,
        FIVE,
        SIX,
        SEVEN,
        EIGHT,
        NINE,
        TEN,
        J,
        Q,
        K
    }

    public suit Suit;
    public rank Rank;

    public Card(suit s, rank r) {
        this.Suit = s;
        this.Rank = r;
    }

    public string GetCardUI() {
        char cardSuit = GetSuitIcon();
        string cardRank = GetCardValue()[0].ToString();
        string cardDisplayed =
                "+-------+" +
                "\n| " + cardSuit + "     |" +
                "\n|       |";

        if (GetCardValue()[0] == 1)
            cardDisplayed +=
                "\n|   " + "A" + "   |";

        else if (GetCardValue()[0] > 9)
            if (this.Rank != rank.TEN) {
                cardDisplayed +=
                    "\n|   " + this.Rank.ToString() + "   |";
            }
            else {
                cardDisplayed +=
                    "\n|  " + cardRank.Substring(0, 1) + " " + cardRank.Substring(1) + "  |";
            }
        else
            cardDisplayed +=
                "\n|   " + cardRank + "   |";

        cardDisplayed +=
                "\n|       |" +
                "\n|     " + cardSuit + " |" +
                "\n+-------+";

        return cardDisplayed;
    }

    public string GetCardUIFlat() {
        char cardSuit = GetSuitIcon();
        string cardRank = GetCardValue()[0].ToString();
        string cardDisplayed =
                "+-------+" +
                "| " + cardSuit + "     |" +
                "|       |";

        if (GetCardValue()[0] == 1)
            cardDisplayed +=
                "|   " + "A" + "   |";

        else if (GetCardValue()[0] > 9)
            if (this.Rank != rank.TEN) {
                cardDisplayed +=
                    "|   " + this.Rank.ToString() + "   |";
            }
            else {
                cardDisplayed +=
                    "|  " + cardRank.Substring(0, 1) + " " + cardRank.Substring(1) + "  |";
            }
        else
            cardDisplayed +=
                "|   " + cardRank + "   |";

        cardDisplayed +=
                "|       |" +
                "|     " + cardSuit + " |" +
                "+-------+";

        return cardDisplayed;
    }

    public char GetSuitIcon() {
        switch (this.Suit) {
            case suit.HEARTS:
                return '\u2665';

            case suit.CLUBS:
                return '\u2667';

            case suit.SPADES:
                return '\u2664';

            case suit.DIAMONDS:
                return '\u2666';

            default:
                return '◙';
        }
    }

    public int[] GetCardValue() {
        switch (this.Rank) {
            case rank.ACE:
                return [1, 11];

            case rank.TWO:
                return [2];

            case rank.THREE:
                return [3];

            case rank.FOUR:
                return [4];

            case rank.FIVE:
                return [5];

            case rank.SIX:
                return [6];

            case rank.SEVEN:
                return [7];

            case rank.EIGHT:
                return [8];

            case rank.NINE:
                return [9];

            case rank.TEN:
            case rank.J:
            case rank.Q:
            case rank.K:
                return [10];

            default:
                return [0];
        }
    }
}

public class Deck {

    public static Deck? Instance;

    public List<Card> FullDeck = new List<Card>();

    public Deck() {
        DeckFiller();
        /*if (FullDeck.Length == 52)
            foreach (var card in FullDeck) {
                Console.WriteLine(card.GetCardUI());
            }*/
    }

    private void DeckFiller() {
        List<Card> auxDeck = new List<Card>();
        foreach (Card.suit suit in Enum.GetValues(typeof(Card.suit))) {
            foreach (Card.rank rank in Enum.GetValues(typeof(Card.rank))) {
                auxDeck.Add(new Card(suit, rank));
            }
        }

        FullDeck = auxDeck;
    }

    public void ShuffleDeck() {
        var count = FullDeck.Count;
        var last = count - 1;
        for (var i = 0; i < last; ++i) {
            Random rnd = new Random();
            var r = rnd.Next(i, count);
            var tmp = FullDeck[i];
            FullDeck[i] = FullDeck[r];
            FullDeck[r] = tmp;
        }
    }

    public Card[] DealCards(int amount) {
        Card[] dealedCards = new Card[amount];
        for (int i = 0; i < amount; i++) {
            dealedCards[i] = FullDeck.First();
            FullDeck.Remove(FullDeck.First());
        }
        return dealedCards;
    }
}

class BlackJack {

    private static bool isMultiplayer;
    private static List<BlackJackPlayer> players;
    private static BlackJackPlayer crupier = null;
    private static List<string> invalidNames;
    private static int maxHandSize = 12;

    private static bool gameLooping = false;

    private static void DisplayCards(Card[] cardsInput, int maxWidth) {
        string topLine = "";
        string topLine2 = "";
        string topLine3 = "";
        string middleLine = "";
        string bottomLine = "";
        string bottomLine2 = "";
        string bottomLine3 = "";

        string fullString = "";

        for (int i = 0; i < cardsInput.Length; i += maxWidth) {
            if (maxWidth > cardsInput.Length)
                maxWidth = cardsInput.Length;

            Card[] cards = new Card[maxWidth];
            if (i < cardsInput.Length - maxWidth)
                Array.Copy(cardsInput, i, cards, 0, cards.Length);
            else
                Array.Copy(cardsInput, i, cards, 0, cardsInput.Length - i);

            string[] cardsStrings = new string[maxWidth];
            for (int j = 0; j < maxWidth; j++) {
                if (cards[j] == null)
                    cardsStrings[j] = null;

                else
                    cardsStrings[j] = cards[j].GetCardUIFlat();
            }

            topLine = "";
            topLine2 = "";
            topLine3 = "";
            middleLine = "";
            bottomLine = "";
            bottomLine2 = "";
            bottomLine3 = "";

            foreach (var cardString in cardsStrings) {
                if (cardString == null)
                    break;

                topLine += cardString.Substring(0, 9);
                topLine2 += cardString.Substring(9, 9);
                topLine3 += cardString.Substring(18, 9);
                middleLine += cardString.Substring(27, 9);
                bottomLine += cardString.Substring(36, 9);
                bottomLine2 += cardString.Substring(45, 9);
                bottomLine3 += cardString.Substring(54, 9);

            }
            fullString += topLine + "\n" + topLine2 + "\n" + topLine3 + "\n" + middleLine + "\n" + bottomLine + "\n" + bottomLine2 + "\n" + bottomLine3 + "\n";
        }
        Console.Clear();
        Console.Write(fullString);
    }

    private static string DisplayCardString(Card[] cardsInput, int maxWidth) {
        string topLine = "";
        string topLine2 = "";
        string topLine3 = "";
        string middleLine = "";
        string bottomLine = "";
        string bottomLine2 = "";
        string bottomLine3 = "";

        string fullString = "";

        for (int i = 0; i < cardsInput.Length; i += maxWidth) {
            if (maxWidth > cardsInput.Length)
                maxWidth = cardsInput.Length;

            Card[] cards = new Card[maxWidth];
            if (i < cardsInput.Length - maxWidth)
                Array.Copy(cardsInput, i, cards, 0, cards.Length);
            else
                Array.Copy(cardsInput, i, cards, 0, cardsInput.Length - i);

            string[] cardsStrings = new string[maxWidth];
            for (int j = 0; j < maxWidth; j++) {
                if (cards[j] == null)
                    cardsStrings[j] = null;

                else
                    cardsStrings[j] = cards[j].GetCardUIFlat();
            }

            topLine = "";
            topLine2 = "";
            topLine3 = "";
            middleLine = "";
            bottomLine = "";
            bottomLine2 = "";
            bottomLine3 = "";

            foreach (var cardString in cardsStrings) {
                if (cardString == null)
                    break;

                topLine += cardString.Substring(0, 9);
                topLine2 += cardString.Substring(9, 9);
                topLine3 += cardString.Substring(18, 9);
                middleLine += cardString.Substring(27, 9);
                bottomLine += cardString.Substring(36, 9);
                bottomLine2 += cardString.Substring(45, 9);
                bottomLine3 += cardString.Substring(54, 9);

            }
            fullString += topLine + "\n" + topLine2 + "\n" + topLine3 + "\n" + middleLine + "\n" + bottomLine + "\n" + bottomLine2 + "\n" + bottomLine3 + "\n";
        }

        return fullString;
    }

    private static void UIHandler(bool clear, string output) {
        if (clear)
            Console.Clear();

        Console.WriteLine(output);
    }

    private static void InitializeGame() {
        UIHandler(true, "BlackJack - by Coru");

        Deck.Instance = new Deck();

        players = new List<BlackJackPlayer>();
        invalidNames = new List<string>();
        invalidNames.Add("Crupier");

        ModeSelector();

        crupier = new BlackJackPlayer("Crupier");

        gameLooping = true;

        Blackjack();
    }

    private static void ModeSelector() {

        UIHandler(false, $"Modo Multijugador(Y/N): \nModo Multijugador: {isMultiplayer}");

        while (true) {

            var key = Console.ReadKey(true);

            if (key.Key == ConsoleKey.Enter) {
                break;
            }

            if (char.IsLetter(key.KeyChar)) {
                char inputLetter = char.ToUpper(key.KeyChar);
                switch (inputLetter) {
                    case 'Y':
                        isMultiplayer = true;
                        break;

                    case 'N':
                        isMultiplayer = false;
                        break;
                }


                if (inputLetter.Equals('Y') || inputLetter.Equals('N')) {
                    Console.SetCursorPosition(0, 1);
                    UIHandler(false, $"\rModo Multijugador(Y/N): {inputLetter} \nModo Multijugador: {isMultiplayer} ");
                }
            }
        }

        if (isMultiplayer) {
            PrepareMultiplayer();
        }
        else {
            string inputName = "";
            Console.Write($"\rInserte nombre del jugador:");
            while (true) {

                var key = Console.ReadKey(true);

                if (key.Key == ConsoleKey.Enter) {
                    if (invalidNames.Contains(inputName)) {
                        inputName = "";
                        Console.SetCursorPosition(0, 4);
                        Console.Write($"\r Nombre no válido. Inserte otro nombre del jugador: {inputName} {new string(' ', Console.WindowWidth)}");
                    }
                    else {
                        BlackJackPlayer thisPlayer = new BlackJackPlayer(inputName);
                        players.Add(thisPlayer);
                        invalidNames.Add(inputName);
                        break;
                    }
                }

                if (key.Key == ConsoleKey.Backspace) {
                    if (inputName != null && inputName.Length > 0) {
                        inputName = inputName.Substring(0, inputName.Length - 1);

                        Console.SetCursorPosition(0, 4);
                        Console.Write($"\rInserte nombre del jugador: {inputName} {new string(' ', Console.WindowWidth)}");
                    }

                }

                if (char.IsLetterOrDigit(key.KeyChar)) {
                    inputName += key.KeyChar;

                    Console.SetCursorPosition(0, 4);
                    UIHandler(false, $"\rInserte nombre del jugador: {inputName} {new string(' ', Console.WindowWidth)}");
                }
            }
        }
    }

    private static void PrepareMultiplayer() {
        UIHandler(false, $"\rInserte número de jugadores:");
        string inputNumber = "";
        while (true) {

            var key = Console.ReadKey(true);

            if (key.Key == ConsoleKey.Enter) {
                break;
            }

            if (key.Key == ConsoleKey.Backspace) {
                if (inputNumber != null && inputNumber.Length > 0) {
                    inputNumber = inputNumber.Substring(0, inputNumber.Length - 1);

                    Console.SetCursorPosition(0, 3);
                    Console.Write($"\rInserte número de jugadores: {inputNumber} {new string(' ', Console.WindowWidth)}");
                }

            }

            if (char.IsNumber(key.KeyChar)) {
                inputNumber += key.KeyChar;

                Console.SetCursorPosition(0, 3);
                UIHandler(false, $"\rInserte número de jugadores: {inputNumber}");
            }
        }

        int playerNumber = int.Parse(inputNumber);
        for (int i = 0; i < playerNumber; i++) {
            string inputName = "";
            Console.Write($"\rInserte nombre del jugador {i + 1}:");
            while (true) {

                var key = Console.ReadKey(true);

                if (key.Key == ConsoleKey.Enter) {
                    if (invalidNames.Contains(inputName)) {
                        inputName = "";
                        Console.SetCursorPosition(0, i + 4);
                        Console.Write($"\r Nombre no válido. Inserte otro nombre del jugador {i + 1}: {inputName} {new string(' ', Console.WindowWidth)}");
                    }
                    else {
                        BlackJackPlayer thisPlayer = new BlackJackPlayer(inputName);
                        players.Add(thisPlayer);
                        invalidNames.Add(inputName);
                        break;
                    }
                }

                if (key.Key == ConsoleKey.Backspace) {
                    if (inputName != null && inputName.Length > 0) {
                        inputName = inputName.Substring(0, inputName.Length - 1);

                        Console.SetCursorPosition(0, i + 4);
                        Console.Write($"\rInserte nombre del jugador {i + 1}: {inputName} {new string(' ', Console.WindowWidth)}");
                    }

                }

                if (char.IsLetterOrDigit(key.KeyChar)) {
                    inputName += key.KeyChar;

                    Console.SetCursorPosition(0, i + 4);
                    UIHandler(false, $"\rInserte nombre del jugador {i + 1}: {inputName} {new string(' ', Console.WindowWidth)}");
                }
            }
        }

        UIHandler(true, $"\rIniciando con {players.Count} jugadores.");
    }



    private static void Blackjack() {
        Deck.Instance.ShuffleDeck();

        crupier.TakeCards(2);
        crupier.CalculateCurrentPoints();

        foreach (var player in players) {

            Deck.Instance.ShuffleDeck();

            player.TakeCards(2);
            player.CalculateCurrentPoints();

            while (!player.isStaying) {

                UIHandler(true,
                    "======================\n" +
                    $"Mano del crupier\n" +
                    "======================\n");
                UIHandler(false, $"Valor de mano actual: {crupier.CurrentPoints.ToString()}");
                UIHandler(false, DisplayCardString(crupier.Hand.ToArray(), 12));

                UIHandler(false,
                    "======================\n" +
                    $"Mano del jugador {player.Name}\n" +
                    "======================\n");
                UIHandler(false, $"Valor de mano actual: {player.CurrentPoints.ToString()}");
                UIHandler(false, DisplayCardString(player.Hand.ToArray(), 12));
                if (player.CurrentPoints >= 21) {
                    player.isStaying = true;
                    break;
                } 
                UIHandler(false, $"¿Quieres otra carta (H) or plantarte (S)?");

                bool playerChoosing = true;

                while (playerChoosing) {
                    var key = Console.ReadKey(true);

                    switch (char.ToUpper(key.KeyChar)) {
                        case 'H':
                            player.TakeCards(1);
                            playerChoosing = false;
                            break;

                        case 'S':
                            player.isStaying = true;
                            playerChoosing = false;
                            break;
                    }
                }

                player.CalculateCurrentPoints();
            }

        }

        while(crupier.CurrentPoints < 17) {
            crupier.TakeCards(1);
            crupier.CalculateCurrentPoints();
        }

        UIHandler(isMultiplayer,
                    "======================\n" +
                    $"Mano del crupier\n" +
                    "======================\n");
        UIHandler(false, $"Valor de mano actual: {crupier.CurrentPoints.ToString()}");
        UIHandler(false, DisplayCardString(crupier.Hand.ToArray(), 12));

        List<BlackJackPlayer> blackjackList = new List<BlackJackPlayer>();
        List<BlackJackPlayer> stayedList = new List<BlackJackPlayer>();
        List<BlackJackPlayer> bustList = new List<BlackJackPlayer>();

        if (crupier.CurrentPoints == 21) {
            blackjackList.Add(crupier);
        }
        else if (crupier.CurrentPoints < 21) {
            stayedList.Add(crupier);
        }
        else {
            bustList.Add(crupier);
        }

        foreach (var player in players) {
            int currentPoint = player.CurrentPoints;
            if(currentPoint == 21) {
                blackjackList.Add(player);
            } else if (currentPoint < 21){
                stayedList.Add(player);
            } else {
                bustList.Add(player);
            }
        }

        List<BlackJackPlayer> sortedBlackjackResult = blackjackList.OrderBy(x => x.Hand.Count).ToList();
        List<BlackJackPlayer> sortedStayedResult = stayedList.OrderByDescending(x => x.CurrentPoints).ToList();
        List<BlackJackPlayer> sortedBustResult = bustList.OrderBy(x => x.CurrentPoints).ThenByDescending(x => x.Hand.Count).ToList();

        UIHandler(false, 
            "=================" +
            "RESULTADOS" +
            "=================");
        int currentLoop = 0;
        foreach (var player in sortedBlackjackResult) {
            if(currentLoop == 0)
                UIHandler(false, $"Ganador - {player.Name}: {player.CurrentPoints} con {player.Hand.Count} cartas. - Ganador");
            else
                UIHandler(false, $"{player.Name}: {player.CurrentPoints} con {player.Hand.Count} cartas.");
            currentLoop++;
        }
        foreach (var player in sortedStayedResult) {
            UIHandler(false, $"{player.Name}: {player.CurrentPoints} con {player.Hand.Count} cartas.");
        }
        foreach (var player in sortedBustResult) {
            UIHandler(false, $"{player.Name}: {player.CurrentPoints} con {player.Hand.Count} cartas.");
        }

        UIHandler(false, $"\n\n¿Juego Nuevo?");

        isMultiplayer = false;
        while (true) {

            var key = Console.ReadKey(true);

            if (key.Key == ConsoleKey.Enter) {
                break;
            }

            if (char.IsLetter(key.KeyChar)) {
                char inputLetter = char.ToUpper(key.KeyChar);
                switch (inputLetter) {
                    case 'Y':
                        InitializeGame();
                        break;

                    case 'N':
                        Environment.Exit(0);
                        break;
                }
            }
        }

    }

    public static void Main(String[] args) {
        Console.OutputEncoding = System.Text.Encoding.UTF8;

        InitializeGame();
    }
}