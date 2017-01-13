using UnityEngine;
using System.Collections;

public class TheInformationBridge
{
	public static bool gestModeOpen;
	public static bool permission;
	public static string instruction;
	public static int distFeet;
	public static int distMeter;
	public static float angleOfNextWP;
	public static bool music;

	// Use this for initialization
	void Start () { 
		permission = false;
		gestModeOpen = false;
		music = true;
	}
	 
	// Setters (Mutator)
	public void setInstruct(string instr)
	{
		instruction = instr;
	}
	public void setGestureModeActivity(bool g)
	{
		gestModeOpen = g;
	}
	public void setPermission(bool p)
	{
		permission = p;
	}
	public void setDistanceF(int df)
	{
		distFeet = df;
	}
	public void setDistanceM(int dm)
	{
		distMeter = dm;
	}
	public void setAngleOfNextWP(float dgre)
	{
		angleOfNextWP = dgre;
	}
	public void setMusicOfforOn(bool on)
	{
		music = on;
	}


	//Getters (Accessor)
	public string getNextInstruction()
	{
		return instruction;
	}
	public bool isGestureModeActive()
	{
		return gestModeOpen;
	}
	public bool hasPermission()
	{
		return permission;
	}
	public int getDistanceF()
	{
		return distFeet;
	}
	public int getDistanceM()
	{
		return distMeter;
	}
	public float getAngleOfNextWP()
	{
		return angleOfNextWP;
	}
	public bool musicOn()
	{
		return music;
	}
}
