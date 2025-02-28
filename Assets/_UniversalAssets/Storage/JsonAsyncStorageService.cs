using System;
using System.IO;
using UnityEngine;
using Newtonsoft.Json;
using Cysharp.Threading.Tasks;

public class JsonAsyncStorageService : IStorageService
{
    public async void Save(string key, object data, Action<bool> callback = null)
    {
        string path = BuildPath(key);

        try
        {
            // Выполняем сохранение в фоновом потоке
            await UniTask.RunOnThreadPool(() =>
            {
                string json = JsonConvert.SerializeObject(data);

                // Создаем директорию, если ее нет
                Directory.CreateDirectory(Path.GetDirectoryName(path));

                using (StreamWriter fileStream = new StreamWriter(path))
                {
                    fileStream.Write(json);
                }
            });

            callback?.Invoke(true); // Успешное завершение
        }
        catch (Exception ex)
        {
            Debug.LogError($"Error saving data to '{path}': {ex.Message}");
            callback?.Invoke(false); // Возврат ошибки
        }
    }

    public async void Load<T>(string key, Action<T> callback)
    {
        string path = BuildPath(key);

        try
        {
            if (!File.Exists(path))
            {
                Debug.LogWarning($"File '{path}' not found.");
                callback?.Invoke(default); // Возвращаем default, если файл отсутствует
                return;
            }

            // Выполняем загрузку в фоновом потоке
            T data = await UniTask.RunOnThreadPool(() =>
            {
                using (StreamReader fileStream = new StreamReader(path))
                {
                    string json = fileStream.ReadToEnd();
                    return JsonConvert.DeserializeObject<T>(json);
                }
            });

            callback?.Invoke(data); // Успешное завершение
        }
        catch (Exception ex)
        {
            Debug.LogError($"Error loading data from '{path}': {ex.Message}");
            callback?.Invoke(default); // Возврат default при ошибке
        }
    }

    private string BuildPath(string key)
    {
        return Path.Combine(Application.persistentDataPath, key);
    }
}
