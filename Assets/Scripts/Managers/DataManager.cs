using UnityEngine;

namespace Managers
{
  public class DataManager : MonoBehaviour
  {
    public static DataManager Instance;

    public PlayerDataVo PlayerDataVo { get; private set; }

    private void Awake()
    {
      if (Instance == null) Instance = this;
      
      SetInitialData();
    }

    private void SetPlayerDataVo()
    {
      PlayerDataVo playerDataVo = new()
      {
        Level = PlayerPrefs.GetInt(PlayerPrefKey.Level.ToString()),
        Coin = PlayerPrefs.GetInt(PlayerPrefKey.Coin.ToString()),
        Diamond = PlayerPrefs.GetInt(PlayerPrefKey.Diamond.ToString()),
        Energy = PlayerPrefs.GetInt(PlayerPrefKey.Energy.ToString()),
      };

      PlayerDataVo = playerDataVo;
    }

    private void SetInitialData()
    {
      if (PlayerPrefs.HasKey(PlayerPrefKey.Level.ToString())) return;
      
      SaveInt(PlayerPrefKey.Level, 1);
      SaveInt(PlayerPrefKey.Coin, 0);
      SaveInt(PlayerPrefKey.Diamond, 0);
      SaveInt(PlayerPrefKey.Energy, 20);
    }

    public int LoadInt(PlayerPrefKey playerPrefKey)
    {
      return PlayerPrefs.GetInt(playerPrefKey.ToString());
    }

    public void SaveInt(PlayerPrefKey playerPrefKey, int value)
    {
      PlayerPrefs.SetInt(playerPrefKey.ToString(), value);
      
      SetPlayerDataVo();
    }
  }

  public class PlayerDataVo
  {
    public int Level;
    
    public int Coin;

    public int Diamond;
    
    public int Energy;
  }

  public enum PlayerPrefKey
  {
    Level,
    Coin,
    Diamond,
    Energy,
  }
}