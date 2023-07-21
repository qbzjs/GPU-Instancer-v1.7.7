using System.Collections;
using System.Collections.Generic;
using Mirror.Examples.Chat;
using UnityEngine;

public class GoldAccount : MonoBehaviour
{
    //Variables
    [Header("Gold Information")]
    [SerializeField] private float Gold = 250f;
    
    //Banking Events
    
    
    //eventually i believe this gold should be split up into a interface, and added onto each character since it will allow me to make an ai and then use this on the ai controller.


    public void DepositGold(float valueToDeposit)
    {
        Gold += valueToDeposit;
    }

    public void WithdrawlGold(float valueToWithdrawl)
    {
        Gold -= valueToWithdrawl;
    }

    public void ResetGoldToZero()
    {
        Gold = 0f;
    }

    public void SetGoldValue(float newGoldValue)
    {
        Gold = newGoldValue;
    }
    
    public bool IsBankEmpty()
    {
        return (Gold <= 0);
    }

    public bool IsPurchaseValid(float purchaseCost)
    {
        return ((Gold - purchaseCost) >= 0);
    }
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
