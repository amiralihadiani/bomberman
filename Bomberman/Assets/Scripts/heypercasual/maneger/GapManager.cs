using UnityEngine;

public class GapManager : MonoBehaviour
{
    public static GapManager Instance { get; private set; }
    [SerializeField] private float initialGapLength = 3f; 
    [SerializeField] private float maxGapLength = 10f;    
    [SerializeField] private float gapIncreaseRate = 0.5f; 
    public float CurrentGapLength { get; private set; }
    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
        CurrentGapLength = initialGapLength;
    }
    public void IncreaseGapLength()
    {
        CurrentGapLength = Mathf.Min(CurrentGapLength + gapIncreaseRate, maxGapLength);
    } 
}
