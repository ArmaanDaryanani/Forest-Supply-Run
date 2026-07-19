using UnityEngine;

public class CollectibleSupply : MonoBehaviour
{
    public string itemName = "supply";
    public float interactDistance = 2.2f;

    Transform player;
    CheckpointGameManager gameManager;
    Renderer itemRenderer;

    void Start()
    {
        itemRenderer = GetComponent<Renderer>();
        SetSupplyColor();

        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
        if (playerObject != null)
        {
            player = playerObject.transform;
        }

        gameManager = FindAnyObjectByType<CheckpointGameManager>();
    }

    void SetSupplyColor()
    {
        if (itemRenderer == null)
        {
            return;
        }

        Color color = Color.yellow;
        if (itemName == "water")
        {
            color = Color.cyan;
        }
        else if (itemName == "food")
        {
            color = Color.red;
        }
        else if (itemName == "wood")
        {
            color = new Color(0.45f, 0.22f, 0.08f);
        }
        else if (itemName == "rope")
        {
            color = new Color(0.9f, 0.65f, 0.25f);
        }
        else if (itemName == "map")
        {
            color = Color.white;
        }

        itemRenderer.material.color = color;
    }

    void Update()
    {
        transform.Rotate(0f, 60f * Time.deltaTime, 0f);

        if (player == null || gameManager == null || gameManager.IsGameEnded)
        {
            return;
        }

        float distance = Vector3.Distance(transform.position, player.position);
        if (distance <= interactDistance)
        {
            gameManager.ShowPrompt("press e to collect " + itemName);
            if (Input.GetKeyDown(KeyCode.E))
            {
                gameManager.CollectSupply(itemName);
                Destroy(gameObject);
            }
        }
    }
}
