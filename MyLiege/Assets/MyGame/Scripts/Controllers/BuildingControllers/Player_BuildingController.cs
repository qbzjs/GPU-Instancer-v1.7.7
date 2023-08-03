using System.Collections;
using System.Collections.Generic;
using MyGame.Scripts.Building;
using UnityEngine;

public class Player_BuildingController : MonoBehaviour
{
    public GoldAccount player_BankAccount;

    public List<Plot> OccupiedPlots = new List<Plot>();
    public List<Plot> UnoccupiedPlots = new List<Plot>();
    
    public List<Building> CurrentBuildingsPlaced = new List<Building>();

    [Header("Buildings Available To Build")]
    public List<GameObject> Cavlary_Buildings_AvailableToBuild = new List<GameObject>();
    public List<GameObject> Infantry_Buildings_AvailableToBuild = new List<GameObject>();
    public List<GameObject> Defence_Buildings_AvailableToBuild = new List<GameObject>();
    public List<GameObject> StatUpgrade_Buildings_AvailableToBuild = new List<GameObject>();
    public List<GameObject> Ranged_Buildings_AvailableToBuild = new List<GameObject>();
    public List<GameObject> SiegeVehicles_Buildings_AvailableToBuild = new List<GameObject>();
    public List<GameObject> Legendary_Buildings_AvailableToBuild = new List<GameObject>();


    public GameObject UI_Content_ButtonHolder;

    //This will be called by generic action when we interact with a empty plot.
    public void InteractedWithEmptyPlot(GameObject interactedWith)
    {
        Debug.Log("i have interacted with this ");
        Debug.Log("the object you interacted with: " + interactedWith.gameObject.name);
    }

    //This is called by generic action when we interact with a building that currently exists on a plot
    public void InteractedWithBuilding(GameObject interactedWith)
    {
        Debug.Log("i have interacted with this ");
        Debug.Log("the object you interacted with: " + interactedWith.gameObject.name);
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