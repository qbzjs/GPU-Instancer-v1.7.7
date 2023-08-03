using System.Collections;
using System.Collections.Generic;
using MyGame.Scripts.Building;
using UnityEngine;

public abstract class Building : MonoBehaviour
{
    //TODO: AI NEEDS TO BE HELPING TO BUILD, OR THE PLAYER NEEDS TO BE BUILDING.

    public MyLiege_Building_Types.BuildingType currentBuildingType = MyLiege_Building_Types.BuildingType.INFANTRY;

    //Variables
    [Header("Building General Settings")] public string BuildingName = "DefaultBuildingName";
    public float CostOfBuilding = 250f;
    public float BuildingHealth = 200f;
    public float BuildingMaintenanceCost = 35f;

    public Sprite BuildingSpriteForMenus;

    public bool IsCurrentlyUnderConstruction = false;
    public GameObject FullyBuiltModel;

    public List<ParticleSystem> DestroyedBuildingParticles;
    public GameObject DestroyedBuildingModel;
    public List<AudioClip> DestructionSoundClips;

    [Header("Building Construction Settings")]
    public float TimeToBuild = 5f;

    public List<GameObject> StagesOfBuildingProgress = new List<GameObject>();
    public List<AudioClip> BuildingConstructionSoundEffect = new List<AudioClip>();
    public List<GameObject> BuildingConstructionParticleSystem = new List<GameObject>();

    //Eventually use this : 
    public float buildingProgress = 0f;
    public float NumberOfBuildersHelping = 0f;

    [Header("Building Upgrade Settings")] public float BuildingLevel;
    public List<GameObject> UpgradedBuildingModel = new List<GameObject>();

    //Methods
    public abstract void RepairBuilding();
    public abstract void DamageBuilding();

    public abstract void DestroyBuilding();

    public abstract void MakeProgressTowardsConstruction();
    public abstract void MakeProgressTowardsUpgradedBuilding();
}