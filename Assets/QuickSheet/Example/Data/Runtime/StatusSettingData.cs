using UnityEngine;
using System.Collections;

///
/// !!! Machine generated code !!!
/// !!! DO NOT CHANGE Tabs to Spaces !!!
/// 
[System.Serializable]
public class StatusSettingData
{
  [SerializeField]
  string key;
  public string Key { get {return key; } set { this.key = value;} }
  
  [SerializeField]
  string description;
  public string Description { get {return description; } set { this.description = value;} }
  
  [SerializeField]
  StatusWhere statuswhere;
  public StatusWhere STATUSWHERE { get {return statuswhere; } set { this.statuswhere = value;} }
  
  [SerializeField]
  int maxlv;
  public int Maxlv { get {return maxlv; } set { this.maxlv = value;} }
  
  [SerializeField]
  bool active;
  public bool Active { get {return active; } set { this.active = value;} }
  
  [SerializeField]
  bool ispercent;
  public bool Ispercent { get {return ispercent; } set { this.ispercent = value;} }
  
  [SerializeField]
  float basevalue;
  public float Basevalue { get {return basevalue; } set { this.basevalue = value;} }
  
  [SerializeField]
  float addvalue;
  public float Addvalue { get {return addvalue; } set { this.addvalue = value;} }
  
  [SerializeField]
  string destable;
  public string Destable { get {return destable; } set { this.destable = value;} }
  
  [SerializeField]
  int statustype;
  public int Statustype { get {return statustype; } set { this.statustype = value;} }
  
}