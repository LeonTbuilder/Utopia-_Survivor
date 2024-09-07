using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BossEnemyEngine : MonoBehaviour
{
    #region Variables

    [Header("Enemy Settings")]
    public GameObject Target;
    public float moveSpeed = 5f;
    public int hP = 2;
    public int expReward = 1;

    [Header("Loot Settings")]
    public GameObject diamondPrefab;
    public GameObject heartPrefab;
    public float diamondDropChance = 25f;
    public float heartDropChance = 10f;

    [Header("Freeze Settings")]
    public static bool isFreezing;
    public float freezeDuration = 2f;
    private Color originalColor;

    private NavMeshAgent agent;
    private Animator animator;

    #endregion

    #region Unity Methods

    private void Start()
    {
        InitializeEnemy();
    }

    private void Update()
    {
        HandleMovement();
        CheckHealth();
    }

    private void OnTriggerEnter(Collider other)
    {
        HandleCollision(other);
    }

    private void OnTriggerStay(Collider other)
    {
        HandleAirDamage(other);
    }

    #endregion

    #region Initialization

    private void InitializeEnemy()
    {
        Target = GameObject.FindWithTag("Player");
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();

        agent.speed = moveSpeed;
        originalColor = GetComponent<MeshRenderer>().material.color;

        isFreezing = false;
    }

    #endregion

    #region Movement

    private void HandleMovement()
    {
        if (Target != null)
        {
            agent.SetDestination(Target.transform.position);
        }
    }

    #endregion

    #region Combat

    private void HandleCollision(Collider other)
    {
        if (other.CompareTag("FireBall"))
        {
            ApplyDamage(other.GetComponent<BossFireBallEngine>().damage);
            Destroy(other.gameObject);
        }
        else if (other.CompareTag("WaterBall"))
        {
            ApplyDamage(other.GetComponent<BossWaterEngine>().damage);

            if (isFreezing)
            {
                FreezeEnemy();
            }

            Destroy(other.gameObject);
        }
        else if (other.CompareTag("Lightning"))
        {
            ApplyDamage(other.GetComponent<BossLightningEngine>().damage);
        }
    }

    private void ApplyDamage(int damageAmount)
    {
        hP -= damageAmount;
    }

    private void FreezeEnemy()
    {
        StartCoroutine(FreezeRoutine());
    }

    private IEnumerator FreezeRoutine()
    {
        agent.speed = 0f;
        SetEnemyColor(Color.cyan);

        yield return new WaitForSeconds(freezeDuration);

        SetEnemyColor(originalColor);
        agent.speed = moveSpeed;
    }

    private void SetEnemyColor(Color color)
    {
        MeshRenderer renderer = GetComponent<MeshRenderer>();
        renderer.material.color = color;
    }

    #endregion

    #region Health Management

    private void CheckHealth()
    {
        if (hP <= 0)
        {
            OnDeath();
        }
    }

    private void OnDeath()
    {
        BossLvlSpawner.instance.RemoveEnemy(gameObject);
        BossLvlSpawner.instance.DeadBoss(gameObject);
        TryDropLoot();

        Target.GetComponent<BossPlayerEngine>().AddEXP(expReward);
        Destroy(gameObject);
    }

    private void TryDropLoot()
    {
        if (Random.Range(0f, 100f) <= diamondDropChance)
        {
            Instantiate(diamondPrefab, transform.position, Quaternion.identity);
        }

        if (Random.Range(0f, 100f) <= heartDropChance)
        {
            Instantiate(heartPrefab, transform.position, Quaternion.identity);
        }
    }

    #endregion

    #region Air Damage

    private void HandleAirDamage(Collider other)
    {
        if (other.CompareTag("AirSphere"))
        {
            StartCoroutine(AirDamageRoutine());
        }
    }

    private IEnumerator AirDamageRoutine()
    {
        yield return new WaitForSeconds(0.5f);
        hP--;
    }

    #endregion

    #region Public Methods

    public void ReduceHealthByLightning()
    {
        ApplyDamage(1);
    }

    #endregion
}
