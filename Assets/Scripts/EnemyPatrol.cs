using UnityEngine;

public class EnemyPatrol : MonoBehaviour
{
    public Vector3 pointA;
    public Vector3 pointB;
    public float speed = 3f;
    public float damageDistance = 2f;
    public float damageCooldown = 1.5f;

    Transform player;
    CheckpointGameManager gameManager;
    Vector3 targetPoint;
    float nextDamageTime;

    void Start()
    {
        targetPoint = pointB;

        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
        if (playerObject != null)
        {
            player = playerObject.transform;
        }

        gameManager = FindAnyObjectByType<CheckpointGameManager>();
    }

    void Update()
    {
        if (gameManager != null && gameManager.IsGameEnded)
        {
            return;
        }

        transform.position = Vector3.MoveTowards(transform.position, targetPoint, speed * Time.deltaTime);
        if (Vector3.Distance(transform.position, targetPoint) < 0.2f)
        {
            targetPoint = targetPoint == pointA ? pointB : pointA;
        }

        Vector3 lookDirection = targetPoint - transform.position;
        if (lookDirection.sqrMagnitude > 0.01f)
        {
            transform.rotation = Quaternion.LookRotation(lookDirection);
        }

        if (player != null && gameManager != null && Time.time >= nextDamageTime && Vector3.Distance(transform.position, player.position) <= damageDistance)
        {
            gameManager.DamagePlayer(1);
            nextDamageTime = Time.time + damageCooldown;
        }
    }
}
