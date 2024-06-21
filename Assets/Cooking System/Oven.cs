using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Oven : MonoBehaviour
{
    public List<Meal> availableMeals;
    private bool isCooking = false;
    private float cookingTimer;
    private Meal currentMeal;

    public List<Meal> GetAvailableMeals()
    {
        return availableMeals;
    }

    public bool IsCooking()
    {
        return isCooking;
    }

    public void StartCooking(Meal meal)
    {
        if (isCooking) return;

        isCooking = true;
        currentMeal = meal;
        cookingTimer = meal.cookingTime;

        StartCoroutine(CookMeal());
    }

    private IEnumerator CookMeal()
    {
        while (cookingTimer > 0)
        {
            cookingTimer -= Time.deltaTime;
            yield return null;
        }

        FinishCooking();
    }

    private void FinishCooking()
    {
        isCooking = false;
        Instantiate(currentMeal.mealPrefab, transform.position + Vector3.up, Quaternion.identity);
        currentMeal = null;
    }
}
