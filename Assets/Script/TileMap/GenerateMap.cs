using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static TileEnum;

public class GenerateMap 
{
    //Temperature
    private float temperatureNoiseScale = 5f;
    private float temperatureOffsetX;
    private float temperatureOffsetY;
    private const int minTemperature = 0;
    private const int maxTemperature = 9;
    private float noiseInfluenceTemperature = 5f;
    private float temperatureModifier;

    //Altitude noise
    private float altitudeNoiseScale;
    private float altitudeOffsetX;
    private float altitudeOffsetY;
    private const int maxAltitude = 99;

    //Moisture
    private const int minMoisture = 0;
    private const int maxMoisture = 9;
    private float moistureNoiseScale = 5f;
    private float moistureOffsetX;
    private float moistureOffsetY;
    private float noiseInfluenceMoisture = 3f;
    private float moistureModifier;

    private int seed;
    TileMapModel map;

    int width=>map.width;
    int height=>map.height;



    public GenerateMap(int width,int height,int altitudeScale,int seed,int temperatureModifier,int moistureModifier)
    {
        map=new TileMapModel(width,height);
        //Seed
        altitudeNoiseScale = altitudeScale;
        this.temperatureModifier = temperatureModifier;
        this.moistureModifier = moistureModifier;
        System.Random rnd = new System.Random(seed);
        int maxOffset = height * width;
        this.seed = seed;

        this.temperatureOffsetX = rnd.Next(maxOffset);
        this.temperatureOffsetY = rnd.Next(maxOffset);
        this.altitudeOffsetX = rnd.Next(maxOffset);
        this.altitudeOffsetY = rnd.Next(maxOffset);
        this.moistureOffsetX = rnd.Next(maxOffset);
        this.moistureOffsetY = rnd.Next(maxOffset);
    }
    //Altitude noise
    private AltitudeType altitudeNoise(int w, int h)
    {
        float wCoord = (float)w / width * altitudeNoiseScale + altitudeOffsetX;
        float hCoord = (float)h / height * altitudeNoiseScale + altitudeOffsetY;
        float sample = Mathf.PerlinNoise(wCoord, hCoord);
        sample = Mathf.Clamp01(sample);
        return retrieveAltitudeType((int)Math.Round(sample * maxAltitude));
    }

    //Calcul renvoit la temperature d'une case
    private int tileTemperature(int w, int h)
    {
        // Calculer la distance normalisée par rapport au centre (équateur)
        float distanceFromCenter = Mathf.Abs((float)h / height - 0.5f) * 2f;

        // Calculer la température de base
        float baseTemperature = Mathf.Lerp(maxTemperature, minTemperature, distanceFromCenter);

        // Ajouter des variations locales avec du bruit de Perlin
        float xCoord = (float)w / width * temperatureNoiseScale + temperatureOffsetX;
        float yCoord = (float)h / height * temperatureNoiseScale + temperatureOffsetY;
        float noise = Mathf.PerlinNoise(yCoord, xCoord) * 2f - 1f * temperatureModifier; //permet d'avoir un noise entre -1 et 1 
        float temperature = baseTemperature + noise * noiseInfluenceTemperature;

        // S'assurer que la température reste dans les limites
        temperature = Mathf.Clamp(temperature, minTemperature, maxTemperature);
        return (int)Math.Round(temperature);
    }
    private AltitudeType retrieveAltitudeType(int altitudeNum)
    {
        TileEnum.AltitudeEnum[] altitudesEnum = (TileEnum.AltitudeEnum[])Enum.GetValues(typeof(TileEnum.AltitudeEnum));
        foreach (AltitudeEnum altitudeEnum in altitudesEnum)
        {
            AltitudeType currentAltitude = AltitudeType.get(altitudeEnum);
            if (currentAltitude.isType(altitudeNum))
                return currentAltitude;

        }
        return null;
    }
    private BiomeType retrieveBiomeType(int temperature, AltitudeType altitude, int moisture)
    {
        TileEnum.BiomeEnum[] biomesEnum = (TileEnum.BiomeEnum[])Enum.GetValues(typeof(TileEnum.BiomeEnum));
        if (altitude.type == TileEnum.AltitudeEnum.SEA)
        {
            return BiomeType.get(TileEnum.BiomeEnum.WATER);
        }
        foreach (TileEnum.BiomeEnum biomeEnum in biomesEnum)
        {
            BiomeType biome = BiomeType.get(biomeEnum);
            if (biome.minSpawnTemperature <= temperature && temperature <= biome.maxSpawnTemperature && //temperature
                biome.minSpawnMoisture <= moisture && biome.maxSpawnMoisture >= moisture && //moisture
                biome.minSpawnAltitude <= (int)altitude.type && biome.maxSpawnAltitude >= (int)altitude.type) //Altitude
            {
                return biome;
            }
        }
        return null;
    }

    private float CalculateDistanceNormalized(int x1, int y1, int x2, int y2)
    {
        return (float)Math.Sqrt(Math.Pow((float)x2 / width - (float)x1 / width, 2) + Math.Pow((float)y2 / height - (float)y1 / height, 2));
    }

    private float CalculateHumidity(float distance, float waterPercentage)
    {
        return Mathf.Lerp(maxMoisture, minMoisture, distance * waterPercentage);
    }

    private int tileMoisture(int w, int h, List<(int, int)> waterTiles)
    {
        // Si aucune tuile d'eau n'existe, retourner 0 (pas d'humidité)
        if (waterTiles.Count == 0)
        {
            return 0;
        }

        // Initialiser la distance minimale à la valeur maximale possible
        float minDistance = float.MaxValue;

        // Parcourir toutes les tuiles d'eau pour trouver la plus proche
        foreach ((int, int) waterTile in waterTiles)
        {

            // Calculer la distance normalisée entre la tuile actuelle et la tuile d'eau
            float distance = CalculateDistanceNormalized(w, h, waterTile.Item1, waterTile.Item2);
            minDistance = Math.Min(minDistance, distance);
        }
        /* Calculer l'humidité de base en fonction de la distance à l'eau la plus proche
         et du pourcentage de tuiles d'eau sur la carte */
        float baseMoisture = Math.Clamp(CalculateHumidity(minDistance, (width * height) / waterTiles.Count), minMoisture, maxMoisture);

        // Ajouter des variations locales avec du bruit de Perlin
        float xCoord = (float)w / width * moistureNoiseScale + moistureOffsetX;
        float yCoord = (float)h / height * moistureNoiseScale + moistureOffsetY;
        // Générer un bruit entre -1 et 1
        float noise = Mathf.PerlinNoise(xCoord, yCoord) * 2f - 1f * moistureModifier;

        // Combiner l'humidité de base avec le bruit
        float moisture = baseMoisture + noise * noiseInfluenceMoisture;

        // S'assurer que l'humidité finale reste dans les limites définies
        moisture = Math.Clamp(moisture, minMoisture, maxMoisture);

        // Retourner l'humidité finale arrondie à l'entier le plus proche
        return (int)Math.Round(moisture);
    }

    private void updateNeighboorsElev(ElevationType newElev, AltitudeType[,] tab,int w,int h)
    {
        if (h > 0)
        {
            tab[w, h - 1].setNeighborElev(GlobalEnum.Direction.SOUTH, newElev);
        }
        else
        {
            tab[w, h].setNeighborElev(GlobalEnum.Direction.NORTH, newElev);
        }
        if (h<height-1)
        {
            tab[w, h + 1].setNeighborElev(GlobalEnum.Direction.NORTH, newElev);
        }
        else
        {
            tab[w, h].setNeighborElev(GlobalEnum.Direction.SOUTH, newElev);
        }
        if (w < width - 1)
        {

            tab[w + 1, h].setNeighborElev(GlobalEnum.Direction.WEST, newElev);
        }
        else
        {
            tab[w, h].setNeighborElev(GlobalEnum.Direction.EAST, newElev);
        }
        if (w>0)
        {
            tab[w - 1, h].setNeighborElev(GlobalEnum.Direction.EAST, newElev);
        }
        else
        {
            tab[w, h].setNeighborElev(GlobalEnum.Direction.WEST, newElev);
        }
    }
    //Création de la tileWest Map Stockage du resultat dans le paramétre tiles
    public TileMapModel generateTilesMap()
    {
        // Initialisation du tableau de tuiles avec les dimensions spécifiées
        Tile[,] newMap = new Tile[width, height];
        int[,] temperatureTab= new int[width, height];
        AltitudeType[,] altitudeTab=new AltitudeType[width, height];
        // Liste pour garder une trace des tuiles d'eau
        List<(int, int)> waterTiles = new List<(int, int)>();

        // Première boucle : génération de l'altitude et de la température
        for (int h = 0; h < height; h++)
        {
            for (int w = 0; w < width; w++)
            {
                AltitudeType altitude= altitudeNoise(w, h);
                altitudeTab[w, h] = altitude;
                if (altitude.type == TileEnum.AltitudeEnum.SEA)
                {
                    waterTiles.Add((w, h));
                }
                temperatureTab[w,h] = tileTemperature(w, h);

            }
        }
        List < Tile> borderTiles=new List<Tile>();
        for (int h=0; h < height; h++)
        {
            for(int w = 0; w < width; w++)
            {
                int moisture = tileMoisture(w, h, waterTiles);
                AltitudeType altitude = altitudeTab[w, h];
                //North
                if (h>0 && (int)altitude.type > (int)altitudeTab[w,h-1].type)
                {
                    altitude.isRising = true;
                }
                //South
                else if (h < height - 1 && (int)altitude.type > (int)altitudeTab[w, h + 1].type) {
                    altitude.isRising = true;
                }
                //WEST
                else if (w > 0 && (int)altitude.type > (int)altitudeTab[w-1, h ].type) {
                    altitude.isRising = true;
                }
                //EAST
                else if(w < width - 1 && (int)altitude.type > (int)altitudeTab[w+1, h].type) {
                    altitude.isRising = true;
                }
                else
                {
                    altitude.isRising=false;
                    altitude.setElevationType(ElevationType.getDefault(altitude.type));
                    updateNeighboorsElev(altitude.elevationType,altitudeTab,w,h);
                }
                
                BiomeType biome = retrieveBiomeType(temperatureTab[w,h], altitude, moisture);
                Tile newTile = new Tile(biome, w, h, altitudeTab[w, h]);
                if (altitude.isRising)
                {
                    updateNeighboorsElev(ElevationType.getUnknow(altitude.type),altitudeTab,w,h);
                    borderTiles.Add(newTile);
                }
                map.setTile(w, h, newTile);
            }
        }
        while (borderTiles.Count > 0)
        {
            borderTiles.Sort((p1, p2) => p1.altitude.nbUnknow.CompareTo(p2.altitude.nbUnknow));
            Tile currentTile = borderTiles[0];
            borderTiles.RemoveAt(0);
            int w = currentTile.coordW;
            int h = currentTile.coordH;
            bool changeNeeded = currentTile.altitude.setTypeWithNeighboor(seed,w,h);
            //NORTH
            if (h > 0)
            {
                Tile northTile = map.getTile(w,h-1);
                if (northTile.altitude.level >= currentTile.altitude.level)
                {
                    northTile.altitude.setNeighborElev(GlobalEnum.Direction.SOUTH, currentTile.altitude.elevationType) ;
                    if (changeNeeded && !borderTiles.Contains(northTile))
                    {
                        Debug.Log(w + " north " + h);
                        borderTiles.Add(northTile);
                    }
                }
            }
            //WEST
            if (w>0)
            {
                Tile westTile = map.getTile(w-1,h);
                if (westTile.altitude.level >= currentTile.altitude.level)
                {
                    westTile.altitude.setNeighborElev(GlobalEnum.Direction.EAST, currentTile.altitude.elevationType);
                    if (changeNeeded && !borderTiles.Contains(westTile))
                    {
                        Debug.Log(w + " west " + h);
                        borderTiles.Add(westTile);
                    }
                }
            }
            //EAST
            if (width - 1 > w)
            {
                Tile eastTile = map.getTile(w + 1, h);
                if (eastTile.altitude.level >= currentTile.altitude.level)
                {

                    eastTile.altitude.setNeighborElev(GlobalEnum.Direction.WEST, currentTile.altitude.elevationType);
                    if (changeNeeded && !borderTiles.Contains(eastTile))
                    {
                        Debug.Log(w + " east " + h);
                        borderTiles.Add(eastTile);
                    }
                }
            }
            //SOUTH
            if (h < height - 1)
            {
                Tile southTile = map.getTile(w , h+1);
                if (southTile.altitude.level >= currentTile.altitude.level)
                {

                    southTile.altitude.setNeighborElev(GlobalEnum.Direction.NORTH, currentTile.altitude.elevationType);
                    if (changeNeeded && !borderTiles.Contains(southTile))
                    {
                        Debug.Log(w + " south " + h);
                        borderTiles.Add(southTile);
                    }
                }
            }
        }
        return map;
    }

    public TileNode[,] createListOfTileNode()
    {
        TileNode[,] tilesNodes = new TileNode[height, width];
        for (int w = 0; w < width; w++)
        {
            for (int h = 0; h < height; h++)
            {
                tilesNodes[w, h] = new TileNode(map.getTile(w, h));
                if (h < 0)
                {
                    //North self
                    tilesNodes[w, h].addNeighbor(GlobalEnum.Direction.NORTH,tilesNodes[w, h-1]);
                    //South neighboor north
                    tilesNodes[w, h - 1].addNeighbor(GlobalEnum.Direction.SOUTH, tilesNodes[w, h]);
                }
                if (w > 0)
                {
                    //West self
                    tilesNodes[w, h].addNeighbor(GlobalEnum.Direction.WEST, tilesNodes[w-1, h ]);
                    //East neighboor west
                    tilesNodes[w-1, h].addNeighbor(GlobalEnum.Direction.EAST, tilesNodes[w, h]);
                }

            }
        }
        return tilesNodes;
    }

}
