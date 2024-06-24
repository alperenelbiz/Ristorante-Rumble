using System.Collections.Generic;
using UnityEngine;

public class OvenManager : MonoBehaviour
{
    private static OvenManager instance;
    public static OvenManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<OvenManager>();
            }
            return instance;
        }
    }

    private List<Oven> ovens = new List<Oven>();

    public void RegisterOven(Oven oven)
    {
        if (!ovens.Contains(oven))
        {
            ovens.Add(oven);
        }
    }

    public void UnregisterOven(Oven oven)
    {
        if (ovens.Contains(oven))
        {
            ovens.Remove(oven);
        }
    }

    public List<Meal> GetAllAvailableMeals()
    {
        List<Meal> allMeals = new List<Meal>();

        foreach (Oven oven in ovens)
        {
            allMeals.AddRange(oven.GetAvailableMeals());
        }

        return allMeals;
    }
}
