using SimpleJSON;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class APIManager : MonoBehaviour
{
    GameObject objectWeather;
    GameObject objectGPS;
    Text textWeather;
    Text textGPS;
    public ArrayList emotionlist = new ArrayList(); //유저 감정분석
    private string emotion = "";

    public void sendText(string answer)
    {
        StartCoroutine(Sentimental(answer));
    }
    
    void Start()
    {
        StartCoroutine(GeoCode(35.1536142f, 129.0495141f));
        StartCoroutine(Weather("Busan"));
        /*StartCoroutine(Sentimental("I am afraid!"));*/
        objectWeather = GameObject.Find( "Weather");
        textWeather = objectWeather.GetComponent<Text>();

        objectGPS = GameObject.Find("GPS");
        textGPS = objectGPS.GetComponent<Text>();
    } 

    IEnumerator GeoCode(float lat,float lng)
    {
        var latlng = lat + "," + lng;
        var url = "https://maps.googleapis.com/maps/api/geocode/json?latlng="+latlng+"&key=AIzaSyA5EQm7-s0qIUVNQahlXexiatm1oaBR-Uc";
        
        WWWForm form = new WWWForm();
        using (UnityWebRequest www = UnityWebRequest.Post(url, form))
        {
            www.SetRequestHeader("Content-Type", "application/x-www-form-urlencoded");
            yield return www.SendWebRequest();

            if (www.isNetworkError || www.isHttpError)
            {
                Debug.Log(www.error);
            }
            else
            {
                string data = www.downloadHandler.text;
                var N = JSONNode.Parse(data);
                var n2 = N["results"];
                
                var loc = n2[0]["address_components"];                
                
                print(loc);

                PlayerPrefs.SetString("location", loc);
            }
        }
    }

    IEnumerator Weather(string place)
    {
        var url = "https://api.openweathermap.org/data/2.5/weather?q="+ place + "&appid=6ff47243ed9b57bf78a361d2f489a2c9";

        WWWForm form = new WWWForm();
        using (UnityWebRequest www = UnityWebRequest.Post(url, form))
        {
            www.SetRequestHeader("Content-Type", "application/x-www-form-urlencoded");
            yield return www.SendWebRequest();

            if (www.isNetworkError || www.isHttpError)
            {
                Debug.Log(www.error);
            }
            else
            {
                string data = www.downloadHandler.text;
                var N = JSONNode.Parse(data);
                var n2 = N["main"];

                var temp = float.Parse(n2["temp"]) - (273.15f);
                var humidity = n2["humidity"];

                var w = N["weather"][0]["main"];
                var country = N["sys"]["country"];
                var city = N["name"];

                textWeather.text = "Temperature: " + temp.ToString() + "°C" + "  Humidity:  " + humidity.ToString() + "  Weather:  " + w.ToString();
                textGPS.text = "Country :  " + country.ToString() + "   City : " + city.ToString();
                //replace를 활용
                //print(temp);
                //print(humidity);
                //print(w);
                // console.log("현재온도 : "+ (resp.main.temp- 273.15) );
                // console.log("현재습도 : "+ resp.main.humidity);
                // console.log("날씨 : "+ resp.weather[0].main );
                // console.log("상세날씨설명 : "+ resp.weather[0].description );
                // console.log("날씨 이미지 : "+ resp.weather[0].icon );
                // console.log("바람   : "+ resp.wind.speed );
                // console.log("나라   : "+ resp.sys.country );
                // console.log("도시이름  : "+ resp.name );
                // console.log("구름  : "+ (resp.clouds.all) +"%" );
                PlayerPrefs.SetString("Weather", w + "");
                PlayerPrefs.SetString("temp", temp + "");
                PlayerPrefs.SetString("humidity", humidity + "") ;
                PlayerPrefs.SetString("Country", country + "");
                PlayerPrefs.SetString("City", city + "");
            }
        }
    }

    IEnumerator Sentimental(string text)
    {
        WWWForm form = new WWWForm();
        var url = "http://ec2-52-78-77-42.ap-northeast-2.compute.amazonaws.com:3001/v1/sentimental?text="+ text;
        
        using (UnityWebRequest www = UnityWebRequest.Get(url))
        {
            www.SetRequestHeader("Content-Type", "application/json");
            //www.SetRequestHeader("json", "true");
            yield return www.SendWebRequest();

            if (www.isNetworkError || www.isHttpError)
            {
                Debug.Log(www.error);
            }
            else
            {
                string data = www.downloadHandler.text;
                var N = JSONNode.Parse(data);
                var n2 = N["Result"][0][0];
                var n3 = N["Result"][0][1];
                //print(n2);
                print(n3);

                emotion = n3;

                emotionlist.Add(emotion);
            }
        }
    }
}