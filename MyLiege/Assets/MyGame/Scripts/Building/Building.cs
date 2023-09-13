using System.Collections;
using System.Collections.Generic;
using MyGame.Scripts.Building;
using UnityEngine;

public class Building : MonoBehaviour
{
    //The buildings name
    [SerializeField] private string BuildingName = "Default Name";

    [SerializeField] private string BuildingDescription = "This is a default description....Fill it in.";

    //This is the buildings current Type
    [SerializeField]
    private MyLiege_Building_Types.BuildingType currBuildingsType = MyLiege_Building_Types.BuildingType.INFANTRY;

    //The cost to construct the building
    [SerializeField] private float CostOfBuilding = 150f;

    //The progress of the current buildings construction 
    [SerializeField] private float BuildingConstructionProgress;

    //The progress needed for the building to be finished construction
    [SerializeField] private float BuildingConstruction_PointsNeeded;

    [SerializeField] private GameObject Building_Before_Finish;
    [SerializeField] private GameObject Building_After_Finish;

    //The current buildings level
    [SerializeField] private int BuildingLevel = 1;

    //The current buildings health amount
    [SerializeField] private float BuildingCurrHealth = 100f;
    [SerializeField] private float BuildingsMaxHealth = 100f;

    #region Getters

    public string GetBuildingName()
    {
        return BuildingName;
    }

    public string GetBuildingDescription()
    {
        return BuildingDescription;
    }

    public float GetCostOfBuilding()
    {
        return CostOfBuilding;
    }

    public float GetBuildingConstructionProgress()
    {
        return BuildingConstructionProgress;
    }

    public float GetBuildingConstructionPointsNeeded()
    {
        return BuildingConstruction_PointsNeeded;
    }

    public int GetBuildingLevel()
    {
        return BuildingLevel;
    }

    public float GetCurrBuildingHealth()
    {
        return BuildingCurrHealth;
    }

    public float GetCurrBuildingMaxHealth()
    {
        return BuildingsMaxHealth;
    }

    #endregion

    #region Setters

    public void SetBuildingName(string newVal)
    {
        BuildingName = newVal;
    }


    public void SetBuildingDescription(string newVal)
    {
        BuildingDescription = newVal;
    }

    public void SetCostOfBuilding(float newVal)
    {
        CostOfBuilding = newVal;
    }

    public void SetBuildingConstructionProgess(float newVal)
    {
        BuildingConstructionProgress = newVal;
    }


    public void SetBuildingConstructionPointsNeeded(float newVal)
    {
        BuildingConstruction_PointsNeeded = newVal;
    }

    public void SetBuildingLevel(int newVal)
    {
        BuildingLevel = newVal;
    }


    public void SetCurrBuildingHealth(float newVal)
    {
        BuildingCurrHealth = newVal;
    }

    public void SetCurrBuildingMaxHealth(float newVal)
    {
        BuildingsMaxHealth = newVal;
    }

    #endregion

    public void DamageBuilding()
    {
        
    }

    public void RepairBuilding()
    {
        
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