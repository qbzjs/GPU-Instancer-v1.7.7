using System.Collections;
using System.Collections.Generic;
using Mirror.Examples.Chat;
using UnityEngine;

public class GoldAccount : MonoBehaviour
{
    [Header("Gold General Setting")] [SerializeField]
    private float GoldAmountInAccount;

    [SerializeField] private float MaxGoldAbleToStore;

    public void SetMaxGoldAbleToStore(float newVal)
    {
        MaxGoldAbleToStore = newVal;
    }

    public float GetMaxGoldAbleToStore()
    {
        return MaxGoldAbleToStore;
    }


    public float GetGoldAmountInAccount()
    {
        return GoldAmountInAccount;
    }

    public void SetGoldAmountInAccount(float newVal)
    {
        GoldAmountInAccount = newVal;
    }

    public void DepositGold(float depositAmount)
    {
        if (CheckIfGoldExceedsMaximum(depositAmount))
        {
            GoldAmountInAccount += depositAmount;
        }
        else
        {
            GoldAmountInAccount = MaxGoldAbleToStore;
        }
    }


    public bool WithdrawlGold(float withdrawAmount)
    {
        if (CheckIfBalanceValidBeforePurchase(withdrawAmount))
        {
            GoldAmountInAccount -= withdrawAmount;
            return true;
        }
        else
        {
            //Technically it would be not a valid purchase.
            Debug.Log("Cannot withdraw more then the gold account has to offer.");
            return false;
        }
    }

    public bool CheckIfGoldExceedsMaximum(float goldAmountBeingAdded)
    {
        if (GoldAmountInAccount + goldAmountBeingAdded > MaxGoldAbleToStore)
        {
            return false;
        }

        return true;
    }

    public bool CheckIfBalanceValidBeforePurchase(float costOfProduct)
    {
        if (GoldAmountInAccount - costOfProduct <= 0)
        {
            return false;
        }

        return true;
    }
}