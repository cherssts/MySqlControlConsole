using System;
using MySql.Data.MySqlClient;

namespace ConsoleMySQLControl
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string sql = "";
            bool _selectionTable;
            //var con = new MySqlConnection(sql);
            MySqlConnection con = new MySqlConnection(sql);

            string _table = "";
            string _server = "";
            string _port = "";
            string _userId = "";
            string _password = "";
            string _database = "";
            while (Check_dbConnection(_server, _port, _userId, _password, _database, con) != true)
            {
                //To establish a connection with the database
                //Write the server
                Console.WriteLine("please write at what server are connecting :");
                _server = Console.ReadLine();
                Console.Clear();

                //Write the Port
                Console.WriteLine("Please write the port you are connecting to :");
                _port = Console.ReadLine();
                Console.Clear();

                //Write username
                Console.WriteLine("Please write your username :");
                _userId = Console.ReadLine();
                Console.Clear();

                //Write password
                Console.WriteLine("Please input your password :");
                _password = Console.ReadLine();
                Console.Clear();

                //Write database
                Console.WriteLine("Write the name of the data base you are connecting to :");
                _database = Console.ReadLine();
                Console.Clear();
                _selectionTable = true;

                //sql = $"server={_server};port={_port};userid={_userId};password={_password};database={_database}";
            }
            string _input = "";



            while (_input.ToLower() != "exit" || _input.ToLower() != "stop")
            {
                if(_selectionTable = true)
                {
                    string a = "Show Tables;";
                    var tableShowcase = new MySqlCommand(a,con);
                    Console.WriteLine("Finally write the table you want to see");
                    _table = Console.ReadLine();
                    Console.Clear();
                }

                Display_text(con, _table);

                con.Open();
                _Text();
                _input = Console.ReadLine();
                if (_input.ToLower() == "exit" || _input.ToLower() == "stop")
                {
                    con.Close();
                    return;
                }
                else
                {
                    if (command(_input, con) == true)
                    {
                        Console.WriteLine("Your command was excuted successfully!");
                        Console.WriteLine("Press 'enter' to continue");
                        Console.ReadKey();
                    }
                    else if (command(_input, con) == false)
                    {
                        Console.WriteLine("Your command was not executed successfully!");
                        Console.WriteLine("Please try again!");
                        Console.WriteLine("Press 'enter' to continue");
                        Console.ReadKey();
                    }
                }
                con.Close();
                Console.Clear();
                _input = "";

            }

            static bool command(string input, MySqlConnection con)
            {
                try
                {
                    var command = new MySqlCommand(input, con);
                    var resultSet = command.ExecuteNonQuery();

                    if (!resultSet.Equals(0))
                        return true;
                    else
                        return false;
                }
                catch
                {
                }
                return false;
            }
        }





        static public void _Text()
        {

            Console.WriteLine("('Exit' or 'stop' to stop program)");
            Console.WriteLine("Please write Your Command: ");
        }
        static public void Display_text(MySqlConnection a, string b)
        {
            //a = new MySqlConnection(_Data(b,c,d,e,f));
            a.Open();
            string viewTable = $"Select * from `{b}`";
            var viewcomd = new MySqlCommand(viewTable, a);
            viewcomd.CommandText = viewTable;
            viewcomd.ExecuteNonQuery();
            MySqlDataReader _reader = viewcomd.ExecuteReader();
            Console.WriteLine("Here is your Table:");

            for (int i = 0; i < _reader.VisibleFieldCount; i++)
            {
                Console.Write($"{_reader.GetName(i)}\t");
            }
            Console.WriteLine();
            int x = 0;
            while (_reader.Read())
            {
                for (int i = 0; i < _reader.VisibleFieldCount; i++)
                {
                    
                    var data = _reader[i];
                    if (x != _reader.VisibleFieldCount)
                    {
                        if(data.GetType() == typeof(DateTime))
                        {
                            DateTime dt = (DateTime)data;
                            Console.Write($"{dt.ToString("dd/MM/yyyy")}\t");
                        }
                        else
                        {
                            Console.Write($"{data}\t");
                        }
                        x++;
                    }
                    if(x == _reader.VisibleFieldCount)
                    {
                        Console.WriteLine();
                        x = 0;
                    }
                }
            }
            Console.WriteLine();
            a.Close();
        }

        //public static string _Data(string a, string b, string c, string d, string e)
        //{
        //    string sql = $"server={a};port={b};userid={c};password={d};database={e}";
        //    return sql;
        //}
        public static bool Check_dbConnection(string a, string b, string c, string d, string e, MySqlConnection f)
        {
            string sql = $"server={a};port={b};userid={c};password={d};database={e}";
            bool _isconnected = false;

            try
            {
                f.ConnectionString = sql;
                f.Open();
                _isconnected = true;

            }
            catch (ArgumentException a_ex)
            {
                Console.WriteLine("Check the connection String");
                Console.WriteLine(a_ex.Message);
                Console.WriteLine(a_ex.ToString());

            }
            catch (MySqlException ex)
            {
                _isconnected = false;
                switch (ex.Number)
                {
                    //http://dev.mysql.com/doc/refman/5.0/en/error-messages-server.html
                    // Unable to connect to any of the specified MySQL hosts (Check Server,Port)
                    case 1042:
                        break;
                    // Access denied (Check DB name,username,password)
                    case 0:
                        Console.WriteLine("Check The DataBase's name, username or password");
                        break;
                    default:
                        break;
                }
            }
            finally
            {
                if (f.State == System.Data.ConnectionState.Open)
                {
                    Console.WriteLine("You are connected!");
                    f.Close();
                    Console.WriteLine("Press 'Enter' to Continue!");
                    Console.ReadKey();
                    Console.Clear();
                }
            }
            return _isconnected;
        }



    }
}
