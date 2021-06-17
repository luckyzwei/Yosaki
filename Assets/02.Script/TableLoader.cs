using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface TableLoader 
{
   void Initialize();
   void UpData(string key, bool LocalOnly);

   void UpData(string key, float data, bool LocalOnly);

   void SyncToServerEach(string key);

   void SyncAllData();
}
