using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class WWiseBankManager : MonoBehaviour
{

    public static WWiseBankManager singleton = null;

    void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
        if(singleton == null)
        {
            singleton = this;
        }
    }

    public static void Charge(GameObject gameObject) => AkSoundEngine.PostEvent("Add_To_Pack", gameObject);
    public static void Chase(GameObject gameObject) => AkSoundEngine.PostEvent("Add_To_Pack", gameObject);
    public static void Mount(GameObject gameObject) => AkSoundEngine.PostEvent("Add_To_Pack", gameObject);

    public static void MainMusic(GameObject gameObject )
    {
        print("WWISE: Main music trigger...");
        AkSoundEngine.StopAll();
        AkSoundEngine.PostEvent("MainWithTheme", gameObject);
    }

    public static void PlayerDeath(GameObject gameObject) => AkSoundEngine.PostEvent("Death", gameObject);
    public static void RidingMusic(GameObject gameObject) => AkSoundEngine.PostEvent("Riding_Music", gameObject);
}
