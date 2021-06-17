#if UNITY_ANDROID || UNITY_IPHONE || UNITY_STANDALONE_OSX || UNITY_TVOS
// WARNING: Do not modify! Generated file.

namespace UnityEngine.Purchasing.Security {
    public class GooglePlayTangle
    {
        private static byte[] data = System.Convert.FromBase64String("mvW5v5hY6KkT2HcVn9/ihD5c8E2uHJ+8rpOYl7QY1hhpk5+fn5uenWXPVn2OEiS8vOWvlOtNB8X0q88MnKUhG5kQbKMEzQCC/wTWOPpzo3cocWPzJHcgV7o/SxRLtabjOEHfx72+Yz3YSLLApBIyyzMkI1/HQlv8BD03EFPO00k0zqqYXXPtXHw/hDt/+7uzRpKkVPXl7k9JRHfsbaqHBiKuqtvgPAko0QVm2Z5E+byLPxac9BUi0MZoq3LELwJ6HkW69xwXZQNz5idKKkONO8m/Ugod7fve+fgzUeNNofj+W8TYnJmpmRdjAxeyiXA3HJ+Rnq4cn5ScHJ+fngqr6Ha2GQzPhtIGiz/bsaHA+t1/IZkXrY297yIC88YlGWsNEZydn56f");
        private static int[] order = new int[] { 3,3,6,4,5,8,7,12,11,10,10,13,13,13,14 };
        private static int key = 158;

        public static readonly bool IsPopulated = true;

        public static byte[] Data() {
        	if (IsPopulated == false)
        		return null;
            return Obfuscator.DeObfuscate(data, order, key);
        }
    }
}
#endif
