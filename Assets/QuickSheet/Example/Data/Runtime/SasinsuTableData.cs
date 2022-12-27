using UnityEngine;
using System.Collections;

///
/// !!! Machine generated code !!!
/// !!! DO NOT CHANGE Tabs to Spaces !!!
/// 
[System.Serializable]
public class SasinsuTableData
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
  double[] score = new double[0];
  public double[] Score { get {return score; } set { this.score = value;} }
  
  [SerializeField]
  string[] scoredescription = new string[0];
  public string[] Scoredescription { get {return scoredescription; } set { this.scoredescription = value;} }
  
  [SerializeField]
  int[] abiltype0 = new int[0];
  public int[] Abiltype0 { get {return abiltype0; } set { this.abiltype0 = value;} }
  
  [SerializeField]
  float[] abilvalue0 = new float[0];
  public float[] Abilvalue0 { get {return abilvalue0; } set { this.abilvalue0 = value;} }
  
}