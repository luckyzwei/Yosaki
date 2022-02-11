using UnityEngine;
using System.Collections;

///
/// !!! Machine generated code !!!
/// !!! DO NOT CHANGE Tabs to Spaces !!!
/// 
[System.Serializable]
public class SmithEnemyData
{
  [SerializeField]
  int stage;
  public int Stage { get {return stage; } set { this.stage = value;} }
  
  [SerializeField]
  double hp;
  public double Hp { get {return hp; } set { this.hp = value;} }
  
  [SerializeField]
  double tankerhpratio;
  public double Tankerhpratio { get {return tankerhpratio; } set { this.tankerhpratio = value;} }
  
  [SerializeField]
  double attackpower;
  public double Attackpower { get {return attackpower; } set { this.attackpower = value;} }
  
  [SerializeField]
  double attackerratio;
  public double Attackerratio { get {return attackerratio; } set { this.attackerratio = value;} }
  
  [SerializeField]
  float movespeed;
  public float Movespeed { get {return movespeed; } set { this.movespeed = value;} }
  
  [SerializeField]
  float speederratio;
  public float Speederratio { get {return speederratio; } set { this.speederratio = value;} }
  
  [SerializeField]
  float defense;
  public float Defense { get {return defense; } set { this.defense = value;} }
  
  [SerializeField]
  float defenserratio;
  public float Defenserratio { get {return defenserratio; } set { this.defenserratio = value;} }
  
  [SerializeField]
  int spawnnum;
  public int Spawnnum { get {return spawnnum; } set { this.spawnnum = value;} }
  
}