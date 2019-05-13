using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;

// various helper classes 

// game stages
public enum Stage { placeTiles, placeDefenders, defend };

// tile types
public enum TileType { ground, wall, turret, spawn, objective };

public struct Index
{
    public int y, x;

    public Index(int _x, int _y)
    {
        x = _x;
        y = _y;
    }
}

   


