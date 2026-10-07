using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using TMPro;
public class BridgeManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI woodCountText; 
    [SerializeField] private Transform woodStackParent; 
    [SerializeField] private GameObject woodPlankPrefab; 
    [SerializeField] private float plankHeightThickness = 0.15f; 
    [SerializeField] private Transform player;
    [SerializeField] private BridgeTilePool bridgeTilePool;
    [SerializeField] private float woodLength = 0.4f;
    [SerializeField] private float bridgeYPosition = -0.5f;
    [SerializeField] private Vector3 woodRotation = new Vector3(0f, 0f, 0f);
    [SerializeField] private int maxPlanksOnBridge = 10;
    [SerializeField] private float destroyDelay = 1.5f; 
    [SerializeField] private float gameOverDelay = 1.0f; 
    [SerializeField] private int collectedWoodCount = 0;
    private List<GameObject> visualWoodStack = new List<GameObject>(); 
    private Queue<GameObject> activeBridgePlanks = new Queue<GameObject>();
    private float nextSpawnZ = 0f;
    private bool isBuildingBridge = false;
    private Coroutine gameOverCoroutine;
    private void Start()
    {
        if (player == null)
        {
            GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
            if (playerObj != null) player = playerObj.transform;
        }
        if (bridgeTilePool == null)
        {
            bridgeTilePool = FindObjectOfType<BridgeTilePool>();
        }

        UpdateWoodUIAndVisuals();
    }
    private void Update()
    {
        CheckAndBuildBridge();
    }
    private void UpdateWoodUIAndVisuals()
    {
        if (woodCountText != null)
        {
            woodCountText.text = collectedWoodCount.ToString();
        }
        if (woodStackParent == null || woodPlankPrefab == null) return;
        while (visualWoodStack.Count < collectedWoodCount)
        {
            Vector3 spawnPos = woodStackParent.position + new Vector3(0f, visualWoodStack.Count * plankHeightThickness, 0f);
            GameObject newPlank = Instantiate(woodPlankPrefab, spawnPos, woodStackParent.rotation, woodStackParent);
            newPlank.transform.localPosition = new Vector3(0f, visualWoodStack.Count * plankHeightThickness, 0f);
            visualWoodStack.Add(newPlank);
        }
        while (visualWoodStack.Count > collectedWoodCount)
        {
            int lastIndex = visualWoodStack.Count - 1;
            GameObject objToDestroy = visualWoodStack[lastIndex];
            visualWoodStack.RemoveAt(lastIndex);
            Destroy(objToDestroy);
        }
    }
    public void AddWood()
    {
        collectedWoodCount++;
        UpdateWoodUIAndVisuals();
    }
    public void RemoveWood()
    {
        if (collectedWoodCount > 0)
        {
            collectedWoodCount--;
            UpdateWoodUIAndVisuals();
        }
    }

    private void CheckAndBuildBridge()
    {
        if (player == null || bridgeTilePool == null) return;
        Vector3 rayOrigin = player.position + Vector3.forward * 0.4f + Vector3.up * 0.5f;
        int groundLayer = LayerMask.GetMask("Ground");
        bool isGrounded = Physics.Raycast(rayOrigin, Vector3.down, 2f, groundLayer);
        if (!isGrounded)
        {
            if (!isBuildingBridge)
            {
                isBuildingBridge = true;
                nextSpawnZ = player.position.z + 0.1f;
            }
            while (player.position.z >= nextSpawnZ - 0.1f && collectedWoodCount > 0)
            {
                BuildPlank();
            }
            if (collectedWoodCount <= 0 && player.position.z >= nextSpawnZ - 0.1f)
            {
                if (gameOverCoroutine == null)
                {
                    gameOverCoroutine = StartCoroutine(TriggerGameOverWithDelay(gameOverDelay));
                }
            }
        }
        else
        {
            if (gameOverCoroutine != null)
            {
                StopCoroutine(gameOverCoroutine);
                gameOverCoroutine = null;
            }
            if (isBuildingBridge)
            {
                ClearActiveBridgePlanks();
                isBuildingBridge = false;
            }
        }
    }
    private void BuildPlank()
    {
        collectedWoodCount--;
        UpdateWoodUIAndVisuals(); 
        if (activeBridgePlanks.Count >= maxPlanksOnBridge)
        {
            GameObject oldestPlank = activeBridgePlanks.Dequeue();
            if (oldestPlank != null)
            {
                StartCoroutine(DisablePlankWithDelay(oldestPlank, destroyDelay));
            }
        }
        Vector3 spawnPos = new Vector3(0f, bridgeYPosition, nextSpawnZ);
        GameObject newPlank = bridgeTilePool.GetTileAtPosition(spawnPos, woodRotation);
        if (newPlank != null)
        {
            activeBridgePlanks.Enqueue(newPlank);
        }
        nextSpawnZ += woodLength;
    }
    private void ClearActiveBridgePlanks()
    {
        while (activeBridgePlanks.Count > 0)
        {
            GameObject plank = activeBridgePlanks.Dequeue();
            if (plank != null)
            {
                StartCoroutine(DisablePlankWithDelay(plank, destroyDelay));
            }
        }
    }
    private IEnumerator DisablePlankWithDelay(GameObject plank, float delay)
    {
        yield return new WaitForSeconds(delay);
        if (plank != null)
        {
            plank.SetActive(false);
        }
    } 
    private IEnumerator TriggerGameOverWithDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        if (GameManager.Instance != null)
        {
            GameManager.Instance.GameOver();
        }
    }
    public void AddWoodAmount(int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            AddWood();
        }
    }
    public void RemoveWoodAmount(int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            if (collectedWoodCount > 0)
            {
                RemoveWood();
            }
        }
    }
    public void MultiplyWoodAmount(int factor)
    {
        int currentCount = collectedWoodCount;
        int amountToAdd = (currentCount * factor) - currentCount;
        AddWoodAmount(amountToAdd);
    }
    public void DivideWoodAmount(int divisor)
    {
        if (divisor <= 0) return;
        int currentCount = collectedWoodCount;
        int targetCount = currentCount / divisor;
        int amountToRemove = currentCount - targetCount;
        RemoveWoodAmount(amountToRemove);
    }
}
