using System;
using System.Globalization;
using System.IO;
using System.Xml;
using UnityEngine;

public static class XMLAsset
{

   
    
    public static string getPathXml(string pathName)
    {
        string cheminRelatif = Path.Combine("Script/TileMap/Xml", pathName);

        #if UNITY_EDITOR
        // Chemin pour l'éditeur Unity
        cheminRelatif= Path.Combine(Application.dataPath, cheminRelatif);
        #else
            return Path.Combine(Application.persistentDataPath, cheminRelatif);
        #endif
        return cheminRelatif;
    }
}