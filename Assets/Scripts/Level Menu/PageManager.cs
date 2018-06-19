using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PageManager : MonoBehaviour
{
    public Text pageName;
    public int currentLevelPage;
    public int currentWorld;
    public LevelThumbnail[] levels = new LevelThumbnail[10];

    private DataController dataController;

    void Start()
    {
        dataController = GameObject.FindGameObjectWithTag(Tags.DataController).GetComponent<DataController>();
        CreateCurrentLevelPage();
    }

    public void GoToNextPage()
    {
        currentLevelPage = ++currentLevelPage % LevelGroups().Length;
        CreateCurrentLevelPage();
    }

    public void GoToPreviousPage()
    {
        currentLevelPage--;
        if (currentLevelPage < 0)
        {
            currentLevelPage = LevelGroups().Length - 1;
        }
        CreateCurrentLevelPage();
    }

    private void CreateCurrentLevelPage()
    {
        LevelGroupData page = LevelGroups()[currentLevelPage];
        pageName.text = page.levelGroupName;

        if(page.levels.Length != 10) {
            Debug.Log("Invalid level group data: each level group MUST have exactly 10 levels. Check the data in the Game Data Editor.");
        }

        for (int i = 0; i < page.levels.Length; i++) {
            levels[i].SetLevelData(page.levels[i]);
        }
    }

    private LevelGroupData[] LevelGroups() {
        return dataController.Worlds[currentWorld].levelGroups;
    }
}
