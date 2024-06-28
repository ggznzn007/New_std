using Gpm.Common.ThirdParty.LitJson;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UIElements;
using WPM;

public class ClickStar : MonoBehaviour
{

    WorldMapGlobe map;              // 에셋 지구본

    void Start()
    {
        map = WorldMapGlobe.instance;
        map.OnCountryClick += Map_OnStarClick;
    }

    private void Map_OnStarClick(int countryIndex, int regionIndex, int buttonIndex)
    {
        string country = map.countries[countryIndex].name;
        Vector2 latlon = map.countries[countryIndex].mainRegion.latlonCenter; // 나라의 중심 위치 위도와 경도
        Debug.Log("Country = " + country);
        map.FlyToLocation(map.countries[countryIndex].mainRegion.latlonCenter);       
    }    
}
