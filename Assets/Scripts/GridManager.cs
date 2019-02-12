using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public enum GridType
{
	ThreeByThree,
	FourByFour
}
public class GridManager : MonoBehaviour {
	public Transform parentCanvas;
	public GameObject gridTile;
	public List<Tile> tileList;

	public int numOfRows;
	public int numOfCols;
	private float tileWidth;
	private float tileHeight;
	private int row_offSet;
	private int col_offSet;
	 
	// Use this for initialization
	void Start () {
		GetHeightAndWidthOfTile ();
		SetGridType ();
		CreateGrid ();
	}


	void GetHeightAndWidthOfTile()
	{
		RectTransform tileRect = gridTile.GetComponent<RectTransform> ();
		tileWidth = tileRect.rect.width;
		tileHeight = tileRect.rect.height;
	}

	void SetGridType()
	{
		switch (GameManager.instance.gridTypeSelectedByUser) {
		case GridType.ThreeByThree:
			numOfRows = 3;
			numOfCols = 3;
			row_offSet = -50;
			col_offSet = -50;
			break;
		case GridType.FourByFour:
			numOfRows = 4;
			numOfCols = 4;
			row_offSet = -75;
			col_offSet = -75;
			break;

		}
	}

	void CreateGrid()
	{
		for (int row = 0; row < numOfRows; row++) {
			for (int col = 0; col < numOfCols; col++) {
				GameObject tile = Instantiate (gridTile, parentCanvas);
				tile.GetComponent<Tile> ().tileNumber = row*numOfRows + col;
				tileList.Add (tile.GetComponent<Tile> ());
				tile.transform.localScale = Vector3.one;
				tile.transform.localPosition = new Vector3 (row * tileWidth + row_offSet, col * tileHeight + col_offSet, 0);
			}
		}
	}


	// Update is called once per frame
	void Update () {
		
	}
}
