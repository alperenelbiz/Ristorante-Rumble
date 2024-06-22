using UnityEngine;

public class Restaurant : MonoBehaviour
{
    public Chair[] chairs;

    private void Start()
    {
       
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
}
