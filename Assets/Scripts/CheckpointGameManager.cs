using UnityEngine;
using UnityEngine.SceneManagement;

public class CheckpointGameManager : MonoBehaviour
{
    public int totalSupplies = 5;
    public int maxHealth = 3;
    public float timeLimit = 180f;
    public AudioSource audioSource;
    public AudioClip collectSound;
    public AudioClip finishSound;

    int suppliesCollected;
    int health;
    float timeLeft;
    string promptText = "";
    int promptFrame;
    bool checkpointComplete;
    bool gameOver;
    bool hasBattery;

    public bool HasAllSupplies => suppliesCollected >= totalSupplies;
    public bool HasBattery => hasBattery;
    public bool IsGameEnded => checkpointComplete || gameOver;

    void Start()
    {
        health = maxHealth;
        timeLeft = timeLimit;

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

    void Update()
    {
        if (IsGameEnded)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            }

            return;
        }

        timeLeft -= Time.deltaTime;
        if (timeLeft <= 0f)
        {
            timeLeft = 0f;
            LoseGame();
        }
    }

    public void CollectSupply(string itemName)
    {
        if (IsGameEnded)
        {
            return;
        }

        suppliesCollected++;
        if (audioSource != null && collectSound != null)
        {
            audioSource.PlayOneShot(collectSound);
        }
    }

    public void CollectBattery()
    {
        if (IsGameEnded)
        {
            return;
        }

        hasBattery = true;
        if (audioSource != null && collectSound != null)
        {
            audioSource.PlayOneShot(collectSound);
        }
    }

    public void HealPlayer(int amount)
    {
        if (IsGameEnded)
        {
            return;
        }

        health = Mathf.Min(maxHealth, health + amount);
        if (audioSource != null && collectSound != null)
        {
            audioSource.PlayOneShot(collectSound);
        }
    }

    public void DamagePlayer(int amount)
    {
        if (IsGameEnded)
        {
            return;
        }

        health -= amount;
        if (health <= 0)
        {
            health = 0;
            LoseGame();
        }
    }

    public void ShowPrompt(string text)
    {
        promptText = text;
        promptFrame = Time.frameCount;
    }

    public void FinishCheckpoint()
    {
        if (IsGameEnded || !HasAllSupplies || !hasBattery)
        {
            return;
        }

        checkpointComplete = true;
        if (audioSource != null && finishSound != null)
        {
            audioSource.PlayOneShot(finishSound, 1f);
        }
    }

    void LoseGame()
    {
        gameOver = true;
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
        DrawLabel(new Rect(20, 60, 600, 40), "health " + health + " / " + maxHealth, mainStyle, shadowStyle);
        DrawLabel(new Rect(20, 100, 600, 40), "time " + Mathf.CeilToInt(timeLeft), mainStyle, shadowStyle);
        DrawLabel(new Rect(20, 140, 600, 40), "battery " + (hasBattery ? "yes" : "no"), mainStyle, shadowStyle);

        string objective = GetObjectiveText();
        DrawLabel(new Rect(20, 180, 900, 40), objective, mainStyle, shadowStyle);

        if (checkpointComplete || gameOver)
        {
            GUIStyle completeStyle = new GUIStyle(mainStyle);
            completeStyle.fontSize = 44;
            completeStyle.alignment = TextAnchor.MiddleCenter;
            GUIStyle completeShadow = new GUIStyle(completeStyle);
            completeShadow.normal.textColor = Color.black;
            string endText = checkpointComplete ? "you survived\npress space to restart" : "game over\npress space to restart";
            DrawLabel(new Rect(Screen.width / 2f - 360f, Screen.height / 2f - 80f, 720f, 160f), endText, completeStyle, completeShadow);
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

    string GetObjectiveText()
    {
        if (checkpointComplete)
        {
            return "objective complete";
        }

        if (gameOver)
        {
            return "objective failed";
        }

        if (!HasAllSupplies)
        {
            return "objective collect supplies";
        }

        if (!hasBattery)
        {
            return "objective find the radio battery";
        }

        return "objective return to camp radio";
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
