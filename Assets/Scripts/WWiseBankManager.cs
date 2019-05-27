using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WWiseBankManager : MonoBehaviour
{
    public static void Charge(GameObject gameObject) => AkSoundEngine.PostEvent("Add_To_Pack", gameObject);
    public static void Chase(GameObject gameObject) => AkSoundEngine.PostEvent("Add_To_Pack", gameObject);
    public static void Mount(GameObject gameObject) => AkSoundEngine.PostEvent("Add_To_Pack", gameObject);

    public static void MainMusic(GameObject gameObject )
    {
        AkSoundEngine.StopAll();
        AkSoundEngine.PostEvent("Buffaloids", gameObject);
    }
    public static void RidingMusic(GameObject gameObject) => AkSoundEngine.PostEvent("Riding_Music", gameObject);
}
