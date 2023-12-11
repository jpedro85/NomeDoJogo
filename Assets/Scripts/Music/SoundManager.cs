using DataPersistence;
using DataPersistence.Data;
using UnityEngine;
using UnityEngine.UI;

public class SoundManager : MonoBehaviour, IDataPersistence
{
    [SerializeField] Image soundOnIcon;
    [SerializeField] Image soundOffIcon;
    private bool muted = false;
    void Start()
    {
        if(!PlayerPrefs.HasKey("muted")){
            PlayerPrefs.SetInt("muted",0);
            Load();
        }else{
            Load();
        }
        UpdateButtonIcon();
        AudioListener.pause = muted;
    }
    public void OnButtonPress(){
        if(muted == false){
            muted = true;
            AudioListener.pause = true;
        }else{
            muted = false;
            AudioListener.pause = false;
        }
        Save();
        UpdateButtonIcon();
    }
    private void UpdateButtonIcon(){
        if(muted == false){
            soundOnIcon.enabled=true;
            soundOffIcon.enabled=false;
        }else{
            soundOnIcon.enabled=false;
            soundOffIcon.enabled=true;
        }
    }
    private void Load(){
        muted= PlayerPrefs.GetInt("muted")==1;
    }
    private void Save(){
        PlayerPrefs.SetInt("muted",muted ? 1:0);
    }

    public void loadData(GameData gameData)
    {
        this.muted = gameData.isMuted;
    }

    public void saveData(GameData gameData)
    {
        gameData.isMuted = this.muted;
    }
}
