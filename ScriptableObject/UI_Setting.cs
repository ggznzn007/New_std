using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class UI_Setting : ScriptableObject
{
    private const string SettingFileDirectory = "Assets/Resources";
    private const string SettingFilePath = "Assets/Resources/UI_Setting.asset";

    private static UI_Setting _instance;

    public static UI_Setting Instance
    {
        get
        {
            if(_instance != null)
            {
                return _instance;
            }

            _instance = Resources.Load<UI_Setting>("UI_Setting");

#if UNITY_EDITOR

            if (_instance == null)
            {
                if(!AssetDatabase.IsValidFolder(SettingFileDirectory))
                {
                    AssetDatabase.CreateFolder("Assets", "Resources");
                }

                _instance = AssetDatabase.LoadAssetAtPath<UI_Setting>(SettingFilePath);

                if(_instance == null)
                {
                   _instance =  CreateInstance<UI_Setting>();
                    AssetDatabase.CreateAsset(_instance, SettingFilePath);
                }
            }

#endif
            return _instance;
        }
    }

    public string language = "KOR";

    public Color themeColor;
    public Sprite emptyThumbnailSprite;
    public GameObject popupPrefab;

    public Font defaultFont;
    public int defaultFontSize = 100;
    public Color defaultFontColor = Color.blue;
}
