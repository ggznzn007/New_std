using Gpm.Common.ThirdParty.LitJson;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UIElements;


namespace WPM
{
    [System.Serializable]
    public class OWM_Coord // ��ǥ
    {
        public float lon;
        public float lat;
    }

    [System.Serializable]
    public class OWM_Weather // ����
    {
        public int id;
        public string main;
        public string description;
        public string icon;
    }

    [System.Serializable]
    public class OWM_Main
    {
        public int temp;
        public float feels_like;
        public int temp_min;
        public int temp_max;
        public int pressure;
        public int humidity;
    }

    [System.Serializable]
    public class OWM_Wind // �ٶ�
    {
        public float speed;
        public int deg;
    }

    [System.Serializable]
    public class OWM_Clouds //����
    {
        public int all;
    }

    [System.Serializable]
    public class OWM_Sys
    {
        public int type;
        public int id;
        public string country;
        public int sunrise;
        public int sunset;
    }

    [System.Serializable]
    public class WeatherData
    {
        public OWM_Coord coord;
        public OWM_Weather[] weather;
        public string basem;
        public OWM_Main main;
        public int visibility;
        public OWM_Wind wind;
        public OWM_Clouds clouds;
        public int dt;
        public OWM_Sys sys;
        public int timezone;
        public int id;
        public string name;
        public int cod;
        public JsonData data;
    }

    public class ClickEarthCountry : MonoBehaviour
    {
        public GameObject info;
        public TMP_Text countryText;    // ����
        public TMP_Text weatherText;    // ����
        public TMP_Text maintempText;  //  �µ�
        public TMP_Text latitudeText;   // ����
        public TMP_Text longitudeText;  // �浵
        public TMP_Text dtText;         // ����ð�
        public string lang = "kr";      // ���
        public string APP_ID;           // API ���̵�
        public WeatherData weatherInfo; // ���� ������        
        WorldMapGlobe map;              // ���� ������
        Vector3 lastRestyleEarthNormalsScaleCheck;
        Vector3 globePos;
        TickerTextAnimator tickerTextAnimator;
        bool isClick;
        CallbackHandler callback;
        GlobeClickEvent globeClick;

        void Start()
        {            
            map = WorldMapGlobe.instance;
            isClick = false;

            map.OnCountryClick += Map_OnCountryClick;
        }

        private void Update()
        {            
            if (Input.GetKeyDown(KeyCode.Space))
            {
#if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif   
            }
        }

        /*public float ComputeCenteredHorizontalOffset()
        {
            Vector3 centerLocation = map.GetCurrentMapLocation();
            Vector2 uv = Conversion.GetUVFromSpherePoint(centerLocation);
            return uv.x;
        }*/

        private void Map_OnCountryClick(int countryIndex, int regionIndex, int buttonIndex)
        {
            string country = map.countries[countryIndex].name;

            Vector2 latlon = map.countries[countryIndex].mainRegion.latlonCenter; // ������ �߽� ��ġ ������ �浵

            Debug.Log("Country = " + country);

            // map.FlyToLocation(map.countries[countryIndex].mainRegion.latlonCenter);
                        
            if (isClick) return;
            StartCoroutine(GetWeather(country, latlon.x, latlon.y));
        }

        IEnumerator CenterTo(int countryIndex)
        {
            yield return new WaitForSeconds(0.0001f);
            
            map.FlyToLocation(map.countries[countryIndex].mainRegion.latlonCenter);
        }

        IEnumerator GetWeather(string country, float lat, float lon)
        {            
            string srcCountry = country;
            country = UnityWebRequest.EscapeURL(country);
            string url = $"https://api.openweathermap.org/data/2.5/weather?lat={lat}&lon={lon}&appid={APP_ID}&lang={lang}&units=metric";

            Debug.Log("url=" + url);
            UnityWebRequest www = UnityWebRequest.Get(url);
            yield return www.SendWebRequest();
            if (!isClick)
            {
                StartCoroutine(LoadInfo());
            }
            string json = www.downloadHandler.text;
            Debug.Log("json=" + json);
            json = json.Replace("\"base\":", "\"basem\":");
            weatherInfo = JsonUtility.FromJson<WeatherData>(json);
            //main = JsonUtility.FromJson<OWM_Main>(json);

            if (weatherInfo.weather.Length > 0)
            {
                //string weather = weatherInfo.name;
                string weather = weatherInfo.weather[0].main;
                int temp = weatherInfo.main.temp;
                //int realtemp = temp - 273;
                Debug.Log("weather = " + weather);

                DateTime dt = UnixTimeStampToDateTime((double)(weatherInfo.dt + weatherInfo.timezone - (9 * 3600)));
                DateTime dt2 = UnixTimeStampToDateTime((double)(weatherInfo.dt + weatherInfo.timezone - (8 * 3600)));
                // DateTime dt3 = UnixTimeStampToDateTime((double)(weatherInfo.dt+weatherInfo.timezone-(10*3600)));
                // DateTime dt3 = UnixTimeStampToDateTime((double)(weatherInfo.dt+weatherInfo.timezone-(-5*3600)));
                // DateTime dt = UnixTimeStampToDateTime((double)weatherInfo.dt);

                countryText.text = srcCountry;// + ", " + weatherInfo.name;
                if (weather == "Clean")
                {
                    weather = "����";
                    weatherText.text = weather;
                }
                else if (weather == "Clouds")
                {
                    weather = "����";
                    weatherText.text = weather;
                }
                else if (weather == "Rain")
                {
                    weather = "��";
                    weatherText.text = weather;
                }
                else if (weather == "Snow")
                {
                    weather = "��";
                    weatherText.text = weather;
                }
                //weatherText.text = weather;
                maintempText.text = " " + temp.ToString() + " ��C";
                latitudeText.text = lat.ToString();
                longitudeText.text = lon.ToString();

                if (srcCountry == "���ѹα�")
                {
                    dtText.text = DateTime.Now.ToString("yyyy-MM-dd tt hh:mm:ss");
                }
                else if (srcCountry == "�Ϻ�")
                {
                    dtText.text = dt2.ToString();
                }
                else
                {
                    dtText.text = dt.ToString();
                }
                // dtText.text = time.ToString();               
            }
        }     

        public DateTime UnixTimeStampToDateTime(double unixTimeStamp)
        {
            // Unix timestamp is seconds past epoch
            DateTime dateTime = new(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            dateTime = dateTime.AddSeconds(unixTimeStamp).ToLocalTime();

            return dateTime;
        }

        IEnumerator LoadInfo()
        {
            StartCoroutine(CenterTo(map.countryHighlightedIndex));
            yield return new WaitForSecondsRealtime(1);
            isClick = true;
            info.transform.LeanScale(new Vector3(1, 1, 1), 0.1f).setEaseLinear();
            yield return new WaitForSecondsRealtime(2.25f);
            info.transform.LeanScale(new Vector3(0, 0, 0), 0.1f).setEaseLinear();
            yield return new WaitForSecondsRealtime(0.00001f);
            isClick = false;
        }
    }
}
