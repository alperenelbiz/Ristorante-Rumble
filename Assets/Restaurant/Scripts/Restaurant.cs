using UnityEngine;

public class Restaurant : MonoBehaviour
{
    public Chair[] chairs;
    public float totalReputation = 50f; 
    public int totalMoney;
    public int totalIngredient=0;
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

    public void IncreaseReputation(float reputationIncrease)
    {
        totalReputation += reputationIncrease;
        if (totalReputation >= winThreshold)
        {
            //kazanma kýsmý
           
        }
    }

    public void DecreaseReputation(float reputationDecrease)
    {
        totalReputation -= reputationDecrease;
        if (totalReputation <= loseThreshold)
        {
            //kaybetme kýsmý
            
        }
    }

    public void IncreaseIngredient(int increaseIngredient)
    {
        totalIngredient += increaseIngredient;


    }

    public void DecreaseIngredient(int decreaseIngredient)
    {
        totalIngredient += decreaseIngredient;

    }
}
