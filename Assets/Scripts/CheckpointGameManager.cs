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
    public AudioClip damageSound;

    int suppliesCollected;
    int health;
    float timeLeft;
    float damageFlashTime;
    float actionTextTime;
    string promptText = "";
    string actionText = "";
    int promptFrame;
    bool checkpointComplete;
    bool gameOver;
    bool hasBattery;
    bool campCrateOpened;

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

        if (damageSound == null)
        {
            damageSound = CreateDamageSound();
        }

        SpawnCampCrate();
    }

    void Update()
    {
        if (damageFlashTime > 0f)
        {
            damageFlashTime -= Time.deltaTime;
        }

        if (actionTextTime > 0f)
        {
            actionTextTime -= Time.deltaTime;
        }

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

    public void OpenCampCrate()
    {
        if (IsGameEnded || campCrateOpened)
        {
            return;
        }

        campCrateOpened = true;
        health = Mathf.Min(maxHealth, health + 1);
        timeLeft = Mathf.Min(timeLimit, timeLeft + 20f);
        ShowAction("camp crate opened health and time added");
        if (audioSource != null && collectSound != null)
        {
            audioSource.PlayOneShot(collectSound);
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
        damageFlashTime = 0.45f;
        if (audioSource != null && damageSound != null)
        {
            audioSource.PlayOneShot(damageSound, 1f);
        }

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

    public void ShowAction(string text)
    {
        actionText = text;
        actionTextTime = 2.2f;
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
        mainStyle.fontSize = 20;
        mainStyle.fontStyle = FontStyle.Bold;
        mainStyle.normal.textColor = Color.white;

        GUIStyle shadowStyle = new GUIStyle(mainStyle);
        shadowStyle.normal.textColor = Color.black;

        if (damageFlashTime > 0f)
        {
            GUI.color = new Color(1f, 0f, 0f, Mathf.Clamp01(damageFlashTime / 0.45f) * 0.28f);
            GUI.DrawTexture(new Rect(0f, 0f, Screen.width, Screen.height), Texture2D.whiteTexture);
            GUI.color = Color.white;
        }

        DrawLabel(new Rect(18, 18, 440, 30), "supplies " + suppliesCollected + " / " + totalSupplies, mainStyle, shadowStyle);
        DrawLabel(new Rect(18, 46, 440, 30), "health " + health + " / " + maxHealth, mainStyle, shadowStyle);
        DrawLabel(new Rect(18, 74, 440, 30), "time " + Mathf.CeilToInt(timeLeft), mainStyle, shadowStyle);
        DrawLabel(new Rect(18, 102, 440, 30), "battery " + (hasBattery ? "yes" : "no"), mainStyle, shadowStyle);

        string objective = GetObjectiveText();
        DrawLabel(new Rect(18, 130, 620, 30), objective, mainStyle, shadowStyle);

        if (checkpointComplete || gameOver)
        {
            GUIStyle completeStyle = new GUIStyle(mainStyle);
            completeStyle.fontSize = 32;
            completeStyle.alignment = TextAnchor.MiddleCenter;
            GUIStyle completeShadow = new GUIStyle(completeStyle);
            completeShadow.normal.textColor = Color.black;
            string endText = checkpointComplete ? "you survived\npress space to restart" : "game over\npress space to restart";
            DrawLabel(new Rect(Screen.width / 2f - 300f, Screen.height / 2f - 70f, 600f, 140f), endText, completeStyle, completeShadow);
        }

        if (!string.IsNullOrEmpty(promptText))
        {
            GUIStyle promptStyle = new GUIStyle(mainStyle);
            promptStyle.fontSize = 22;
            promptStyle.alignment = TextAnchor.MiddleCenter;
            GUIStyle promptShadow = new GUIStyle(promptStyle);
            promptShadow.normal.textColor = Color.black;
            DrawLabel(new Rect(Screen.width / 2f - 240f, Screen.height - 82f, 480f, 50f), promptText, promptStyle, promptShadow);
        }

        if (actionTextTime > 0f)
        {
            GUIStyle actionStyle = new GUIStyle(mainStyle);
            actionStyle.fontSize = 22;
            actionStyle.alignment = TextAnchor.MiddleCenter;
            GUIStyle actionShadow = new GUIStyle(actionStyle);
            actionShadow.normal.textColor = Color.black;
            DrawLabel(new Rect(Screen.width / 2f - 300f, 96f, 600f, 50f), actionText, actionStyle, actionShadow);
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
            return "objective collect 5 supplies for radio";
        }

        if (!hasBattery)
        {
            return "objective find battery for radio";
        }

        return "objective return to radio at camp";
    }

    void SpawnCampCrate()
    {
        if (GameObject.Find("Camp Supply Crate") != null)
        {
            return;
        }

        GameObject crate = GameObject.CreatePrimitive(PrimitiveType.Cube);
        crate.name = "Camp Supply Crate";
        crate.transform.position = new Vector3(-2.5f, 0.65f, -22f);
        crate.transform.localScale = new Vector3(1.6f, 1.1f, 1.2f);
        Renderer renderer = crate.GetComponent<Renderer>();
        if (renderer != null)
        {
            renderer.material.color = new Color(0.45f, 0.24f, 0.08f);
        }

        crate.AddComponent<CampCrate>();
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

    AudioClip CreateDamageSound()
    {
        int sampleRate = 22050;
        int length = sampleRate / 3;
        float[] data = new float[length];

        for (int i = 0; i < length; i++)
        {
            float t = i / (float)sampleRate;
            float fade = 1f - i / (float)length;
            float tone = Mathf.Sin(t * 160f * Mathf.PI * 2f);
            float buzz = Mathf.Sin(t * 52f * Mathf.PI * 2f);
            data[i] = (tone * 0.35f + buzz * 0.25f) * fade;
        }

        AudioClip clip = AudioClip.Create("damage sound", length, 1, sampleRate, false);
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
