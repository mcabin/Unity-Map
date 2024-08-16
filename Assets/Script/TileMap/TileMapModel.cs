using System;
using System.Collections.Generic;
using UnityEngine;
using static TileAsset;
using static TileEnum;

public class TileMapModel
{
    //Temperature
    private float temperatureNoiseScale = 5f;
    private float temperatureOffsetX ;
    private float temperatureOffsetY ;
    private const int minTemperature = 0;
    private const int maxTemperature = 9;
    private float noiseInfluenceTemperature = 5f;
    private float temperatureModifier;

    //Altitude noise
    private float altitudeNoiseScale ;
    private float altitudeOffsetX;
    private float altitudeOffsetY;
    private const int maxAltitude =99;

    //Moisture
    private const int minMoisture=0;
    private const int maxMoisture=9;
    private float moistureNoiseScale = 5f;
    private float moistureOffsetX;
    private float moistureOffsetY;
    private float noiseInfluenceMoisture =3f;
    private float moistureModifier;


    public int height { get; private set; }
    public int width{get;private set;}
    private Tile[,] tiles;

    //Constructor
    public TileMapModel(int height, int width,int seed,float altitudeScale,float temperatureModifier,float moistureModifier)
    {
        this.height = height;
        this.width = width;
        //Seed
        altitudeNoiseScale=altitudeScale;
         this.temperatureModifier=temperatureModifier;
        this.moistureModifier= moistureModifier;
        System.Random rnd =new System.Random(seed);
        int maxOffset=height*width;

        this.temperatureOffsetX = rnd.Next(maxOffset);
        this.temperatureOffsetY = rnd.Next(maxOffset);
        this.altitudeOffsetX = rnd.Next(maxOffset);
        this.altitudeOffsetY = rnd.Next(maxOffset);
        this.moistureOffsetX = rnd.Next(maxOffset);
        this.moistureOffsetY = rnd.Next(maxOffset);

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

    private int tileMoisture(int h, int w, List<(int, int)> waterTiles)
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
            float distance = CalculateDistanceNormalized(h, w, waterTile.Item1, waterTile.Item2);
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

    //Création de la tileWest Map Stockage du resultat dans le paramétre tiles
    private void generateTilesMap()
    {
        // Initialisation du tableau de tuiles avec les dimensions spécifiées
        tiles = new Tile[height, width];

        // Création de tableaux pour stocker les altitudes et températures
        AltitudeType[,] altitudes = new AltitudeType[height, width];
        int[,] temperatures = new int[height, width];
        List<(int, int)> elevationTilesCoord = new List<(int, int)>();
        List<(int, int)> plainTilesCoord = new List<(int, int)>();
        List<TileElevation> borderTilesCoord = new List<TileElevation>();


        // Liste pour garder une trace des tuiles d'eau
        List<(int, int)> waterTiles = new List<(int, int)>();

        // Première boucle : génération de l'altitude et de la température
        for (int h = 0; h < height; h++)
        {
            for (int w = 0; w < width; w++)
            {

                AltitudeType altitude = altitudeNoise(w, h);
                if (altitude.level >= (int)AltitudeEnum.ELEVATION)
                    elevationTilesCoord.Add((h, w));
                else
                    plainTilesCoord.Add((h, w)); 
                temperatures[h, w] = tileTemperature(w, h);


                // Si l'altitude est inférieure ou égale au niveau d'eau, c'est une tuile d'eau
                if (altitude.type ==TileEnum.AltitudeEnum.SEA)
                {
                    waterTiles.Add((h, w));                
                }
                altitudes[h, w] = altitude;
            }
        }

        // Deuxième boucle : génération de l'humidité et détermination du biome
        foreach(var coord in elevationTilesCoord)
        {
            int h = coord.Item1;
            int w = coord.Item2;
            int moisture=tileMoisture(h, w, waterTiles);
             
            BiomeType biome = retrieveBiomeType(temperatures[h, w], altitudes[h,w],moisture);
            bool isBorder = false;
            //L'altitude de chaque voisin des elevations eey
            AltitudeType altiNorth;
            if (h > 0)
                altiNorth = altitudes[h - 1, w];
            else
                altiNorth= altitudes[h, w];
            if (altiNorth.level <= (int)AltitudeEnum.PLAIN)
                isBorder = true;
            AltitudeType altiWest;
            if (w<width-1)
                altiWest = altitudes[h, w+1];
            else
                altiWest = altitudes[h, w];
            if (altiWest.level <= (int)AltitudeEnum.PLAIN)
                isBorder = true;
            AltitudeType altiSouth;
            if (h < height - 1)
                altiSouth = altitudes[h + 1, w];
            else
                altiSouth = altitudes[h, w];

            if(altiSouth.level <= (int)AltitudeEnum.PLAIN)
                isBorder = true;
            AltitudeType altiEast;
            if (w > 0)
                altiEast = altitudes[h, w - 1];
            else
                altiEast = altitudes[h, w];
            if (altiEast.level <= (int)AltitudeEnum.PLAIN)
                isBorder = true;
            //Creation de la case
            TileElevation elevTile = new TileElevation(biome, w, h, altitudes[h, w]);
            //Si ce n'est pas une bordure alors on créer le plateau par défaut
            if (!isBorder)
            {
                elevTile.setElevationType(ElevEnum.PLATEAU);
                //On ajoute au case adjacente le voisin
                if (w > 0)
                {
                    Tile tileEast = tiles[h, w - 1];
                    TileElevation elevEast = (TileElevation)tileEast;
                    elevEast.westNeighboor= TileAsset.getElevationType(ElevEnum.PLATEAU);
                }
                if (h > 0)
                {
                    Tile tileNorth = tiles[h-1, w ];
                    TileElevation elevNorth = (TileElevation)tileNorth;
                    elevNorth.southNeighboor = TileAsset.getElevationType(ElevEnum.PLATEAU);
                }

            }
            //Si c'est une bordure on ajoute les voisins
            else
            {   
                if (altiNorth.level <= (int)AltitudeEnum.PLAIN)
                   elevTile.northNeighboor = TileAsset.getElevationType(ElevEnum.PLAIN);
                else if (h > 0)
                {
                        TileElevation elevNorth = (TileElevation)tiles[h - 1, w];
                        if (elevNorth.elevationType != null)
                            elevTile.northNeighboor = elevNorth.elevationType;
                        
                        else
                           elevTile.northNeighboor = TileAsset.getElevationType(ElevEnum.UNKNOW);
                }
                else
                    elevTile.northNeighboor = TileAsset.getElevationType(ElevEnum.UNKNOW);

                if (altiEast.level <= (int)AltitudeEnum.PLAIN)
                    elevTile.eastNeighboor = TileAsset.getElevationType(ElevEnum.PLAIN);
                else if (w > 0)
                {
                    TileElevation elevEast = (TileElevation)tiles[h, w - 1];
                    if (elevEast.elevationType != null)
                    {
                        elevTile.eastNeighboor = elevEast.elevationType;
                    }
                    else
                    {
                        elevTile.eastNeighboor = TileAsset.getElevationType(ElevEnum.UNKNOW);
                    }
                }
                else
                    elevTile.eastNeighboor = TileAsset.getElevationType(ElevEnum.UNKNOW);

                if (altiSouth.level <= (int)AltitudeEnum.PLAIN) 
                    elevTile.southNeighboor = TileAsset.getElevationType(ElevEnum.PLAIN);
                else
                    elevTile.southNeighboor = TileAsset.getElevationType(ElevEnum.UNKNOW);

                if (altiWest.level <= (int)AltitudeEnum.PLAIN)
                    elevTile.westNeighboor = TileAsset.getElevationType(ElevEnum.PLAIN);
                
                else
                    elevTile.westNeighboor = TileAsset.getElevationType(ElevEnum.UNKNOW);
                borderTilesCoord.Add(elevTile);
            }
            tiles[h, w] = elevTile;

        }
        foreach (var coord in plainTilesCoord)
        {
            int h = coord.Item1;
            int w = coord.Item2;
            int moisture = tileMoisture(h, w, waterTiles);

            BiomeType biome = retrieveBiomeType(temperatures[h, w], altitudes[h, w], moisture);
            Tile elevTile = new Tile(biome, w, h, altitudes[h, w]);
            tiles[h, w] = elevTile;
            
        }
        while(borderTilesCoord.Count > 0)
        {
            borderTilesCoord.Sort((p1, p2) => p1.nbUnknow.CompareTo(p2.nbUnknow));
            TileElevation elev = borderTilesCoord[0];
            borderTilesCoord.RemoveAt(0);
            elev.setTypeWithNeighboor(0);
            int w = elev.coordX;
            int h = elev.coordY;
            if (h > 0)
            {
                Tile northTile = tiles[h - 1, w];
                if (northTile.altitude.level >= (int)TileEnum.AltitudeEnum.ELEVATION)
                {
                    TileElevation currentTileElevation = (TileElevation)northTile;
                    currentTileElevation.southNeighboor = elev.elevationType;
                }
            }
            if (width - 1 > w)
            {
                Tile currentTile = tiles[h, w + 1];
                if (currentTile.altitude.level >= (int)TileEnum.AltitudeEnum.ELEVATION)
                {
                    TileElevation currentTileElevation = (TileElevation)currentTile;
                    currentTileElevation.eastNeighboor = elev.elevationType;
                }
            }
            if (w > 0)
            {
                Tile currentTile = tiles[h, w - 1];
                if (currentTile.altitude.level >= (int)TileEnum.AltitudeEnum.ELEVATION)
                {
                    TileElevation currentTileElevation = (TileElevation)currentTile;
                    currentTileElevation.westNeighboor = elev.elevationType;
                }
            }
            if (h < height - 1)
            {
                Tile currentTile = tiles[h + 1, w];
                if (currentTile.altitude.level >= (int)TileEnum.AltitudeEnum.ELEVATION)
                {
                    TileElevation currentTileElevation = (TileElevation)currentTile;
                    currentTileElevation.northNeighboor = elev.elevationType;
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
