using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;


public class Tutorial : MonoBehaviour
{
    [SerializeField] GameObject tutorialCanvas;
    [SerializeField] Button CloseButton;
    [SerializeField] Button OpenButton;

    public GameObject pathfindingObject;
    public Pathfinding pathfindingScript;

    GameObject CreateAnim;
    GameObject DeleteAnim;
    GameObject MoveAnim;

    private void Awake() 
    {
        
        CloseButton.onClick.RemoveAllListeners();
        CloseButton.onClick.AddListener(HideTutorial);

        OpenButton.onClick.RemoveAllListeners();
        OpenButton.onClick.AddListener(ShowTutorial);
    }

    public void ShowTutorial()
    {
        tutorialCanvas.SetActive(true);
        CreateAnim = GameObject.Find("CreateAnim");
        DeleteAnim = GameObject.Find("DeleteAnim");
        MoveAnim = GameObject.Find("MoveAnim");

        VideoPlayer CreateAnimVideoPlayer = CreateAnim.GetComponent<VideoPlayer>();
        VideoPlayer DeleteAnimVideoPlayer = DeleteAnim.GetComponent<VideoPlayer>();
        VideoPlayer MoveAnimVideoPlayer = MoveAnim.GetComponent<VideoPlayer>();
        
        CreateAnimVideoPlayer.url = System.IO.Path.Combine (Application.streamingAssetsPath,"DrawingClip.mp4");
        DeleteAnimVideoPlayer.url = System.IO.Path.Combine (Application.streamingAssetsPath,"DeletingWall.mp4");
        MoveAnimVideoPlayer.url = System.IO.Path.Combine (Application.streamingAssetsPath,"MovingStartTile.mp4");
    }

    public void HideTutorial()
    {
        tutorialCanvas.SetActive(false);
    }

}

