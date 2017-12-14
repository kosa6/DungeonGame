using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room {

    private int sizeX ,sizeY;
    private Vector2 startingPosition;
    private List<Grid> gridsInRoom;

    public List<Grid> walls;

    public bool isConected;

    public Room(int sizeX,int sizeY,Vector2 startingPosition,List<Grid> gridsInRoom, List<Grid> walls)
    {
        this.sizeX = sizeX;
        this.sizeY = sizeY;
        this.startingPosition = startingPosition;
        this.gridsInRoom = gridsInRoom;
        this.walls = walls;
        this.isConected = false;
    }

    public int getSizeX()
    {
        return this.sizeX;
    }
    public int getSizeY()
    {
        return this.sizeY;
    }
    public Vector2 getStartingPosition()
    {
        return this.startingPosition;
    }
}
