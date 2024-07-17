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

    void Start() {
        InitWeather();     
        Debug.Log("Weather: " + weather);
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
            }
            else if(random < 56){
                weather = Weather.Rainy;
            }
            else if(random < 84){
                weather = Weather.Foggy;
            }
            else{
                weather = Weather.Sunny;
            }
        }
    }

    public Weather GetWeather(){
        return weather;
    }
}