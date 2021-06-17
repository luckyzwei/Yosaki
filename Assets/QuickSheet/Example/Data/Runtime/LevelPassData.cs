using UnityEngine;
using System.Collections;

///
/// !!! Machine generated code !!!
/// !!! DO NOT CHANGE Tabs to Spaces !!!
/// 
[System.Serializable]
public class LevelPassData
{
  [SerializeField]
  int id;
  public int Id { get {return id; } set { this.id = value;} }
  
  [SerializeField]
  int group;
  public int Group { get {return group; } set { this.group = value;} }
  
  [SerializeField]
  int unlocklevel;
  public int Unlocklevel { get {return unlocklevel; } set { this.unlocklevel = value;} }
  
  [SerializeField]
  int reward1;
  public int Reward1 { get {return reward1; } set { this.reward1 = value;} }
  
  [SerializeField]
  float reward1_value;
  public float Reward1_Value { get {return reward1_value; } set { this.reward1_value = value;} }
  
  [SerializeField]
  int reward2;
  public int Reward2 { get {return reward2; } set { this.reward2 = value;} }
  
  [SerializeField]
  float reward2_value;
  public float Reward2_Value { get {return reward2_value; } set { this.reward2_value = value;} }
  
  [SerializeField]
  string shopid;
  public string Shopid { get {return shopid; } set { this.shopid = value;} }
  
}