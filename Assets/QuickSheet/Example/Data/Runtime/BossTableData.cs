using UnityEngine;
using System.Collections;

///
/// !!! Machine generated code !!!
/// !!! DO NOT CHANGE Tabs to Spaces !!!
/// 
[System.Serializable]
public class BossTableData
{
  [SerializeField]
  int id;
  public int Id { get {return id; } set { this.id = value;} }
  
  [SerializeField]
  string stringid;
  public string Stringid { get {return stringid; } set { this.stringid = value;} }
  
  [SerializeField]
  string name;
  public string Name { get {return name; } set { this.name = value;} }
  
  [SerializeField]
  string title;
  public string Title { get {return title; } set { this.title = value;} }
  
  [SerializeField]
  string description;
  public string Description { get {return description; } set { this.description = value;} }
  
  [SerializeField]
  float hp;
  public float Hp { get {return hp; } set { this.hp = value;} }
  
  [SerializeField]
  float rewardminhp;
  public float Rewardminhp { get {return rewardminhp; } set { this.rewardminhp = value;} }
  
  [SerializeField]
  float rewardmaxhp;
  public float Rewardmaxhp { get {return rewardmaxhp; } set { this.rewardmaxhp = value;} }
  
  [SerializeField]
  float attackpowermin;
  public float Attackpowermin { get {return attackpowermin; } set { this.attackpowermin = value;} }
  
  [SerializeField]
  float attackpowermax;
  public float Attackpowermax { get {return attackpowermax; } set { this.attackpowermax = value;} }
  
  [SerializeField]
  bool islock;
  public bool Islock { get {return islock; } set { this.islock = value;} }
  
  [SerializeField]
  float[] rewardtypes = new float[0];
  public float[] Rewardtypes { get {return rewardtypes; } set { this.rewardtypes = value;} }
  
  [SerializeField]
  float[] rewardmaxvalues = new float[0];
  public float[] Rewardmaxvalues { get {return rewardmaxvalues; } set { this.rewardmaxvalues = value;} }
  
}