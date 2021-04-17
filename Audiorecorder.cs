using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class Audiorecorder : MonoBehaviour {

    public Transform Listener;
    public AudioListener audioListener;
    public AudioClip[] pianos;
    public AudioSource audiosource;
    int num = -1;
    float maxval = 0.0f;

    float[, ] MaxSpe = new float[7, 1024];

    // Start is called before the first frame update
    void Start () {

    }

    // Update is called once per frame
    void Update () {
        if (!audiosource.isPlaying) {
            num++;
            audiosource.PlayOneShot (pianos[num], 1.0f);
        }
        if (num == 7) {
            UnityEditor.EditorApplication.isPlaying = false;
        }
        var spectrum = audiosource.GetSpectrumData (1024, 0, FFTWindow.Hamming);
        if (maxval <= spectrum.Max ()) {
            for (int i = 1; i < spectrum.Length - 1; ++i) {
                MaxSpe[num, i] = spectrum[i];
            }
            maxval = spectrum.Max ();
        }

    }
}