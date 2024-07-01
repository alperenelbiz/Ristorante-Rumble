using Mirror;
using UnityEngine;

public class StealMoney : NetworkBehaviour
{
    [SyncVar] private int carryingMoney = 0;
    private bool isStealing = false;

    public Transform caseTransform;
    [SerializeField] private float stealInterval = 0.5f;
    [SerializeField] private int stealingAmount = 5;
    public KeyCode stealKey = KeyCode.E;
    public KeyCode putKey = KeyCode.Q;

    private TeamManager teamManager;

    private void Start()
    {
        teamManager = GetComponent<TeamManager>();
    }

    void Update()
    {
        if (!isLocalPlayer) return;

        if (Vector3.Distance(transform.position, caseTransform.position) < 2.0f && Input.GetKeyDown(stealKey))
        {
            isStealing = true;
        }
        else if (Input.GetKeyUp(stealKey))
        {
            isStealing = false;
        }

        if (isStealing)
        {
            StealAmountOverTime();
        }

        if (Input.GetKey(putKey))
        {
            PutMoney();
        }
    }

    [Command]
    private void StealAmountOverTime()
    {
        if (Time.time % stealInterval < Time.deltaTime)
        {
            carryingMoney += stealingAmount;
            Restaurant opponentRestaurant = teamManager.team == Team.TeamA ? teamManager.teamBRestaurant : teamManager.teamARestaurant;
            opponentRestaurant.totalMoney -= stealingAmount;
            Debug.Log("Amount decreased! New amount: " + opponentRestaurant.totalMoney);
            Debug.Log("Carrying Money: " + carryingMoney);
        }
    }

    [Command]
    private void PutMoney()
    {
        Restaurant ownRestaurant = teamManager.GetTeamRestaurant();
        ownRestaurant.totalMoney += carryingMoney;
        carryingMoney = 0;
        Debug.Log("Amount increased! New amount: " + ownRestaurant.totalMoney);
        Debug.Log("Carrying Money: " + carryingMoney);
    }
}
