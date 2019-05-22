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
        if (GameManager.instance.gameMode == GameMode.PVP &&
            GameManager.instance.currentTurn == Turn.AI)
        {
            SoundManager.Instance.PlaySound(AudioSound.ClickError);
            return;
        }
        SoundManager.Instance.PlaySound(AudioSound.Click);
        bool isGameWon = false;
		if (GameManager.instance.currentTurn.Equals (Turn.Player1)) {
			tileImage.sprite = symbolOne;
			tileSymbol = Symbol.Symbol1;
			if (GameManager.instance.CheckWinner (Symbol.Symbol1)) {
				isGameWon = true;
				GameManager.instance.winner = "Player1";
				GameManager.Player1Score++;
			}
            if (GameManager.instance.gameMode == GameMode.PVP)
            {
                GameManager.instance.currentTurn = Turn.Player2;
            }
            else {
                GameManager.instance.currentTurn = Turn.AI;
            }
        } else {
            if (GameManager.instance.gameMode == GameMode.PVP)
            {
                tileImage.sprite = symbolTwo;
                tileSymbol = Symbol.Symbol2;
                if (GameManager.instance.CheckWinner(Symbol.Symbol2))
                {
                    isGameWon = true;
                    GameManager.instance.winner = "Player2";
                    GameManager.Player2Score++;
                }
            }
            else {
                return;
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
                if (GameManager.instance.gameMode == GameMode.AI)
                {
                    Invoke("CallAITurn", 1f);
                }
            }
		}
		button.enabled = false;
	}

    void CallAITurn()
    {
        GameManager.instance.PlayAITurn();
        
    }
}
