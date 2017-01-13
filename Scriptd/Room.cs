using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Room : DynamicRooms
{

    public static bool select = true;
    //public static int redund = 0;
    public static string x;
    public static string y;
    public Text room_text;
    public string choice="";
    public string[] rooms=new string[15];
    //   public GameObject droom;

    //private DynamicRooms dr = new DynamicRooms();

    public void Update() {
        if (choice != "")
            TryHand(choice);
        Debug.Log (getStartPoint());

    }
    void Start()
    {

        EasyTTSUtil.Initialize (EasyTTSUtil.UnitedStates);
        Debug.Log (floorPick());
        introToCurrentMenu();

        //TryHand();
        if (floorPick() == 1)
        {
            Debug.Log ("Firstfloor array selected");
            rooms = new string[] { "Grand Hall" };

        }

        if (floorPick() == 2)
        {
            Debug.Log ("second floor array selected");
            rooms = new string[] { "Gaslamp A",
                                   "Gaslamp B",
                                   "Gaslamp C",
                                   "Gaslamp D",
                                   "La Jolla A",
                                   "La Jolla B",
                                   "Old Town A",
                                   "Old Town B",
                                   "Balbora A",
                                   "Balbora B",
                                   "Balbora C"
                                 };

        }
        if (floorPick() == 3)
        {
            Debug.Log ("third floor array selected");
            rooms = new string[] { "Torrey Hills A",
                                   "Torrey Hills B",
                                   "Golden Hill A",
                                   "Golden Hill B",

                                   "HillCrest A",
                                   "HillCrest B",
                                   "HillCrest C",
                                   "HillCrest D",
                                   "Cortez A",
                                   "Cortez B",
                                   "Cortez C",

                                   "Bankers Hill",
                                   "Mission Beach A",
                                   "Mission Beach B",
                                   "Mission Beach C",
                                   "Solana Beach A",

                                   "Solana Beach B",
                                   "Ocean Beach",
                                   "Promenade A",
                                   "Promenade B",
                                   "Pier",
                                   "Cove"
                                 };

        }
    }

    public void ChangeSliderValue (Slider slider)
    {
        slider.maxValue = rooms.Length;
        for (int i = 0; i < rooms.Length; i++)
        {

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

