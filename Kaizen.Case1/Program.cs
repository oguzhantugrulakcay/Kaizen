using System;
using System.Reflection.Emit;
using System.Security.Cryptography;
using System.Text;


internal class Program
{
    public static void Main(string[] args)
    {
        var close = true;
        Console.WriteLine("Kod üretmek için 1, Kod kontrolü yapmak için 2 yazınız.:");
        string selectAction = Console.ReadLine();

        while (close)
        {
            if (selectAction == "1")
            {
                Console.WriteLine("Üretmek istediğiniz kod miktarını yazınız.:");
                int countCode = Convert.ToInt32(Console.ReadLine());
                var codes = CodCreate(countCode);
                foreach (var code in codes)
                {
                    Console.WriteLine(code);
                }
            }
            else if (selectAction == "2")
            {
                Console.WriteLine("Kontrolünü yapmak istediğiniz kodu yazınız.");
                var code = Console.ReadLine();
                var isValid = !IsValid(code);
                Console.WriteLine(isValid);
            }
            else
            {
                Console.WriteLine("Hatalı bir giriş yaptınız.");
            }
            Console.WriteLine("Uygulamayı kapatmak istiyor musunuz?Y/N");
            var q = Console.ReadLine();
            if (q.ToUpper() == "N")
            {
                Console.WriteLine("Kod üretmek için 1, Kod kontrolü yapmak için 2 yazınız.:");
                selectAction = Console.ReadLine();
            }
            else
            {
                close = false;
            }
        }

    }
    public static bool IsExist(string NewCode, List<string> CreatedCodes)
    {
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

    public static bool IsValid(string Code)
    {
        int leng = Code.Length;
        if (leng != 8)
            return true;

        var splidValid = "ACDEFGHKLMNPRTXYZ234579".ToCharArray();
        var splidCode = Code.ToUpper().ToCharArray();

        foreach (var chr in splidCode)
        {
            var has = splidValid
                .Where(s => s == chr)
                .FirstOrDefault();
            if (has == null)
                return true;
        }

        bool hasC = Code.Contains("C");
        bool has5 = Code.Contains("5");
        bool has3 = Code.Contains("3");
        return (!(has3 && has5 && hasC));

    }

    public static List<string> CodCreate(int count)
    {
        var length = 8;
        List<string> codes = new();
        for (int i = 0; i < count; i++)
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
            var code = sb.ToString();

            while (IsExist(code, codes))
            {
                length = 8;
                sb = new StringBuilder();
                using (RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider())
                {
                    byte[] uintBuffer = new byte[sizeof(uint)];

                    while (length-- > 0)
                    {
                        rng.GetBytes(uintBuffer);
                        uint num = BitConverter.ToUInt32(uintBuffer, 0);
                        sb.Append(valid[(int)(num % (uint)valid.Length)]);
                    }
                    code = sb.ToString();
                }
            };
            codes.Add(code);
        }
        return codes;
    }
}