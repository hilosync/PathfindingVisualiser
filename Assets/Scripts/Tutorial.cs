using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Tutorial : MonoBehaviour
{
    [SerializeField] GameObject tutorialCanvas;
    [SerializeField] Button CloseButton;
    [SerializeField] Button OpenButton;

    public GameObject pathfindingObject;
    public Pathfinding pathfindingScript;

    private void Awake() 
    {
           
        
        CloseButton.onClick.RemoveAllListeners();
        CloseButton.onClick.AddListener(HideTutorial);

        OpenButton.onClick.RemoveAllListeners();
        OpenButton.onClick.AddListener(ShowTutorial);
    }

    void Update()
    {
        pathfindingObject = GameObject.Find("TileGenerator");
        pathfindingScript = pathfindingObject.GetComponent<Pathfinding>(); 
        if (pathfindingScript.sceneReset)
        {
            Debug.Log("tut");
            tutorialCanvas.SetActive(false);
            pathfindingScript.sceneReset = false;
        }
    }

    public void ShowTutorial()
    {
        tutorialCanvas.SetActive(true);
    }

    public void HideTutorial()
    {
        tutorialCanvas.SetActive(false);
    }

}

