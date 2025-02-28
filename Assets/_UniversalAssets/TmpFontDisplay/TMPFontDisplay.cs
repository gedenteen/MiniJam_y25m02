using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEditor;

public class TMPFontDisplay : MonoBehaviour
{
    [SerializeField]
    private Transform _container;

    [SerializeField]
    private GameObject _prefabItemForFont;

    private void Start()
    {
        DisplayAllTMPFonts();
    }

    private void DisplayAllTMPFonts()
    {
#if UNITY_EDITOR
        // Проверка наличия контейнера и шаблона текста
        if (_container == null)
        {
            Debug.LogError("TMPFontDisplay: Container is not assigned.");
            return;
        }

        if (_prefabItemForFont == null)
        {
            Debug.LogError("TMPFontDisplay: Template ItemForFont is not assigned.");
            return;
        }

        // Найти все TMP_FontAsset в проекте
        string[] guids = AssetDatabase.FindAssets("t:TMP_FontAsset");
        List<TMP_FontAsset> fonts = new List<TMP_FontAsset>();

        foreach (string guid in guids)
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);
            TMP_FontAsset font = AssetDatabase.LoadAssetAtPath<TMP_FontAsset>(path);
            if (font != null)
            {
                fonts.Add(font);
            }
        }

        // Создать текст для каждого шрифта
        foreach (TMP_FontAsset font in fonts)
        {
            GameObject gameObject = Instantiate(_prefabItemForFont, _container);
            TextMeshProUGUI textMesh = gameObject.GetComponentInChildren<TextMeshProUGUI>();
            textMesh.font = font;
            textMesh.text = $"Font: {font.name}\n" +
                $"ABCDEFGHIJKLMNOPQRSTUVWXYZ\nabcdefghijklmnopqrstuvwxyz\n" + 
                $"АБВГДЕЖЗИЙКЛМНОПРСТУФХЦЧШЩЪЫЬЭЮЯ\nабвгдежзийклмнопрстуфхцчшщъыьэюя";
        }

        Debug.Log($"TMPFontDisplay: Displayed {fonts.Count} fonts.");
#endif
    }
}
