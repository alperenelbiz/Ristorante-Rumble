using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class PlayerScript : NetworkBehaviour
{
    public override void OnStartClient()
    {
        DontDestroyOnLoad(gameObject);
        base.OnStartClient();
    }
}
