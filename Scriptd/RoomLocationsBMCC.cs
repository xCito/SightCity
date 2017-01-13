using UnityEngine;
using System.Collections;

public class RoomLocationsBMCC : DynamicRooms
{
    //Jagged Array
    private string[][] floorRoom = new string[4][];
    private Vector3[][] floorCoor = new Vector3[4][];
    public int floor2Y = 0;
    public int floor3Y = 100;

    public RoomLocationsBMCC()
    {

        //String[]: Rooms           // 2nd
        floorRoom[0] = new string[] {  "Richard Harris Terrace ",  "Cafeteria ",  "Hudson Room",  "Gymnasium",  "Student Government",
                                       "Theatre 1", "Theatre 2",  "Threatre Studio",  "Student Activities",
                                    // 3rd
                                        "Bridge"
                                    };

        //String[]: Comforts        // 2nd
        floorRoom[1] = new string[] {  "Bathroom near Student Activites",  "Bathroom near Athletics Office",  "Bathroom near Gymnasium",
                                       "Bathroom near Bookstore",
                                    // 3rd
                                        /*"Bathroom near Registrar",*/ "Bathroom near Bursars", "Bathroom near Federal WorkStudy",
                                    };

        //String[]: Services        // 2nd 
        floorRoom[2] = new string[] { "BookStore", "Public Safety", "Information Booth", "ID Room", "Athletics and Intramurals",
                                      "ComputerLab",
                                    // 3rd
                                      /*"Admissions Office",*/ "Bursars Office", "Health Services Office", "Perkin Office",
                                      /*"Registrars Office",*/ "Financial Aid Office", "Federal Work Study Office",
                                      "Services For Students Yith Disabilities Office",
                                    };

        //String[]:Accessibility    // 2nd
        floorRoom[3] = new string[] { "Second Floor South Elevator", "Second Floor North Elevator", "Second Floor Staircase 1",
                                      "Second Floor Staircase 2", "Second Floor Staircase 3", "Second Floor Staircase 4",
                                      "Second Floor Staircase 5", "Main Entrance", "North Moore Street Exit",
                                      "Second Floor North Escalator", "Second Floor South Escalator",
                                    // 3rd
                                      "Third Floor Staircase 1", "Third Floor Staircase 2", "Third Floor Staircase 3",
                                      "Third Floor Staircase 4", "Third Floor Staircase 5", "Third Floor North Elevator",
                                      "Third Floor South Elevator", "Third Floor North Escalator", "Third Floor South Escalator",
                                    };


        //Vector3[]: Rooms              // 2nd
        floorCoor[0] = new Vector3[]{   new Vector3(-177,floor2Y,58), new Vector3(-33,floor2Y,83), new Vector3(25,floor2Y,81),
                                        new Vector3(359,floor2Y,25), new Vector3(-3,floor2Y,-44), new Vector3(-414,floor2Y,76),
                                        new Vector3(-273,floor2Y,-9), new Vector3(358,floor2Y,40), new Vector3(-134,floor2Y,-52),
                                        // 3rd
                                        new Vector3(121,floor3Y,48)
                                    };

        //Vector3[]: Comforts           // 2nd
        floorCoor[1] = new Vector3[] {  new Vector3(-140,floor2Y,-7), new Vector3(195,floor2Y,39), new Vector3(359,floor2Y,38),
                                        new Vector3(-298,floor2Y,35),
                                        // 3rd
                                        new Vector3(-139,floor3Y,2), new Vector3(195,floor3Y,20)
                                     };

        //Vector3[]: Services           // 2nd
        floorCoor[2] = new Vector3[]{   new Vector3(-308,floor2Y,-50), new Vector3(-251,floor2Y,48), new Vector3(-284,floor2Y,17),
                                        new Vector3(-172,floor2Y,-27), new Vector3(198,floor2Y,57), new Vector3(274,floor2Y,57),
                                        // 3rd
                                        new Vector3(-129,floor3Y,67),  new Vector3(324,floor3Y,30),
                                        new Vector3(-111,floor3Y,52), new Vector3(255,floor3Y,58), new Vector3(281,floor3Y,16),
                                        new Vector3(194,floor3Y,59)
                                    };

        //Vector3[]: Accessibility      // 2nd
        floorCoor[3] = new Vector3[] {  new Vector3(22,floor2Y,14), new Vector3(353,floor2Y,40), new Vector3(-298,floor2Y,43),
                                        new Vector3(-140,floor2Y,-7), new Vector3(24,floor2Y,13), new Vector3(195,floor2Y,38),
                                        new Vector3(357,floor2Y,42), new Vector3(-366,floor2Y,-5), new Vector3(404,floor2Y,32),
                                        new Vector3(-154,floor2Y,24), new Vector3(204,floor2Y,32), 
                                        // 3rd
                                        new Vector3(-137,floor3Y,0), new Vector3(29,floor3Y,25), new Vector3(196,floor3Y,20),
                                        new Vector3(361,floor3Y,42), new Vector3(360,floor3Y,42), new Vector3(20,floor3Y,21),
                                        new Vector3(216,floor3Y,48), new Vector3(-188,floor3Y,28)
                                    };

    }


    public Vector3 setStartingPoint()
    {
        Vector3 vec = new Vector3(-5, 0, 0);

        for (int ctgry = 0; ctgry < floorRoom.GetLength(0); ctgry++)
        {
            for (int rm = 0; rm < floorRoom[ctgry].Length; rm++)
            {
                if (floorRoom[ctgry][rm].Equals(getStartPoint()))
                {
                    vec = floorCoor[ctgry][rm];
                    return vec;
                }
            }
        }
        Debug.Log("Room does not exist: " + getStartPoint());
        return vec;
    }
    public Vector3 setEndingPoint()
    {
        Vector3 vec = new Vector3(5, 0, 0);

        for (int ctgry = 0; ctgry < floorRoom.GetLength(0); ctgry++)
        {
            for (int rm = 0; rm < floorRoom[ctgry].Length; rm++)
            {
                if (floorRoom[ctgry][rm].Equals(getEndPoint()))
                {
                    vec = floorCoor[ctgry][rm];
                    return vec;
                }
            }
        }
        Debug.Log("Room does not exist: " + getEndPoint());
        return vec;
    }

}

