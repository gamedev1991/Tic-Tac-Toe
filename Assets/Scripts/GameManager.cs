using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

public enum Turn
{
	Player1,
	Player2,
    AI
}

public enum GameMode
{
    PVP,
    AI
}

public enum GameLevel
{
    Easy,
    Hard
}

public class GameManager : MonoBehaviour {
	public GameObject startMenu;
	public GameObject gridSelection;
	public GameObject gameModeSelection;
	public GameObject gameLevelSelection;
	public GameObject gameHUD;
    public GameObject grid;
	public Turn currentTurn;
	public Text currentStatus;
	public string winner;
	public GameObject restartButton;
	public GameObject resetButton;
	public static GameManager instance;
	private GridManager gridManager;

	public Text player1Score;
	public Text player2Score;

    public Text player2SymbolText;
    public Text player2ScoreText;

    public GridType gridTypeSelectedByUser;
    public GameMode gameMode;
    public GameLevel gameLevel;
    public static int NumberOfGames
	{
		get{ 
			return PlayerPrefs.GetInt ("NumOfGames");
		}
		set{ 
			PlayerPrefs.SetInt ("NumOfGames", value);
		}
	}
	public static int Player1Score
	{
		get{ 
			return PlayerPrefs.GetInt ("Player1Score");
		}
		set{ 
			PlayerPrefs.SetInt ("Player1Score", value);
		}
	}
	public static int Player2Score
	{
		get{ 
			return PlayerPrefs.GetInt ("Player2Score");
		}
		set{ 
			PlayerPrefs.SetInt ("Player2Score", value);
		}
	}
	void Awake()
	{
		if (instance == null) {
			instance = this;
		} else if (instance != this) {
			Destroy (gameObject);    
		}

		gridManager = grid.GetComponent<GridManager> ();
	}

	// Use this for initialization
	void Start () {
		player1Score.text = Player1Score.ToString ();
		player2Score.text = Player2Score.ToString ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void OnClickStart()
	{
        SoundManager.Instance.PlaySound(AudioSound.Click);
        startMenu.SetActive (false);
        gameModeSelection.SetActive(true);
    }

    public void OnClickBack()
	{
        SoundManager.Instance.PlaySound(AudioSound.Click);
        if (gridSelection.activeSelf)
        {
            gridSelection.SetActive(false);
            gameModeSelection.SetActive(true);
        }
        else if (gameLevelSelection.activeSelf)
        {
            gameLevelSelection.SetActive(false);
            gameModeSelection.SetActive(true);
        }
        else if (gameModeSelection.activeSelf)
        {
            gameModeSelection.SetActive(false);
            startMenu.SetActive(true);
        }
        else {
            gridSelection.SetActive(true);
            grid.SetActive(false);
            gridManager.DestroyGrid();
            gameHUD.SetActive(false);
        }
    }

    public void OnClickPVP()
    {
        SoundManager.Instance.PlaySound(AudioSound.Click);
        gameModeSelection.SetActive(false);
        gridSelection.SetActive(true);
        gameMode = GameMode.PVP;
        player2SymbolText.text = "Player 2:";
        player2ScoreText.text = "Player 2:";
    }

    public void OnClickAI()
    {
        SoundManager.Instance.PlaySound(AudioSound.Click);
        gameModeSelection.SetActive(false);
        gameMode = GameMode.AI;
        gameLevelSelection.SetActive(true);
        player2SymbolText.text = "AI :";
        player2ScoreText.text = "AI :";
    }

    public void OnClickEasy()
    {
        SoundManager.Instance.PlaySound(AudioSound.Click);
        gameLevel = GameLevel.Easy;
        OnClickThreeByThree();
    }

    public void OnClickHard()
    {
        SoundManager.Instance.PlaySound(AudioSound.Click);
        gameLevel = GameLevel.Hard;
        OnClickThreeByThree();
    }

    public void OnClickThreeByThree()
	{
        SoundManager.Instance.PlaySound(AudioSound.Click);
        if (gameMode == GameMode.PVP)
        {
            gridSelection.SetActive(false);
        }
        else {
            gameLevelSelection.SetActive(false);
        }
		gridTypeSelectedByUser = GridType.ThreeByThree;
        StartGame();

    }

	public void OnClickFourByFour()
	{
        SoundManager.Instance.PlaySound(AudioSound.Click);
        gridTypeSelectedByUser = GridType.FourByFour;
        StartGame();
    }

    void StartGame()
    {
        gridSelection.SetActive(false);
        gameHUD.SetActive(true);
        grid.SetActive(true);
        if (gridManager.isGridDestroyed)
        {
            gridManager.SetGrid();
        }
        currentStatus.gameObject.SetActive(true);
        currentStatus.text = "Player 1's Turn";
    }

    public void OnClickRestart()
	{
        SoundManager.Instance.PlaySound(AudioSound.Click);
        SceneManager.LoadScene (0);
	}

	public void OnClickReset()
	{
        SoundManager.Instance.PlaySound(AudioSound.Click);
        PlayerPrefs.DeleteAll ();
		SceneManager.LoadScene (0);
	}

	public void OnClickExit()
	{
        SoundManager.Instance.PlaySound(AudioSound.Click);
        Application.Quit ();
	}
		
	public void GameOver()
	{
		for (int i = 0; i < gridManager.tileList.Count; i++) {
			gridManager.tileList [i].button.enabled = false;
		}

		if (winner.Contains ("Player")|| winner.Contains("AI")) {
			currentStatus.text = winner + " WINS";
		} else {
			currentStatus.text = winner;
		}
		player1Score.text = Player1Score.ToString ();
		player2Score.text = Player2Score.ToString ();
		restartButton.SetActive (true);
		resetButton.SetActive (true);
	}

	public bool CheckWinner(Symbol symbol)
	{
		Symbol currentSymbol;
        currentSymbol = symbol;

		//Check Horizontal
		for (int i = 0; i < gridManager.tileList.Count; i = i + gridManager.numOfRows) {
			int numOfRowsWithSameSymbol = 0;
			for (int j = 0; j < gridManager.numOfCols; j++) {
				if (gridManager.tileList [i + j].tileSymbol.Equals (currentSymbol)) {
					numOfRowsWithSameSymbol++;
				}
			}
			if (numOfRowsWithSameSymbol >= gridManager.numOfRows) {
				return true;
			}
		}

		//Check Vertical
		for (int i = 0; i < gridManager.numOfRows; i++) {
			int numOfColsWithSameSymbol = 0;

			for (int j = 0; j <  gridManager.tileList.Count; j=j+gridManager.numOfCols) {
				if (gridManager.tileList [i + j].tileSymbol.Equals (currentSymbol)) {
					numOfColsWithSameSymbol++;
				}
			}
			if (numOfColsWithSameSymbol >= gridManager.numOfCols) {
				return true;
			}
		}

		//Check Diagonal
		int numOfLeftDiagsWithSameSymbol = 0;
		int numOfRightDiagsWithSameSymbol = 0;

		for (int i = 0; i < gridManager.numOfRows; i++) {
			if (gridManager.tileList [i*gridManager.numOfRows +i].tileSymbol.Equals (currentSymbol)) {
				numOfLeftDiagsWithSameSymbol++;
			}
			if (numOfLeftDiagsWithSameSymbol >= gridManager.numOfCols) {
				return true;
			}

			if (gridManager.tileList [i * (gridManager.numOfRows - 1) + gridManager.numOfRows - 1].tileSymbol.Equals (currentSymbol)) {
				numOfRightDiagsWithSameSymbol++;
			}
			if (numOfRightDiagsWithSameSymbol >= gridManager.numOfCols) {
				return true;
			}
		}
		return false;
	}

	public bool CheckForDraw()
	{
		for (int i = 0; i < gridManager.tileList.Count; i++) {
			if (gridManager.tileList [i].tileSymbol.Equals (Symbol.None)) {
				return false;
			}
		}
		return true;
	}

    public void PlayAITurn()
    {

        bool isGameWon = false;
        if (gameLevel == GameLevel.Easy)
        {
            List<int> tilesThatCanBeSelected = new List<int>();

            for (int i = 0; i < gridManager.tileList.Count; i++)
            {
                if (gridManager.tileList[i].tileSymbol.Equals(Symbol.None))
                {
                    tilesThatCanBeSelected.Add(i);
                    gridManager.tileList[i].tileSymbol = Symbol.Symbol2;
                    if(CheckWinner(Symbol.Symbol2)){
                        tilesThatCanBeSelected.Remove(i);
                    }
                    gridManager.tileList[i].tileSymbol = Symbol.None;
                }
            }
            int tileNumberSelected = UnityEngine.Random.Range(0, tilesThatCanBeSelected.Count);

            gridManager.tileList[tilesThatCanBeSelected[tileNumberSelected]].tileImage.sprite = gridManager.tileList[tilesThatCanBeSelected[tileNumberSelected]].symbolTwo;
            gridManager.tileList[tilesThatCanBeSelected[tileNumberSelected]].tileSymbol = Symbol.Symbol2;
        }
        else {
            bool isTurnPlayed = false;
            List<int> tilesThatCanBeSelected = new List<int>();

            for (int i = 0; i < gridManager.tileList.Count; i++)
            {
                if (gridManager.tileList[i].tileSymbol.Equals(Symbol.None))
                {
                    tilesThatCanBeSelected.Add(i);
                    gridManager.tileList[i].tileSymbol = Symbol.Symbol2;
                    if (CheckWinner(Symbol.Symbol2))// First Check if AI can win
                    {
                        Debug.Log("Here1");
                        gridManager.tileList[i].tileImage.sprite = gridManager.tileList[i].symbolTwo;
                        gridManager.tileList[i].tileSymbol = Symbol.Symbol2;
                        isTurnPlayed = true;
                        break;
                    }
                    else {
                        gridManager.tileList[i].tileSymbol = Symbol.Symbol1;
                        if (CheckWinner(Symbol.Symbol1))// Check if Player can win
                        {
                            Debug.Log("Here2");
                            gridManager.tileList[i].tileImage.sprite = gridManager.tileList[i].symbolTwo;
                            gridManager.tileList[i].tileSymbol = Symbol.Symbol2;
                            isTurnPlayed = true;
                            break;
                        }
                    }
                    gridManager.tileList[i].tileSymbol = Symbol.None;

                }
            }
            if (!isTurnPlayed)
            {
                int tileNumberSelected = UnityEngine.Random.Range(0, tilesThatCanBeSelected.Count);// if No one can win then any random symbol
                Debug.Log("Here3");

                gridManager.tileList[tilesThatCanBeSelected[tileNumberSelected]].tileImage.sprite = gridManager.tileList[tilesThatCanBeSelected[tileNumberSelected]].symbolTwo;
                gridManager.tileList[tilesThatCanBeSelected[tileNumberSelected]].tileSymbol = Symbol.Symbol2;
            }

        }
        currentTurn = Turn.Player1;

        if (CheckWinner(Symbol.Symbol2))
        {
            isGameWon = true;
            winner = "AI";
            Player2Score++;
        }
        if (isGameWon)
        {
            GameOver();
        }
        else
        {
            if (CheckForDraw())
            {
                winner = "Draw";
                GameOver();
            }
            else
            {
                currentStatus.text = currentTurn + "'s turn";
            }
        }
    }
}
