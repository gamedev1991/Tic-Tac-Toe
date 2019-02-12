using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

public enum Turn
{
	Player1,
	Player2
}
public class GameManager : MonoBehaviour {
	public GameObject startMenu;
	public GameObject gridSelection;
	public GridType gridTypeSelectedByUser;
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
		startMenu.SetActive (false);
		gridSelection.SetActive (true);
	}

	public void OnClickBack()
	{
		startMenu.SetActive (true);
		gridSelection.SetActive (false);
	}

	public void OnClickThreeByThree()
	{
		gridSelection.SetActive (false);
		gridTypeSelectedByUser = GridType.ThreeByThree;
		grid.SetActive (true);
		currentStatus.gameObject.SetActive (true);
		currentStatus.text = "Player 1's Turn";
	}

	public void OnClickFourByFour()
	{
		gridSelection.SetActive (false);
		gridTypeSelectedByUser = GridType.FourByFour;
		grid.SetActive (true);
		currentStatus.gameObject.SetActive (true);
		currentStatus.text = "Player 1's Turn";
	}

	public void OnClickRestart()
	{
		SceneManager.LoadScene (0);
	}

	public void OnClickReset()
	{
		PlayerPrefs.DeleteAll ();
		SceneManager.LoadScene (0);
	}

	public void OnClickExit()
	{
		Application.Quit ();
	}
		
	public void GameOver()
	{
		for (int i = 0; i < gridManager.tileList.Count; i++) {
			gridManager.tileList [i].button.enabled = false;
		}

		if (winner.Contains ("Player")) {
			currentStatus.text = winner + " WINS";
		} else {
			currentStatus.text = winner;
		}
		player1Score.text = Player1Score.ToString ();
		player2Score.text = Player2Score.ToString ();
		restartButton.SetActive (true);
		resetButton.SetActive (true);
	}

	public bool CheckWinner()
	{
		Symbol currentSymbol;
		if (currentTurn.Equals (Turn.Player1)) {
			currentSymbol = Symbol.Symbol1;
		} else {
			currentSymbol = Symbol.Symbol2;
		}

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
}
