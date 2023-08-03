using System.Collections;
using System.Collections.Generic;
using MyGame.Scripts.Building;
using UnityEngine;

public class Plot : MonoBehaviour
{
    public List<MyLiege_Building_Types.BuildingType> BuildingTypeFilter = new List<MyLiege_Building_Types.BuildingType>();

    public GameObject CurrentBuilding;
    
    

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }
}