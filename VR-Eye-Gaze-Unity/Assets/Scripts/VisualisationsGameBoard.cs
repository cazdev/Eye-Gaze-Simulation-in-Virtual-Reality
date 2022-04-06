using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Globalization;
using System;

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
    List<Vector2> listWhitePlacementCoordinates = new List<Vector2>();
    List<Vector2> listBlackPlacementCoordinates = new List<Vector2>();
    List<Vector3> listLocalLookAtGameboardCoordinates = new List<Vector3>();
    List<Vector3> listLocalLookAtGlobalCoordinates = new List<Vector3>();

    private const string whitePlacementsFile =  @"C:\white-placed.csv";
    private const string blackPlacementsFile = @"C:\black-placed.csv";
    private const string objectLookedAtFile = @"C:\looking-at-objects.csv";

    // Start is called before the first frame update
    void Start()
    {
        // Generate basic gameboard
        CreateGameBoard();

        //Reading data
        ReadObjectLookedAt();
        ReadPlacementData();

        // Visualisations
        LoadPreviousGame();
        GenerateHeatMap();
        DrawLinesBetweenPoints(Color.red);
    }

    void ReadPlacementData()
    {
        try
        {
            using (var reader = new StreamReader(whitePlacementsFile))
            {
                string headerLine = reader.ReadLine();
                while (!reader.EndOfStream)
                {
                    // Add every x_interval line to prevent too much data and running out of memeory
                    var line = reader.ReadLine();
                    var values = line.Split(',');

                    Vector2 newPoint = new Vector2(Int32.Parse(values[2]), Int32.Parse(values[3]));
                    listWhitePlacementCoordinates.Add(newPoint);
                }
            }
        }
        catch (System.Exception ex)
        {
            Debug.LogError(ex);
        }
        try
        {
            using (var reader = new StreamReader(blackPlacementsFile))
            {
                string headerLine = reader.ReadLine();
                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();
                    var values = line.Split(',');

                    Vector2 newPoint = new Vector2(Int32.Parse(values[2]), Int32.Parse(values[3]));
                    listBlackPlacementCoordinates.Add(newPoint);
                }
            }
        }
        catch (System.Exception ex)
        {
            Debug.LogError(ex);
        }
    }

    void ReadObjectLookedAt()
    {
        try
        {
            using (var reader = new StreamReader(objectLookedAtFile))
            {
                string headerLine = reader.ReadLine();
                int count = 0;
                while (!reader.EndOfStream)
                {
                    // Add every x_interval line to prevent too much data and running out of memeory
                    if (count % data_interval == 0)
                    {
                        var line = reader.ReadLine();
                        var values = line.Split(',');

                        // Creating list of vector3s with gameboard points
                        if (values[1].Equals("event_looked_at_Gameboard"))
                        {
                            Vector3 newPoint = new Vector3(float.Parse(values[2], CultureInfo.InvariantCulture.NumberFormat),
                                                        float.Parse(values[3], CultureInfo.InvariantCulture.NumberFormat),
                                                        float.Parse(values[4], CultureInfo.InvariantCulture.NumberFormat));
                            listLocalLookAtGameboardCoordinates.Add(newPoint);
                        }
                    }

                    count++;
                }
            }
        }
        catch (System.Exception ex)
        {
            Debug.LogError(ex);
        }
    }

    void LoadPreviousGame()
    {
        foreach (Vector2 v in listWhitePlacementCoordinates)
        {
            GameObject placement = Instantiate(
                        WhiteTokenClone,
                        new Vector3(v.x * 0.03f, 0, v.y * 0.03f) + StartPos.position - new Vector3(0.015f, -0.015f, 0.015f),
                        Quaternion.identity
                        );
        }
        foreach (Vector2 v in listBlackPlacementCoordinates)
        {
            GameObject placement = Instantiate(
                        BlackTokenClone,
                        new Vector3(v.x * 0.03f, 0, v.y * 0.03f) + StartPos.position - new Vector3(0.015f, -0.015f, 0.015f),
                        Quaternion.identity
                        );
        }
    }

    void DrawLinesBetweenPoints(Color color)
    {
        lineRenderer.SetPositions(listLocalLookAtGlobalCoordinates.ToArray());
    }

    void GenerateHeatMap()
    {
        foreach (Vector3 v in listLocalLookAtGameboardCoordinates)
        {
            GameObject placement = Instantiate(
                        HeatSphere,
                        this.transform.TransformPoint(new Vector3(v.x, v.y, v.z) + new Vector3(0f, 1f, 0f)),
                        Quaternion.identity
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
}
