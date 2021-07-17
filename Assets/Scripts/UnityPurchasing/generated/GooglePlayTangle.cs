#if UNITY_ANDROID || UNITY_IPHONE || UNITY_STANDALONE_OSX || UNITY_TVOS
// WARNING: Do not modify! Generated file.

namespace UnityEngine.Purchasing.Security {
    public class GooglePlayTangle
    {
        private static byte[] data = System.Convert.FromBase64String("y8LrmD1G1Z1/9kjwBT9gs/fouXWU9QN8drxQ7PRnnSP+jbet43NzD8dKfbSzmyC94qydTRGjZECGLKr2MrrZgT0gtf2GvVbv1Gk7bpNvAT8OjyMgwH2DrOq7Ym0fId0xIhoGRvINdEMM/+69RSehotB+BJlStB+o9mURxU6x6uOyEl4lgGDZtPLS/60cx+th0k7YLWX+bNBcO2uCY9Xz8TqICyg6BwwDIIxCjP0HCwsLDwoJ0k5vrBL7yo87RNxS/r9nRjK7c7S3U3bgnOXPbdxuacouiCPBOF0EFogLBQo6iAsACIgLCwq4YHWC+NkLWyNyhcFaA1ThysB6jglZhVuxlmBAZBsm/KGcXVgycpWosuiRsK/lT2BNkF7vV92lPwgJCwoL");
        private static int[] order = new int[] { 6,4,3,9,9,10,13,11,13,11,13,11,13,13,14 };
        private static int key = 10;

        public static readonly bool IsPopulated = true;

        public static byte[] Data() {
        	if (IsPopulated == false)
        		return null;
            return Obfuscator.DeObfuscate(data, order, key);
        }
    }
}
#endif
