using UnityEngine;

[CreateAssetMenu(fileName = "New Meal", menuName = "Meal")]
public class Meal : ScriptableObject
{
    public string mealName;
    public GameObject mealPrefab;
    public float cookingTime;
    public int ingredientAmount;
    public int price;
}
