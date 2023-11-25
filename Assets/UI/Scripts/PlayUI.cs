using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NewBehaviourScript : MonoBehaviour
{
    //bar health
    public RectTransform Bk_Bar_Health;
    public RectTransform Bk_Bar_Health_Change;
    public RectTransform Bk_Bar_Health_Atual;

    private float Health;
    private float HealthCharging;
    private float Bar_Health_maxLenght;
    public float HealthRegenerationSpeed = 1;
  
    //bar Energy
    public RectTransform Bk_Bar_Energy;
    public RectTransform Bk_Bar_Energy_Change;
    public RectTransform Bk_Bar_Energy_Atual;

    private float Energy;
    private float EnergyCharging;
    private float Bar_Energy_maxLenght;
    public float EnergyRegenerationSpeed = 1;
 
    //bar Hapinesss
    public RectTransform Bk_Bar_Hapinesss;
    public RectTransform Bk_Bar_Hapinesss_Change;
    public RectTransform Bk_Bar_Hapinesss_Atual;

    private float Hapiness;
    private float HapinessCharging;
    private float Bar_Hapinesss_maxLenght;
    public float HapinessRegenerationSpeed = 1;
   
    public GameObject obj_MoedasConvertidas;
    public GameObject obj_NumeroDePistas;
    private TextMeshPro MoedasConvertidas;
    private TextMeshPro NumeroDePistas;

    //buttons
    public Toggle Inventory;
    public Sprite InventoryOff;
    public Sprite InventoryOn;
    public void ClickButtonInventario()
    {
        Inventory.image.sprite = (Inventory.isOn) ? InventoryOn : InventoryOff;
    }

    public Toggle Crawling;
    public Sprite CrawlingOff;
    public Sprite CrawlingOn;
    public void ClickButtonCrawling()
    {
        Crawling.image.sprite = (Crawling.isOn) ? CrawlingOn : CrawlingOff;
        if (Crawling.isOn)
        {
            // Toggle is ON
            Debug.Log("Crawling is ON");
        }
        else
        {
            // Toggle is OFF
            Debug.Log("Crawling is OFF");
        }
    }

    public Toggle Crouching;
    public Sprite CrouchingOff;
    public Sprite CrouchingOn;
    public  void ClickButtonCrounching()
    {
        Crouching.image.sprite = (Crouching.isOn) ? CrouchingOn : CrouchingOff;
        if (Crouching.isOn)
        {
            // Toggle is ON
            Debug.Log("Crouching is ON");
        }
        else
        {
            // Toggle is OFF
            Debug.Log("Crouching is OFF");
        }
    }

    public Button Hint;
    public Sprite HintOff;
    public Sprite HintOn;
    public  void ClickButtonHint()
    {
       // Hint.image.sprite = (Hint.is) ? HintOn : HintOff;
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
        
        MoedasConvertidas = obj_MoedasConvertidas.GetComponent<TextMeshPro>();
        NumeroDePistas = obj_NumeroDePistas.GetComponent<TextMeshPro>();
        
        Bar_Health_maxLenght = Bk_Bar_Health.sizeDelta.x * Bk_Bar_Health_Atual.localScale.x;
        Bar_Energy_maxLenght= Bk_Bar_Energy.sizeDelta.x * Bk_Bar_Health_Atual.localScale.x;
        Bar_Hapinesss_maxLenght = Bk_Bar_Hapinesss.sizeDelta.x * Bk_Bar_Health_Atual.localScale.x;

        resizaBar(Bk_Bar_Health_Atual, Health, Bar_Health_maxLenght);
        resizaBar(Bk_Bar_Energy_Atual, Health, Bar_Energy_maxLenght);
        resizaBar(Bk_Bar_Hapinesss_Atual, Health, Bar_Hapinesss_maxLenght);
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

    private float updateBar(RectTransform bar,float atual,float change, float max,float speed)
    {
        if (atual == change)
            return atual;

        float step = speed * Time.deltaTime;
        Debug.Log("s:" + step);
        if (atual + step >= 100)
        {
            atual = 100;
        }
        else if (atual - step < 0)
        {
            atual = 0;
        }
        else if(atual < change)
        {
            atual += step;
        }
        else if(atual > change)
        {
            atual -= step;
        }

        resizaBar(bar, atual, max);
        return atual;
    }
}
