using UnityEngine;

public class Restaurant : MonoBehaviour
{
    public Chair[] chairs;
    public float totalReputation = 50f; 
    public int totalMoney;
    public float reputationIncrease = 5f; 
    public float reputationDecrease = 5f; 
    public static float winThreshold = 70f;
    public static float loseThreshold = 30f;

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

    public void IncreaseReputation()
    {
        totalReputation += reputationIncrease;
        if (totalReputation >= winThreshold)
        {
            //kazanma kýsmý
           
        }
    }

    public void DecreaseReputation()
    {
        totalReputation -= reputationDecrease;
        if (totalReputation <= loseThreshold)
        {
            //kaybetme kýsmý
            
        }
    }
}
