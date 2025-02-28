    using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace Testing
{
    public class TestStorageService : MonoBehaviour
    {
        [SerializeField] TextMeshProUGUI textForUpdate;

        private IStorageService storageService;
        private IStorageService asyncStorageService;
        private int lengthOfList = 423456;
        private List<TestClassForJson> bigData;
        private readonly string filename = "SaveFile.txt";

        private void Awake()
        {
            storageService = new JsonStorageService();
            asyncStorageService = new JsonAsyncStorageService();

            string longString = string.Concat("qweoiqjw9ehqw9eqhweuiqiheuiqhweiuqhweiuqwheiuqhequiehqwuiehqiuwehqe",
                "qweoiqjw9ehqw9eqhweuiqiheuiqhweiuqhweiuqwheiuqhequiehqwuiehqiuwehqe",
                "qweoiqjw9ehqw9eqhweuiqiheuiqhweiuqhweiuqwheiuqhequiehqwuiehqiuwehqe",
                "qweoiqjw9ehqw9eqhweuiqiheuiqhweiuqhweiuqwheiuqhequiehqwuiehqiuwehqe",
                "qweoiqjw9ehqw9eqhweuiqiheuiqhweiuqhweiuqwheiuqhequiehqwuiehqiuwehqe",
                "qweoiqjw9ehqw9eqhweuiqiheuiqhweiuqhweiuqwheiuqhequiehqwuiehqiuwehqe",
                "qweoiqjw9ehqw9eqhweuiqiheuiqhweiuqhweiuqwheiuqhequiehqwuiehqiuwehqe",
                "qweoiqjw9ehqw9eqhweuiqiheuiqhweiuqhweiuqwheiuqhequiehqwuiehqiuwehqe",
                "qweoiqjw9ehqw9eqhweuiqiheuiqhweiuqhweiuqwheiuqhequiehqwuiehqiuwehqe");

            bigData = new List<TestClassForJson>(lengthOfList);
            for (int i = 0; i < lengthOfList; i++)
            {
                bigData.Add(new TestClassForJson(longString));
            }
        }

        void Update()
        {
            textForUpdate.text = Time.frameCount.ToString();

            if (Input.GetKeyDown(KeyCode.F1))
            {
                Debug.Log($"TestStorageService: Update: going to call **sync** Save()");
                storageService.Save(filename, bigData, OnSave);
            }
            else if (Input.GetKeyDown(KeyCode.F2))
            {
                Debug.Log($"TestStorageService: Update: going to call **sync** Load()");
                storageService.Load<List<TestClassForJson>>(filename, OnLoad);
            }
            else if (Input.GetKeyDown(KeyCode.F3))
            {
                Debug.Log($"TestStorageService: Update: going to call **async** Save()");
                asyncStorageService.Save(filename, bigData, OnSave);
            }
            else if (Input.GetKeyDown(KeyCode.F4))
            {
                Debug.Log($"TestStorageService: Update: going to call **async** Load()");
                asyncStorageService.Load<List<TestClassForJson>>(filename, OnLoad);
            }
        }

        private void OnSave(bool success)
        {
            Debug.Log($"TestStorageService: OnSave: success={success}");
        }

        private void OnLoad(List<TestClassForJson> list)
        {
            Debug.Log($"TestStorageService: OnLoad: list.Count={list.Count}");
        }
    }
}

