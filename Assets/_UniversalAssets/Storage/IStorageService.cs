using System;

public interface IStorageService
{
    // Method for saving data. It can be used synchronously or asynchronously.
    // Parameters: key for saving, data for saving, optional callback that returns operation status
    void Save(string key, object data, Action<bool> callback = null);
    
    // Parameters: key for loading, callback that returns data
    void Load<T>(string key, Action<T> callback);
}
