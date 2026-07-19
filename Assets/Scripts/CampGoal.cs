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
        if (player == null || gameManager == null || gameManager.IsGameEnded)
        {
            return;
        }

        if (Vector3.Distance(transform.position, player.position) <= interactDistance)
        {
            gameManager.ShowPrompt("press e to use radio");
            if (Input.GetKeyDown(KeyCode.E))
            {
                if (!gameManager.HasAllSupplies)
                {
                    gameManager.ShowAction("radio needs 5 supplies");
                }
                else if (!gameManager.HasBattery)
                {
                    gameManager.ShowAction("radio needs battery");
                }
                else
                {
                    gameManager.ShowAction("calling for rescue");
                    gameManager.FinishCheckpoint();
                }
            }
        }
    }
}
