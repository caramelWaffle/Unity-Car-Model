using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class MouseDataManager: MonoBehaviour
{

    
    public string save(string file_name, List<MouseData> dataLists)
    {
        Debug.Log("Get JSON");

        int dataSize = dataLists.Count;
        MouseData[] MouseArray = new MouseData[dataSize];

        for(int i = 0; i < dataSize; i++) 
        {
            MouseArray[i] = new MouseData();
            MouseArray[i].mTime = dataLists[i].mTime;
            MouseArray[i].logicalTime = dataLists[i].logicalTime;
            MouseArray[i].positionX =  dataLists[i].positionX;
            MouseArray[i].positionY = dataLists[i].positionY;
            MouseArray[i].positionZ = dataLists[i].positionZ;
        }

        string mouseJson = JsonHelper.ToJson(MouseArray, true);
        Debug.Log(MouseArray);
        Debug.Log(mouseJson);

        file_name = file_name + ".json";
        return WriteFile(file_name, mouseJson);
    }

    private string WriteFile(string filename, string json)
    {
        Debug.Log("Write JSON");
        string path = GetFilePath(filename);
        FileStream fileStream = new FileStream(path, FileMode.Create);

        using (StreamWriter writer = new StreamWriter(fileStream))
        {
            writer.Write(json);
        }
        return path;
    }

    private string GetFilePath(string fileName)
    {
        Debug.Log(Application.persistentDataPath + ' ' + fileName);
        return Application.persistentDataPath + '/' + fileName;
    }
}
