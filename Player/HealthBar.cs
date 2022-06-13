using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using PData;


public class HealthBar : MonoBehaviour
{
    enum BarType {Calorie,Health};
    [SerializeField] BarType UiBar;
    protected float BarLenght;
    protected float BarLenghtsmoother;
    [SerializeField] float BarLenghtsmoothervalue;
    [SerializeField] PlayerData Player_Data;

    private void Awake()
    {
       
    }

    private void Update()
    {
        if (UiBar == BarType.Calorie)
        {
            BarLenght = Player_Data.PCalories;
            BarLenghtsmoother += (BarLenght - BarLenghtsmoother) * Time.deltaTime * BarLenghtsmoothervalue;
            transform.localScale = new Vector2(BarLenghtsmoother, transform.localScale.y);
        }
        else
        {
            
            BarLenght = Player_Data.PHealth;
            BarLenghtsmoother += (BarLenght - BarLenghtsmoother) * Time.deltaTime * BarLenghtsmoothervalue;
            if (BarLenghtsmoother <= Mathf.Epsilon)
            { return; }
            else
            {
            transform.localScale = new Vector2(BarLenghtsmoother, transform.localScale.y);
            }
        }
        
       
    }

}
