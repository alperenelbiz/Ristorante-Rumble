using UnityEngine;

public class Restaurant : MonoBehaviour
{
    public Chair[] chairs;

  

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
