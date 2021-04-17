using System.Collections;
using System.Collections.Generic;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using UnityEngine;
using UnityEngine.UI;

public class ListenAnalyzer : Agent {
    public Transform Listener;
    public AudioListener audioListener;
    public AudioClip[] pianos;
    public AudioSource audiosource;
    public Text Anstx, reptx;
    int num;

    public override void Initialize () {

    }

    public override void OnEpisodeBegin () {
        if (!audiosource.isPlaying) {
            num = Random.Range (0, pianos.Length);
            audiosource.PlayOneShot (pianos[num], 1.0f);
        }
        Listener.localPosition = new Vector3 (Random.Range (-4.5f, -4.5f), Random.Range (-4.5f, -4.5f), Random.Range (-4.5f, -4.5f));
    }
    public override void CollectObservations (VectorSensor sensor) {
        var spectrum = new float[1024];
        sensor.AddObservation (pianos[num].GetData(spectrum, 0));
        for (int i = 1; i < spectrum.Length - 1; ++i) {
            Debug.DrawLine (
                new Vector3 (i - 1, spectrum[i] + 10, 0),
                new Vector3 (i, spectrum[i + 1] + 10, 0),
                Color.red);
            Debug.DrawLine (
                new Vector3 (i - 1, Mathf.Log (spectrum[i - 1]) + 10, 2),
                new Vector3 (i, Mathf.Log (spectrum[i]) + 10, 2),
                Color.cyan);
            Debug.DrawLine (
                new Vector3 (Mathf.Log (i - 1), spectrum[i - 1] - 10, 1),
                new Vector3 (Mathf.Log (i), spectrum[i] - 10, 1),
                Color.green);
            Debug.DrawLine (
                new Vector3 (Mathf.Log (i - 1), Mathf.Log (spectrum[i - 1]), 3),
                new Vector3 (Mathf.Log (i), Mathf.Log (spectrum[i]), 3),
                Color.yellow);
        }
    }
    public string[] ansst = { "C", "D", "E", "F", "G", "A", "B" };
    public override void OnActionReceived (float[] vectorAction) {
        if (audiosource.isPlaying) {
            float agent_ans = vectorAction[0];
            Anstx.text = ansst[num];
            reptx.text = ansst[(int) agent_ans];
            if ((int) agent_ans == num) {
                AddReward(0.01f);
            }
            else{
                AddReward(-0.0057f);
            }
        }
        if (!audiosource.isPlaying) {
            EndEpisode ();
        }
    }
}