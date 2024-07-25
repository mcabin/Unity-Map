using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class ElevationTile : Tile
{
    public AltitudeType neighboorNorth { get; private set; }
    public AltitudeType neighboorSouth { get; private set; }
    public AltitudeType neighboorEast { get; private set;}
    public AltitudeType neighboorWest { get; private set; }

    public TileEnum.ElevEnum elevType;

    //Nombre de rotation a 90 degrés que l'objet doit faire
    public int direction;
    public int nbPlateau=0;
    public int nbElevation=0;
    public int nbTotal => nbElevation+nbPlateau;
   public ElevationTile(BiomeType biome, int coordX, int coordY, AltitudeType altitude) : base(biome, coordX, coordY, altitude)
    {
        
    }
    private void countNeighboor(AltitudeType neighboor)
    {
        nbPlateau += (neighboor.level >= (int)TileEnum.AltitudeEnum.PLATEAU) ? 1 : 0;
        nbElevation += (neighboor.type == TileEnum.AltitudeEnum.ELEVATION) ? 1 : 0;

    }

    public void setNorth(AltitudeType north)
    {
        nbPlateau = 0;
        nbElevation = 0;
        neighboorNorth = north;
        countNeighboor(neighboorEast); countNeighboor(neighboorWest);
        countNeighboor(neighboorNorth); countNeighboor(neighboorSouth);
        setTileType();
    }
    public void setSouth(AltitudeType south)
    {
        nbPlateau = 0;
        nbElevation = 0;
        neighboorSouth = south;
        countNeighboor(neighboorEast); countNeighboor(neighboorWest);
        countNeighboor(neighboorNorth); countNeighboor(south);
        setTileType();
    }
    public void setWest(AltitudeType west)
    {
        nbPlateau = 0;
        nbElevation = 0;
        neighboorWest = west;
        countNeighboor(neighboorEast); countNeighboor(neighboorWest);
        countNeighboor(neighboorNorth); countNeighboor(neighboorSouth);
        setTileType();
    }
    public void setEast(AltitudeType east)
    {
        nbPlateau = 0;
        nbElevation = 0;
        neighboorEast = east;
        countNeighboor(neighboorEast); countNeighboor(neighboorWest);
        countNeighboor(neighboorNorth); countNeighboor(neighboorSouth);
        setTileType();
    }

    public void setNeighboors(AltitudeType north,AltitudeType south,AltitudeType west,AltitudeType east)
    {
        nbPlateau = 0;
        nbElevation = 0;
        neighboorEast=east;
        neighboorWest=west;
        neighboorNorth=north;
        neighboorSouth=south;

        countNeighboor(east); countNeighboor(west);
        countNeighboor(north); countNeighboor(south);
        setTileType();
    }


    public void setTileType()
    {
        direction = -1;
        if (nbTotal == 0)
        {
            //Petite Colline
        }
        else if(nbTotal == 1)
        {
            //Inclinaison vers 
        }
        else if (nbTotal == 2)
        {
            //Deux plateaux
            if (nbPlateau == 2)
            {
                if (isDuoPlateau(neighboorNorth,neighboorSouth))
                {
                    //Pas de rotation + tuile elev parallele
                }
                else if (isDuoPlateau(neighboorEast,neighboorWest))
                {
                    //rotation 90 parallele aussi
                }
                else if(isDuoPlateau(neighboorNorth, neighboorEast))
                {
                    //Direction par default
                    direction = 0;
                    elevType = TileEnum.ElevEnum.ELEV_PP_VIRAGE;
                }
                else if (isDuoPlateau(neighboorEast, neighboorSouth))
                {
                    direction = 1;
                    elevType = TileEnum.ElevEnum.ELEV_PP_VIRAGE;
                }
                else if (isDuoPlateau(neighboorSouth, neighboorWest))
                {
                    direction=2;
                    elevType = TileEnum.ElevEnum.ELEV_PP_VIRAGE;

                }
                else if (isDuoPlateau(neighboorWest, neighboorNorth))
                {
                    direction = 3;
                    elevType = TileEnum.ElevEnum.ELEV_PP_VIRAGE;
                }
            }
            //Deux elevation
            else if (nbElevation == 2)
            {
                if (isDuoElevation(neighboorNorth, neighboorSouth))
                {
                    direction = 0;
                    elevType = TileEnum.ElevEnum.ELEV_PARA_EE;
                }
                else if (isDuoElevation(neighboorEast, neighboorWest))
                {
                    direction = 90;
                    elevType = TileEnum.ElevEnum.ELEV_PARA_EE;
                }
                else if (isDuoElevation(neighboorNorth, neighboorEast))//neighboorWest, neighboorNorth
                {
                    //Direction par default
                    direction = 0;
                    elevType = TileEnum.ElevEnum.ELEV_EE_VIRAGE;
                }
                else if (isDuoElevation(neighboorEast, neighboorSouth))
                {
                    direction = 1;
                    elevType = TileEnum.ElevEnum.ELEV_EE_VIRAGE;
                }
                else if (isDuoElevation(neighboorSouth, neighboorWest))
                {
                    direction = 2;
                    elevType = TileEnum.ElevEnum.ELEV_EE_VIRAGE;

                }
                else if (isDuoElevation(neighboorWest, neighboorNorth))
                {
                    direction = 3;
                    elevType = TileEnum.ElevEnum.ELEV_EE_VIRAGE;
                }
            }
            //Un de chaque
            else
            {
                if (isDuoElevationPlateau(neighboorNorth, neighboorSouth))
                {
                    //Pas de rotation + tuile elev parallele
                }
                else if (isDuoElevationPlateau(neighboorSouth, neighboorNorth))
                {
                    //Pas de rotation + tuile elev parallele

                }
                else if (isDuoElevationPlateau(neighboorEast, neighboorWest))
                {
                    //rotation 90 parallele aussi
                }
                else if (isDuoElevationPlateau(neighboorWest, neighboorEast))
                {
                    //rotation 90 parallele aussi
                }

                //Les virages
                //PE VIRAGE
                else if (isDuoElevationPlateau(neighboorEast, neighboorNorth))
                {
                    direction = 0;
                    elevType = TileEnum.ElevEnum.ELEV_PE_VIRAGE;
                }
                else if (isDuoElevationPlateau(neighboorSouth, neighboorEast))
                {
                    direction = 1;
                    elevType = TileEnum.ElevEnum.ELEV_PE_VIRAGE;
                }
                else if(isDuoElevationPlateau(neighboorWest,neighboorSouth))
                {
                    direction = 2;
                    elevType = TileEnum.ElevEnum.ELEV_PE_VIRAGE;
                }
                else if (isDuoElevationPlateau(neighboorNorth, neighboorWest)){
                    direction = 3;
                    elevType = TileEnum.ElevEnum.ELEV_PE_VIRAGE;
                }

                //EP VIRAGE
                else if (isDuoElevationPlateau(neighboorNorth, neighboorEast))
                {
                    direction = 0;
                    elevType = TileEnum.ElevEnum.ELEV_EP_VIRAGE;

                }
                else if (isDuoElevationPlateau(neighboorEast, neighboorSouth))
                {
                    direction = 1;
                    elevType = TileEnum.ElevEnum.ELEV_EP_VIRAGE;

                }
                else if (isDuoElevationPlateau(neighboorSouth, neighboorWest))
                {
                    direction = 2;
                    elevType = TileEnum.ElevEnum.ELEV_EP_VIRAGE;

                }
                else if(isDuoElevationPlateau(neighboorWest,neighboorNorth))
                {
                    direction = 3;
                    elevType = TileEnum.ElevEnum.ELEV_EP_VIRAGE;
                }
            }
        }
        else if (nbTotal==3)
        {
            if(3==nbPlateau )
            {
                if(neighboorSouth.type==TileEnum.AltitudeEnum.PLAIN)
                {
                    direction=0;
                    elevType = TileEnum.ElevEnum.ELEV_3_PLAT;
                }
                if (neighboorWest.type == TileEnum.AltitudeEnum.PLAIN)
                {
                    direction = 1;
                    elevType = TileEnum.ElevEnum.ELEV_3_PLAT;
                }
                if (neighboorNorth.type == TileEnum.AltitudeEnum.PLAIN)
                {
                    direction = 2;
                    elevType = TileEnum.ElevEnum.ELEV_3_PLAT;
                }
                if(neighboorEast.type== TileEnum.AltitudeEnum.PLAIN)
                {
                    direction = 3;
                    elevType = TileEnum.ElevEnum.ELEV_3_PLAT;
                }
            }
            if(nbElevation==2 && nbPlateau == 1)
            {
                if ( neighboorNorth.type == TileEnum.AltitudeEnum.PLATEAU)
                {
                    direction = 0;
                    elevType = TileEnum.ElevEnum.ELEV_BASIC_SLOPE;
                }
                else if (neighboorEast.type == TileEnum.AltitudeEnum.PLATEAU)
                {
                    direction = 1;
                    elevType = TileEnum.ElevEnum.ELEV_BASIC_SLOPE;
                }
                else if (neighboorSouth.type == TileEnum.AltitudeEnum.PLATEAU)
                {
                    direction = 2;
                    elevType = TileEnum.ElevEnum.ELEV_BASIC_SLOPE;
                }
                else if (neighboorWest.type == TileEnum.AltitudeEnum.PLATEAU)
                {
                    direction = 3;
                    elevType = TileEnum.ElevEnum.ELEV_BASIC_SLOPE;
                }
            }
            else if(nbPlateau==2 && nbElevation==1) {
                //Left
                if(isDuoPlateau(neighboorWest,neighboorNorth) && neighboorEast.type == TileEnum.AltitudeEnum.ELEVATION)
                {
                    direction = 0;
                    elevType = TileEnum.ElevEnum.ELEV_SLOP_LEFT;
                }
                else if(isDuoPlateau(neighboorNorth,neighboorEast) && neighboorSouth.type == TileEnum.AltitudeEnum.ELEVATION)
                {
                    direction = 1;
                    elevType = TileEnum.ElevEnum.ELEV_SLOP_LEFT;
                }
                else if (isDuoPlateau(neighboorEast, neighboorSouth) && neighboorWest.type == TileEnum.AltitudeEnum.ELEVATION)
                {
                    direction = 2;
                    elevType = TileEnum.ElevEnum.ELEV_SLOP_LEFT;
                }
                else if (isDuoPlateau(neighboorSouth, neighboorWest) && neighboorNorth.type == TileEnum.AltitudeEnum.ELEVATION)
                {
                    direction = 3;
                    elevType = TileEnum.ElevEnum.ELEV_SLOP_LEFT;
                }
                //Right
                if (isDuoPlateau(neighboorNorth, neighboorEast) && neighboorWest.type == TileEnum.AltitudeEnum.ELEVATION)
                {
                    direction = 0;
                    elevType = TileEnum.ElevEnum.ELEV_SLOP_RIGHT;
                }
                if (isDuoPlateau(neighboorEast, neighboorSouth) && neighboorNorth.type == TileEnum.AltitudeEnum.ELEVATION)
                {
                    direction = 1;
                    elevType = TileEnum.ElevEnum.ELEV_SLOP_RIGHT;
                }
                if (isDuoPlateau(neighboorSouth, neighboorWest) && neighboorEast.type == TileEnum.AltitudeEnum.ELEVATION)
                {
                    direction = 2;
                    elevType = TileEnum.ElevEnum.ELEV_SLOP_RIGHT;
                }
                if (isDuoPlateau(neighboorWest, neighboorNorth) && neighboorSouth.type == TileEnum.AltitudeEnum.ELEVATION)
                {
                    direction = 3;
                    elevType = TileEnum.ElevEnum.ELEV_SLOP_RIGHT;
                }
            }
        }
        if (direction == -1)
        {
            Debug.Log(coordX + " " + coordY + " N " + neighboorNorth.type + " E " + neighboorEast.type + " S " + neighboorSouth.type + " W " + neighboorWest.type);
            elevType = TileEnum.ElevEnum.ERROR;
            direction = 0;
        }
    }
    private bool isDuoPlateau(AltitudeType a, AltitudeType b)
    {
        return a.type == TileEnum.AltitudeEnum.PLATEAU && b.type == TileEnum.AltitudeEnum.PLATEAU;
    }
    private bool isDuoElevation(AltitudeType a, AltitudeType b)
    {
        return a.type == TileEnum.AltitudeEnum.ELEVATION && b.type == TileEnum.AltitudeEnum.ELEVATION;

    }

    private bool isDuoElevationPlateau(AltitudeType elev, AltitudeType plateau)
    {
        return elev.type == TileEnum.AltitudeEnum.ELEVATION && plateau.type == TileEnum.AltitudeEnum.PLATEAU;
    }
}
