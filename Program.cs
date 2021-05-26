using System;
using MySql.Data.MySqlClient;
using System.Collections.Generic;

namespace project
{
    class Program
    {
        static void PrintMenu(){
        }
        public static int GetDigit(string message, int max)
        {
            int number;
            do
            {
                Console.Write(message);
                number = Console.ReadKey(true).KeyChar - '0';
            } while (number > max || number < 1);
            return number;
        }

        public static string GetLogin(){
            //string server = GetString("Indtast server ip: ");
            //string userid = GetString("Indtast userid: ");
            //string password = GetPass();
            //string database = GetString("Indtast database: ");

            //return $"server={server};userid={userid};password={password};database={database}";
            return @"server=localhost;userid=root;password=nBgPA6MHdp";

        }
        public static string GetString(string message)
        {
            Console.Clear();
            Console.Write(message);
            return Console.ReadLine();
        }


        public static int GetInput(string message, int max)
        {
            int number;
            do
            {
                Console.Clear();
                Console.Write(message);
            } while (!int.TryParse(Console.ReadLine(), out number) || number > max);
            return number;
        }

        // funktion til at lave stjerne GetPass
        public static string PrintStars(int starCount)
        {
            string stars = "";
            for(int i = 0; i < starCount; i++){
                stars += "*";
            }
            return stars;
        }

        public static string GetPass()
        {
            int starCount = 0;
            // tom string hvor hver char bliver appended
            string password = "";
            do
            {
                Console.Clear();
                Console.Write("Indtast kode: {0}", PrintStars(starCount));
                // tager keypress fra user putter ConsoleKeyInfo ind i keyinfo
                ConsoleKeyInfo keyinfo = Console.ReadKey(true);

                // hvis backspace fjern sidste char og fjern stjerne, hvis password.Length er 0 continue for at undgå crash
                if(keyinfo.Key == ConsoleKey.Backspace){
                    if(password.Length == 0)
                        continue;
                    password = password.Remove(password.Length-1);
                    starCount--;
                    continue;
                }
                starCount++;
                // hvis enter så break
                if(keyinfo.Key == ConsoleKey.Enter)
                    break;

                // append keyinfo lavet til char
                password += keyinfo.KeyChar;
            }while(true);
            return password;
        }

        static void Main()
        {
            using(var con = new MySqlConnection(GetLogin())){
                con.Open();
                using(var cmd = new MySqlCommand()){
                    cmd.Connection = con;
                    string sql = "";
RunMenu:
                    List<string> bufferDB = new List<string>();
                    cmd.CommandText = "SHOW DATABASES";
                    using(MySqlDataReader rdr = cmd.ExecuteReader()){
                        bufferDB.Add(rdr.GetName(0));
                        while (rdr.Read()){
                            bufferDB.Add(rdr.GetString(0));
                        }
                    }
                    if(String.IsNullOrEmpty(con.Database)){
                        con.ChangeDatabase(bufferDB[GetInput(PrintBox(bufferDB, true) + "Select database: ", bufferDB.Count-1)]);
                    }
                    Console.WriteLine(con.Database);
                    switch (GetDigit(PrintBox(bufferDB, false) + "1) Create Database\n2) Create Table\n3) Insert Row\n4) Delete Database\n5) Delete Table\n6) Delete Row\n7) Change using database\n8) Logout\n", 8))
                    {
                        case 1:
                            Console.Clear();
                            sql = $"CREATE DATABASE {GetString("Indtast database navn: ")}";
                            cmd.CommandText = sql;
                            cmd.ExecuteNonQuery();
                            goto RunMenu;
                        case 2:

                            goto RunMenu;
                        case 3:

                            goto RunMenu;
                        case 4:
                            Console.Clear();
                            string inputString = GetString("Indtast database navn: ");
                            if(bufferDB.Contains(inputString)){
                                sql = $"DROP DATABASE {inputString}";
                                cmd.CommandText = sql;
                                cmd.ExecuteNonQuery();
                                Console.Clear();
                                Console.WriteLine("Success!");
                            }else {
                                Console.Clear();
                                Console.WriteLine("Database does not exist!");
                            }
                            goto RunMenu;
                        case 5:

                            goto RunMenu;
                        case 6:

                            goto RunMenu;
                        case 7:
                            int dbNum = GetInput(PrintBox(bufferDB, true) + "Select database: ", bufferDB.Count-1);
                            con.ChangeDatabase(bufferDB[dbNum]);
                            goto RunMenu;
                        case 8:
                            break;

                        default:
                            goto RunMenu;
                    }
                }
            }
        }
        static string PrintBox(List<string> buffer, bool showNum){
            string boxString = "";
            int longest = 0;
            int chCount = showNum ? 4 : 2;
            foreach (var item in buffer)
            {
                if(item.Length > longest)
                    longest = item.Length;
            }
            boxString += $"+{GetLine(longest+chCount)}+\n";
            boxString += $"| {buffer[0]}{GetPadding(longest-buffer[0].Length+chCount-2)} |\n";
            boxString += $"+{GetLine(longest+chCount)}+\n";
            for(int i = 1; i < buffer.Count; i++){
                string num = i > 9 ? $"{i})" : $"{i}) " ;
                boxString += showNum ? $"|{num}{buffer[i]}{GetPadding(longest-buffer[i].Length)} |\n" : $"| {buffer[i]}{GetPadding(longest-buffer[i].Length)} |\n";
            }
            return boxString += $"+{GetLine(longest+chCount)}+\n";
        }
        static string GetPadding(int length){
            string line = "";
            for(int i = 0; i < length; i++){
                line += " ";
            }
            return line;
        }
        static string GetLine(int length){
            string line = "";
            for(int i = 0; i < length; i++){
                line += "-";
            }
            return line;
        }
    }
}
/*

                    switch (GetDigit(PrintBox(bufferDatabases, true) + "1) Create Database\n2) Create Table\n3) Insert Row\n4) Delete Database\n5) Delete Table\n6) Delete Row\n7) Logout\n", 7))
                    {
                        case 1:
                            Console.Clear();
                            sql = $"CREATE DATABASE {GetString("Indtast database navn: ")}";
                            cmd.CommandText = sql;
                            cmd.ExecuteNonQuery();
                            goto RunMenu;
                        case 2:

                            goto RunMenu;
                        case 3:

                            goto RunMenu;
                        case 4:
                            Console.Clear();
                            string inputString = GetString("Indtast database navn: ");
                            if(bufferDatabases.Contains(inputString)){
                                sql = $"DROP DATABASE {inputString}";
                                cmd.CommandText = sql;
                                cmd.ExecuteNonQuery();
                                Console.Clear();
                                Console.WriteLine("Success!");
                            }else {
                                Console.Clear();
                                Console.WriteLine("Database does not exist!");
                            }
                            goto RunMenu;
                        case 5:

                            goto RunMenu;
                        case 6:

                            goto RunMenu;
                        case 7:
                            break;

                        default:
                            goto RunMenu;
                    }

   string cs = @"server=localhost;userid=root;password=nBgPA6MHdp;database=test";
   using (var con = new MySqlConnection(cs)){
   con.Open();

   using(var cmd = new MySqlCommand()){
   cmd.Connection = con;

   var stm = "SELECT VERSION()";
   cmd.CommandText = stm;
   var version = cmd.ExecuteScalar().ToString();
   Console.WriteLine($"MySQL version : {con.ServerVersion}");
   Console.WriteLine($"MySQL version : {version}");

   cmd.CommandText = "DROP TABLE IF EXISTS test_table";
   cmd.ExecuteNonQuery();

   cmd.CommandText = @"CREATE TABLE test_table(id INT PRIMARY KEY AUTO_INCREMENT,
   name TEXT, price INT)";
   cmd.ExecuteNonQuery();

   cmd.CommandText =  "INSERT INTO test_table(name, price) VALUES('Audi', 10001)";
   cmd.ExecuteNonQuery();






// this way stops sql injections and is faster
var sql = "INSERT INTO test_table(name, price) VALUES(@name, @price)";
cmd.CommandText = sql;

cmd.Parameters.AddWithValue("@name", "BMW");
cmd.Parameters.AddWithValue("@price", 30000);
cmd.Prepare();
cmd.ExecuteNonQuery();

cmd.CommandText = "SELECT * FROM test_table";

using(MySqlDataReader rdr = cmd.ExecuteReader()){

Console.WriteLine($"{rdr.GetName(0),-4} {rdr.GetName(1),-10}{rdr.GetName(2),10}");

while (rdr.Read()){
Console.WriteLine($"{rdr.GetInt32(0),-4} {rdr.GetString(1),-10}{rdr.GetInt32(2),10}");
}
cmd.CommandText = "SELECT * FROM test_table";
}
cmd.CommandText = "SELECT * FROM test_table";
using(MySqlDataReader rdr = cmd.ExecuteReader()){

Console.WriteLine($"{rdr.GetName(0),-4} {rdr.GetName(1),-10}{rdr.GetName(2),10}");
cmd.CommandText = "SELECT * FROM test_table";

// read() method advaced the data reader to the next record(row). it returns true if thre are more rows otherwise false
while (rdr.Read()){
Console.WriteLine("{0} {1} {2}", rdr.GetInt32(0), rdr.GetString(1), rdr.GetInt32(2));
}
}
}
}
*/
