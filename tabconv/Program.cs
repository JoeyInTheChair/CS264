using System;
using System.IO;
using System.Collections.Generic;
using static System.Console;

namespace CS264Assignment1
{
    class Program
    {
        static void Main(string[] args)
        {
            List<Table> table = new List<Table>();
            Table t = new Table();
            t.header = "a1";
            t.addData("pog");
            t.addData("CSgang");
            t.addData("-");
            table.Add(t);
            t = new Table();
            t.header = "bb";
            t.addData("young");
            t.addData("dunne");
            t.addData("aljarrah");
            table.Add(t);
            t = new Table();
            t.header = "c4";
            t.addData("eolas");
            t.addData("iontas");
            t.addData("phoenix");
            table.Add(t);
            WriteLine("Input Conversion: ");
            string inp = ReadLine();
            bool inpValid = true;
            while(inpValid)
            {
                //check if user input is  valid for conversion
                if(!(inp.Contains("$ tabconv")) && !(inp.Contains("-v")) && !(inp.Contains("-i")) && !(inp.Contains("-o")))
                {
                    WriteLine("Input is invalid");
                    WriteLine("Input must be written as; $ tabconv -v -i <file.exe> -o <file.exe>");
                    WriteLine("-v, -verbose");
                    WriteLine("-o <file>, -output=<file>");
                    WriteLine("-h, -help");
                    WriteLine("-i, -info");
                    inp = "";
                    WriteLine("Input Conversion: ");
                    inp = ReadLine();
                }
                else
                {
                    inpValid = false;
                }
            }
            //find .exe file
            string startEx = ""; //first extension file
            string lastEx = ""; //second extension file
            //finding first extension file
            for(int i = 0; i < inp.Length; i++)
            {
                if(inp[i] == '.')
                {
                    inp = inp.Substring(i+1);
                    for(int j = 0; j < inp.Length; j++)
                    {
                        if(inp[j] == ' ')
                        {
                            startEx = inp.Substring(0, j);
                            inp = inp.Substring(j + 1);
                            break;
                        }
                    }
                }
            }
            WriteLine();
            //finding second extension file
            for (int i = 0; i < inp.Length; i++)
            {
                if (inp[i] == '.')
                {
                    lastEx = inp.Substring(i + 1);
                    break;
                }
            }
            Console.Clear();
            WriteLine(startEx + " file: \n");
            WriteLine(getFile(startEx, table));
            WriteLine("--------------------------------------------------------------------------------------------------");
            WriteLine(lastEx + " file: \n");
            WriteLine(getFile(lastEx, table));
            WriteLine(lastEx + " file exported as file." + lastEx);
            exportFile(lastEx, getFile(lastEx, table));
        }
        static string getFile(string s, List<Table> table)
        {
            string file = "";
            switch(s)
            {
                case "csv": 
                    foreach(Table t in table)
                    {
                        file += "\"" + t.header + "\",";
                    }
                    file = file.Substring(0, file.Length - 1) + "\n";
                    foreach (Table t in table)
                    {
                        List<string> temp = t.getData();
                        foreach (string p in temp)
                        {
                            file += "\"" + p + "\",";
                        }
                        file = file.Substring(0, file.Length - 1) + "\n";
                    }
                    break;
                case "html":
                    file += "<table>";
                    file += "\n	 <tr>";
                    foreach (Table t in table)
                    {
                        file += "\n	 	 <th>" + t.header + "</th>";
                    }
                    file += "\n	 </tr>";
                    foreach (Table t in table)
                    {
                        file += "\n	 <tr>";
                        List<string> temp = t.getData();
                        foreach (string p in temp)
                        {
                            file += "\n	 	 <td>" + p + "</td>";
                        }
                        file += "\n	 </tr>";
                    }
                    file += "\n</table>";
                    break;
                case "json":
                    file += "[";
                    foreach(Table t in table)
                    {
                        List<string> temp = t.getData();
                        string[] arr = new string[temp.Count];
                        string[] result = getJSONData(table, temp);
                        file += "\n  {";
                        for(int i = 0; i < result.Length; i++)
                        {
                            file += "\n   " + result[i];
                        }
                        file += "\n  },";
                    }
                    file = file.Substring(0, file.Length - 1);
                    file += "\n]";
                    break;
            }
            return file;
        }
        static string [] getJSONData(List <Table> table, List<string> s)
        {
            string[] result = new string [table.Count];
            int pos = 0;
            foreach(Table t in table)
            {
                result[pos++] = "\"" + t.header + "\": \"";
            }
            for(int i = 0; i < result.Length; i++)
            {
                result[i] += s[i] + "\",";
            }
            result[result.Length - 1] = result[result.Length - 1].Substring(0, result[result.Length - 1].Length - 1);
            return result;
        }
        static void exportFile(string s, string file)
        {
            
            switch (s)
            {
                case "csv":
                    File.WriteAllText("file.csv", file);
                    break;
                case "html":
                    File.WriteAllText("file.html", file);
                    break;
                case "json":
                    File.WriteAllText("file.json", file);
                    break;
            }
        }
    }
    class Table
    {
        List<string> data = new List<string>();
        public String header{ set; get; }
        public void addData(string s)
        {
            this.data.Add(s);
        }
        public List<string> getData()
        {
            return this.data;
        }
    }
}
