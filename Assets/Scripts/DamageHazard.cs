using UnityEngine;

public class DamageHazard : MonoBehaviour
{
    public int damage = 1;
    public float damageDistance = 1.8f;
    public float damageCooldown = 1.2f;

    Transform player;
    CheckpointGameManager gameManager;
    float nextDamageTime;

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

        if (Time.time >= nextDamageTime && Vector3.Distance(transform.position, player.position) <= damageDistance)
        {
            gameManager.DamagePlayer(damage);
            nextDamageTime = Time.time + damageCooldown;
        }
    }
}
