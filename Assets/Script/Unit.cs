using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseUnit 
{
    public Tile tile;
    public int UnitType; //Changer le type quand un type d'unit� serra cr�e
    public int speed; 

    public BaseUnit(Tile spawnTile,int speed) {
        this.tile= spawnTile;
        this.speed = speed;
    }
}
