using UnityEngine;

public class CampCrate : MonoBehaviour
{
    public float interactDistance = 2.8f;

    Transform player;
    CheckpointGameManager gameManager;
    bool opened;

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
        if (opened || player == null || gameManager == null || gameManager.IsGameEnded)
        {
            return;
        }

        if (Vector3.Distance(transform.position, player.position) <= interactDistance)
        {
            gameManager.ShowPrompt("press e to open crate");
            if (Input.GetKeyDown(KeyCode.E))
            {
                opened = true;
                gameManager.OpenCampCrate();
                transform.localScale = new Vector3(transform.localScale.x, 0.45f, transform.localScale.z);
                transform.position += Vector3.down * 0.3f;
            }
        }
    }
}
