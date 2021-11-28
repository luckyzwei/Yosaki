using UnityEngine;
using System.Collections;

///
/// !!! Machine generated code !!!
/// !!! DO NOT CHANGE Tabs to Spaces !!!
/// 
[System.Serializable]
public class InAppPurchaseData
{
  [SerializeField]
  int id;
  public int Id { get {return id; } set { this.id = value;} }
  
  [SerializeField]
  string productid;
  public string Productid { get {return productid; } set { this.productid = value;} }
  
  [SerializeField]
  bool issubscribeitem;
  public bool Issubscribeitem { get {return issubscribeitem; } set { this.issubscribeitem = value;} }
  
  [SerializeField]
  string title;
  public string Title { get {return title; } set { this.title = value;} }
  
  [SerializeField]
  string description;
  public string Description { get {return description; } set { this.description = value;} }
  
  [SerializeField]
  int[] rewardtypes = new int[0];
  public int[] Rewardtypes { get {return rewardtypes; } set { this.rewardtypes = value;} }
  
  [SerializeField]
  int[] rewardvalues = new int[0];
  public int[] Rewardvalues { get {return rewardvalues; } set { this.rewardvalues = value;} }
  
  [SerializeField]
  BuyType buytype;
  public BuyType BUYTYPE { get {return buytype; } set { this.buytype = value;} }
  
  [SerializeField]
  int price;
  public int Price { get {return price; } set { this.price = value;} }
  
  [SerializeField]
  ShopCategory shopcategory;
  public ShopCategory SHOPCATEGORY { get {return shopcategory; } set { this.shopcategory = value;} }
  
  [SerializeField]
  int grade;
  public int Grade { get {return grade; } set { this.grade = value;} }
  
  [SerializeField]
  string bonusratio;
  public string Bonusratio { get {return bonusratio; } set { this.bonusratio = value;} }
  
  [SerializeField]
  bool active;
  public bool Active { get {return active; } set { this.active = value;} }
  
  [SerializeField]
  SellWhere sellwhere;
  public SellWhere SELLWHERE { get {return sellwhere; } set { this.sellwhere = value;} }
  
  [SerializeField]
  int displayorder;
  public int Displayorder { get {return displayorder; } set { this.displayorder = value;} }
  
  [SerializeField]
  string productidios;
  public string Productidios { get {return productidios; } set { this.productidios = value;} }
  
  [SerializeField]
  int fixedbuycount;
  public int Fixedbuycount { get {return fixedbuycount; } set { this.fixedbuycount = value;} }
  
}