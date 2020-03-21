using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UiManager : MonoBehaviour
{
    [HideInInspector]
    public static UiManager instance;


    public Animator animator;
    public List<UiBase> FullHeart;
    public List<UiBase> EmptyHeart;
    public UiBase Menu;
    public UiBase SplashArt;
    public UiBase Gameplay;
    public UiBase PauseMenu;
    public UiBase BackGroundPause;
    public UiBase BackGroundMenu;
    public void Start()
    {
   

        foreach (var item in animator.GetBehaviours<UiBaseState>())
        {
            item.SetContext(this, animator);
        }
    }
    public void init()
    {
        if (instance != null && instance == this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            instance = this;
        }

        DontDestroyOnLoad(this);
    }
    public void DisableMenu(MenuType _menuType)
    {
        switch (_menuType)
        {
            case MenuType.Menu:

                Menu.Disable();
                break;
            case MenuType.SplashPage:
                SplashArt.Disable();
                break;
            case MenuType.Gameplay:
                Gameplay.Disable();
                break;
            case MenuType.BackGroundPause:
                BackGroundPause.Disable();
                break;
            case MenuType.PauseMenu:
                PauseMenu.Disable();
                break;
            case MenuType.BackGroundMenu:
                BackGroundMenu.Disable();
                break;
        }
    }
    public void ChangeMenu(MenuType _menuType)
    {
        switch (_menuType)
        {
            case MenuType.Menu:
                
                Menu.Setup();
                break;
            case MenuType.SplashPage:
                SplashArt.Setup();
                break;
            case MenuType.Gameplay:
                Gameplay.Setup();
                break;
            case MenuType.BackGroundPause:
                BackGroundPause.Setup();
                break;
            case MenuType.PauseMenu:
                PauseMenu.Setup();
                break;
            case MenuType.BackGroundMenu:
                BackGroundMenu.Setup();
                break;


        }
    }
    public void LifeUpdate(int _lifes)
    {

        FullHeart[_lifes].Disable();
        EmptyHeart[_lifes].Setup();
    }

    public enum MenuType
    {
      Menu,
      SplashPage,
      Gameplay,
      BackGroundPause,
      PauseMenu,
      BackGroundMenu,

    }
    public void PlayGameplay()
    {
        
        SceneManager.LoadScene(1);
        init();
        animator.SetTrigger("Gameplay");
    }
    public void PlayTutorial()
    {
       
        SceneManager.LoadScene(5);
        init();
        animator.SetTrigger("Gameplay");
    }
    public void PlayMenu()
    {
        Debug.Log("HEYYYYYYYYYY");
        SceneManager.LoadScene(0);
        Time.timeScale = 1;
        init();
        animator.SetTrigger("SplashArt");
    }
    public void Resume()
    {
        animator.SetTrigger("Gameplay");
    }
    public void ExitGame()
    {
        Application.Quit();
    }
}
