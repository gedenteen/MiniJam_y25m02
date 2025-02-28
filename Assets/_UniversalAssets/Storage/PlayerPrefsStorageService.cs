using System;
using UnityEngine;
using Newtonsoft.Json;

public class PlayerPrefsStorageService : IStorageService
{
    public void Save(string key, object data, Action<bool> callback = null)
    {
        try
        {
            // Сериализуем объект в JSON-строку
            string json = JsonConvert.SerializeObject(data);
            PlayerPrefs.SetString(key, json);
            PlayerPrefs.Save(); // Сохраняем изменения
            callback?.Invoke(true); // Успешно
        }
        catch (Exception ex)
        {
            Debug.LogError($"Error saving data with key '{key}': {ex.Message}");
            callback?.Invoke(false); // Ошибка
        }
    }

    public void Load<T>(string key, Action<T> callback)
    {
        try
        {
            if (PlayerPrefs.HasKey(key))
            {
                // Загружаем строку и десериализуем в нужный тип
                string json = PlayerPrefs.GetString(key);
                T data = JsonConvert.DeserializeObject<T>(json);
                callback?.Invoke(data); // Успешно возвращаем данные
            }
            else
            {
                Debug.LogWarning($"Key '{key}' not found in PlayerPrefs.");
                callback?.Invoke(default); // Возвращаем default значение
            }
        }
        catch (Exception ex)
        {
            Debug.LogError($"Error loading data with key '{key}': {ex.Message}");
            callback?.Invoke(default); // Ошибка
        }
    }
}
