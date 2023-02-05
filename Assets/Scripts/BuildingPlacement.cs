using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingPlacement : MonoBehaviour
{
    private bool currentlyPlacing;
    private bool currentlyBulldozering;

    private BuilldingPreset curBuildingPreset;

    private float indicatorUpdateRate = 0.05f; //seconds
    private float lastUpdateTime;
    private Vector3 curIndicatorPos;
    private GameObject build;

    public GameObject placementIndicator;
    public GameObject bulldozeIndicator;



    //called when we press a building UI button
    public void BeginNewBuildingPlacement(BuilldingPreset preset)
    {
        //TODO: make sure we have enough money

        currentlyPlacing = true;
        curBuildingPreset = preset;
        placementIndicator.SetActive(true);

    }

    //called when we place down a building or press Escape
    private void CancelBuildingPlacement()
    {
        
        currentlyPlacing = false;
        placementIndicator.SetActive(false);
       
    }

    //turn bulldoze on off
    public void ToggleBulldoze()
    {
        currentlyBulldozering = !currentlyBulldozering;
        bulldozeIndicator.SetActive(currentlyBulldozering);
    }

    private void Update()
    {
        //cancel building placement
        if(Input.GetKeyDown(KeyCode.Escape))
            CancelBuildingPlacement();

        //called every 0.05 seconds 
        if (Time.time - lastUpdateTime > indicatorUpdateRate)
        {
            lastUpdateTime = Time.time;

            //get the currently selected tile position
            curIndicatorPos = Selector.instance.GetCurTilePosition();

            //move the placement indicator or bulldoze indicator to the selected tile
            if (currentlyPlacing)
                placementIndicator.transform.position = curIndicatorPos;
            else if(currentlyBulldozering)
                bulldozeIndicator.transform.position = curIndicatorPos;
        }

        if (Input.GetKeyDown(KeyCode.R))
            RotateBuilding();

        //called when we press left mouse button
        if (Input.GetMouseButtonDown(0) && currentlyPlacing)
            PlaceBuilding();
        
        else if (Input.GetMouseButtonDown(0) && currentlyBulldozering)
            Bulldoze();

       
    }

    //deletes the currently selected building
    private void Bulldoze()
    {
        Building buildingToDestroy = City.instance.buildings.Find(x=>x.transform.position == curIndicatorPos);

        if (buildingToDestroy != null)
        {
            City.instance.OnRemoveBuilding(buildingToDestroy);
        }
    }

    //places down the currently selected building
    private void PlaceBuilding()
    {
        GameObject buildingObj = Instantiate(curBuildingPreset.prefab, curIndicatorPos, Quaternion.identity);
        

/*        Vector3 dummyPos = new Vector3(0, -99, 9);
        if (buildingObj.transform.position == dummyPos)
            Destroy(gameObject);
        else
            PlaceBuilding();*/


        City.instance.OnPlaceBuilding(buildingObj.GetComponent<Building>());
        build = buildingObj;

        if (!curBuildingPreset.prefab.tag.Equals("Road"))
            CancelBuildingPlacement();
    }

    private void RotateBuilding()
    {
        build.transform.Rotate(0f, 90f, 0f);

    }
}
