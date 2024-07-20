using System;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using static TileAsset;
using static TileEnum;

public class TileMapModel
{
    //Temperature
    public float temperatureNoiseScale = 5f;
    public float temperatureOffsetX = 1f;
    public float temperatureOffsetY = 10f;
    public int minTemperature = 0;
    public int maxTemperature = 9;
    public float noiseInfluenceTemperature = 5f;
    public float temperatureModifier = 1f;

    //Altitude noise
    public float altitudeNoiseScale = 3f;
    public float altitudeOffsetX=10;
    public float altitudeOffsetY=90;
    public int maxAltitude=9;

    //Moisture
    public int minMoisture=0;
    public int maxMoisture=9;
    public float moistureNoiseScale = 5f;
    public float moistureOffsetX = 1f;
    public float moistureOffsetY = 10f;
    public float noiseInfluenceMoisture =3f;
    public float moistureModifier = 1f;


    public int height { get; private set; }
    public int width{get;private set;}
    private Tile[,] tiles;

    //Constructor
    public TileMapModel(int height, int width)
    {
        this.height = height;
        this.width = width;
        generateTilesMap();
    }


    
    public Tile getTile(int w,int h)
    {
        return tiles[h,w];
    }

    //Altitude noise
    private AltitudeType altitudeNoise(int w,int h)
    {
        float wCoord = (float)w / width * altitudeNoiseScale+altitudeOffsetX;
        float hCoord = (float)h / height * altitudeNoiseScale+altitudeOffsetY;
        float sample = Mathf.PerlinNoise(wCoord, hCoord);
        sample = Mathf.Clamp01(sample);
        return retrieveAltitudeType( (int) Math.Round(sample *maxAltitude));
    }

    //Calcul renvoit la temperature d'une case
    private int tileTemperature(int x,int y)
    {
        // Calculer la distance normalisée par rapport au centre (équateur)
        float distanceFromCenter = Mathf.Abs((float)y / height - 0.5f) * 2f;

        // Calculer la température de base
        float baseTemperature = Mathf.Lerp(maxTemperature, minTemperature, distanceFromCenter);

        // Ajouter des variations locales avec du bruit de Perlin
        float xCoord = (float)x / width * temperatureNoiseScale + temperatureOffsetX;
        float yCoord = (float)y / height * temperatureNoiseScale + temperatureOffsetY;
        float noise = Mathf.PerlinNoise(yCoord, xCoord) * 2f - 1f*temperatureModifier  ; //permet d'avoir un noise entre -1 et 1 
        float temperature = baseTemperature + noise *noiseInfluenceTemperature;

        // S'assurer que la température reste dans les limites
        temperature = Mathf.Clamp(temperature, minTemperature, maxTemperature);
        return (int)Math.Round(temperature);
    }
    private AltitudeType retrieveAltitudeType(int altitudeNum)
    {
        TileEnum.AltitudeEnum[] altitudesEnum = (TileEnum.AltitudeEnum[])Enum.GetValues(typeof(TileEnum.AltitudeEnum));
        foreach(AltitudeEnum altitudeEnum in altitudesEnum)
        {
            AltitudeType currentAltitude = TileAsset.GetAltitudeType(altitudeEnum);
            if(currentAltitude.isType(altitudeNum))
                return currentAltitude;
            
        }
        return null;
    }
    private BiomeType retrieveBiomeType(int temperature,AltitudeType altitude,int moisture)
    {
        TileEnum.BiomeEnum[] biomesEnum = (TileEnum.BiomeEnum[])Enum.GetValues(typeof(TileEnum.BiomeEnum));
        if (altitude.type == TileEnum.AltitudeEnum.SEA) {
            return TileAsset.getBiomeType(TileEnum.BiomeEnum.WATER);
        }
        foreach (TileEnum.BiomeEnum biomeEnum in biomesEnum)
        {
            BiomeType biome=TileAsset.getBiomeType(biomeEnum);
            if (biome.minSpawnTemperature <=temperature &&temperature <= biome.maxSpawnTemperature && //temperature
                biome.minSpawnMoisture <=moisture && biome.maxSpawnMoisture>=moisture && //moisture
                biome.minSpawnAltitude<=(int)altitude.type && biome.maxSpawnAltitude >= (int)altitude.type) //Altitude
            {
                return biome;
            }
        }
        return null;
    }

    private float CalculateDistanceNormalized(int x1, int y1, int x2, int y2)
    {
        return (float)Math.Sqrt(Math.Pow((float)x2/width - (float)x1/width, 2) + Math.Pow((float)y2/height - (float)y1/height, 2));
    }

    private float CalculateHumidity(float distance,float waterPercentage)
    {
        return Mathf.Lerp(maxMoisture, minMoisture, distance* waterPercentage);
    }

    private int tileMoisture(int x, int y, List<(int, int)> waterTiles)
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
            float distance = CalculateDistanceNormalized(x, y, waterTile.Item1, waterTile.Item2);
            minDistance = Math.Min(minDistance, distance);
        }
        /* Calculer l'humidité de base en fonction de la distance à l'eau la plus proche
         et du pourcentage de tuiles d'eau sur la carte */
        float baseMoisture = Math.Clamp(CalculateHumidity(minDistance, (width * height) / waterTiles.Count), minMoisture, maxMoisture);

        // Ajouter des variations locales avec du bruit de Perlin
        float xCoord = (float)x / width * moistureNoiseScale + moistureOffsetX;
        float yCoord = (float)y / height * moistureNoiseScale + moistureOffsetY;
        // Générer un bruit entre -1 et 1
        float noise = Mathf.PerlinNoise(xCoord, yCoord) * 2f - 1f * moistureModifier;

        // Combiner l'humidité de base avec le bruit
        float moisture = baseMoisture + noise * noiseInfluenceMoisture;

        // S'assurer que l'humidité finale reste dans les limites définies
        moisture = Math.Clamp(moisture, minMoisture, maxMoisture);

        // Retourner l'humidité finale arrondie à l'entier le plus proche
        return (int)Math.Round(moisture);
    }

    //Création de la tile Map Stockage du resultat dans le paramétre tiles
    private void generateTilesMap()
    {
        // Initialisation du tableau de tuiles avec les dimensions spécifiées
        tiles = new Tile[height, width];

        // Création de tableaux pour stocker les altitudes et températures
        AltitudeType[,] altitudes = new AltitudeType[height, width];
        int[,] temperatures = new int[height, width];

        // Liste pour garder une trace des tuiles d'eau
        List<(int, int)> waterTiles = new List<(int, int)>();

        // Première boucle : génération de l'altitude et de la température
        for (int h = 0; h < height; h++)
        {
            for (int w = 0; w < width; w++)
            {

                AltitudeType altitude = altitudeNoise(w, h);
                temperatures[h, w] = tileTemperature(w, h);


                // Si l'altitude est inférieure ou égale au niveau d'eau, c'est une tuile d'eau
                if (altitude.type ==TileEnum.AltitudeEnum.SEA)
                {
                    waterTiles.Add((w, h));                
                }
                altitudes[h, w] = altitude;
            }
        }

        // Deuxième boucle : génération de l'humidité et détermination du biome
        for (int h = 0; h < height; h++)
        {
            for (int w = 0; w < width; w++)
            {
                // Calcul de l'humidité pour cette tuile
                int moisture = tileMoisture(w, h, waterTiles);
                AltitudeType altitude = altitudes[h, w];
                

                BiomeType biome;
                // on détermine le biome en fonction de la température, de l'altitude et de l'humidité
                biome = retrieveBiomeType(temperatures[h, w], altitude, moisture);

                // Vérification que le biome a bien été déterminé
                if (biome == null)
                {
                    throw new Exception("Biome for " + w + " " + h + " with temperature of :" + temperatures[w, h] + " and altitude of " + altitude + " is null");
                }
                if (altitude.type == TileEnum.AltitudeEnum.ELEVATION)
                {
                    AltitudeType north;
                    if (h > 0)
                         north = altitudes[h-1,w];
                    else
                        north = TileAsset.GetAltitudeType(TileEnum.AltitudeEnum.PLAIN);

                    AltitudeType south;
                    if(h<height-1)
                        south=altitudes[h + 1,w];
                    else
                        south= TileAsset.GetAltitudeType(TileEnum.AltitudeEnum.PLAIN);

                    AltitudeType west;
                    if (w > 0)
                        west = altitudes[h, w-1];
                    else
                        west = TileAsset.GetAltitudeType(TileEnum.AltitudeEnum.PLAIN);

                    AltitudeType east;
                    if (w < width - 1)
                        east = altitudes[h, w+1];
                    else
                        east = TileAsset.GetAltitudeType(TileEnum.AltitudeEnum.PLAIN);
                    if(west.level>=(int)TileEnum.AltitudeEnum.ELEVATION && east.level >= (int)TileEnum.AltitudeEnum.ELEVATION && south.level >= (int)TileEnum.AltitudeEnum.ELEVATION && north.level >= (int)TileEnum.AltitudeEnum.ELEVATION)
                    {
                        altitude= TileAsset.GetAltitudeType(TileEnum.AltitudeEnum.PLATEAU);
                        tiles[h, w] = new Tile(biome, w, h, altitude);
                        //Change la valeur des voisins déja initialisé si le voisin est une élévation
                        //Voisin du haut
                        if (h > 0){
                            Tile northNeighboor = tiles[h-1,w];
                            if (northNeighboor.altitude.type == TileEnum.AltitudeEnum.ELEVATION)
                            {
                                ElevationTile northElevation = (ElevationTile)northNeighboor;
                                northElevation.setSouth(altitude);
                            }
                        }
                        //Voisin de gauche
                        if (w > 0)
                        {
                            Tile westNeighboor = tiles[h, w-1];
                            if(westNeighboor.altitude.type== TileEnum.AltitudeEnum.ELEVATION)
                            {
                                ElevationTile westElevation=(ElevationTile)westNeighboor;
                                westElevation.setEast(altitude);
                            }
                        }

                    }
                    else
                    {
                        ElevationTile elev = new ElevationTile(biome, w, h, altitude);
                        elev.setNeighboors(north, south, west,east);
                        tiles[h, w] = elev;
                    }
                }
                else
                {
                    // Création de la tuile avec les informations générées
                    tiles[h, w] = new Tile(biome, w, h, altitude);
                }
            }
        }
    }
    public TileNode[,] createListOfTileNode()
    {
        TileNode[,] tilesNodes=new TileNode[height,width];
        for (int w = 0; w < width; w++)
        {
            for (int h = 0; h < height; h++)
            {
                tilesNodes[h, w]=new TileNode(tiles[h,w]);
                if (h < 0)
                {
                    //Ajouter la tuile en a gauche
                    tilesNodes[h,w].addNeighbor(tilesNodes[h-1,w]);
                    tilesNodes[h-1, w].addNeighbor(tilesNodes[h, w]);
                    //Ajouter la tuile en haut a gauche
                    if (w > 0)
                    {
                        tilesNodes[h, w].addNeighbor(tilesNodes[h - 1, w-1]);
                        tilesNodes[h - 1, w-1].addNeighbor(tilesNodes[h, w]);
                    }
                    if (h < height - 1)
                    {
                        tilesNodes[h, w].addNeighbor(tilesNodes[h + 1, w - 1]);
                        tilesNodes[h + 1, w-1].addNeighbor(tilesNodes[h, w]);
                    }
                }
                if(w>0)
                {
                    tilesNodes[h, w].addNeighbor(tilesNodes[h, w - 1]);
                    tilesNodes[h , w - 1].addNeighbor(tilesNodes[h, w]);
                }
            }
        }
        return tilesNodes;
    }
}
