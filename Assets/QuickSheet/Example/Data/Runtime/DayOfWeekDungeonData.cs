using UnityEngine;
using System.Collections;

///
/// !!! Machine generated code !!!
/// !!! DO NOT CHANGE Tabs to Spaces !!!
/// 
[System.Serializable]
public class DayOfWeekDungeonData
{
  [SerializeField]
  int id;
  public int Id { get {return id; } set { this.id = value;} }
  
  [SerializeField]
  float[] score = new float[0];
  public float[] Score { get {return score; } set { this.score = value;} }
  
  [SerializeField]
  string dayofweek;
  public string Dayofweek { get {return dayofweek; } set { this.dayofweek = value;} }
  
  [SerializeField]
  string rewarddesc;
  public string Rewarddesc { get {return rewarddesc; } set { this.rewarddesc = value;} }
  
  [SerializeField]
  int rewardtype;
  public int Rewardtype { get {return rewardtype; } set { this.rewardtype = value;} }
  
  [SerializeField]
  float[] rewardvalue = new float[0];
  public float[] Rewardvalue { get {return rewardvalue; } set { this.rewardvalue = value;} }
  
  [SerializeField]
  string rewardstring;
  public string Rewardstring { get {return rewardstring; } set { this.rewardstring = value;} }
  
}