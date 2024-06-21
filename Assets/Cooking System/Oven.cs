using System.Collections.Generic;
using UnityEngine;

public class Oven : MonoBehaviour
{
    public List<Meal> availableMeals;

    public List<Meal> GetAvailableMeals()
    {
        return availableMeals;
    }
}
