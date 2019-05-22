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
	private float row_offSet;
	private float col_offSet;
    public bool isGridDestroyed;
    // Use this for initialization
    void Start () {
        SetGrid();
	}

    public void SetGrid()
    {
        GetHeightAndWidthOfTile();
        SetGridType();
        CalculateStartPositions();
        CreateGrid();
    }
    void CalculateStartPositions()
    {
        row_offSet = -((numOfRows / 2) * (tileWidth));
        col_offSet = -((numOfCols / 2) * (tileHeight));
        if (numOfRows % 2 == 0)
        {
            row_offSet += tileWidth / 2;
            col_offSet += tileHeight / 2;
        }

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
			
			break;
		case GridType.FourByFour:
			numOfRows = 4;
			numOfCols = 4;		
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

    public void DestroyGrid()
    {
        for (int i = 0; i < parentCanvas.transform.childCount; i++)
        {
            Destroy(parentCanvas.GetChild(i).gameObject);
        }
        isGridDestroyed = true;
    }

	// Update is called once per frame
	void Update () {
		
	}
}
