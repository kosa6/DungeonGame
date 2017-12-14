using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid {
    public enum TypeOfGrid
    {
        Floor,
        Wall,
        MazeFloor,
        Door,
        Blank,
        NotStartingNode,
        ConerWall
    }

    public int positionX;
    public int positionY;
    public TypeOfGrid typeOfGrid;

    private GameObject gridObject;

    public List<Grid> neightbourNotToRemove;
    public List<Grid> neightbour;
    public List<Grid> neightboutFourWay;
    public Grid parent;
    public bool visited;


    public Grid(int positionX,int positionY,GameObject gridObject,TypeOfGrid typeOfGrid)
    {
        this.positionX = positionX;
        this.positionY = positionY;
        this.gridObject = gridObject;
        this.typeOfGrid = typeOfGrid;
        this.neightbour = new List<Grid>();
        this.neightbourNotToRemove = new List<Grid>();
        this.neightboutFourWay = new List<Grid>();
        this.parent = null;
        this.visited = false;
    }

    public int getPositionX()
    {
        return this.positionX;
    }

    public int getPositionY()
    {
        return this.positionY;
    }
    public bool getVisited()
    {
        return this.visited;
    }
    public void setVisitedToTrue()
    {
        this.visited = true;
    }
    public Grid getParent()
    {
        return this.parent;
    }
    public void setParent(Grid parent)
    {
        this.parent = parent;
    }
    public void setTypeOfGrid(TypeOfGrid typeOfGrid)
    {
        this.typeOfGrid = typeOfGrid;
    }
    public int numberOfFloorAround()
    {
        int count = 0;
        for(int i=0; i<neightboutFourWay.Count; i++)
        {
            if(neightboutFourWay[i].typeOfGrid == TypeOfGrid.MazeFloor || neightboutFourWay[i].typeOfGrid == TypeOfGrid.Floor)
            {
                count++;
            }
        }
        return count;
    }
}
