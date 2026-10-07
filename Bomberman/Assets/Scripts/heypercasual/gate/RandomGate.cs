using UnityEngine;
using TMPro;
public enum GateType { Add, Subtract, Multiply, Divide }
public class RandomGate : MonoBehaviour
{
    [SerializeField] private TextMeshPro gateText;
    [SerializeField] private MeshRenderer gateMeshRenderer;
    [SerializeField] private Color addColor = Color.green;        
    [SerializeField] private Color subtractColor = Color.red;      
    [SerializeField] private Color multiplyColor = Color.blue;    
    [SerializeField] private Color divideColor = new Color(0.6f, 0f, 0.8f); 
    public GateType CurrentType { get; private set; }
    public int Value { get; private set; }
    private Collider gateCollider;
    private void Awake()
    {
        gateCollider = GetComponent<Collider>();
    }
    public void RandomizeGate()
    {
        CurrentType = (GateType)Random.Range(0, 4);
        Value = GetRandomValueForType(CurrentType);
        UpdateGateVisuals();
    }
    public void SetGateValues(GateType type, int value)
    {
        CurrentType = type;
        Value = value;
        UpdateGateVisuals();
    }
    public int GetRandomValueForType(GateType type)
    {
        switch (type)
        {
            case GateType.Add: return Random.Range(3, 15);
            case GateType.Subtract: return Random.Range(1, 10);
            case GateType.Multiply: return Random.Range(2, 4);
            case GateType.Divide: return Random.Range(2, 3);
            default: return 1;
        }
    }
    private void UpdateGateVisuals()
    {
        string symbol = "";
        Color targetColor = Color.white;
        switch (CurrentType)
        {
            case GateType.Add:
                symbol = "+";
                targetColor = addColor;
                break;
            case GateType.Subtract:
                symbol = "-";
                targetColor = subtractColor;
                break;
            case GateType.Multiply:
                symbol = "x";
                targetColor = multiplyColor;
                break;
            case GateType.Divide:
                symbol = "÷";
                targetColor = divideColor;
                break;
        }
        if (gateText != null) gateText.text = symbol + Value;
        if (gateMeshRenderer != null) gateMeshRenderer.material.color = targetColor;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            BridgeManager bridgeManager = FindObjectOfType<BridgeManager>();
            if (bridgeManager != null)
            {
                ApplyGateEffect(bridgeManager);
            }
            if (gateMeshRenderer != null) gateMeshRenderer.enabled = false;
            if (gateCollider != null) gateCollider.enabled = false;
            if (gateText != null) gateText.enabled = false;
        }
    }
    private void ApplyGateEffect(BridgeManager bridgeManager)
    {
        switch (CurrentType)
        {
            case GateType.Add: bridgeManager.AddWoodAmount(Value); break;
            case GateType.Subtract: bridgeManager.RemoveWoodAmount(Value); break;
            case GateType.Multiply: bridgeManager.MultiplyWoodAmount(Value); break;
            case GateType.Divide: bridgeManager.DivideWoodAmount(Value); break;
        }
    } 
}
