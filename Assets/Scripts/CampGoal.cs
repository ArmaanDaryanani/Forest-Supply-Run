using UnityEngine;

public class CampGoal : MonoBehaviour
{
    public float interactDistance = 7f;

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
        if (player == null || gameManager == null || !gameManager.HasAllSupplies)
        {
            return;
        }

        if (Vector3.Distance(transform.position, player.position) <= interactDistance)
        {
            gameManager.ShowPrompt("press e to finish at camp");
            if (Input.GetKeyDown(KeyCode.E))
            {
                gameManager.FinishCheckpoint();
            }
        }
    }
}
