using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public enum Symbol
{
	None,
	Symbol1,
	Symbol2
}
public class Tile : MonoBehaviour {
	public Image tileImage;
	public Sprite symbolOne;
	public Sprite symbolTwo;
	public Button button;
	public Symbol tileSymbol = Symbol.None;
	public int tileNumber;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void OnClickTile()
	{
		bool isGameWon = false;
		if (GameManager.instance.currentTurn.Equals (Turn.Player1)) {
			tileImage.sprite = symbolOne;
			tileSymbol = Symbol.Symbol1;
			if (GameManager.instance.CheckWinner ()) {
				isGameWon = true;
				GameManager.instance.winner = "Player1";
				GameManager.Player1Score++;
			}
			GameManager.instance.currentTurn = Turn.Player2;
		} else {
			tileImage.sprite = symbolTwo;
			tileSymbol = Symbol.Symbol2;
			if (GameManager.instance.CheckWinner ()) {
				isGameWon = true;
				GameManager.instance.winner = "Player2";
				GameManager.Player2Score++;
			}
			GameManager.instance.currentTurn = Turn.Player1;
		}

		if (isGameWon) {
			GameManager.instance.GameOver ();
		} else {
			if (GameManager.instance.CheckForDraw ()) {
				GameManager.instance.winner = "Draw";
				GameManager.instance.GameOver ();
			} else {
				GameManager.instance.currentStatus.text = GameManager.instance.currentTurn + "'s turn";
			}
		}
		button.enabled = false;
	}
}
