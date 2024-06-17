using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
/// <summary>
/// Manages the UI elements for the main menu.
/// </summary>
public class MenuUI : MonoBehaviour
{
    [Header("Screens")]
    public GameObject UIroot;
    public GameObject Conversation;
    public GameObject Complete;
    public GameObject Garden;
    public GameObject PlantPosCollection;
    public GameObject Gallery;
    public GameObject Oxygen;
    public GameObject PopupFlower;
    public GameObject PopupQuestion;
    public DisableTrackedVisuals disableTracked;
    public GameObject ExitPanel;

    [Header("Levels")]
     
    private bool[] levelsUnlocked;

    [Header("Buttons")]
    public Button[] levelButtons;

    [Header("Options")]
    public Slider volumeSlider;

    //Private Values
    private string sceneToLoad;
     
    //Displays the sent screen, while disabling all others.
    public void SetScreen (GameObject screen)
    {
        UIroot.SetActive(false);
        Conversation.SetActive(false);
        Complete.SetActive(false);
        Garden.SetActive(false);
        Gallery.SetActive(false);
        PopupFlower.SetActive(false);
        PopupQuestion.SetActive(false);

        screen.SetActive(true);
        if (screen == Garden)//UI가 정원꾸미기이면
        {
            
            PlaceObjectsOnPlane.PlaceObjectsOn.DestroyFairy();
        }
        if (screen == UIroot)
        {
            
            DecoGarden.decoGarden.removePlants();            
            
        }
        if (screen == Gallery)
        {
            Oxygen.SetActive(false);
        }
        
    }    

    //Loads options from PlayerPrefs and sets them to the UI elements on the options screen.
    void LoadOptions ()
    {
        if(PlayerPrefs.HasKey("Volume"))
        {
            volumeSlider.value = PlayerPrefs.GetFloat("Volume");
        }
        else
        {
            volumeSlider.value = AudioListener.volume;
        }
        //settings.LoadPushSet();
    }

    //Called when the "Play" button on the menu screen is pressed.
    public void OnPlayButton ()
    {
       
    }

    //Called when the "Quit" button on the menu screen is pressed.
    public void OnQuitButton () 
    {
        //Application.Quit();
        
            
    }   

    //Called when the "Options" button (gear icon) on the menu screen is pressed.
    public void OnOptionsButton ()
    {
        //SetScreen(optionsScreen);
        LoadOptions();
    }

    //Called when the "Reset Progress" button on the options screen is pressed.
    public void OnResetProgressButton ()
    {
        
    }

    //Called when a "Level" button on the play screen is pressed.
    public void OnLevelButton (int level)
    {
         
    }

    //Loads the "sceneToLoad" scene.
    void LoadTargetScene ()
    {
        
    }

    //Called when the "Back" button on the play screen is pressed.
    public void OnBackButton ()
    {
        
    }

    //Called when the volume slider is changed.
    public void OnVolumeSliderChangeValue ()
    {
        AudioListener.volume = volumeSlider.value;
        PlayerPrefs.SetFloat("Volume", AudioListener.volume);
    }
     
}
