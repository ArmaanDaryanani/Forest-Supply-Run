using UnityEngine;

public class ForestAmbience : MonoBehaviour
{
    public float volume = 0.18f;

    void Start()
    {
        AudioSource source = GetComponent<AudioSource>();
        if (source == null)
        {
            source = gameObject.AddComponent<AudioSource>();
        }

        source.clip = CreateLoop();
        source.loop = true;
        source.volume = volume;
        source.spatialBlend = 0f;
        source.Play();
    }

    AudioClip CreateLoop()
    {
        int sampleRate = 22050;
        int length = sampleRate * 4;
        float[] data = new float[length];

        for (int i = 0; i < length; i++)
        {
            float t = i / (float)sampleRate;
            float wind = Mathf.Sin(t * 2.2f) * 0.08f;
            float low = Mathf.Sin(t * 0.7f) * 0.05f;
            float chirp = Mathf.Sin(t * 28f) * Mathf.Sin(t * 1.3f) * 0.015f;
            data[i] = wind + low + chirp;
        }

        AudioClip clip = AudioClip.Create("forest ambience", length, 1, sampleRate, false);
        clip.SetData(data, 0);
        return clip;
    }
}
