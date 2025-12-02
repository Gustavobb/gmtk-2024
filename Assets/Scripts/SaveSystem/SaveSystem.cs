using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public static class SaveSystem
{
    public static void Save(FM fm){
        BinaryFormatter bf = new BinaryFormatter();
        string path = Application.persistentDataPath + "/data.dat";
        FileStream stream = new FileStream(path, FileMode.Create);

        Data data = new Data(fm);
        bf.Serialize(stream, data);
        stream.Close();
        //Debug.Log (SaveData.currentLevel);
    }
 
    public static Data Load(){
        string path = Application.persistentDataPath + "/data.dat";
        if (File.Exists (path)) {
            BinaryFormatter bf = new BinaryFormatter ();
            FileStream stream = File.Open(path, FileMode.Open);


            Data data = bf.Deserialize(stream) as Data;
            stream.Close();
            return data;
        } else {
            return null;
        }
    }
}
