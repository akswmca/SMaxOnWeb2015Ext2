using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Globalization;
using System.Collections;
using Microsoft.VisualBasic;


namespace WebAPI.Controllers
{
    public class LoginCheck
    {
        public string DecryptPassword(string password)
        {

            CultureInfo myCul = new CultureInfo("tr-TR");

            Comparer objComp = new Comparer(myCul);

            char[] plainArray = password.ToCharArray();
            string decrypted = "";
            bool hasGot = false;
            for (int j = 0; j < plainArray.GetLength(0); j++)
            {
                string currentChar = plainArray[j].ToString();

                int i = 0;
                int plain = 65;
                int crypt = 190;
                for (i = 0; i < 26; i++)
                {
                    
                    if (currentChar == Strings.Chr(crypt - i).ToString())
                    {
                        if (objComp.Compare(currentChar, Strings.Chr(crypt - i).ToString()) == 0)
                        {
                            decrypted += Strings.Chr(plain + i);
                            hasGot = true;
                            break;
                        }
                    }
                }
                if (hasGot == true)
                {
                    hasGot = false;
                    continue;
                }
                // for a-z
                plain = 97;
                crypt = 158;
                for (i = 0; i < 26; i++)
                {
                    if (currentChar == Strings.Chr(crypt - i).ToString())
                    {
                        if (objComp.Compare(currentChar, Strings.Chr(crypt - i).ToString()) == 0)
                        {
                            decrypted += Strings.Chr(plain + i);
                            hasGot = true;
                            break;
                        }
                    }
                }
                if (hasGot == true)
                {
                    hasGot = false;
                    continue;
                }
                //Code Change By Abhishek for Special Characters
                // for Special Characters !-/
                plain = 33;
                crypt = 223;
                for (i = 0; i < 15; i++)
                {
                    if (currentChar == Strings.Chr(crypt - i).ToString())
                    {
                        if (objComp.Compare(currentChar, Strings.Chr(crypt - i).ToString()) == 0)
                        {
                            decrypted += Strings.Chr(plain + i);
                            hasGot = true;
                            break;
                        }
                    }
                }
                if (hasGot == true)
                {
                    hasGot = false;
                    continue;
                }
                //-------------------------------//
                plain = 58;
                crypt = 230;
                for (i = 0; i < 7; i++)
                {
                    if (currentChar == Strings.Chr(crypt - i).ToString())
                    {
                        if (objComp.Compare(currentChar, Strings.Chr(crypt - i).ToString()) == 0)
                        {
                            decrypted += Strings.Chr(plain + i);
                            hasGot = true;
                            break;
                        }
                    }
                }
                if (hasGot == true)
                {
                    hasGot = false;
                    continue;
                }
                //-------------------------------//
                plain = 91;
                crypt = 236;
                for (i = 0; i < 6; i++)
                {
                    if (currentChar == Strings.Chr(crypt - i).ToString())
                    {
                        if (objComp.Compare(currentChar, Strings.Chr(crypt - i).ToString()) == 0)
                        {
                            decrypted += Strings.Chr(plain + i);
                            hasGot = true;
                            break;
                        }
                    }
                }
                if (hasGot == true)
                {
                    hasGot = false;
                    continue;
                }
                //Code End
                // for 1-0
                plain = 48;
                crypt = 207;
                for (i = 0; i < 10; i++)
                {
                    if (currentChar == Strings.Chr(crypt - i).ToString())
                    {
                        decrypted += Strings.Chr(plain + i);
                        break;
                    }
                }
            }
            return decrypted;
        }
        public string EncryptPassword(string password)
        {
            //			NameValueCollection map = new NameValueCollection();
            //
            //			// for A-Z
            //			int i = 0;
            //			int plain = 65;
            //			int crypt = 190;
            //			for (i = 0; i < 26; i++) 
            //			{
            //				//map[Strings.Chr(crypt - i).ToString()] = Strings.Chr(plain + i).ToString();
            //				map[Strings.Chr(plain + i).ToString()] = Strings.Chr(crypt - i).ToString();
            //			}
            // 
            //			// for a-z
            //			plain = 97;
            //			crypt = 158;
            //			for (i = 0; i < 26; i++) 
            //			{
            //				//map[Strings.Chr(crypt - i).ToString()] = Strings.Chr(plain + i).ToString();
            //				map[Strings.Chr(plain + i).ToString()] = Strings.Chr(crypt - i).ToString();
            //
            //			}
            //
            //			// for 1-0
            //			plain = 48;
            //			crypt = 207;
            //			for (i = 0; i < 10; i++) 
            //			{
            ////				map[Strings.Chr(crypt - i).ToString()] = Strings.Chr(plain + i).ToString();
            //				map[Strings.Chr(plain + i).ToString()] = Strings.Chr(crypt - i).ToString();
            //
            //			}
            //
            //			char[] plainArray = password.ToCharArray();
            //			string encrypted = "";
            //			for (i = 0; i < plainArray.GetLength(0); i++) 
            //			{
            //				string currentChar = plainArray[i].ToString();
            //				if (map[currentChar] == null) 
            //				{
            //					encrypted += currentChar;
            //				}
            //				else 
            //				{
            //					encrypted += map[currentChar];
            //				}
            //			}
            //
            //			return encrypted;
            CultureInfo myCul = new CultureInfo("tr-TR");
            Comparer objComp = new Comparer(myCul);

            char[] plainArray = password.ToCharArray();
            string encrypted = "";
            bool hasGot = false;
            for (int j = 0; j < plainArray.GetLength(0); j++)
            {
                string currentChar = plainArray[j].ToString();

                int i = 0;
                int plain = 65;
                int crypt = 190;
                for (i = 0; i < 26; i++)
                {
                    if (currentChar == Strings.Chr(plain + i).ToString())
                    {
                        if (objComp.Compare(currentChar, Strings.Chr(plain + i).ToString()) == 0)
                        {
                            encrypted += Strings.Chr(crypt - i);
                            hasGot = true;
                            break;
                        }
                    }
                }
                if (hasGot == true)
                {
                    hasGot = false;
                    continue;
                }
                // for a-z
                plain = 97;
                crypt = 158;
                for (i = 0; i < 26; i++)
                {
                    if (currentChar == Strings.Chr(plain + i).ToString())
                    {
                        if (objComp.Compare(currentChar, Strings.Chr(plain + i).ToString()) == 0)
                        {
                            encrypted += Strings.Chr(crypt - i);
                            hasGot = true;
                            break;
                        }
                    }
                }
                if (hasGot == true)
                {
                    hasGot = false;
                    continue;
                }
                //Code Change By Abhishek for Special Characters
                // for Special Characters !-/
                plain = 33;
                crypt = 223;
                for (i = 0; i < 15; i++)
                {
                    if (currentChar == Strings.Chr(plain + i).ToString())
                    {
                        if (objComp.Compare(currentChar, Strings.Chr(plain + i).ToString()) == 0)
                        {
                            encrypted += Strings.Chr(crypt - i);
                            hasGot = true;
                            break;
                        }
                    }
                }
                if (hasGot == true)
                {
                    hasGot = false;
                    continue;
                }
                //-------------------------------//
                plain = 58;
                crypt = 230;
                for (i = 0; i < 7; i++)
                {
                    if (currentChar == Strings.Chr(plain + i).ToString())
                    {
                        if (objComp.Compare(currentChar, Strings.Chr(plain + i).ToString()) == 0)
                        {
                            encrypted += Strings.Chr(crypt - i);
                            hasGot = true;
                            break;
                        }
                    }
                }
                if (hasGot == true)
                {
                    hasGot = false;
                    continue;
                }
                //-------------------------------//
                plain = 91;
                crypt = 236;
                for (i = 0; i < 6; i++)
                {
                    if (currentChar == Strings.Chr(plain + i).ToString())
                    {
                        if (objComp.Compare(currentChar, Strings.Chr(plain + i).ToString()) == 0)
                        {
                            encrypted += Strings.Chr(crypt - i);
                            hasGot = true;
                            break;
                        }
                    }
                }
                if (hasGot == true)
                {
                    hasGot = false;
                    continue;
                }
                //Code End
                // for 1-0
                plain = 48;
                crypt = 207;
                for (i = 0; i < 10; i++)
                {
                    if (currentChar == Strings.Chr(plain + i).ToString())
                    {
                        encrypted += Strings.Chr(crypt - i);
                        break;
                    }
                }
            }
            return encrypted;
        }
    }
}