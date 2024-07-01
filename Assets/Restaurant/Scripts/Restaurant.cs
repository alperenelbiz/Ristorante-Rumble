using Mirror;
using UnityEngine;

public class Restaurant : NetworkBehaviour
{
    public Chair[] chairs;

    [SyncVar] public float totalReputation = 50f;
    [SyncVar] public int totalMoney;
    [SyncVar] public int totalIngredient = 0;

    private static float winThreshold = 70f;
    private static float loseThreshold = 30f;
    private int ingredient;

    private void Start()
    {
        totalMoney = 0;
        if (chairs.Length == 0)
        {
            Debug.LogError("No chairs found in " + gameObject.name);
        }
        else
        {
            Debug.Log(chairs.Length + " chairs found in " + gameObject.name);
        }
    }

    public Chair GetEmptyChair()
    {
        foreach (var chair in chairs)
        {
            if (!chair.IsOccupied)
            {
                return chair;
            }
        }
        return null;
    }

    [Command]
    public void IncreaseReputation(float reputationIncrease)
    {
        totalReputation += reputationIncrease;
        if (totalReputation >= winThreshold)
        {
            RpcWin();
        }
    }

    [Command]
    public void DecreaseReputation(float reputationDecrease)
    {
        totalReputation -= reputationDecrease;
        if (totalReputation <= loseThreshold)
        {
            RpcLose();
        }
    }

    [Command]
    public void IncreaseIngredient(int increaseIngredient)
    {
        totalIngredient += increaseIngredient;
    }

    [Command]
    public void DecreaseIngredient(int decreaseIngredient)
    {
        totalIngredient -= decreaseIngredient;
    }

    [ClientRpc]
    private void RpcWin()
    {
        // Handle winning logic
        Debug.Log("Team wins!");
    }

    [ClientRpc]
    private void RpcLose()
    {
        // Handle losing logic
        Debug.Log("Team loses!");
    }
}
