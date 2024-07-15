using UnityEngine;

public class WeatherManager : MonoBehaviour{
    public static WeatherManager Instance { get; set; }

    public static OrderDB orderDB = null;

    private void Awake() {
        if (Instance != null && Instance != this) {
            Destroy(this.gameObject);
        } else {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
    }

    void Start() {
    }

    void Update() {
    }
}