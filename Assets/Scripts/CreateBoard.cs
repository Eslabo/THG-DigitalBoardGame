using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;
using Random = UnityEngine.Random;

public class CreateBoard : MonoBehaviour
{
    public GameObject[] cellPrefabs;

	public int mapWidth = 7;

	private Dictionary<Vector2, Vector2> coordToPixelVectorLookup = new Dictionary<Vector2, Vector2>();
	private Dictionary<Vector2, HashSet<Vector2>> neighbourLookup = new Dictionary<Vector2, HashSet<Vector2>>();

	// Use this for initialization
	void Start ()
	{
		double ang30 = (Math.PI / 180) * 30;

		Action<double, double, int> generateBoard = (hexRadius, hexPadding, boardSize) =>
		{
			double xOffset = Math.Cos(ang30) * (hexRadius + hexPadding);
			double yOffset = Math.Sin(ang30) * (hexRadius + hexPadding);

			int half = boardSize / 2;

			Vector2 origin = new Vector2(0, 0);

			Action<float, float, int, int> generateHex = (x, y, xLookup, yLookup) =>
			{
				var hexCell = Instantiate(this.cellPrefabs[Random.Range(0, this.cellPrefabs.Length)]) as GameObject;

				hexCell.transform.parent = this.transform;
				hexCell.transform.position = new Vector2(x, y);

				Func<int, int, HashSet<Vector2>> generateNeighbours = (xLbl, yLbl) => new HashSet<Vector2>
				{
					new Vector2(xLbl + 1, yLbl + 0), new Vector2(xLbl + 1, yLbl - 1), new Vector2(xLbl + 0, yLbl - 1),
					new Vector2(xLbl - 1, yLbl + 0), new Vector2(xLbl - 1, yLbl + 1), new Vector2(xLbl + 0, yLbl + 1)
				};

				neighbourLookup.Add(new Vector2(x, y), generateNeighbours(xLookup, yLookup));
				coordToPixelVectorLookup.Add(new Vector2(xLookup, yLookup), new Vector2(x, y));
				// TODO: Implement boundary checking for neighbouring coordinates
			};

			for (int row = 0; row < boardSize; ++row)
			{
				int cols = boardSize - Math.Abs(row - half);

				for (int col = 0; col < cols; ++col)
				{
					int xLbl = (row < half) ? (col - row) : (col - half);
					int yLbl = row - half;

					int x = (int)(origin.x + xOffset * (col * 2 + 1 - cols));
					int y = (int)(origin.y + yOffset * (row - half) * 3);

					generateHex(x, y, xLbl, yLbl);

					// Debug.Log("x=" + x + ", y=" + y + ", xLbl=" + xLbl + ", yLbl" + yLbl);
				}
			}
		};

		generateBoard(3.75, 0, mapWidth);
	}
	
	// Update is called once per frame
	void Update () {
		
	}


	bool isInBounds(Vector2 coords)
	{
		return neighbourLookup.ContainsKey(coords);
	}
}
