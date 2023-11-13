using DataPersistence.Data;

namespace DataPersistence
{
    public interface IDataPersistence
    {
        void loadData(GameData gameData);
        void saveData(GameData gameData);
    }
}