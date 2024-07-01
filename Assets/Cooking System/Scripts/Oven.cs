using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Oven : NetworkBehaviour
{
    public List<Meal> availableMeals;
    private bool isCooking = false;
    private float cookingTimer;
    private Meal currentMeal;

    private void OnEnable()
    {
        OvenManager.Instance.RegisterOven(this);
    }

    private void OnDisable()
    {
        OvenManager.Instance.UnregisterOven(this);
    }

    public List<Meal> GetAvailableMeals()
    {
        return availableMeals;
    }

    public bool IsCooking()
    {
        return isCooking;
    }

    [Server]
    public void StartCooking(Meal meal)
    {
        if (isCooking) return;

        isCooking = true;
        currentMeal = meal;
        cookingTimer = meal.cookingTime;

        StartCoroutine(CookMeal());
    }

    [Server]
    private IEnumerator CookMeal()
    {
        while (cookingTimer > 0)
        {
            cookingTimer -= Time.deltaTime;
            yield return null;
        }

        FinishCooking();
    }

    [Server]
    private void FinishCooking()
    {
        isCooking = false;

        GameObject cookedMeal = Instantiate(currentMeal.mealPrefab, transform.position + Vector3.up, Quaternion.identity);
        NetworkServer.Spawn(cookedMeal);

        currentMeal = null;
    }
}
