using UnityEngine;

public class Utils:MonoBehaviour
{   
    public static float move_Speed = 7f;
    public static float scroll_Speed = 3000.0f;

    public static float minPosX = -22f; 
    public static float maxPosX = 22f; 
    public static float minPosZ = -4f; 
    public static float maxPosZ = 4f; 

    public static float maxZoom = 87.5f; 
    public static float minZoom = 30f;

    public static float minOrth = 8f;                           //2d Ä«¸Þ¶ó ÁÜÀÎ¾Æ¿ô
    public static float maxOrth = 15f;
    public static float minOrth_x = -7f;
    public static float maxOrth_x = 7f;
    public static float minOrth_z = -4f;
    public static float maxOrth_z = 4f;

    public static int goalMin = 4;
    public static int goalMax = 20;
}
