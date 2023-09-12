using System.Collections;
using System.Collections.Generic;
using MyGame.Scripts.Building;
using UnityEngine;

public class Building : MonoBehaviour
{
    //The buildings name
    [SerializeField]
    private string BuildingName = "Default Name";

    [SerializeField]
    private string BuildingDescription = "This is a default description....Fill it in.";
    
    //This is the buildings current Type
    [SerializeField]
    private MyLiege_Building_Types.BuildingType currBuildingsType = MyLiege_Building_Types.BuildingType.INFANTRY;
    
    //The cost to construct the building
    [SerializeField]
    private float CostOfBuilding = 150f;

    //The progress of the current buildings construction 
    [SerializeField]
    private float BuildingConstructionProgress;

    //The progress needed for the building to be finished construction
    [SerializeField]
    private float BuildingConstruction_PointsNeeded;

    [SerializeField]
    private GameObject Building_Before_Finish;
    [SerializeField]
    private GameObject Building_After_Finish;

    //The current buildings level
    [SerializeField]
    private int BuildingLevel = 1;

    //The current buildings health amount
    [SerializeField]
    private float BuildingCurrHealth = 100f;
    [SerializeField]
    private float BuildingsMaxHealth = 100f;
    
    
    
    
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
