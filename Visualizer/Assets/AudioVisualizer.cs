using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioVisualizer : MonoBehaviour
{
    private const int SAMPLE_SIZE = 1024;
    public float visualModifier = 50.0f;
    public float smoothSpeed = 10.0f;
    public float maxVisualScale = 25.0f;
    public float keePercentage = 0.5f;
    public float rmsValue;
    public float dbValue;
    public float pitchValue;
    private AudioSource source;
    private float[] samples;
    private float[] spectrum;
    private float sampleRate;

    private Transform[] visualList;
    private float[] visualScale;
    private int amnVisual = 64;


    private void Start()
    {
        {
            //Assuming audio source on top of the object
            source = GetComponent<AudioSource>();
            samples = new float[1024];
            spectrum = new float[1024];
            sampleRate = AudioSettings.outputSampleRate;
            SpawnLine();
        }
    }

    private void SpawnLine()
    {
        visualScale = new float[amnVisual];
        visualList = new Transform[amnVisual];
        for (int i = 0;i< amnVisual; i++)
        {
            GameObject go = GameObject.CreatePrimitive(PrimitiveType.Cube) as GameObject;
            visualList[i] = go.transform;
            visualList[i].position = Vector3.right * i;

        }
    }

    private void Update()
    {
        AnalyzeSound();
        UpdateVisual();
    }

    private void UpdateVisual()
    {
        int visualIndex = 0;
        int spectrumIndex = 0;
        int averageSize = (int)((SAMPLE_SIZE * keePercentage) / amnVisual);

        while(visualIndex < amnVisual)
        {
            int j = 0;
            float sum = 0;
            while(j < averageSize)
            {
                sum += spectrum[spectrumIndex];
                spectrumIndex++;
                j++;
            }
            float scaleY = sum / averageSize * visualModifier;
            visualScale[visualIndex] -= Time.deltaTime * smoothSpeed;
            if (visualScale[visualIndex] < scaleY)
                visualScale[visualIndex] = scaleY;
            visualList[visualIndex].localScale = Vector3.one + Vector3.up * visualScale[visualIndex];
            visualIndex++;

            if (visualScale[visualIndex] > maxVisualScale)
                visualScale[visualIndex] = maxVisualScale;

        }

    }

    private void AnalyzeSound()
    {
        source.GetOutputData(samples, 0);
        // Fetching RMS Value

        int i = 0;
        float sum = 0;
        for (;i<SAMPLE_SIZE;i++)
        {
            sum = samples[i] * samples[i];
        }

        rmsValue = Mathf.Sqrt(sum / SAMPLE_SIZE);
        //Get db Value
        dbValue = 20 * Mathf.Log10(rmsValue / 0.1f);

        //Fetching Sound spextrum 
        source.GetSpectrumData(spectrum, 0, FFTWindow.BlackmanHarris);

        //Finding pitch
        float maxV = 0;
        var maxN = 0;
        for (i =0; i<SAMPLE_SIZE; i++)
        {
            if (!(spectrum[i] > maxV) || !(spectrum[i] > 0.0f))
                continue;
            maxV = spectrum[i];
            maxN = i;

        }

        float freqN = maxN;
        if(maxN > 0 && maxN < SAMPLE_SIZE - 1)
        {
            var dL = spectrum[maxN - 1] / spectrum[maxN];
            var dR = spectrum[maxN + 1] / spectrum[maxN];
            freqN += 0.5f * (dR * dR - dL * dL);
        }
        pitchValue = freqN * (sampleRate / 2) / SAMPLE_SIZE;
    }
}
