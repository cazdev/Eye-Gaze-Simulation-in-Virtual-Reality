using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateGameBoard : MonoBehaviour
{
    public GameObject TileSquare1;
    public GameObject TileSquare2;
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

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
