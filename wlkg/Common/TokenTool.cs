using System;

namespace ServiceInterface.Common
{
    public static class TokenTool
    {
        public static bool VerifyToken(string token)
        {
            string tk = "MV4FGbDeCY/c0E5Xh9k8Mg==";
            if (token == tk)
            {
                return true;
            }
            else { return false; }
            
        }
    }
}