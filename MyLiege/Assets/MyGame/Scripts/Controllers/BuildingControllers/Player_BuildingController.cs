using System.Collections;
using System.Collections.Generic;
using MyGame.Scripts.Building;
using UnityEngine;

public class Player_BuildingController : MonoBehaviour
{
    //todo: add in a empty plot check to be able to see if we have any certain constraints for that plot, like its a "defense" only plot meaning only defence buildings can be bought there.

    public GoldAccount player_BankAccount;

    public List<GameObject> Cavlary_Buildings_AvailableToBuild = new List<GameObject>();
    public List<GameObject> Infantry_Buildings_AvailableToBuild = new List<GameObject>();
    public List<GameObject> Defence_Buildings_AvailableToBuild = new List<GameObject>();
    public List<GameObject> StatUpgrade_Buildings_AvailableToBuild = new List<GameObject>();
    public List<GameObject> Ranged_Buildings_AvailableToBuild = new List<GameObject>();
    public List<GameObject> SiegeVehicles_Buildings_AvailableToBuild = new List<GameObject>();
    public List<GameObject> Legendary_Buildings_AvailableToBuild = new List<GameObject>();
    
    //all the buildings currently on the map.    
    public List<GameObject> Buildings_CurrentlyOnMap = new List<GameObject>();
    
    
    public void BuildBuilding(int buildingToBuild)
    {
    }

    //This checks the building and compares what the type is.
    public void CheckBuildingType()
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