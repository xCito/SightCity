using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Service : DynamicRooms
{

    public static bool select = true;
    //public static int redund = 0;
    public static string x;
    public static string y;
    public Text room_text;
    public string choice = "";
    public string[] rooms = new string[15];
    //public GameObject droom;

    //private DynamicRooms dr = new DynamicRooms();

    public void Update()
    {

        if (choice != "")
            TryHand(choice);


        // Debug.Log(getStartPoint());

    }
    void Start()

    {
        EasyTTSUtil.Initialize (EasyTTSUtil.UnitedStates);
        Debug.Log (floorPick());
        introToCurrentMenu();

        if (floorPick() == 1)
        {
            Debug.Log ("Firstfloor array selected");
            rooms = new string[] { "Grand Hall" };

        }

        if (floorPick() == 2)
        {
            Debug.Log ("second floor array selected");
            rooms = new string[] {  "Registration",     "Showcase Suite A", "Showcase Suite B", "Showcase Suite F",
                                    "Showcase Suite G", "Showcase Suite H", "Seaport Ball Room"
                                 };

        }
        if (floorPick() == 3)
        {
            Debug.Log ("third floor array selected");
            rooms = new string[] {
                "Foyer 3"
            };

        }
    }

    public void ChangeSliderValue (Slider slider)
    {
        Debug.Log (slider.maxValue);

        slider.maxValue = rooms.Length;
        for (int i = 0; i < rooms.Length; i++) {

            if ((int)slider.value == i)
            {
                choice = rooms[ (int)slider.value];
                room_text.text = "Room: " + rooms[ (int)slider.value];
                Debug.Log (rooms[ (int)slider.value]);
                EasyTTSUtil.SpeechFlush (rooms[ (int)slider.value]);
            }
        }
    }

    void introToCurrentMenu()
    {
        EasyTTSUtil.SpeechFlush("To select a room from a list of choices, slide your thumb on the left" +
                                "side of the screen up or down");
        EasyTTSUtil.SpeechAdd("To finalize your choice. With your thumb, double tap the right side of the screen");
    }

    void OnApplicationQuit()
    {
        EasyTTSUtil.Stop();
    }
}

