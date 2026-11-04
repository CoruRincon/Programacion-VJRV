Random rnd = new Random();

bool gameRunning = true;

string[] possibleWords = new string[10];

string roundWord;

List<char> usedLetters = new List<char>();
List<int> foundIndexes = new List<int>();

int maxAttempts = 6;
int currentAttempts;

bool isMultiplayer = false;

bool gameWon = false;

void UIHandler(bool clear, string output) {
    if (clear)
        Console.Clear();

    Console.WriteLine(output);
}

void ModeSelector() {

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


            if(inputLetter.Equals('Y') || inputLetter.Equals('N'))
                UIHandler(true, $"\rModo Multijugador(Y/N): {inputLetter} \nModo Multijugador: {isMultiplayer}");
        }

    }
}

void PreloadWords() {

    possibleWords = [
        "nube",
        "espejo",
        "volcan",
        "canela",
        "brujula",
        "bosque",
        "relampago",
        "almohada",
        "cometa",
        "laberinto"
    ];
}

void InitializeGame() {
    PreloadWords();

    ModeSelector();

    Console.Clear();
    Console.Write($"\rElige una palabra sin que tu oponente la vea: ");
    roundWord = "";
    if (isMultiplayer) {
        string wordInput = "";

        while (true) {

            var key = Console.ReadKey(true);

            if (key.Key == ConsoleKey.Enter) {
                if (wordInput != null && wordInput != "") {
                    roundWord = wordInput;
                    break;
                }
            }

            if (key.Key == ConsoleKey.Backspace) {
                if (wordInput != null && wordInput.Length > 0) {
                    wordInput = wordInput.Substring(0, wordInput.Length - 1);
                    Console.Write($"\rElige una palabra sin que tu oponente la vea: {wordInput} \b");
                }

            }

            if (char.IsLetter(key.KeyChar)) {
                char charInput = char.ToLower(key.KeyChar);
                wordInput = wordInput + charInput;
                Console.Write($"\rElige una palabra sin que tu oponente la vea: {wordInput} \b");
            }

        }
    }
    else {
        roundWord = SelectRandomWord();
    }
    currentAttempts = 0;
}

void ShowHeader() {
    string headerString =
        $"\n===========================================================" +
        HangedState() + 
        $"\n===========================================================" +
        $"\nAHORCADO" +
        $"\nAdivina la palabra introduciendo letras. Máximo de 5 intentos." +
        $"\nPalabra: {ReplaceLines()}. " +
        $"\nIntentos restantes: {AttemptsLeft()}. " +
        $"\n===========================================================\n"
    ;

    UIHandler(true, headerString);
}

string HangedState() {
    string hangedUI = "";

    switch(AttemptsLeft()) {
        case 0:
            hangedUI =
                $"\n||________" +
                $"\n|/       |" +
                $"\n||       O" +
                $"\n||      /|\\" +
                $"\n||      / \\" +
                $"\n||";
            break;
        
        case 1:
            hangedUI =
                $"\n||________" +
                $"\n|/       |" +
                $"\n||       O" +
                $"\n||      /|\\" +
                $"\n||      / " +
                $"\n||";
            break;

        case 2:
            hangedUI =
                $"\n||________" +
                $"\n|/       |" +
                $"\n||       O" +
                $"\n||      /|\\" +
                $"\n||       " +
                $"\n||";
            break;
        
        case 3:
            hangedUI =
                $"\n||________" +
                $"\n|/       |" +
                $"\n||       O" +
                $"\n||      /|" +
                $"\n||      " +
                $"\n||";
            break;
        
        case 4:
            hangedUI =
                $"\n||________" +
                $"\n|/       |" +
                $"\n||       O" +
                $"\n||       |" +
                $"\n||      " +
                $"\n||";
            break;
        
        case 5:
            hangedUI =
                $"\n||________" +
                $"\n|/       |" +
                $"\n||       O" +
                $"\n||      " +
                $"\n||      " +
                $"\n||";
            break;
            
        case 6:
            hangedUI =
                $"\n||________" +
                $"\n|/" +
                $"\n||" +
                $"\n||" +
                $"\n||" +
                $"\n||";
            break;
    }

    return hangedUI;
}

string SelectRandomWord() {
    return possibleWords[rnd.Next(0, 6)].ToLower();
}

int AttemptsLeft() {
    return maxAttempts - currentAttempts;
}

char RequestLetter() {
    char? input = null;

    Console.Write($"Elige una letra: {input} \b");

    while (true) {

        var key = Console.ReadKey(true);

        if (key.Key == ConsoleKey.Enter) {
            if (input.HasValue) {
                return input.Value;
            }
        }

        if (char.IsLetter(key.KeyChar)) {
            input = char.ToLower(key.KeyChar);
            Console.Write($"\rElige una letra: {input} \b");
        }
    }
}

void AttemptFailed() {
    currentAttempts++;
}

string ReplaceLines() {
    string revealedString = "";
    for (int i = 0; i < roundWord.Length; i++) {
        if (foundIndexes.Contains(i))
            revealedString += roundWord[i];
        else
            revealedString += "_";
    }
    return revealedString;
}

void CheckEndCondition() {

    if (foundIndexes.Count == roundWord.Length) {
        gameRunning = false;
        ShowHeader();
        UIHandler(false, "Victoria");
    }

    if (AttemptsLeft() == 0) {
        gameRunning = false;
        ShowHeader();
        UIHandler(false, "Derrota");
    }
}

void Hangman() {

    ShowHeader();
    string GameUI = "";

    char inputtedLetter = RequestLetter();

    GameUI += $"\nLetra seleccionada: {inputtedLetter} \nComprobando...";

    if (usedLetters.Contains(inputtedLetter)) {
        GameUI += $"\nLetra ya seleccionada.";
    }
    else {
        GameUI += $"\nLetras ya usadas: ";
        usedLetters.Add(inputtedLetter);
        if (roundWord.Contains(inputtedLetter)) {
            for (int j = 0; j < roundWord.Length; j++) {
                if (roundWord[j].Equals(inputtedLetter))
                    foundIndexes.Add(j);
            }
        }
        else
            AttemptFailed();
    }


    GameUI +=
        $"\nElige una letra: " +
        $"\n==========================================================="
    ;

    UIHandler(false, GameUI);
}

InitializeGame();
while (gameRunning) {
    Hangman();
    CheckEndCondition();
}

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