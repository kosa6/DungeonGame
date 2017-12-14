using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeGenerator {

    public Grid[,] grids;
    private int sizeOfMapX, sizeOfMapY;

    private Stack<Grid> VisitedNodeWithOutNeightbour;

    public MazeGenerator(int sizeOfMapX,int sizeOfMapY)
    {
        this.sizeOfMapX = sizeOfMapX;
        this.sizeOfMapY = sizeOfMapY;
        this.grids = GameObject.Find("DungeonMenager").GetComponent<DungeonGenerator>().grids;
        this.VisitedNodeWithOutNeightbour = new Stack<Grid>();
        while (FindStartingNode() != null)
        {
            Debug.Log("Dlaczeg");
            GenerateMaze();
        }
    }
    private void AddNeightbourToNode(Grid grid)
    {
        int positionX = grid.getPositionX();
        int positionY = grid.getPositionY();
        if (positionX - 2 >= 0 )
        {
            if (this.grids[positionX - 2, positionY] != null && this.grids[positionX - 2, positionY].getVisited() == false && GridCheckNeightBour(grids[positionX - 2, positionY]) == true)
            {
                grid.neightbour.Add(this.grids[positionX - 2, positionY]);
            }
        }
        if (positionX + 2 <= sizeOfMapX)
        {
            if (this.grids[positionX + 2, positionY] != null && this.grids[positionX + 2, positionY].getVisited() == false && GridCheckNeightBour(grids[positionX + 2, positionY]) == true)
            {
                grid.neightbour.Add(this.grids[positionX + 2, positionY]);
            }
        }
        if (positionY - 2 >= 0)
        {
            if (this.grids[positionX, positionY - 2] != null && this.grids[positionX, positionY - 2].getVisited() == false && GridCheckNeightBour(grids[positionX, positionY - 2]) == true)
            {
                grid.neightbour.Add(this.grids[positionX, positionY - 2]);
            }
        }
        if (positionY + 2 <= sizeOfMapY)
        {
            if (this.grids[positionX, positionY + 2] != null && this.grids[positionX, positionY + 2].getVisited() == false && GridCheckNeightBour(grids[positionX, positionY + 2]) == true)
            {
                grid.neightbour.Add(this.grids[positionX, positionY + 2]);
            }
        }
    }
    public void GenerateMaze()
    {
        bool keepMoving = true;
        Grid currGrid = FindStartingNode();
        Grid mainParent = currGrid;
        while (keepMoving)
        {
            currGrid.setVisitedToTrue();
            AddNeightbourToNode(currGrid);
            if((currGrid.neightbour.Count > 0))
            {
                int r = (Random.Range(0, currGrid.neightbour.Count));
                Grid tempGrid = currGrid.neightbour[r];
                if(tempGrid.getVisited() == true)
                {
                    currGrid.neightbour.Remove(tempGrid);
                }
                else
                {
                    tempGrid.setParent(currGrid);
                    currGrid.neightbour.Remove(tempGrid);
                    this.VisitedNodeWithOutNeightbour.Push(tempGrid);
                    conectNode(currGrid, tempGrid);
                    currGrid.typeOfGrid = Grid.TypeOfGrid.MazeFloor;
                    currGrid = tempGrid;
                }
            }
            else if(this.VisitedNodeWithOutNeightbour.Count != 0)
            {
                currGrid.typeOfGrid = Grid.TypeOfGrid.MazeFloor;
                currGrid = this.VisitedNodeWithOutNeightbour.Pop();
            }
            else if(currGrid.neightbour.Count == 0 && this.VisitedNodeWithOutNeightbour.Count == 0)
            {
                if (currGrid == mainParent)
                {
                    currGrid.typeOfGrid = Grid.TypeOfGrid.NotStartingNode;
                }
                keepMoving = false;
            }
        }
    }
    private Grid FindStartingNode()
    {
        for(int i=0; i<sizeOfMapX; i++)
        {
            for(int j=0; j<sizeOfMapY; j++)
            {
                if(GridCheckNeightBour(grids[i,j]) == true)
                {
                    return this.grids[i, j];
                }
            }
        }
        return null;
    }
    private bool GridCheckNeightBour(Grid grid)
    {
        if(grid.typeOfGrid == Grid.TypeOfGrid.Blank)
        {
            for (int k = 0; k < grid.neightbourNotToRemove.Count; k++)
            {
                if (grid.neightbourNotToRemove[k].typeOfGrid == Grid.TypeOfGrid.Floor || grid.neightbourNotToRemove[k].typeOfGrid == Grid.TypeOfGrid.MazeFloor)
                {
                    return false;
                }
            }
            return true;
        }
        return false;
    }
    private void conectNode(Grid one,Grid two)
    {
        Vector2 Direction = new Vector2(two.getPositionX(), two.getPositionY()) - new Vector2(one.getPositionX(),one.getPositionY());
        int posX = one.getPositionX();
        int posY = one.getPositionY();
        if(Direction.x > 0)
        {
            grids[posX + 1, posY].typeOfGrid = Grid.TypeOfGrid.MazeFloor;
            grids[posX + 1, posY].setVisitedToTrue();
            grids[posX + 1, posY].setParent(one);
            two.setParent(grids[posX + 1, posY]);
        }
        if (Direction.x < 0)
        {
            grids[posX - 1, posY].typeOfGrid = Grid.TypeOfGrid.MazeFloor;
            grids[posX - 1, posY].setVisitedToTrue();
            grids[posX - 1, posY].setParent(one);
            two.setParent(grids[posX - 1, posY]);
        }
        if (Direction.y > 0)
        {
            grids[posX, posY +1].typeOfGrid = Grid.TypeOfGrid.MazeFloor;
            grids[posX, posY + 1].setVisitedToTrue();
            grids[posX, posY + 1].setParent(one);
            two.setParent(grids[posX, posY + 1]);
        }
        if (Direction.y < 0)
        {
            grids[posX, posY - 1].typeOfGrid = Grid.TypeOfGrid.MazeFloor;
            grids[posX, posY - 1].setVisitedToTrue();
            grids[posX, posY - 1].setParent(one);
            two.setParent(grids[posX, posY - 1]);
        }
    }
}
