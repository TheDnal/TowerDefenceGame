using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class GameController : MonoBehaviour
{
    public static GameController instance;
    private float currHealth, currMoney;
    public float startHealth, startMoney;
    public TextMeshProUGUI healthText,moneyText;
    public GameObject mouseHighlight;
    public Color clearColor, obstructedColor;
    public GameObject turretPrefab;
    private Vector3 mousePos;
    private Tile nearestTile;
    public GameObject turretGhost = null;
    public enum MouseClickEffect
    {
        none,
        buildTurret,
        sellTurret
    }
    private MouseClickEffect currClickEffect = MouseClickEffect.none;
    public void Awake()
    {
        if(instance != null)
        {
            if(instance != this)
            {
                Destroy(this.gameObject);
            }
        }
        instance = this;
    }
    public void Start()
    {
        currHealth = startHealth;
        currMoney = startMoney;
        UpdateUI();
    }
    void Update()
    {
        // if(Input.GetKeyDown(KeyCode.Escape)){currClickEffect = MouseClickEffect.none;}
        // nearestTile = TileGrid.instance.GetNearestTile(mousePos);
        // UpdateHighlight();
        // switch(currClickEffect)
        // {
        //     case MouseClickEffect.buildTurret:
        //         turretGhost.SetActive(true);
        //         UpdateBuildTurret();
        //         break;
        //     case MouseClickEffect.sellTurret:
        //         turretGhost.SetActive(false);
        //         UpdateSellTurret();
        //         break;
        //     default:
        //         break;
        // }    
    }
    private void UpdateHighlight()
    {
        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.x = Mathf.Round(mousePos.x);
        mousePos.y = Mathf.Round(mousePos.y);
        mousePos.z = 0;
        mouseHighlight.SetActive(true);
        if(nearestTile == null){return;}
        if(turretGhost != null)
        {
            turretGhost.transform.position = mousePos;
        }
        if(nearestTile != null)
        {
            Color col = nearestTile.GetIsClear() ? clearColor : obstructedColor;
            mouseHighlight.transform.position = mousePos;
            mouseHighlight.GetComponent<SpriteRenderer>().color = col; 
        }
    }
    private void UpdateBuildTurret()
    {
        if(turretPrefab == null){return;}
        if(nearestTile == null){return;}
        if(Input.GetMouseButtonDown(0) && nearestTile.GetIsClear())
        {
            if(!CanAffordPrice(10)){return;}
            ChangeCoins(-10);
            GameObject newTurret = Instantiate(turretPrefab,mousePos,Quaternion.identity);
            nearestTile.SetIsClear(false);
            nearestTile.SetTurret(newTurret);
        }
    }
    private void UpdateSellTurret()
    {
        if(!Input.GetMouseButtonDown(0)){return;}
        GameObject turret = nearestTile.GetTurret();
        if(turret == null){return;}
        Destroy(turret.gameObject);
        ChangeCoins(10);
        nearestTile.ClearTurret();
        nearestTile.SetIsClear(true);
    }
    #region Getters/Setters
    public void DecrementHealth(){
        currHealth--;
        if(currHealth <= 0){currHealth = 0; EndGame();}
        UpdateUI();
    }
    public void ChangeCoins(float _value)
    {
        currMoney += _value;
        if(currMoney < 0){currMoney = 0;}
        UpdateUI();
    }
    public bool CanAffordPrice(float _price)
    {
        return currMoney >= _price;
    }
    public void SetMouseToBuildMode(GameObject _prefab)
    {
        Debug.Log("Set to build mode");
        turretPrefab = _prefab;
        currClickEffect = MouseClickEffect.buildTurret;
    }
    public void SetTurretGhost(GameObject _prefab)
    {
        UpdateTurretGhost(_prefab);
    }
    public void SetMouseToSellMode(){currClickEffect = MouseClickEffect.sellTurret; Debug.Log("Set to sell mode");}
    #endregion
    private void UpdateUI()
    {
        healthText.text = "Health : " + currHealth.ToString();
        moneyText.text = "Money : " + currMoney.ToString();
    }
    private void EndGame()
    {
        EnemySpawner.instance.SetSpawnerEnabled(false);
        Debug.Log("Game Over!");
    }
    private void UpdateTurretGhost(GameObject _prefab)
    {
        Destroy(turretGhost.gameObject);
        turretGhost = Instantiate(_prefab,mousePos,Quaternion.identity);
    }
}
