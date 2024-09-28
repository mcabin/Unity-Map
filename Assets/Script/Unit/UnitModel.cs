using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitModel
{
    public int life;
    public int speed;
    private int coordW, coordH;
    UnitType type;

    public UnitModel(int life, int speed, int coordW, int coordH)
    {
        this.life = life;
        this.speed = speed;
        this.coordW = coordW;
        this.coordH = coordH;
    }

    public void move(int w, int h)
    {
        coordH = h;
        coordW = w;
    }

    public (int w,int h) getCoordinate()
    {
        return (coordW,coordH);
    }
}
