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
  int unlocklevel;
  public int Unlocklevel { get {return unlocklevel; } set { this.unlocklevel = value;} }
  
  [SerializeField]
  int reward1_free;
  public int Reward1_Free { get {return reward1_free; } set { this.reward1_free = value;} }
  
  [SerializeField]
  float reward1_value;
  public float Reward1_Value { get {return reward1_value; } set { this.reward1_value = value;} }
  
  [SerializeField]
  int reward2_pass;
  public int Reward2_Pass { get {return reward2_pass; } set { this.reward2_pass = value;} }
  
  [SerializeField]
  float reward2_value;
  public float Reward2_Value { get {return reward2_value; } set { this.reward2_value = value;} }
  
  [SerializeField]
  int passgrade;
  public int Passgrade { get {return passgrade; } set { this.passgrade = value;} }
  
  [SerializeField]
  string shopid;
  public string Shopid { get {return shopid; } set { this.shopid = value;} }
  
  [SerializeField]
  float wrongbefore;
  public float Wrongbefore { get {return wrongbefore; } set { this.wrongbefore = value;} }
  
}