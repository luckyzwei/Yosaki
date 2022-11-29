using UnityEngine;
using System.Collections;

///
/// !!! Machine generated code !!!
/// !!! DO NOT CHANGE Tabs to Spaces !!!
/// 
[System.Serializable]
public class CaveBeltData
{
  [SerializeField]
  int id;
  public int Id { get {return id; } set { this.id = value;} }
  
  [SerializeField]
  double hp;
  public double Hp { get {return hp; } set { this.hp = value;} }
  
  [SerializeField]
  double attackpower;
  public double Attackpower { get {return attackpower; } set { this.attackpower = value;} }
  
  [SerializeField]
  float movespeed;
  public float Movespeed { get {return movespeed; } set { this.movespeed = value;} }
  
  [SerializeField]
  float defense;
  public float Defense { get {return defense; } set { this.defense = value;} }
  
  [SerializeField]
  int abiltype;
  public int Abiltype { get {return abiltype; } set { this.abiltype = value;} }
  
  [SerializeField]
  double abilvalue;
  public double Abilvalue { get {return abilvalue; } set { this.abilvalue = value;} }
  
  [SerializeField]
  string name;
  public string Name { get {return name; } set { this.name = value;} }
  
}