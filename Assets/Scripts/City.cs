using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using UnityEngine.UI;

public class City : MonoBehaviour
{

    [Header("Day")]
    public GameObject sun;
    public float curDayTime;
    public float dayTime;


    public int money;
    public int day;
    public int curPopulation;
    public int curJobs;
    public int curFood;
    public int maxPopulation;
    public int maxJobs;
    public int incomePerJob;

    public TextMeshProUGUI statsText;
    public TextMeshProUGUI timeText;

    public List<Building> buildings = new List<Building>();

    public static City instance;

    
    private int hours;
    private int minutes;
    private float perDay;
    private float newDayTime;
    private int x = 1; //velocity day multiplier
  


    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
       

    }
    private void Update()
    {
        
    }

    private void FixedUpdate()
    {
        DayCicle();
        UpdateStatText();

    }

    private void DayCicle()
    {
        curDayTime += Time.deltaTime;
        perDay = curDayTime * 100 / dayTime;
        hours = (int)(perDay * 24 / 100);
        minutes = (int)((perDay * 3600 / 100) % 60);
        if (curDayTime > dayTime)
        {
            curDayTime = 0;
            EndTurn();
        }
        sun.transform.rotation = Quaternion.Euler((curDayTime/dayTime)*360, 0f, 0f);

        RenderSettings.skybox.SetFloat("_Rotation", Time.time*dayTime/60);
    }

    public void SubTime()
    {
        if (x == 2)
        {
            GameObject.Find("Canvas/StatsBarPanel/SubButton").SetActive(false);
            dayTime *= 2;
            GameObject.Find("Canvas/StatsBarPanel/Image/ImgX"+x).SetActive(false);
            x /= 2;
        }
        else
        {
            GameObject.Find("Canvas/StatsBarPanel/PlusButton").SetActive(true);
            dayTime *= 2;
            GameObject.Find("Canvas/StatsBarPanel/Image/ImgX"+x).SetActive(false);
            x /= 2;
        }
        

    }

    public void PlusTime()
    {
        if (x == 8)
        {
            GameObject.Find("Canvas/StatsBarPanel/PlusButton").SetActive(false);
            x *= 2;
            GameObject.Find("Canvas/StatsBarPanel/Image/ImgX"+x).SetActive(true);
            dayTime /= 2;
        }
        else
        {
            x *= 2;
            GameObject.Find("Canvas/StatsBarPanel/SubButton").SetActive(true);
            dayTime /= 2;
            GameObject.Find("Canvas/StatsBarPanel/Image/ImgX"+x).SetActive(true);
            
        }
    }

    //called when we place down a building
    public void OnPlaceBuilding (Building building)
    {
        buildings.Add(building);

        money -= building.preset.cost;

        maxPopulation += building.preset.population;
        maxJobs += building.preset.jobs;

        
    }

    //called when we bulldoze a building
    public void OnRemoveBuilding(Building building)
    {
        buildings.Remove(building);

        maxPopulation -= building.preset.population;
        maxJobs -= building.preset.jobs;
        Destroy(building.gameObject);

        UpdateStatText();
    }

    public void EndTurn()
    {
        day++;

        CalculateMoney();
        CalculatePopulation();
        CalculateJobs();
        CalculateFood();

        UpdateStatText();
    }

    private void UpdateStatText()
    {
        statsText.text = String.Format("Day:{0} Money:{1} Pop:{2}/{3} Jobs:{4}/{5} Food:{6}", new object[7] { day, money, curPopulation, maxPopulation, curJobs, maxJobs, curFood });
        timeText.text = String.Format("{0:D2}:{1:D2}",new object[2] { hours, minutes });
    }

    private void CalculateFood()
    {
        curFood = 0;

        foreach (Building building in buildings)
            curFood += building.preset.food;
    }

    private void CalculateJobs()
    {
        curJobs = Mathf.Min(curPopulation, maxJobs);
    }

    private void CalculatePopulation()
    {
        if (curFood >= curPopulation && curPopulation < maxPopulation)
        {
            curFood -= curPopulation / 4;
            curPopulation = Mathf.Min(curPopulation + (curFood / 4 ) , maxPopulation);
        }
        else if (curFood < curPopulation)
        {
            curPopulation = curFood;
        }
    }

    private void CalculateMoney()
    {
        money += curJobs * incomePerJob;

        foreach(Building building in buildings) 
            money -= building.preset.costPerTurn;
    }
}
