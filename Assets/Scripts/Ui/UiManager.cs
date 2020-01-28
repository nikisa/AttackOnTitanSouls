using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UiManager : MonoBehaviour
{
    public List<UiBase> FullHeart;
    public List<UiBase> EmptyHeart;

    public void DisableMenu(MenuType _menuType)
    {
        //switch (_menuType)
        //{
        //    case MenuType.Cover:
        //        isCover = false;
        //        StateCover.Disable();
        //        break;
       
        //}
    }
    public void ChangeMenu(MenuType _menuType)
    {
        //switch (_menuType)
        //{
        //    case MenuType.Cover:
        //        isCover = true;
        //        StateCover.Setup();
        //        break;
          
        //}
    }
    public void LifeUpdate(int _lifes)
    {

        FullHeart[_lifes].Disable();
        EmptyHeart[_lifes].Setup();
    }

    public enum MenuType
    {
      

    }
}
