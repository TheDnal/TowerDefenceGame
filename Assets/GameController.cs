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
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.x = Mathf.Round(mousePos.x);
        mousePos.y = Mathf.Round(mousePos.y);
        mousePos.z = 0;
        mouseHighlight.SetActive(true);
        Tile nearestTile = TileGrid.instance.GetNearestTile(mousePos);
        if(nearestTile == null){mouseHighlight.SetActive(false); return;}
        Color col = nearestTile.GetIsClear() ? clearColor : obstructedColor;
        mouseHighlight.transform.position = mousePos;
        mouseHighlight.GetComponent<SpriteRenderer>().color = col; 

        if(Input.GetMouseButtonDown(0) && nearestTile.GetIsClear())
        {
            GameObject newTurret = Instantiate(turretPrefab,mousePos,Quaternion.identity);
            nearestTile.SetIsClear(false);
        }
    }
    #region Getters/Setters
    public void DecrementHealth(){
        currHealth--;
        UpdateUI();
        if(currHealth <= 0){Debug.Log("GAME OVER");}
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
    #endregion
    private void UpdateUI()
    {
        healthText.text = "Health : " + currHealth.ToString();
        moneyText.text = "Money : " + currMoney.ToString();
    }
}
