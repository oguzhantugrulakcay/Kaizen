using System;
using System.Reflection.Emit;
using System.Security.Cryptography;
using System.Text;


internal class Program
{
    public static void Main(string[] args)
    {
        List<string> codes = new();
        for (int i = 0; i < 1000; i++)
        {
            string code = CodCreate(8);
            
            while (IsExist(code, codes))
            {
                code = CodCreate(8);
            };
            codes.Add(code);
            Console.WriteLine(code);

        }
    }
    public static bool IsExist(string NewCode,List<string> CreatedCodes) {
        var sameCode = CreatedCodes.Where(c => c == NewCode).FirstOrDefault();
        if (sameCode == null)
        {

            return IsValid(NewCode);
        }
        else
        {
            return false;
        }
        
    }

    public static bool IsValid (string Code){
        bool hasC = Code.Contains("C");
        bool has5 = Code.Contains("5");
        bool has3 = Code.Contains("3");
        return (!(has3&&has5&&hasC));

    }

    public static string CodCreate(int length)
    {
        string valid = "ACDEFGHKLMNPRTXYZ234579";
        StringBuilder sb = new StringBuilder();
        using (RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider())
        {
            byte[] uintBuffer = new byte[sizeof(uint)];

            while (length-- > 0)
            {
                rng.GetBytes(uintBuffer);
                uint num = BitConverter.ToUInt32(uintBuffer, 0);
                sb.Append(valid[(int)(num % (uint)valid.Length)]);
            }
        }
        return sb.ToString();
    }
}