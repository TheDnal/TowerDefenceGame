using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
public class MouseController : MonoBehaviour
{
    /*
        This script is in control of what the mouse does at all times.
        The different mouse functions are controlled via the mouseFunctions
        enum. This allows outside methods like buttons etc to dictate what the mouse
        is currently doing.

        BUILD :
        BUILD means the building of turrets from the shop onto the game map. This is
        done via a left/right click stratgey. The user clicks the store button for a turret,
        and a "ghost" of where the turret will place hovers around the mouse. If the player presses
        left click again the turret is bought and placed, then the mouse function is returned to 
        IDLE. if right click is pressed, the buy is aborted and the mouse function returns to IDLE.
    
        SELL : 
        SELL means to sell any turrets the user clicks on. Therefore a left click means the turret
        is sold and the user gets a percentage of the value of the turret back, and right click aborts 
        the function and returns to IDLE. 
    */
    private ShopItem currDraggedItem;
    public SpriteRenderer ghostRnd;
    private enum MouseFunctions
    {
        IDLE,   //inspection and idle 
        BUILD, //Dragging towers onto the map
        SELL, //Selling towers on the map
        TARGET //set target for non-direct shooting towers like artillery
    }
    private MouseFunctions currMouseFunction;
    private Vector3 mousePos;
    private Tile nearestTile;
    public Color invalidPlacementColor, standardGhostColor;
    public Sprite sellGhostSprite;
    private bool mouseOverUI = false;
    void Start()
    {
        currMouseFunction = MouseFunctions.IDLE;
    }
    void Update()
    {
        CalcualteMousePos();
        CalculateNearestTile();
        ConfigureGhost();
        mouseOverUI = GetMouseOverUI();
        switch(currMouseFunction)
        {
            case MouseFunctions.IDLE:
                IdleUpdate();
                break;
            case MouseFunctions.BUILD:
                BuildUpdate();
                break;
            case MouseFunctions.SELL:
                SellUpdate();
                break;
        }
    }
    private void IdleUpdate()
    {
        //Inspect turrets
    }
    private void BuildUpdate()
    {
        //Move sprite to mouse pos
        if(Input.GetMouseButtonDown(1)){SetMouseFunctionToIdle(); return;} //aborted build
        if(mouseOverUI){return;} //mouse is over ui
        if(!Input.GetMouseButtonDown(0)){return;} //not pressing down left click
        if(!nearestTile.GetIsClear()){return;} //tile not clear
        //check if budget is high enough
        if(!GameController.instance.CanAffordPrice(currDraggedItem.cost)){ return;}

        GameController.instance.ChangeCoins(-currDraggedItem.cost);
        GameObject newTurret = Instantiate(currDraggedItem.turretPrefab,mousePos,Quaternion.identity);
        nearestTile.SetTurret(newTurret);
        nearestTile.SetIsClear(false);
    }
    private void SellUpdate()
    {
        if(!Input.GetMouseButtonDown(0)){return;}
        if(nearestTile == null){return;}
        GameObject turret = nearestTile.GetTurret();
        if(turret == null){return;}
        GameController.instance.ChangeCoins(turret.GetComponent<Turret>().refundAmount);
        Destroy(turret.gameObject);
        nearestTile.ClearTurret();
        nearestTile.SetIsClear(true);
    }
    private void CalcualteMousePos()
    {
        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.x = Mathf.Round(mousePos.x);
        mousePos.y = Mathf.Round(mousePos.y);
        mousePos.z = 0;
    }
    private void CalculateNearestTile()
    {
        nearestTile = TileGrid.instance.GetNearestTile(mousePos);
        if(nearestTile == null){return;}
    }
    private void ConfigureGhost()
    {
        //If tile is blocked or not
        ghostRnd.transform.position = mousePos;
        ghostRnd.gameObject.SetActive(false);
        if(mouseOverUI)
        {
            return;
        }
        if(currMouseFunction == MouseFunctions.BUILD)
        {
            ghostRnd.gameObject.SetActive(true);
            if(nearestTile != null)
            {
                if(!nearestTile.GetIsClear()){ghostRnd.color = invalidPlacementColor; return;}
            }
            //If turret is too expensive
            if(currDraggedItem != null)
            {
                if(!GameController.instance.CanAffordPrice(currDraggedItem.cost)){ghostRnd.color = invalidPlacementColor; return;}
            }
        }
        //If hovering over turret during sell function
        if(currMouseFunction == MouseFunctions.SELL && nearestTile != null)
        {
            if(nearestTile.GetTurret() != null){ghostRnd.color = invalidPlacementColor; ghostRnd.gameObject.SetActive(true); return;}
        }
        ghostRnd.color = standardGhostColor;
    }
    private bool GetMouseOverUI()
    {
        PointerEventData pData = new PointerEventData(EventSystem.current);
        pData.position = Input.mousePosition;

        List<RaycastResult> raycastResultList = new List<RaycastResult>();
        EventSystem.current.RaycastAll(pData,raycastResultList);
        for(int i =0; i < raycastResultList.Count; i++)
        {
            if(raycastResultList[i].gameObject.tag == "UI")
            {
                return true;
            }
        }
        return false;
    }
    public void SetMouseFunctionToDrag(ShopItem _item)
    {
        currMouseFunction = MouseFunctions.BUILD;
        currDraggedItem = _item;
        ghostRnd.sprite = currDraggedItem.ghostSprite;
    }
    public void SetMouseFunctionToSell(){currMouseFunction = MouseFunctions.SELL; ghostRnd.sprite = sellGhostSprite;}
    public void SetMouseFunctionToIdle(){currMouseFunction = MouseFunctions.IDLE;}
}
