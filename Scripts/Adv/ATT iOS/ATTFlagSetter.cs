using UnityEngine;

public static class ATTFlagSette
{
    /// <summary>
    /// Включить авто-вызов ATT при старте приложения
    /// </summary>
    public static void EnableAutoATT()
    {
        PlayerPrefs.SetInt("AutoATTRequest", 1);
        PlayerPrefs.Save();
    }

    /// <summary>
    /// Отключить авто-вызов ATT
    /// </summary>
    public static void DisableAutoATT()
    {
        PlayerPrefs.SetInt("AutoATTRequest", 0);
        PlayerPrefs.Save();
    }
}

