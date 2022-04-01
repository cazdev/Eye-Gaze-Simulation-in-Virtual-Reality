using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Globalization;

public class VisualisationsGameBoard : MonoBehaviour
{
    public GameObject TileSquare1;
    public GameObject TileSquare2;
    public GameObject PlacementSquare;
    public GameObject BlackTokenClone;
    public GameObject WhiteTokenClone;
    public GameObject HeatSphere;
    public int Columns = 15;
    public int Rows = 15;
    public Transform StartPos;
    public LineRenderer lineRenderer;

    // settings
    const int data_interval = 3;
    private Vector2[] whitePlacementCoordinates = new Vector2[] { 
        new Vector2(1,2), 
        new Vector2(2,3), 
        new Vector2(3,4) 
        };
    private Vector2[] blackPlacementCoordinates = new Vector2[] { 
        new Vector2(1,1), 
        new Vector2(2,2), 
        new Vector2(3,3) 
        };
    List<Vector3> listLocalLookAtGameboardCoordinates = new List<Vector3>();
    List<Vector3> listLocalLookAtGlobalCoordinates = new List<Vector3>();

    // Start is called before the first frame update
    void Start()
    {
        CreateGameBoard();
        ReadObjectLookedAt();
        LoadPreviousGame();
        GenerateHeatMap();
        DrawLinesBetweenPoints(Color.red);
    }

    void ReadObjectLookedAt() {
        Debug.Log("Starting Reading objects looked at");

        try
            {
            using(var reader = new StreamReader(@"C:\looking_at_objects-2022-03-26-16-28-10_Player_1.csv"))
            {
                int count = 0;
                while (!reader.EndOfStream)
                {
                    // Add every x_interval line to prevent too much data and running out of memeory
                    if (count % data_interval == 0) {
                        var line = reader.ReadLine();
                        var values = line.Split(',');

                        // Creating list of vector3s with gameboard points
                        if (values[1].Equals("event_looked_at_Gameboard")) {
                            Vector3 newPoint = new Vector3(float.Parse(values[2], CultureInfo.InvariantCulture.NumberFormat),
                                                        float.Parse(values[3], CultureInfo.InvariantCulture.NumberFormat),
                                                        float.Parse(values[4], CultureInfo.InvariantCulture.NumberFormat));
                            listLocalLookAtGameboardCoordinates.Add(newPoint);
                        }
                    }

                    count++;
                }
            }
        } catch (System.Exception ex)
        {
            Debug.LogError(ex);
        }
    }

    void LoadPreviousGame() {
        for (int i = 0; i < whitePlacementCoordinates.Length; i++)
        {
            Vector2 coords = whitePlacementCoordinates[i];
            GameObject placement = Instantiate(
                        WhiteTokenClone,
                        new Vector3(coords.x * 0.03f, 0, coords.y * 0.03f) + StartPos.position - new Vector3(0.015f, -0.015f, 0.015f),
                        Quaternion.identity,
                        BlackTokenClone.transform
                        );
        }
        for (int i = 0; i < blackPlacementCoordinates.Length; i++)
        {
            Vector2 coords = blackPlacementCoordinates[i];
            GameObject placement = Instantiate(
                        BlackTokenClone,
                        new Vector3(coords.x * 0.03f, 0, coords.y * 0.03f) + StartPos.position - new Vector3(0.015f, -0.015f, 0.015f),
                        Quaternion.identity,
                        BlackTokenClone.transform
                        );
        }
    }

    void DrawLinesBetweenPoints(Color color) {
        lineRenderer.SetPositions(listLocalLookAtGlobalCoordinates.ToArray());
    }

    void GenerateHeatMap() {
        foreach (Vector3 v in listLocalLookAtGameboardCoordinates)
        {
            GameObject placement = Instantiate(
                        HeatSphere,
                        this.transform.TransformPoint(new Vector3(v.x, v.y, v.z) + new Vector3(0f, 1f, 0f)),
                        Quaternion.identity,
                        HeatSphere.transform
                        );
            listLocalLookAtGlobalCoordinates.Add(placement.transform.position);
        }
    }

    void CreateGameBoard()
    {
        bool isAlternateTile = false;
        for (int column = 0; column < Columns + 1; column++)
        {
            for (int row = 0; row < Rows + 1; row++)
            {

                // Corner Placement zones
                GameObject placement = Instantiate(
                        PlacementSquare,
                        new Vector3(row * 0.03f, 0, column * 0.03f) + StartPos.position - new Vector3(0.015f, -0.01f, 0.015f),
                        Quaternion.identity,
                        transform
                        );
                placement.GetComponent<TileCoordinates>().Row = row;
                placement.GetComponent<TileCoordinates>().Column = column;

                // Tiles
                if (column < Columns && row < Rows)
                {
                    if (isAlternateTile)
                    {
                        GameObject tile = Instantiate(
                            TileSquare1,
                            new Vector3(row * 0.03f, 0, column * 0.03f) + StartPos.position,
                            Quaternion.identity,
                            transform
                            );

                        tile.GetComponent<TileCoordinates>().Row = row;
                        tile.GetComponent<TileCoordinates>().Column = column;
                    }
                    else
                    {
                        GameObject tile = Instantiate(
                            TileSquare2,
                            new Vector3(row * 0.03f, 0, column * 0.03f) + StartPos.position,
                            Quaternion.identity,
                            transform
                            );
                        tile.GetComponent<TileCoordinates>().Row = row;
                        tile.GetComponent<TileCoordinates>().Column = column;
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
