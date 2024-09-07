using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using TMPro;

public class BossPlayerEngine : MonoBehaviour
{
    #region Variables

    [Header("Player Status")]
    public GameOverManager gameManager;
    public static BossPlayerEngine instance;
    public Transform closestEnemy;
    public BossLvlSpawner spawner;

    [SerializeField] private Animator animator;
    [SerializeField] private ControlerEngine controlerEngine;

    [SerializeField] private int playerHP = 30;
    [SerializeField] private int playerEXP = 0;
    [SerializeField] private int playerLvl = 0;
    [SerializeField] private int expPerLvl = 100;
    [SerializeField] private int maxSkillLvl = 3;

    [SerializeField] private TMP_Text lvlText;
    [SerializeField] private GameObject lvlUpPrefab;
    [SerializeField] private Slider hpBar;
    [SerializeField] private Slider expBar;

    [Header("Attacks")]
    [SerializeField] private GameObject atkSpawnPoint;
    [SerializeField] private GameObject spellSpawnPoint;

    [Header("Fire")]
    [SerializeField] private GameObject fireBallPrefab;
    [SerializeField] private GameObject fireUpgradeButton;
    [SerializeField] private int numOfFireBalls = 1;
    [SerializeField] private float fireConeRadius = 0;
    [SerializeField] private float fireConeAngle = 0;
    [SerializeField] private float fireBallCounter = 1f;
    private float fireBallTimer;

    [SerializeField] private int fireLVL = 1;

    [Header("Water")]
    [SerializeField] private GameObject waterBallPrefab;
    [SerializeField] private GameObject waterUpgradeButton;
    [SerializeField] private float waterBallCounter = 1f;
    private float waterBallTimer;

    [SerializeField] private int waterLVL = 0;

    [Header("Lightning")]
    [SerializeField] private GameObject lightningPrefab;
    [SerializeField] private GameObject lightningUpgradeButton;
    [SerializeField] private float lightningCounter = 1f;
    private float lightningTimer;

    [SerializeField] private static int lightningHitCount = 0;
    [SerializeField] private int lightningLVL = 0;

    [Header("Earth")]
    [SerializeField] private GameObject earthPrefab;
    [SerializeField] private GameObject earthUpgradeButton;
    [SerializeField] private float earthCounter = 1f;
    private float earthTimer;
    [SerializeField] private float earthSpawnRadius = 5f;

    [SerializeField] private int earthLVL = 0;

    [Header("Air")]
    [SerializeField] private GameObject AirSphere;
    [SerializeField] private GameObject airUpgradeButton;

    [SerializeField] private int airLVL = 0;

    [Header("Lists")]
    [SerializeField] private List<GameObject> upgradeButtons;

    [Header("UI")]
    [SerializeField] private GameObject UpgradePanel;
    [SerializeField] private GameObject UpgradeSelection;
    [SerializeField] private TMP_Text diamondText;

    private int diamondCount = 0;
    [SerializeField] private float playerSpeed = 5f;
    private Rigidbody rb;
    private Vector3 moveInput;

    #endregion

    #region Unity Methods

    private void Awake()
    {
        InitializeSingleton();
        controlerEngine = new ControlerEngine();
    }

    private void Start()
    {
        InitializePlayer();
    }

    private void Update()
    {
        UpdateUI();
        HandleUpgrade();
        HandlePlayerInput();
        HandleStatusUpdate();
        HandleShotEngine();
    }

    private void OnEnable()
    {
        controlerEngine.Player.Enable();
    }

    private void OnDisable()
    {
        controlerEngine.Player.Disable();
    }

    private void OnCollisionEnter(Collision collision)
    {
        HandleCollision(collision);
    }

    private void OnTriggerEnter(Collider other)
    {
        HandleCollectables(other);
    }

    #endregion

    #region Initialization Methods

    private void InitializeSingleton()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void InitializePlayer()
    {
        spawner.enemyCap = 30;

        hpBar.maxValue = playerHP;
        hpBar.value = playerHP;

        expBar.maxValue = expPerLvl;
        expBar.value = playerEXP;

        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();

        ResetTimers();

        animator.Play("Entrance");
    }

    private void ResetTimers()
    {
        fireBallTimer = fireBallCounter;
        earthTimer = earthCounter;
        waterBallTimer = waterBallCounter;
        lightningTimer = lightningCounter;
    }

    #endregion

    #region Player Actions

    private void HandlePlayerInput()
    {
        moveInput = controlerEngine.Player.MoveSet.ReadValue<Vector2>();
        moveInput.Normalize();

        Vector3 movement = new Vector3(moveInput.x, 0f, moveInput.y);
        rb.MovePosition(rb.position + movement * playerSpeed * Time.fixedDeltaTime);

        if (movement != Vector3.zero)
        {
            RotatePlayer(movement);
            animator.SetBool("isRunning", true);
        }
        else
        {
            animator.SetBool("isRunning", false);
        }
    }

    private void RotatePlayer(Vector3 movement)
    {
        float angle = Mathf.Atan2(movement.x, movement.z) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, angle, 0f);
    }

    private void HandleShotEngine()
    {
        HandleFireBallShot();
        HandleWaterBallShot();
        HandleEarthSpawning();
        HandleLightningShot();
    }

    private void HandleFireBallShot()
    {
        fireBallTimer -= Time.deltaTime;

        if (fireBallTimer <= 0f)
        {
            ShootFireBalls();
            fireBallTimer = fireBallCounter;
        }
    }

    private void ShootFireBalls()
    {
        float angleIncrement = fireConeAngle / numOfFireBalls;
        float playerRotationY = transform.eulerAngles.y;

        for (int i = 0; i < numOfFireBalls; i++)
        {
            float currentAngle = i * angleIncrement - fireConeAngle / 2f + playerRotationY;
            Vector3 spawnPos = atkSpawnPoint.transform.position;
            Quaternion rotation = Quaternion.Euler(0f, currentAngle, 0f);

            Instantiate(fireBallPrefab, spawnPos, rotation);
        }
    }

    private void HandleWaterBallShot()
    {
        waterBallTimer -= Time.deltaTime;

        if (waterBallTimer <= 0f)
        {
            closestEnemy = FindClosestEnemy();
            Instantiate(waterBallPrefab, transform.position, transform.rotation);
            waterBallTimer = waterBallCounter;
        }
    }

    private void HandleEarthSpawning()
    {
        earthTimer -= Time.deltaTime;

        if (earthTimer <= 0f)
        {
            StartCoroutine(SpawnEarth());
            earthTimer = earthCounter;
        }
    }

    private void HandleLightningShot()
    {
        lightningTimer -= Time.deltaTime;

        if (lightningTimer <= 0f)
        {
            closestEnemy = FindClosestEnemy();
            Instantiate(lightningPrefab, transform.position, transform.rotation);
            lightningTimer = lightningCounter;
        }
    }

    private IEnumerator SpawnEarth()
    {
        int spawnCount = earthLVL + 1;

        for (int i = 0; i < spawnCount; i++)
        {
            Vector2 randomPoint = Random.insideUnitCircle.normalized * earthSpawnRadius;
            Vector3 earthSpawnPos = new Vector3(randomPoint.x, 0, randomPoint.y) + transform.position;

            Instantiate(earthPrefab, earthSpawnPos, transform.rotation);
            yield return new WaitForSeconds(0.5f);
        }
    }

    private void TakeDamage()
    {
        playerHP--;
        if (playerHP <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Destroy(gameObject, 1.5f);
        gameManager.GameOver();
    }

    #endregion

    #region Upgrade Methods

    private void HandleUpgrade()
    {
        if (playerEXP >= expBar.maxValue)
        {
            LevelUp();
            ShowUpgradePanel();
        }
    }

    private void LevelUp()
    {
        playerLvl++;
        playerEXP = 0;
        expPerLvl += 5;

        expBar.maxValue = expPerLvl;
        expBar.value = playerEXP;

        StartCoroutine(LvlUpRoutine());
    }

    private IEnumerator LvlUpRoutine()
    {
        yield return new WaitForSeconds(0.1f);
        GameObject lvlUpInstance = Instantiate(lvlUpPrefab, spellSpawnPoint.transform.position, transform.rotation);
        lvlUpInstance.transform.SetParent(spellSpawnPoint.transform);
    }

    private void ShowUpgradePanel()
    {
        RemoveMaxedUpgradeButtons();

        if (upgradeButtons.Count > 0)
        {
            DeactivateAllButtons();
            ActivateRandomButtons(3);
            Time.timeScale = 0f;
            UpgradePanel.SetActive(true);
        }
    }

    private void DeactivateAllButtons()
    {
        foreach (GameObject button in upgradeButtons)
        {
            button.SetActive(false);
        }
    }

    private void RemoveMaxedUpgradeButtons()
    {
        if (fireLVL >= maxSkillLvl) RemoveUpgradeButton(fireUpgradeButton);
        if (waterLVL >= maxSkillLvl) RemoveUpgradeButton(waterUpgradeButton);
        if (lightningLVL >= maxSkillLvl) RemoveUpgradeButton(lightningUpgradeButton);
        if (earthLVL >= maxSkillLvl) RemoveUpgradeButton(earthUpgradeButton);
        if (airLVL >= maxSkillLvl) RemoveUpgradeButton(airUpgradeButton);
    }

    private void RemoveUpgradeButton(GameObject upgradeButton)
    {
        upgradeButtons.Remove(upgradeButton);
        upgradeButton.SetActive(false);
    }

    private void ActivateRandomButtons(int numButtons)
    {
        numButtons = Mathf.Min(numButtons, upgradeButtons.Count);

        List<GameObject> availableButtons = new List<GameObject>(upgradeButtons);

        for (int i = 0; i < numButtons; i++)
        {
            if (availableButtons.Count > 0)
            {
                int randomIndex = Random.Range(0, availableButtons.Count);
                availableButtons[randomIndex].SetActive(true);
                availableButtons.RemoveAt(randomIndex);
            }
        }
    }

    #endregion

    #region Status and UI Update

    private void UpdateUI()
    {
        hpBar.value = playerHP;
        expBar.value = playerEXP;
        lvlText.text = "Lvl " + playerLvl;
    }

    private void HandleStatusUpdate()
    {
        UpdateFireStatus();
        UpdateWaterStatus();
        UpdateLightningStatus();
        UpdateAirStatus();
    }

    private void UpdateFireStatus()
    {
        switch (fireLVL)
        {
            case 1:
                numOfFireBalls = 1;
                fireConeRadius = 0;
                fireConeAngle = 0;
                break;
            case 2:
                numOfFireBalls = 3;
                fireConeRadius = 5;
                fireConeAngle = 60;
                break;
            case 3:
                numOfFireBalls = 10;
                fireConeRadius = 5;
                fireConeAngle = 360;
                break;
        }
    }

    private void UpdateWaterStatus()
    {
        switch (waterLVL)
        {
            case 0:
                waterBallCounter = 1f;
                break;
            case 1:
                waterBallCounter = 0.4f;
                break;
            case 2:
                waterBallCounter = 0.3f;
                break;
            case 3:
                waterBallCounter = 0.2f;
                BossEnemyEngine.isFreezing = true;
                break;
        }
    }

    private void UpdateLightningStatus()
    {
        switch (lightningLVL)
        {
            case 0:
                lightningHitCount = 0;
                break;
            case 1:
                lightningHitCount = 2;
                break;
            case 2:
                lightningHitCount = 3;
                break;
            case 3:
                lightningHitCount = 5;
                break;
        }
    }

    private void UpdateAirStatus()
    {
        switch (airLVL)
        {
            case 0:
                AirSphere.SetActive(false);
                AirSphere.transform.localScale = new Vector3(2f, 2f, 2f);
                break;
            case 1:
                AirSphere.SetActive(true);
                AirSphere.transform.localScale = new Vector3(2f, 2f, 2f);
                break;
            case 2:
                AirSphere.SetActive(true);
                AirSphere.transform.localScale = new Vector3(3f, 3f, 3f);
                break;
            case 3:
                AirSphere.SetActive(true);
                AirSphere.transform.localScale = new Vector3(4f, 4f, 4f);
                break;
        }
    }

    #endregion

    #region Combat

    private void HandleCollision(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            TakeDamage();
            Destroy(collision.gameObject);
        }
        else if (collision.gameObject.CompareTag("Boss"))
        {
            TakeDamage();
        }
    }

    private Transform FindClosestEnemy()
    {
        if (BossLvlSpawner.enemies.Count == 0) return null;

        Transform closestEnemyPos = null;
        float closestDistance = float.MaxValue;

        foreach (var enemy in BossLvlSpawner.enemies)
        {
            if (enemy != null)
            {
                float distanceToEnemy = Vector3.Distance(transform.position, enemy.transform.position);
                if (distanceToEnemy < closestDistance)
                {
                    closestDistance = distanceToEnemy;
                    closestEnemyPos = enemy.transform;
                }
            }
        }

        return closestEnemyPos;
    }

    #endregion

    #region Collectables

    private void HandleCollectables(Collider other)
    {
        if (other.gameObject.CompareTag("Heart"))
        {
            HealPlayer(5);
            Destroy(other.gameObject);
        }
        else if (other.gameObject.CompareTag("Diamond"))
        {
            diamondCount++;
            diamondText.text = diamondCount.ToString();
            Destroy(other.gameObject);
        }
    }

    private void HealPlayer(int healAmount)
    {
        playerHP = Mathf.Min(playerHP + healAmount, 30);
        hpBar.value = playerHP;
    }

    #endregion

    #region Public Methods

    public Transform GetClosestEnemy()
    {
        return closestEnemy;
    }

    public void AddEXP(int expReward)
    {
        playerEXP += expReward;
    }

    public void FireUpgrade()
    {
        UpgradeSkill(ref fireLVL);
    }

    public void WaterUpgrade()
    {
        UpgradeSkill(ref waterLVL);
    }

    public void LightningUpgrade()
    {
        UpgradeSkill(ref lightningLVL);
    }

    public void EarthUpgrade()
    {
        UpgradeSkill(ref earthLVL);
    }

    public void AirUpgrade()
    {
        UpgradeSkill(ref airLVL);
    }

    private void UpgradeSkill(ref int skillLevel)
    {
        if (skillLevel < maxSkillLvl)
        {
            skillLevel++;
        }
        UpgradePanel.SetActive(false);
        StartCoroutine(LvlUpRoutine());
        Time.timeScale = 1f;
    }

    #endregion
}
