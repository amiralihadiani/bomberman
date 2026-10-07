using UnityEngine;

public class GatePair : MonoBehaviour
{
    [SerializeField] private RandomGate leftGate;
    [SerializeField] private RandomGate rightGate;
    private void Start()
    {
        SetupGates();
    }
    private void SetupGates()
    {
        if (leftGate == null || rightGate == null) return;
        leftGate.RandomizeGate();
        GateType rightType;
        int rightValue;
        do
        {
            rightType = (GateType)Random.Range(0, 4);
            rightValue = rightGate.GetRandomValueForType(rightType);
        }
        while (rightType == leftGate.CurrentType || rightValue == leftGate.Value);
        rightGate.SetGateValues(rightType, rightValue);
    }
}
