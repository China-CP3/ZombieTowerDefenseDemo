using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
public enum JsonType
{
    JsonUtlity,
    LitJson,
}
public class JsonMgr 
{
    private static JsonMgr instance = new JsonMgr();
    public static JsonMgr Instance => instance;
    private JsonMgr() { }
    
    public void SaveData(object obj,string fileName,JsonType type=JsonType.LitJson)
    {
        string path = Application.persistentDataPath + "/" + fileName + ".Json";
        string jsonData = "";
        switch (type)
        {
            case JsonType.JsonUtlity:
                jsonData = JsonUtility.ToJson(obj);
                break;
            case JsonType.LitJson:
                jsonData = LitJson.JsonMapper.ToJson(obj);
                break;
        }
        File.WriteAllText(path, jsonData);
        Debug.Log(path);
    }
    public T LoadData<T>(string fileName,JsonType type=JsonType.LitJson) where T:new ()
    {
        //这个是游戏一开始的默认文件夹 先判断里面有没有数据 
        string path= Application.streamingAssetsPath + "/" + fileName + ".Json";
        //如果不存在默认文件 就从读写文件夹中去读取
        if(File.Exists(path)==false)
           path = Application.persistentDataPath + "/" + fileName + ".Json";
        //如果读写文件夹也没有 返回默认值
        if(!File.Exists(path))
           return new T();
        string jsonData = File.ReadAllText(path);
        T data = new T();
        switch (type)
        {
            case JsonType.JsonUtlity:
                data= JsonUtility.FromJson<T>(jsonData);
                break;
            case JsonType.LitJson:
                data= LitJson.JsonMapper.ToObject<T>(jsonData);
                break;
        }
        return data;
    }
}
