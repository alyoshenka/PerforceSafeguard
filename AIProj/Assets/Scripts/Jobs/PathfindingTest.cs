using System;
using System.Collections.Generic;
using UnityEngine;

namespace Alexi.Jobs
{
    public struct HelperNode
    {
        public static int idCounter;

        public int id;

        public Node toNode()
        {
            return new Node();
        }

        public bool Equals(Node n)
        {
            return id == n.id;
        }
    }

    // pathfinding node
    public struct Node : IComparable
    {
        public float g;
        public float h;
        public float f;
        public float traversalCost;
        public HelperNode previousNode;
        public List<Node> nextNodes;
        public Index idx;

        public int id;
        public static int idCounter;

        public HelperNode toHelperNode()
        {
            return new HelperNode();
        }

        public Node(int cost, Index _idx)
        {
            nextNodes = new List<Node>();
            g = h = f = Int32.MaxValue;
            traversalCost = cost;
            idx = _idx;
            previousNode = D2.dummyNode;
            id = ++idCounter;
        }

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

    public class D2 : PathfindingAlgorithm
    {
        public static HelperNode dummyNode;

        public List<Index> Pathfind(Node start, Node goal, Node[,] nodeMap, Index size)
        {
            // reset
            openList.Clear();
            closedList.Clear();
            openSet.Clear();
            closedSet.Clear();
            for (int y = 0; y < size.y; y++)
            {
                for(int x = 0; x < size.x; x++)
                {
                    nodeMap[y, x].previousNode = dummyNode;
                    // n.CalculateH(goal);
                    nodeMap[y, x].g = Int32.MaxValue;
                }
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

                for(int y = 0; y < size.y; y++)
                {
                    for(int x = 0; x < size.x; x++)
                    {
                        float dist = currentNode.g + currentNode.traversalCost;
                        if (dist < nodeMap[y, x].g)
                        {
                            nodeMap[y, x].g = dist;
                            nodeMap[y, x].previousNode = currentNode.toHelperNode();
                        }
                        if (!openSet.Contains(nodeMap[y, x]) && !closedSet.Contains(nodeMap[y, x]))
                        {
                            openList.Add(nodeMap[y, x]);
                            openSet.Add(nodeMap[y, x]);
                        }
                    }
                }
                openList.Sort();
            }
            while (openList.Count > 0 /*&& currentNode != goal*/);

            /*
            if (goal != currentNode) { return null; }

            List<Index> path = new List<Index>();
            do
            {
                if (null == currentNode) { Debug.Log(path.Count); return null; } // safeguards against no path, will do what it can
                path.Add(currentNode.idx);
                currentNode = currentNode.previousNode;
            }
            while (currentNode != start);
            */

            // return path;
            return null;
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
}



