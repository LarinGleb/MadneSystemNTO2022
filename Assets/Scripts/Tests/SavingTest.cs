using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using UnityEngine;

public class SavingTest : MonoBehaviour
{
    public GameData data;

    private bool isBusy;
    private string path;
    private GZipStream gzipStream;

    IEnumerator Start () {
        path = Application.persistentDataPath+"/darkRusSave.dat";
        yield return ReadData();
    }

    public void BeginSave () {
        if (isBusy) return;
        StartCoroutine("SaveData");
    }

    IEnumerator ReadData () {
        isBusy = true;
        if (File.Exists(path)) {
            gzipStream = new GZipStream(File.OpenRead(path), CompressionMode.Decompress);
            using (StreamReader reader = new StreamReader(gzipStream)) {
                var decompressionTask = reader.ReadToEndAsync();

                while (!decompressionTask.IsCompleted) {
                    yield return null;
                }

                data = JsonUtility.FromJson<GameData>(decompressionTask.Result);
            }
        } else {
            data = new GameData ();
        }
        isBusy = false;
    }

    IEnumerator SaveData () {
        isBusy = true;
        try {
            gzipStream = new GZipStream (File.Create(path), CompressionMode.Compress);
        } catch (IOException) {
            print ("Saving successfully failed!");
            isBusy = false;
            yield break;
        }
        using (StreamWriter writer = new StreamWriter(gzipStream)) {
            var compressionTask = writer.WriteAsync (JsonUtility.ToJson(data));
            
            while (!compressionTask.IsCompleted) {
                yield return null;
            }
        }

        isBusy = false;
    }
}

[System.Serializable]
public class GameData {
    public int num;
    public string s;
}
