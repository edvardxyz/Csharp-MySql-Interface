using System;
using MySql.Data.MySqlClient;
using System.Collections.Generic;
namespace project
{
    class Program
    {
        static void Main()
        {
            using(var con = new MySqlConnection(GetLogin())){
                con.Open();
                using(var cmd = new MySqlCommand()){
                    cmd.Connection = con;
                    string sql = "";
                    int dbNum;
RunMenu:
                    List<string> bufferDB = new List<string>();
                    List<string> bufferTbl = new List<string>();
                    cmd.CommandText = "SHOW DATABASES";
                    using(MySqlDataReader rdr = cmd.ExecuteReader()){
                        bufferDB.Add(rdr.GetName(0));
                        while (rdr.Read()){
                            bufferDB.Add(rdr.GetString(0));
                        }
                    }
                    if(String.IsNullOrEmpty(con.Database) || !bufferDB.Contains(con.Database)){
                        dbNum = GetInput(PrintBox(bufferDB, true, con.Database) + "Select database: ", bufferDB.Count-1);
                        con.ChangeDatabase(bufferDB[dbNum]);
                    }
                    cmd.CommandText = "SHOW TABLES";
                    using(MySqlDataReader rdr = cmd.ExecuteReader()){
                        bufferTbl.Add(rdr.GetName(0));
                        while (rdr.Read()){
                            bufferTbl.Add(rdr.GetString(0));
                        }
                    }
                    Console.Clear();
                    Console.Write(PrintBox(bufferDB, false, con.Database));
                    Console.Write(PrintBox(bufferTbl, false, ""));
                    switch (GetDigit("1) Create Database\n2) Create Table\n3) Insert Row\n4) Delete Database\n5) Delete Table\n6) Delete Row\n7) Change using database\n8) Logout\n", 8))
                    {
                        case 1:
                            sql = $"CREATE DATABASE {GetString("Type database name: ")}";
                            cmd.CommandText = sql;
                            cmd.ExecuteNonQuery();
                            goto RunMenu;
                        case 2:
                            string table_name = GetString("Table name: ");
                            int columnN = GetInput("Number of columns: ", 99);
                            cmd.CommandText = $"CREATE TABLE {table_name}(";
                            for(int i = 0; i < columnN; i++){
                                string column_name = GetString($"Name column {i+1}: ");
                                cmd.CommandText += $"{column_name} ";
                                string column_type = GetString($"Type of column {column_name}: ");
                                if(columnN-1 == i)
                                    cmd.CommandText += $"{column_type} "; // type correct or die xd
                                else
                                    cmd.CommandText += $"{column_type}, "; // type correct or die xd
                            }
                            cmd.CommandText += $")";
                            cmd.ExecuteNonQuery();
                            goto RunMenu;
                        case 3:
                            int tblNum = GetInput(PrintBox(bufferTbl, true, "") + "Select table to insert row into: ", bufferTbl.Count-1);
                            cmd.CommandText = $"INSERT INTO {bufferTbl[tblNum]}(";
                            int columnValuesN = GetInput("Number of column values: ", 99);
                            string columnNames = "";
                            string columnValues = " VALUES(";
                            for(int i = 0; i < columnValuesN; i++){
                                string nameS = GetString("Name of column: ");
                                columnNames += nameS;
                                columnValues += GetString($"Remember ' ' around strings. Value of {nameS}: ");

                                if(columnValuesN-1 != i){
                                    columnNames += ",";
                                    columnValues += ",";
                                }
                            }
                            cmd.CommandText += $"{columnNames}) {columnValues})";
                            cmd.ExecuteNonQuery();
                            goto RunMenu;
                        case 4:
                            dbNum = GetInput(PrintBox(bufferDB, true, con.Database) + "Select database to delete: ", bufferDB.Count-1);
                            sql = $"DROP DATABASE {bufferDB[dbNum]}";
                            cmd.CommandText = sql;
                            cmd.ExecuteNonQuery();
                            goto RunMenu;
                        case 5:
                            int tbl = GetInput(PrintBox(bufferTbl, true, "") + "Select table to delete: ", bufferTbl.Count-1);
                            cmd.CommandText = $"DROP TABLE {bufferTbl[tbl]}";
                            cmd.ExecuteNonQuery();
                            goto RunMenu;
                        case 6:
                            Console.WriteLine("Nothing here xD");
                            Console.ReadKey(true);
                            goto RunMenu;
                        case 7:
                            dbNum = GetInput(PrintBox(bufferDB, true, con.Database) + "Select database: ", bufferDB.Count-1);
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
        static string PrintBox(List<string> buffer, bool showNum, string selected){
            int selDBindex = buffer.IndexOf(selected);
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
                if(i == selDBindex)
                    boxString += showNum ? $"|{num}{buffer[i]}*{GetPadding(longest-buffer[i].Length-1)} |\n" : $"| {buffer[i]}*{GetPadding(longest-buffer[i].Length-1)} |\n";
                else
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
            string server = GetString("Server ip: ");
            string userid = GetString("Userid: ");
            string password = GetPass();
            //string database = GetString("Database: ");
            return $"server={server};userid={userid};password={password}";
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
                Console.Write("Password: {0}", PrintStars(starCount));
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
    }
}
