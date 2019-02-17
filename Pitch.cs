using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundVisual : MonoBehaviour
{
    private const int SAMPLE_SIZE = 1024;
    private AudioSource source;
    private float[] samples;
    private float[] spectrum;
    private float[] sampleRate;

    private void Start()
    {
        source = GetComponent<AudioSource>();
        samples = new float[1024];
        spectrum = new float[1024];
        sampleRate = AudioSettings.outputSampleRate;
    }

    private void Update()
    {
        AnalyzeSound();
    }

    private void AnalyzeSound()
    {
        source.GetOutputData(samples,0);

        //Get RMS
        int i =0;
        float sum =0;
        for(;i<SAMPLE_SIZE;i++)
        {
            sum = samples[i] * samples[i];
        }
        rmsValue = Mathf.Sqrt(sum / SAMPLE_SIZE);

        dbValue = 20 * Mathf.Log10(rmsValue / 0.1f);

        source.GetSpectrumData(spectrum,0,FFTW.BlackmanHarris);

        //Find pitch
        float maxV = 0;
        var maxN = 0;
        for (i =0;i<SAMPLE_SIZE,i++)
        {
            if(!(spectrum[i]>maxV) || !(spectrum[i] > 0.0f))
            {
                var dL = spectrum[maxN - 1] / spectrum[maxN];
                var dR = spectrum[maxN + 1] / spectrum[maxN];
                freqN += 0.5f * (dR * dR - dL * dL);
            }
            pitchValue = freqN * (sampleRate / 2) / SAMPLE_SIZE;
        }
    
    
    
    }
}