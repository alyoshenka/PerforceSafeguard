using System;
using System.Collections.Generic;
using UnityEngine;

// pathfinding node
public class Node : IComparable
{
    public float g;
    public float h;
    public float f;
    public float traversalCost;
    public Node previousNode;
    public List<Node> nextNodes;
    public Index idx;

    public Node(int cost, Index _idx)
    {
        previousNode = null;
        nextNodes = new List<Node>();
        g = h = f = Int32.MaxValue;
        traversalCost = cost;
        idx = _idx;
    }

    private Node() { }

    public int CompareTo(object obj) // sort by g for dijkstra!!
    {
        Node b = (Node)obj; // run priority queue off Fs
        if (g < b.g) { return -1; }
        if (b.g > g) { return 1; }
        return 0;
    }

    // manhattan distance
    public float CalculateH(Node goalNode)
    {
        h = Math.Abs(goalNode.idx.x - idx.x) + Math.Abs(goalNode.idx.y - idx.y);
        return h;
    }
}

public class Djikstra : PathfindingAlgorithm
{
    public List<Index> Pathfind(Node start, Node goal, Node[,] nodeMap)
    {
        // reset
        openList.Clear();
        closedList.Clear();
        openSet.Clear();
        closedSet.Clear();
        foreach(Node n in nodeMap)
        {
            n.previousNode = null;
            n.CalculateH(goal);
        }
        currentNode = start;
        currentNode.g = 0;
        openList.Add(currentNode);
        openSet.Add(currentNode);

        // search nodes
        while (!closedSet.Contains(goal)) // not good form for a while loop
        {
            currentNode = openList[0];
            foreach(Node n in currentNode.nextNodes)
            {
                if(null == n.previousNode || currentNode.g + currentNode.traversalCost < n.g)
                {
                    n.previousNode = currentNode;
                    n.g = currentNode.g + currentNode.traversalCost;

                    if(!closedSet.Contains(n) && !openSet.Contains(n))
                    {
                        openList.Add(n);
                        openSet.Add(n);
                    } 

                }
            }
            openList.Remove(currentNode);
            openSet.Remove(currentNode);
            closedList.Add(currentNode);
            closedSet.Add(currentNode);

            if (openList.Count <= 0)
            {
                return null;
                // break;
                // ^ works for corners but breaks with walls
            }

            openList.Sort();                          
        }

        List<Index> path = new List<Index>();
        do
        {
            if (null == currentNode) { break; } // safeguards against to path, will do what it can
            path.Add(currentNode.idx);
            currentNode = currentNode.previousNode;
        }
        while (currentNode != start);

        return path;
    }
}

public class D2 : PathfindingAlgorithm
{

    public List<Index> Pathfind(Node start, Node goal, Node[,] nodeMap)
    {
        // reset
        openList.Clear();
        closedList.Clear();
        openSet.Clear();
        closedSet.Clear();
        foreach (Node n in nodeMap)
        {
            n.previousNode = null;
            // n.CalculateH(goal);
            n.g = Int32.MaxValue;
        }
        currentNode = start;
        currentNode.g = 0;
        openList.Add(currentNode);
        openSet.Add(currentNode);        

        do
        {
            currentNode = openList[0];
            openList.Remove(currentNode);
            openSet.Remove(currentNode);
            closedList.Add(currentNode);
            closedSet.Add(currentNode);

            foreach (Node n in currentNode.nextNodes)
            {
                float dist = currentNode.g + currentNode.traversalCost;
                if (dist < n.g)
                {
                    n.g = dist;
                    n.previousNode = currentNode;
                }
                if (!openSet.Contains(n) && !closedSet.Contains(n))
                {                    
                    openList.Add(n);
                    openSet.Add(n);
                }
            }
            openList.Sort();
        }
        while (openList.Count > 0 && currentNode != goal);

        if(goal != currentNode) { return null; }

        List<Index> path = new List<Index>();
        do
        {
            if (null == currentNode) { Debug.Log(path.Count); return null; } // safeguards against no path, will do what it can
            path.Add(currentNode.idx);
            currentNode = currentNode.previousNode;
        }
        while (currentNode != start);

        return path;
    }
}

public abstract class PathfindingAlgorithm
{
    protected List<Node> openList;
    protected List<Node> closedList;
    protected HashSet<Node> openSet;
    protected HashSet<Node> closedSet;
    protected Node currentNode;
    protected Node previousNode;

    public PathfindingAlgorithm()
    {
        openList = new List<Node>();
        closedList = new List<Node>();
        openSet = new HashSet<Node>();
        closedSet = new HashSet<Node>();
    }
}


