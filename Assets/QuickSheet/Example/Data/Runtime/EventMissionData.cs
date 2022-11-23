using UnityEngine;
using System.Collections;

///
/// !!! Machine generated code !!!
/// !!! DO NOT CHANGE Tabs to Spaces !!!
/// 
[System.Serializable]
public class EventMissionData
{
  [SerializeField]
  int id;
  public int Id { get {return id; } set { this.id = value;} }
  
  [SerializeField]
  string stringid;
  public string Stringid { get {return stringid; } set { this.stringid = value;} }
  
  [SerializeField]
  string title;
  public string Title { get {return title; } set { this.title = value;} }
  
  [SerializeField]
  int rewardrequire;
  public int Rewardrequire { get {return rewardrequire; } set { this.rewardrequire = value;} }
  
  [SerializeField]
  int rewardtype;
  public int Rewardtype { get {return rewardtype; } set { this.rewardtype = value;} }
  
  [SerializeField]
  int rewardvalue;
  public int Rewardvalue { get {return rewardvalue; } set { this.rewardvalue = value;} }
  
  [SerializeField]
  bool enable;
  public bool Enable { get {return enable; } set { this.enable = value;} }
  
  [SerializeField]
  int dailymaxclear;
  public int Dailymaxclear { get {return dailymaxclear; } set { this.dailymaxclear = value;} }
  
}