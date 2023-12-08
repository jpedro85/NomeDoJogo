using Inventory;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.EventSystems.EventTrigger;

public class PlayUI : MonoBehaviour
{
    public InventoryUI iventoryUi;
    //bar health
    public RectTransform Bk_Bar_Health;
    public RectTransform Bk_Bar_Health_Change;
    public RectTransform Bk_Bar_Health_Atual;

    private float Health;
    private float HealthCharging;
    private float Bar_Health_maxLenght;
    public float HealthRegenerationSpeed = 1;

    public float AtualHealth
    {
        get { return Health; }
    }

    //bar Energy
    public RectTransform Bk_Bar_Energy;
    public RectTransform Bk_Bar_Energy_Change;
    public RectTransform Bk_Bar_Energy_Atual;

    private float Energy;
    private float EnergyCharging;
    private float Bar_Energy_maxLenght;
    public float EnergyRegenerationSpeed = 1;

    public float AtualEneergy
    {
        get { return Energy; }
    }

//bar Hapinesss
public RectTransform Bk_Bar_Hapinesss;
    public RectTransform Bk_Bar_Hapinesss_Change;
    public RectTransform Bk_Bar_Hapinesss_Atual;

    private float Hapiness;
    private float HapinessCharging;
    private float Bar_Hapinesss_maxLenght;
    public float HapinessRegenerationSpeed = 1;

    public float AtualHapiness 
    { 
        get { return Hapiness; }
    }


    public TextMeshProUGUI NumeroDePistas;

    //buttons
    public Toggle Inventory;
    public Sprite InventoryOff;
    public Sprite InventoryOn;

    public GameObject obj_Inventory;
    public AnalogicManager Analogic;
    public CameraMovement mcamera;
    public PlayerMovement player;
    public void ClickButtonInventario()
    {
        Inventory.image.sprite = (Inventory.isOn) ? InventoryOn : InventoryOff;
        obj_Inventory.SetActive(Inventory.isOn);
        iventoryUi.updateUI();
    }

    public Toggle Crawling;
    public Sprite CrawlingOff;
    public Sprite CrawlingOn;
    public void ClickButtonCrawling()
    {

        if (!player.isJumping)
        {
            if (Crouching.isOn && Crawling.isOn)
            {
                Crouching.isOn = false;
                Crouching.image.sprite = CrouchingOff;
                player.setCrouched(false);
            }

            Crawling.image.sprite = (Crawling.isOn) ? CrawlingOn : CrawlingOff;
            player.setCrawling(Crawling.isOn);
        }

    }

    public Toggle Crouching;
    public Sprite CrouchingOff;
    public Sprite CrouchingOn;
    public  void ClickButtonCrounching()
    {
        if (!player.isJumping)
        {
            if (Crawling.isOn && Crouching.isOn)
            {
                Crawling.isOn = false;
                Crawling.image.sprite = CrawlingOff;
                player.setCrawling(false);
            }

            Crouching.image.sprite = (Crouching.isOn) ? CrouchingOn : CrouchingOff;
            player.setCrouched(Crouching.isOn);
        }
    }

    public Button Jumping;
    public void ClickButtonJumping()
    {
        if (Crawling.isOn)
        {
            Crawling.isOn = false;
            Crawling.image.sprite = CrawlingOff;
            player.setCrawling(false);
        }

        if (Crouching.isOn)
        {
            Crouching.isOn = false;
            Crouching.image.sprite = CrouchingOff;
            player.setCrouched(false);
        }
        player.setJumping();
    }

    public Button Hint;
    public Sprite HintOff;
    public Sprite HintOn;
    private int Hints;
    public void ClickButtonHint()
    {

        if (Hints > 0 && !mcamera.isPista)
        {
            Hint.image.sprite = HintOn;
            mcamera.pista(Hint, HintOff);
            Hints--;
            NumeroDePistas.text = Hints.ToString();
        }
       
 
    }


    //teste vars
    public bool TEsteADDHealth;
    public float TEsteADDHealth_Quantidade;

    public bool TEsteADDEnergy;
    public float TEsteADDEnergy_Quantidade;

    public bool TEsteADDhapiness;
    public float TEsteADDhapiness_Quantidade;




    // Start is called before the first frame update
    void Start()
    {
        Health = 100;
        HealthCharging = 100;

        Energy = 100;
        EnergyCharging = 100;

        Hapiness = 100;
        HapinessCharging = 100;
        
        Bar_Health_maxLenght = Bk_Bar_Health.sizeDelta.x * Bk_Bar_Health_Atual.localScale.x;
        Bar_Energy_maxLenght= Bk_Bar_Energy.sizeDelta.x * Bk_Bar_Health_Atual.localScale.x;
        Bar_Hapinesss_maxLenght = Bk_Bar_Hapinesss.sizeDelta.x * Bk_Bar_Health_Atual.localScale.x;

        resizaBar(Bk_Bar_Health_Atual, Health, Bar_Health_maxLenght);
        resizaBar(Bk_Bar_Energy_Atual, Health, Bar_Energy_maxLenght);
        resizaBar(Bk_Bar_Hapinesss_Atual, Health, Bar_Hapinesss_maxLenght);


        obj_Inventory.SetActive(false);

        Hints = 10;
        NumeroDePistas.text = Hints.ToString();
    }

    public void resetToAfterDeadHealth()
    {
        Health = 50;
        HealthCharging = 50;
        resizaBar(Bk_Bar_Health_Atual, Health, Bar_Health_maxLenght);
    }

    // Update is called once per frame
    void Update()
    {
        Health = updateBar(Bk_Bar_Health_Atual, Health, HealthCharging, Bar_Health_maxLenght, HealthRegenerationSpeed);
        Energy = updateBar(Bk_Bar_Energy_Atual, Energy, EnergyCharging, Bar_Energy_maxLenght, EnergyRegenerationSpeed);
        Hapiness = updateBar(Bk_Bar_Hapinesss_Atual, Hapiness, HapinessCharging, Bar_Hapinesss_maxLenght, HapinessRegenerationSpeed);

        //testzone
        if (TEsteADDHealth) 
        { 
            addDeltaHealth(TEsteADDHealth_Quantidade);
            TEsteADDHealth = false;
        }

        if (TEsteADDEnergy)
        {
            addDeltaEnergy(TEsteADDEnergy_Quantidade);
            TEsteADDEnergy = false;
        }

        if (TEsteADDhapiness)
        {
            addDeltaHapiness(TEsteADDhapiness_Quantidade);
            TEsteADDhapiness = false;
        }
    }

    public void addDeltaHealth(float health)
    {
        HealthCharging = addDeltaValue(HealthCharging, health);
        resizaBar(Bk_Bar_Health_Change, HealthCharging, Bar_Health_maxLenght);

        if (HealthCharging <= Health)
        {
            Health = HealthCharging;
            resizaBar(Bk_Bar_Health_Atual, Health, Bar_Health_maxLenght);
        }
    }

    public void addDeltaEnergy(float energy)
    {
        EnergyCharging = addDeltaValue(EnergyCharging, energy);
        resizaBar(Bk_Bar_Energy_Change, EnergyCharging, Bar_Energy_maxLenght);

        if (EnergyCharging <= Energy)
        {
            Energy = EnergyCharging;
            resizaBar(Bk_Bar_Energy_Atual, Energy, Bar_Energy_maxLenght);
        }

    }
    public void addDeltaHapiness(float hapiness)
    {
        HapinessCharging = addDeltaValue(HapinessCharging, hapiness);
        resizaBar(Bk_Bar_Hapinesss_Change, HapinessCharging, Bar_Hapinesss_maxLenght);

        if (HapinessCharging <= Hapiness)
        {
            Hapiness = HapinessCharging;
            resizaBar(Bk_Bar_Hapinesss_Atual, Hapiness, Bar_Hapinesss_maxLenght);
        }
    }

    private float addDeltaValue(float var ,float value)
    {
        if (var + value > 100)
        {
            var = 100;
        }
        else if (var + value < 0)
        {
            var = 0;
        }
        else
            var += value;

        return var;
    }
    
    private void resizaBar(RectTransform bar,float newPercentage,float max)
    {   
        bar.sizeDelta = new Vector2(max * newPercentage /100, bar.sizeDelta.y);
    }

    private float updateBar(RectTransform bar,float actual,float change, float max,float speed)
    {
        if (actual == change)
        {
            return actual;
        }

        float step = speed * Time.deltaTime;

        change = (change > 100) ? 100 : change;
        //if (actual + step >= 100)
        //{
        //    Debug.Log("actual >= change");
        //    actual = 100;
        //}
        //else if (actual - step < 0)
        //{
        //    Debug.Log("actual = 0");
        //    actual = 0;
        //}

        if (actual < change)
        {
            actual = (actual + step >= change) ? change : actual + step;
        }
        else if(actual > change)
        {
            actual =  (actual - step < 0) ? 0 :  actual - step;
        }

        resizaBar(bar, actual, max);
        return actual;
    }
}
