using System;
using System.Collections;
using System.Collections.Generic;
using BehaviorDesigner.Runtime.Tasks.Unity.UnityPhysics2D;
using Unity.Mathematics;
using UnityEngine;

public class Character_BuildingController : MonoBehaviour
{
    [Header("General Mode Checks")] [SerializeField]
    private bool BuildMode = false;

    [SerializeField] private bool EditMode = false;

    [SerializeField] private bool RotatingRight = false;
    [SerializeField] private bool RotatingLeft = false;

    [SerializeField] private bool AllowNewPosition = false;
    [SerializeField] private bool CurrentlyMoving = false;

    [Header("Building Preview Materials And Game Object")] [SerializeField]
    private Material ValidBuildLocationMaterial;

    [SerializeField] private Material InvalidBuildLocationMaterial;
    [SerializeField] public GameObject PreviewBuildingPoint;

    [Header("General Game Objects For Building Mode")] [SerializeField]
    private GameObject CurrentBuildingToPlace;

    [SerializeField] public GameObject CurrentBuildingSelected;
    [SerializeField] private GameObject CurrBuildingToBuildPreview;

    [Header("Building Important Layers")] [SerializeField]
    private LayerMask ValidBuildingLayers;

    [SerializeField] private LayerMask IgnoreBuildCheckLayers;
    [SerializeField] public LayerMask BuildingLayer;

    [Header("General Settings")] [SerializeField]
    private Vector3 BuildingRayCenterOffset;

    [SerializeField] private float MaxDistanceCheck = 15f;

    [Header("Rotation Settings")] [SerializeField]
    private float RotationSpeed = 50f;

    [SerializeField] public float CurrRotation = 0f;
    [SerializeField] public float SelectedCurrObjectYRotation = 0f;

    private void Update()
    {
        Debug.DrawRay(Camera.main.transform.position + BuildingRayCenterOffset,
            Camera.main.transform.forward * MaxDistanceCheck, Color.magenta);

        if (BuildMode && !EditMode)
        {
            DisplayPreviewOfBuilding();
        }

        if (BuildMode && EditMode && CurrentBuildingSelected != null && CurrentlyMoving)
        {
            Debug.Log("Currently moving building");
            MoveBuildingPosition();
        }

        if (BuildMode && EditMode && !CurrentlyMoving)
        {
            SelectAndDeselectRaycastedBuilding();
        }
    }

    #region Bool Getters And Setters

    public bool GetEditMode()
    {
        return EditMode;
    }

    public void SetEditMode(bool newVal)
    {
        //Edit mode allows us to delete / move buildings
        EditMode = newVal;
    }

    public bool GetBuildMode()
    {
        return BuildMode;
    }

    public void SetCurrentlyMoving(bool newVal)
    {
        CurrentlyMoving = newVal;
    }

    public bool GetCurrentlyMoving()
    {
        return CurrentlyMoving;
    }

    public void SetBuildMode(bool newVal)
    {
        BuildMode = newVal;
    }

    public bool GetAllowedNewPosition()
    {
        return AllowNewPosition;
    }

    public void SetAllowedNewPosition(bool newVal)
    {
        AllowNewPosition = newVal;
    }
    public bool GetRotateModeRight()
    {
        return RotatingRight;
    }

    public bool GetRotateModeLeft()
    {
        return RotatingLeft;
    }

    public void SetRotationRight(bool newVal)
    {
        RotatingRight = newVal;
    }

    public void SetRotationLeft(bool newVal)
    {
        RotatingLeft = newVal;
    }

    #endregion

    //By pressing the rotation keys we can rotate the building either left or right.
    public void RotateBuilding(bool LeftOrRight)
    {
        //If the edit mode is on and the building is selected then move the selected building.
        if (EditMode && CurrentBuildingSelected != null)
        {
            if (LeftOrRight)
            {
                //Rotate left
                Debug.Log("Rotating Selected Building Left");
                SelectedCurrObjectYRotation += -1f * RotationSpeed * Time.deltaTime;
            }
            else
            {
                //rotate right
                Debug.Log("Rotating Selected Building Right");
                SelectedCurrObjectYRotation += 1f * RotationSpeed * Time.deltaTime;
            }
        }
        else if (!EditMode && PreviewBuildingPoint.activeInHierarchy)
        {
            if (LeftOrRight)
            {
                //Rotate left
                Debug.Log("Rotating Left");
                CurrRotation += -1f * RotationSpeed * Time.deltaTime;
            }
            else
            {
                //rotate right
                Debug.Log("Rotating Right");
                CurrRotation += 1f * RotationSpeed * Time.deltaTime;
            }
        }
    }

    //This selects the correct building when your camera is looking at the building and it is a valid building object.
    //with this later we can have a cool ui appear above the building which can "open the building" or have other information such as "move/rotate" and delete.
    public void SelectAndDeselectRaycastedBuilding()
    {
        RaycastHit hit;

        if (Physics.Raycast(Camera.main.transform.position + BuildingRayCenterOffset, Camera.main.transform.forward,
                out hit, MaxDistanceCheck,
                ~IgnoreBuildCheckLayers))
        {
            if ((BuildingLayer & (1 << hit.collider.gameObject.layer)) != 0)
            {
                Debug.Log("Building Successfully Selected");
                CurrentBuildingSelected = hit.collider.GetComponent<Collider>().gameObject;
                SelectedCurrObjectYRotation = CurrentBuildingSelected.transform.rotation.y;
            }
            else
            {
                CurrentBuildingSelected = null;
            }
        }
        else
        {
            Debug.Log("Building is either to far away or no building in sight.");
            CurrentBuildingSelected = null;
        }
    }


    public void MoveBuildingPosition()
    {
        //Only let us move buildings and take this input if we have a building selected, (so in view) or if we have 
        if (!EditMode && !BuildMode && CurrentBuildingSelected == null && !CurrentlyMoving)
        {
            return;
        }

        RaycastHit hit;

        if (Physics.Raycast(Camera.main.transform.position + BuildingRayCenterOffset, Camera.main.transform.forward,
                out hit, MaxDistanceCheck,
                ~IgnoreBuildCheckLayers))
        {
            //Move / rotate regardless of where it is.
            CurrentBuildingSelected.transform.position = hit.point;
            CurrentBuildingSelected.transform.rotation = Quaternion.Euler(0f, SelectedCurrObjectYRotation, 0f);

            if ((ValidBuildingLayers & (1 << hit.collider.gameObject.layer)) != 0)
            {
                AllowNewPosition = true;
            }
            else
            {
                AllowNewPosition = false;
            }
        }
        else
        {
            Debug.Log("i am hitting literally nothing of interest.");
        }
    }

    public void DisplayPreviewOfBuilding()
    {
        RaycastHit hit;

        if (Physics.Raycast(Camera.main.transform.position + BuildingRayCenterOffset, Camera.main.transform.forward,
                out hit, MaxDistanceCheck,
                ~IgnoreBuildCheckLayers))
        {
            PreviewBuildingPoint.SetActive(true);

            PreviewBuildingPoint.transform.position = hit.point;

            PreviewBuildingPoint.GetComponent<MeshFilter>().mesh =
                CurrentBuildingToPlace.GetComponent<MeshFilter>().sharedMesh;

            PreviewBuildingPoint.transform.rotation = Quaternion.Euler(0f, CurrRotation, 0f);


            if ((ValidBuildingLayers & (1 << hit.collider.gameObject.layer)) != 0)
            {
                //Update the material to be the valid build material;
                PreviewBuildingPoint.GetComponent<MeshRenderer>().material = ValidBuildLocationMaterial;
            }
            else
            {
                //Update the material to be the invalid build material;
                PreviewBuildingPoint.GetComponent<MeshRenderer>().material = InvalidBuildLocationMaterial;
            }
        }
        else
        {
            //either we didnt hit or we are outside of raycast check 
            PreviewBuildingPoint.SetActive(false);
        }
    }

    /// <summary>
    /// Before we place down buildings in the future we want to spawn a "mockup" and then make it so we have to finish building the building before we can use it or display the proper finished model.
    /// </summary>
    public void PlaceDownBuilding()
    {
        RaycastHit hit;

        if (Physics.Raycast(Camera.main.transform.position + BuildingRayCenterOffset, Camera.main.transform.forward,
                out hit, MaxDistanceCheck, ~IgnoreBuildCheckLayers))
        {
            if ((ValidBuildingLayers & (1 << hit.collider.gameObject.layer)) != 0)
            {
                GameObject newPlacedBuilding =
                    Instantiate(CurrentBuildingToPlace, hit.point, Quaternion.Euler(0f, CurrRotation, 0f));
                Debug.Log("Placing down building");
            }
            else
            {
                Debug.Log("This is not a valid layer for placement.");
            }
        }
        else
        {
            Debug.Log("Outside of range or no object hit");
        }
    }

    public void DeleteBuilding()
    {
        if (!EditMode && !BuildMode && CurrentBuildingSelected == null && !CurrentlyMoving)
        {
            return;
        }

        Destroy(CurrentBuildingSelected);
        Debug.Log("Currently Destroying the selected Building");
    }

    //Check to be sure we have enough coins to place this current building, also check to make sure we dont have overlapping buildings before we place.
    public void CheckIfValidConditions()
    {
    }
}