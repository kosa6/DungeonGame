using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class DungeonGenerator : MonoBehaviour {

    public int sizeOfDungeonX;
    public int sizeOfDungeonY;
    public int NumberOfAttemptsToGenerateARoom;
    public int MaxNumberOfRoom;

    [SerializeField]
    private GameObject[] floor;
    [SerializeField]
    private GameObject[] outerWall;
    private Transform parentOfRoom;

    private List<Room> rooms;

    public Grid[,] grids;

    private MazeGenerator maze;

    private List<MazeGenerator> mazes;

    // Use this for initialization
    void Start () {
        GenerateMap();
        GenerateMazes();
        connectRoomsWithMaze();
        InstantiateMaze();
    }
    private void GenerateMazes()
    {
        mazes = new List<MazeGenerator>();
        maze = new MazeGenerator(this.sizeOfDungeonX, this.sizeOfDungeonY);
    }
    private void OnDrawGizmos()
    {
        foreach(Grid grid in maze.grids)
        {
            if (grid!=null && grid.getParent()!=null)
            {
                Debug.DrawLine(new Vector3(grid.getParent().getPositionX(), grid.getParent().getPositionY()), new Vector3(grid.getPositionX(), grid.getPositionY()));
            }
        }
    }
    private void InstantiateMaze()
    {
        foreach(Grid grid in maze.grids)
        {
            if(grid != null && grid.typeOfGrid == Grid.TypeOfGrid.MazeFloor)
            {
                int random = Random.Range(0, floor.Length);
                Instantiate(this.floor[random], new Vector2(grid.getPositionX(),grid.getPositionY()), Quaternion.identity);
            }
        }
    }
    private void GenerateMap()
    {
        rooms = new List<Room>();
        InitializeAllGrids();
        GenerateRooms();
    }

    private void GenerateRooms()
    {
        int attempts;
        int numberOfRooms = Random.Range(5, MaxNumberOfRoom);
        for (int i=0; i<numberOfRooms+1; i++)
        {
            attempts = 0;
            int randomSizeX = Random.Range(5, 11);
            int randomSizeY = Random.Range(5, 11);
            do
            {
                int randomStartingPositionX = Random.Range(0, sizeOfDungeonX-randomSizeX);
                int randomStartingPositionY = Random.Range(0, sizeOfDungeonY-randomSizeY);
                if (checkIfCanPlaceRoom(randomStartingPositionX, randomStartingPositionY,randomSizeX,randomSizeY) == true)
                {
                    GenerateRoom(randomStartingPositionX, randomStartingPositionY, randomSizeX, randomSizeY);
                    break;
                }
                else
                {
                    attempts++;
                }
            } while (attempts <= this.NumberOfAttemptsToGenerateARoom);
        }
    }

    private void GenerateRoom(int startPositionX,int startPositionY,int sizeX,int sizeY)
    {
        int random;
        Transform parentOfRoom = GameObject.Find("Grids").transform;
        List<Grid> gridsInRoom = new List<Grid>();
        List<Grid> walls = new List<Grid>();
        for (int i = startPositionX; i<=sizeX + startPositionX; i++)
        {
            for(int j = startPositionY; j<=sizeY + startPositionY; j++)
            {
                if(i==startPositionX || j == startPositionY || i==sizeX+startPositionX || j == sizeY + startPositionY)
                {
                    if(i == startPositionX && j == startPositionY || i == startPositionX + sizeX && j == startPositionY || i == startPositionX && j == startPositionY + sizeY 
                        || i == startPositionX + sizeX && j == startPositionY + sizeY)
                    {
                        grids[i, j].setTypeOfGrid(Grid.TypeOfGrid.ConerWall);
                    }
                    else
                    {
                        grids[i, j].setTypeOfGrid(Grid.TypeOfGrid.Wall);
                        walls.Add(grids[i, j]);
                    }
                }
                else
                {
                    GameObject InitializeGameObject;
                    random = Random.Range(0, floor.Length);
                    InitializeGameObject = Instantiate(this.floor[random].gameObject, new Vector2(Mathf.Round(i), Mathf.Round(j)), Quaternion.identity);
                    InitializeGameObject.transform.SetParent(parentOfRoom);
                    grids[i, j].setTypeOfGrid(Grid.TypeOfGrid.Floor);
                    gridsInRoom.Add(grids[i,j]);
                }
            }
        }
        Room roomToAdd = new Room(sizeX, sizeY, new Vector2(startPositionX, startPositionY),gridsInRoom , walls);
        rooms.Add(roomToAdd);
    }

    private bool checkIfCanPlaceRoom(int startPositionX, int startPositionY, int randomSizeX,int randomSizeY)
    {
        for (int i = startPositionX; i <= randomSizeX + startPositionX; i++)
        {
            for (int j = startPositionY; j <= randomSizeY + startPositionY ; j++)
            {
                if (grids[i,j].typeOfGrid == Grid.TypeOfGrid.Floor)
                {
                    return false;
                }
            }
        }
        return true;
    }
    private void InitializeAllGrids()
    {
        grids = new Grid[sizeOfDungeonX + 1, sizeOfDungeonY + 1];
        for(int i=0; i<sizeOfDungeonX; i++)
        {
            for(int j=0; j<sizeOfDungeonY; j++)
            {
                grids[i, j] = new Grid(i, j, null, Grid.TypeOfGrid.Blank);
            }
        }
        for (int i = 0; i < sizeOfDungeonX; i++)
        {
            for (int j = 0; j < sizeOfDungeonY; j++)
            {
                AddNeightbour(grids[i, j]);
            }
        }
    }
    private void AddNeightbour(Grid grid)
    {
        int positionX = grid.getPositionX();
        int positionY = grid.getPositionY();
        if (positionX - 1 >= 0)
        {
            if(this.grids[positionX - 1, positionY] != null)
            {
                grid.neightbourNotToRemove.Add(this.grids[positionX - 1, positionY]);
                grid.neightboutFourWay.Add(this.grids[positionX - 1, positionY]);
            }
        }
        if (positionX + 1 <= sizeOfDungeonX)
        {
            if (this.grids[positionX + 1, positionY] != null)
            {
                grid.neightbourNotToRemove.Add(this.grids[positionX + 1, positionY]);
                grid.neightboutFourWay.Add(this.grids[positionX + 1, positionY]);
            }
        }
        if (positionY - 1 >= 0)
        {
            if (this.grids[positionX, positionY - 1] != null)
            {
                grid.neightbourNotToRemove.Add(this.grids[positionX, positionY - 1]);
                grid.neightboutFourWay.Add(this.grids[positionX, positionY - 1]);
            }
        }
        if (positionY + 1 <= sizeOfDungeonY)
        {
            if (this.grids[positionX, positionY + 1] != null)
            {
                grid.neightbourNotToRemove.Add(this.grids[positionX, positionY + 1]);
                grid.neightboutFourWay.Add(this.grids[positionX, positionY + 1]);
            }
        }
        if (positionX - 1 >= 0 && positionY + 1 <= sizeOfDungeonY)
        {
            if (this.grids[positionX - 1, positionY + 1] != null)
            {
                grid.neightbourNotToRemove.Add(this.grids[positionX - 1, positionY + 1]);
            }
        }
        if (positionX - 1 >= 0 && positionY - 1 >= 0)
        {
            if (this.grids[positionX - 1, positionY - 1] != null)
            {
                grid.neightbourNotToRemove.Add(this.grids[positionX - 1, positionY - 1]);
            }
        }
        if (positionX + 1 <= sizeOfDungeonX && positionY + 1 <= sizeOfDungeonY)
        {
            if (this.grids[positionX + 1, positionY + 1] != null)
            {
                grid.neightbourNotToRemove.Add(this.grids[positionX + 1, positionY + 1]);
            }
        }
        if (positionX + 1 <= sizeOfDungeonX && positionY - 1 >= 0)
        {
            if (this.grids[positionX + 1, positionY - 1] != null)
            {
                grid.neightbourNotToRemove.Add(this.grids[positionX + 1, positionY - 1]);
            }
        }
    }
    private void connectRoomsWithMaze()
    {
        int numberOfConnection = Random.Range(1, 2);
        int countOfConnection = 0;
        List<Grid> walls = new List<Grid>();
        for(int i=0; i<rooms.Count; i++)
        {
            walls = rooms[i].walls;
            while (countOfConnection <= numberOfConnection)
            {
                Grid wall = walls[Random.Range(0,walls.Count-1)];
                if(wall.numberOfFloorAround() == 2 )
                {
                    Debug.Log("ok");
                    wall.typeOfGrid = Grid.TypeOfGrid.MazeFloor;
                    rooms[i].isConected = true;
                    countOfConnection++;
                }
                else
                {
                    walls.Remove(wall);
                }
            }
            countOfConnection = 0;
        }
    }
}
