using UnityEngine;
using System.Collections;

///
/// !!! Machine generated code !!!
/// !!! DO NOT CHANGE Tabs to Spaces !!!
/// 
[System.Serializable]
public class WingTableData
{
  [SerializeField]
  int id;
  public int Id { get {return id; } set { this.id = value;} }
  
  [SerializeField]
  string stringid;
  public string Stringid { get {return stringid; } set { this.stringid = value;} }
  
  [SerializeField]
  int requirejump;
  public int Requirejump { get {return requirejump; } set { this.requirejump = value;} }
  
  [SerializeField]
  int[] abilitytype = new int[0];
  public int[] Abilitytype { get {return abilitytype; } set { this.abilitytype = value;} }
  
  [SerializeField]
  float[] abilityvalue = new float[0];
  public float[] Abilityvalue { get {return abilityvalue; } set { this.abilityvalue = value;} }
  
}