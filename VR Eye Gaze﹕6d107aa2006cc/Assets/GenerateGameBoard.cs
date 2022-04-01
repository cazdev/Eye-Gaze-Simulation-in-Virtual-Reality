using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateGameBoard : MonoBehaviour
{
    public GameObject TileSquare1;
    public GameObject TileSquare2;
    public GameObject PlacementSquare;
    public int Columns = 15;
    public int Rows = 15;
    public Transform StartPos;

    // Start is called before the first frame update
    void Start()
    {
        CreateGameBoard();
    }

    void CreateGameBoard()
    {
        bool isAlternateTile = false;
        for (int column = 0; column < Columns + 1; column++)
        {
            for (int row = 0; row < Rows + 1; row++)
            {

                // Corner Placement zones
                Instantiate(
                        PlacementSquare,
                        new Vector3(row * 0.03f, 0, column * 0.03f) + StartPos.position - new Vector3(0.015f, -0.01f, 0.015f),
                        Quaternion.identity,
                        transform
                        );

                // Tiles
                if (column < Columns && row < Rows)
                {
                    if (isAlternateTile)
                    {
                        TileSquare1.tag = column + "," + row;
                        Instantiate(
                            TileSquare1,
                            new Vector3(row * 0.03f, 0, column * 0.03f) + StartPos.position,
                            Quaternion.identity,
                            transform
                            );
                    }
                    else
                    {
                        TileSquare2.tag = "GameboardSquare-" + column + "," + row;
                        Instantiate(
                            TileSquare2,
                            new Vector3(row * 0.03f, 0, column * 0.03f) + StartPos.position,
                            Quaternion.identity,
                            transform
                            );
                    }
                    isAlternateTile = !isAlternateTile;
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
