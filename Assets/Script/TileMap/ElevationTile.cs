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

    private bool isDuoPlateau(AltitudeType a,AltitudeType b)
    {
        return a.type == TileEnum.AltitudeEnum.PLATEAU && b.type == TileEnum.AltitudeEnum.PLATEAU;
    }
    private bool isDuoElevation(AltitudeType a,AltitudeType b)
    {
        return a.type == TileEnum.AltitudeEnum.ELEVATION && b.type == TileEnum.AltitudeEnum.ELEVATION;

    }

    private bool isDuoElevationPlateau(AltitudeType elev,AltitudeType plateau)
    {
        return elev.type == TileEnum.AltitudeEnum.ELEVATION && plateau.type == TileEnum.AltitudeEnum.PLATEAU;
    }

    public void setTileType()
    {
        direction = 0;
        elevType = TileEnum.ElevEnum.ELEV_BASIC_SLOPE;
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
                else if(isDuoPlateau(neighboorWest, neighboorNorth))
                {
                    //Direction par default
                    direction = 0;
                    elevType = TileEnum.ElevEnum.ELEV_PP_VIRAGE;
                }
                else if (isDuoPlateau(neighboorNorth, neighboorEast))
                {
                    direction = 1;
                    elevType = TileEnum.ElevEnum.ELEV_PP_VIRAGE;
                }
                else if (isDuoPlateau(neighboorEast, neighboorSouth))
                {
                    direction=2;
                    elevType = TileEnum.ElevEnum.ELEV_PP_VIRAGE;

                }
                else if (isDuoPlateau(neighboorSouth, neighboorWest))
                {
                    direction = 3;
                    elevType = TileEnum.ElevEnum.ELEV_PP_VIRAGE;
                }
            }
            //Deux elevation
            else if (nbElevation == 2)
            {
                Debug.Log(coordX + " " + coordY+ " N " + neighboorNorth.type + " E " + neighboorEast.type + " S " + neighboorSouth.type + " W " + neighboorWest.type);
                if (isDuoElevation(neighboorNorth, neighboorSouth))
                {
                    //Pas de rotation + tuile elev parallele
                }
                else if (isDuoElevation(neighboorEast, neighboorWest))
                {
                    //rotation 90 parallele aussi
                }
                else if (isDuoElevation(neighboorWest, neighboorNorth))
                {
                    //Direction par default
                    direction = 0;
                    elevType = TileEnum.ElevEnum.ELEV_EE_VIRAGE;
                }
                else if (isDuoElevation(neighboorNorth, neighboorEast))
                {
                    direction = 1;
                    elevType = TileEnum.ElevEnum.ELEV_EE_VIRAGE;
                }
                else if (isDuoElevation(neighboorEast, neighboorSouth))
                {
                    direction = 2;
                    elevType = TileEnum.ElevEnum.ELEV_EE_VIRAGE;

                }
                else if (isDuoElevation(neighboorSouth, neighboorWest))
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
    }
}
