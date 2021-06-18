using System.Collections.Generic;
using UnityEngine;

public sealed class FallingObjectsNameManager : MonoBehaviour
{
    private static Dictionary<string, string> _dictionary = new Dictionary<string, string>()
    {
        { "8(Clone)", "BALLSHOT!" },
        { "american_ball(Clone)", "TOUCHDOWN!" },
        { "apple(Clone)", "APPLESHOT!" },
        { "bal_01l(Clone)", "GOAL!" },
        { "ball_02(Clone)", "SLAMDUNK!" },
        { "banana(Clone)", "BANANASHOT!" },
        { "Bowling(Clone)", "STRIKE!" },
        { "bowling_pin(Clone)", "SPARE!" },
        { "box_01(Clone)", "DELIVERED!" },
        { "can(Clone)", "CANSHOT!" },
        { "can_new 1(Clone)", "CANSHOT!" },
        { "Car_01(Clone)", "WASTED!" },
        { "chair_02(Clone)", "CHAIRSHOT!" },
        { "coffee(Clone)", "COFFEESHOT!" },
        { "donut_01(Clone)", "DONUTSHOT!" },
        { "donut_02(Clone)", "DONUTSMACK!" },
        { "Girya(Clone)", "DUMBBELLSTRIKE!" },
        { "golf(Clone)", "BALLHOOK!" },
        { "Ice_cream_truck_01(Clone)", "ICECREAMED!" },
        { "Kamen_normal 1(Clone)", "ROCKED!" },
        { "Kamen_normal7 1(Clone)", "ROCKSHOT!" },
        { "ladder(Clone)", "LADDERSTRIKE!" },
        { "meat(Clone)", "MEATSHOT!" },
        { "office_chair(Clone)", "CHAIRSHOT!" },
        { "pineapple(Clone)", "PINEAPPLESHOT!" },
        { "pong_stick(Clone)", "PING_PONGED!" },
        { "Shield_01(Clone)", "FENCESHOT!" },
        { "spinner(Clone)", "SPINNERSHOT!" },
        { "table(Clone)", "TABLESHOT!" },
        { "tank(Clone)", "BARRELSHOT!" },
        { "tennis(Clone)", "GAMEBALL!" },
        { "Train_01(Clone)", "TRAINED!" },
        { "Wood_box(Clone)", "BOXSHOT!" }
    };

    public static string GetShotString(string gameObjectName)
    {
        if (_dictionary.TryGetValue(gameObjectName, out string value))
        {
            return value;
        }
        else
        {
            return "SHOT!";
        }
    }
}
