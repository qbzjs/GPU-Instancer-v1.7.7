using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BuildingUIInformation : MonoBehaviour
{
    public TextMeshProUGUI BuildMode_Status_Text;
    public TextMeshProUGUI EditMode_Status_Text;
    public TextMeshProUGUI CurrentSelectedBuilding_Text;
    public TextMeshProUGUI CurrentBuildingToPlace_Text;
    public TextMeshProUGUI BuildingError_Text;


    public void ChangeBuidingError(string newVal)
    {
        BuildingError_Text.text = newVal;
    }
    
    public void ChangeBuildModeStatus(string newVal)
    {
        BuildMode_Status_Text.text = newVal;
    }

    public void ChangeEditModeStatus(string newVal)
    {
        EditMode_Status_Text.text = newVal;
        
    }

    public void ChangeCurrentSelectedBuilding(string newVal)
    {
        CurrentSelectedBuilding_Text.text = newVal;
    }

    public void ChangeCurrentBuildingToPlace(string newVal)
    {
        CurrentBuildingToPlace_Text.text = newVal;
    }
}
