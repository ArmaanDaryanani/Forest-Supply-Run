using UnityEngine;

public class MedkitPickup : MonoBehaviour
{
    public float interactDistance = 2.4f;
    public int healAmount = 1;

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
        transform.Rotate(0f, 45f * Time.deltaTime, 0f);

        if (player == null || gameManager == null || gameManager.IsGameEnded)
        {
            return;
        }

        if (Vector3.Distance(transform.position, player.position) <= interactDistance)
        {
            gameManager.ShowPrompt("press e to use medkit");
            if (Input.GetKeyDown(KeyCode.E))
            {
                gameManager.HealPlayer(healAmount);
                Destroy(gameObject);
            }
        }
    }
}
