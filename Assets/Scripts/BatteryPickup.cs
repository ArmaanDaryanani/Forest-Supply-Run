using UnityEngine;

public class BatteryPickup : MonoBehaviour
{
    public float interactDistance = 2.4f;

    Transform player;
    CheckpointGameManager gameManager;

    void Start()
    {
        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
        if (playerObject != null)
        {
            player = playerObject.transform;
        }

        gameManager = FindAnyObjectByType<CheckpointGameManager>();
    }

    void Update()
    {
        transform.Rotate(0f, 80f * Time.deltaTime, 0f);

        if (player == null || gameManager == null || gameManager.IsGameEnded)
        {
            return;
        }

        if (Vector3.Distance(transform.position, player.position) <= interactDistance)
        {
            gameManager.ShowPrompt("press e to collect battery");
            if (Input.GetKeyDown(KeyCode.E))
            {
                gameManager.CollectBattery();
                Destroy(gameObject);
            }
        }
    }
}
