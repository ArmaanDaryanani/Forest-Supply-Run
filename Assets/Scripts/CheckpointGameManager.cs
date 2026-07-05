using UnityEngine;

public class CheckpointGameManager : MonoBehaviour
{
    public int totalSupplies = 5;
    public AudioSource audioSource;
    public AudioClip collectSound;
    public AudioClip finishSound;

    int suppliesCollected;
    string promptText = "";
    int promptFrame;
    bool checkpointComplete;

    public bool HasAllSupplies => suppliesCollected >= totalSupplies;

    void Start()
    {
        if (audioSource == null)
        {
            audioSource = GetComponent<AudioSource>();
            if (audioSource == null)
            {
                audioSource = gameObject.AddComponent<AudioSource>();
            }
        }

        audioSource.spatialBlend = 0f;
        audioSource.volume = 1f;
        audioSource.playOnAwake = false;

        if (collectSound == null)
        {
            collectSound = CreateSound(880f, 4);
        }

        if (finishSound == null)
        {
            finishSound = CreateFinishSound();
        }
    }

    public void CollectSupply(string itemName)
    {
        suppliesCollected++;
        if (audioSource != null && collectSound != null)
        {
            audioSource.PlayOneShot(collectSound);
        }
    }

    public void ShowPrompt(string text)
    {
        promptText = text;
        promptFrame = Time.frameCount;
    }

    public void FinishCheckpoint()
    {
        if (checkpointComplete)
        {
            return;
        }

        checkpointComplete = true;
        if (audioSource != null && finishSound != null)
        {
            audioSource.PlayOneShot(finishSound, 1f);
        }
    }

    void LateUpdate()
    {
        if (promptFrame != Time.frameCount)
        {
            promptText = "";
        }
    }

    void OnGUI()
    {
        GUIStyle mainStyle = new GUIStyle(GUI.skin.label);
        mainStyle.fontSize = 28;
        mainStyle.fontStyle = FontStyle.Bold;
        mainStyle.normal.textColor = Color.white;

        GUIStyle shadowStyle = new GUIStyle(mainStyle);
        shadowStyle.normal.textColor = Color.black;

        DrawLabel(new Rect(20, 20, 600, 40), "supplies " + suppliesCollected + " / " + totalSupplies, mainStyle, shadowStyle);

        string objective = checkpointComplete ? "checkpoint complete" : HasAllSupplies ? "objective return to camp" : "objective collect the missing supplies";
        DrawLabel(new Rect(20, 60, 760, 40), objective, mainStyle, shadowStyle);

        if (checkpointComplete)
        {
            GUIStyle completeStyle = new GUIStyle(mainStyle);
            completeStyle.fontSize = 44;
            completeStyle.alignment = TextAnchor.MiddleCenter;
            GUIStyle completeShadow = new GUIStyle(completeStyle);
            completeShadow.normal.textColor = Color.black;
            DrawLabel(new Rect(Screen.width / 2f - 360f, Screen.height / 2f - 80f, 720f, 120f), "checkpoint complete", completeStyle, completeShadow);
        }

        if (!string.IsNullOrEmpty(promptText))
        {
            GUIStyle promptStyle = new GUIStyle(mainStyle);
            promptStyle.alignment = TextAnchor.MiddleCenter;
            GUIStyle promptShadow = new GUIStyle(promptStyle);
            promptShadow.normal.textColor = Color.black;
            DrawLabel(new Rect(Screen.width / 2f - 260f, Screen.height - 100f, 520f, 60f), promptText, promptStyle, promptShadow);
        }
    }

    void DrawLabel(Rect rect, string text, GUIStyle style, GUIStyle shadowStyle)
    {
        Rect shadowRect = new Rect(rect.x + 2f, rect.y + 2f, rect.width, rect.height);
        GUI.Label(shadowRect, text, shadowStyle);
        GUI.Label(rect, text, style);
    }

    AudioClip CreateSound(float frequency, int lengthDivider)
    {
        int sampleRate = 22050;
        int length = sampleRate / lengthDivider;
        float[] data = new float[length];

        for (int i = 0; i < length; i++)
        {
            float t = i / (float)sampleRate;
            float fade = 1f - i / (float)length;
            data[i] = Mathf.Sin(t * frequency * Mathf.PI * 2f) * fade * 0.35f;
        }

        AudioClip clip = AudioClip.Create("ui sound", length, 1, sampleRate, false);
        clip.SetData(data, 0);
        return clip;
    }

    AudioClip CreateFinishSound()
    {
        int sampleRate = 22050;
        int length = sampleRate;
        float[] data = new float[length];

        for (int i = 0; i < length; i++)
        {
            float t = i / (float)sampleRate;
            float fade = 1f - i / (float)length;
            float toneA = Mathf.Sin(t * 660f * Mathf.PI * 2f);
            float toneB = Mathf.Sin(t * 990f * Mathf.PI * 2f);
            data[i] = (toneA + toneB) * fade * 0.4f;
        }

        AudioClip clip = AudioClip.Create("finish sound", length, 1, sampleRate, false);
        clip.SetData(data, 0);
        return clip;
    }
}
