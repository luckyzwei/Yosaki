using UnityEngine;
using System.Collections;

///
/// !!! Machine generated code !!!
/// !!! DO NOT CHANGE Tabs to Spaces !!!
/// 
[System.Serializable]
public class SmithTableData
{
  [SerializeField]
  int id;
  public int Id { get {return id; } set { this.id = value;} }
  
  [SerializeField]
  int require;
  public int Require { get {return require; } set { this.require = value;} }
  
  [SerializeField]
  StatusType statustype;
  public StatusType STATUSTYPE { get {return statustype; } set { this.statustype = value;} }
  
  [SerializeField]
  float value;
  public float Value { get {return value; } set { this.value = value;} }
  
}