using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class BgmManager : MonoBehaviour {
    // Start is called before the first frame update
    void Start() {
        switch (DeliverymanManager.Instance.round) {
            case 0:
                GetComponent<AudioSource>().clip = Resources.Load<AudioClip>("BGM/I");
                break;
            case 1:
                GetComponent<AudioSource>().clip = Resources.Load<AudioClip>("BGM/II");
                break;
            case 2:
                GetComponent<AudioSource>().clip = Resources.Load<AudioClip>("BGM/III");
                break;
            case 3:
                GetComponent<AudioSource>().clip = Resources.Load<AudioClip>("BGM/IV");
                break;
            case 4:
                GetComponent<AudioSource>().clip = Resources.Load<AudioClip>("BGM/V");
                break;
            default:
                GetComponent<AudioSource>().clip = Resources.Load<AudioClip>("BGM/BGM1");
                break;
        }
        GetComponent<AudioSource>().Play();
    }

    // Update is called once per frame
    void Update() {

    }
}
