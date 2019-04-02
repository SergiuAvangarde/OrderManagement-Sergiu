using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FileManager : MonoBehaviour
{
    private static readonly string filename = "PlayerData.json";

    public static void WriteFile(InventoryList list)
    {
        string path = Path.Combine(Application.persistentDataPath, filename);
        string json = JsonUtility.ToJson(list);
        File.WriteAllText(path, json);

        Debug.Log(Path.Combine(Application.persistentDataPath, filename));
    }
}
