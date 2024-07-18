using UnityEngine;
using System;

public class WeatherManager : MonoBehaviour{
    public static WeatherManager Instance { get; set; }
    private void Awake() {
        if (Instance != null && Instance != this) {
            Destroy(gameObject);
        } else {
            Instance = this;
        }
    }

    public enum Weather {
        Sunny,//无特殊
        Cloudy,//天气好，很多外卖员都来抢单，抢单时间变短
        Rainy,//速度变慢，但订单利润更高
        Foggy,//速度变慢，但抢单时间变长
    }

    public Weather weather;
    public Canvas canvas;

    void Start() {
        InitWeather();     
        Debug.Log("Weather: " + weather);
        if(weather == Weather.Sunny){
            GameObject w = Instantiate(Resources.Load("PreFabs/SunnyPre"),canvas.transform) as GameObject;
        }
        else if(weather == Weather.Cloudy){
            GameObject w = Instantiate(Resources.Load("PreFabs/CloudyPre"),canvas.transform) as GameObject;
        }
        else if(weather == Weather.Rainy){
            GameObject w = Instantiate(Resources.Load("PreFabs/RainyPre"),canvas.transform) as GameObject;
        }
        else if(weather == Weather.Foggy){
            GameObject w = Instantiate(Resources.Load("PreFabs/FoggyPre"),canvas.transform) as GameObject;
        }
    }

    void Update() {
    }

    private void InitWeather() {
        int day = DeliverymanManager.Instance.round;
        if(day == 0){
            weather = Weather.Sunny;
        }
        else{
            int random = UnityEngine.Random.Range(0, 100);
            if(random < 28){
                weather = Weather.Cloudy;
                TutorialManagerBehaviour.Cloudy();
            }
            else if(random < 56){
                weather = Weather.Rainy;
                TutorialManagerBehaviour.Rainy();
            }
            else if(random < 84){
                weather = Weather.Foggy;
                TutorialManagerBehaviour.Foggy();
            }
            else{
                weather = Weather.Sunny;
                TutorialManagerBehaviour.Sunny();
            }
        }
    }

    public Weather GetWeather(){
        return weather;
    }
}