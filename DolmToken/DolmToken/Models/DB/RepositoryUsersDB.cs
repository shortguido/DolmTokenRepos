using DolmToken.Models;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;

namespace FirstWebApp.Models.DB
{

    // diese Klasse implementiert unser Interface

    public class RepositoryUsersDB : IRepositoryUsers
    {

        // Verbindungszeichenkette: enthält Server_IP-Adresse, Datenbankname, User + Passwort am
        //      DB-Server
        private string _connectionString = "Server=localhost;database=dolm_token;user=root;password=";
        // über diese Verbindung wird mit dem DB-Server kommuniziert
        //      SQL-Befehle gesendet, ...
        private DbConnection _conn;

        public bool ChangeUserDate(int userId, User newUserDate)
        {
            if (this._conn?.State == ConnectionState.Open)
            {
                //ein leeres Commmand erzuegen
                DbCommand cmdUpdate = this._conn.CreateCommand();
                // SQL-Befehl angeben: Parameter verwenden, um SQL-Injection zu vermeiden
                //      @username... Paramtername - kann frei gewählt werden
                //       SQL-Injection: es versucht ein Angreifer einen SQL-Befehl zu den MySQL-Server zu senden
                cmdUpdate.CommandText = "update users " +
                    "set username = @username, set password = sha2(@password, 512), set email = @email" +
                    "where user_id = @user_id";

                DbParameter paramID = cmdUpdate.CreateParameter();
                //hier den oben gewählten Parameternamen verwenden
                paramID.ParameterName = "username";
                paramID.DbType = DbType.String;
                paramID.Value = newUserDate.username;

                //Parameter @username befüllen
                // leeres Paramterobjekt
                DbParameter paramUN = cmdUpdate.CreateParameter();
                //hier den oben gewählten Parameternamen verwenden
                paramUN.ParameterName = "username";
                paramUN.DbType = DbType.String;
                paramUN.Value = newUserDate.username;

                DbParameter paramPWD = cmdUpdate.CreateParameter();

                paramPWD.ParameterName = "password";
                paramPWD.DbType = DbType.String;
                paramPWD.Value = newUserDate.password;

                DbParameter paramEmail = cmdUpdate.CreateParameter();

                paramEmail.ParameterName = "email";
                paramEmail.DbType = DbType.String;
                paramEmail.Value = newUserDate.email;

                //Parameter mit unserem Command (cmdUpdate) verbinden
                cmdUpdate.Parameters.Add(paramID);
                cmdUpdate.Parameters.Add(paramUN);
                cmdUpdate.Parameters.Add(paramPWD);
                cmdUpdate.Parameters.Add(paramEmail);

                // nun senden wird das Command (Insert) an den Server
                return cmdUpdate.ExecuteNonQuery() == 1;

            }

            return false;
        }

        public void Connect()
        {
            // falls die Verbindung noch nicht ezeugt wurde
            if(this._conn == null)
            {
                // wird sie erzeugt
                this._conn = new MySqlConnection(this._connectionString);
            }
            if (this._conn.State != ConnectionState.Open)
            {
                // wird sie geöffnet
                this._conn.Open();
            }
        }

        public bool Delete(int userId)
        {
            // ein leeres Command erzeugen
            DbCommand cmdDelete = this._conn.CreateCommand();
            // SQL-Befehl angeben: Parameter verwenden, um SQL-Injections zu vermeiden
            //      @username - kann frei gewählt werden
            //      SQL-Injection: es versucht ein Angreifer einen SQL-Befehl an den MySQL-Server zu senden
            cmdDelete.CommandText = "delete from users where user_id = @user_id";

            // Parameter @username befüllen
            // leeres ParameterObjekt erzeugen
            DbParameter paramUI = cmdDelete.CreateParameter();
            // hier den oben gewählten Parameternamen verwenden
            paramUI.ParameterName = "user_id";
            paramUI.DbType = DbType.Int32;
            paramUI.Value = userId;

            // Parameter mit unserem Command (cmdInsert) verbinden
            cmdDelete.Parameters.Add(paramUI);

            // nun senden wir das Command (INSERT) an den Server
            return cmdDelete.ExecuteNonQuery() == 1;
        }

        public void Disconnect()
        {
            // falls die Verbindung existiert und geöffnet ist
            if((this._conn != null) && (this._conn.State == ConnectionState.Open))
            {
                // wird sie geschlossen
                this._conn.Close();
            }

        }

        public List<User> getAllUsers()
        {

            List<User> users = new List<User>();

            if (this._conn?.State == ConnectionState.Open)
            {
                // leeres Command erzeugen
                DbCommand cmdAllUsers = this._conn.CreateCommand();
                // SQL-Befehl angeben
                cmdAllUsers.CommandText = "select * from users";

                // wir bekommen nun eine komplette Tabelle zurück
                //      diese wird mit einem DbDataReader Zeile fpr Zeile 
                //      durchlaufen
                using(DbDataReader reader = cmdAllUsers.ExecuteReader())
                {
                    // mit Read wird jeweils eine einzige Zeile (Datensatz) gelesen
                    while (reader.Read())
                    {
                        //den User in der Liste abspeichern
                        users.Add(new User()
                        {
                            UserId = Convert.ToInt32(reader["user_id"]),
                            username = Convert.ToString(reader["username"]),
                            password = Convert.ToString(reader["password"]),
                            email = Convert.ToString(reader["email"])
                        });
                    }
                }   // using: hier wird automatisch der DbDataReader freigegeben
                    //      entspricht dem finally

            }

            // Fälle: es wird entweder eine leere Liste oder die Liste mit allen 
            //      Usern zurückgeliefert
            return users;
        }

        public User GetUser(int userId)
        {
            User user;
            if (this._conn?.State == ConnectionState.Open)
            {
                DbCommand cmdGetUser = this._conn.CreateCommand();
                //SQL - Befehl angeben
                cmdGetUser.CommandText = "select * from users where user_id=@id";
                DbParameter paramID = cmdGetUser.CreateParameter();
                //hier den oben gewählten Parameternamen verwenden
                paramID.ParameterName = "id";
                paramID.DbType = DbType.String;
                paramID.Value = userId;
                using (DbDataReader reader = cmdGetUser.ExecuteReader())
                {
                    user = new User
                    {
                        UserId = Convert.ToInt32(reader["user_id"]),
                        username = Convert.ToString(reader["username"]),
                        password = Convert.ToString(reader["password"]),
                        email = Convert.ToString(reader["email"])
                    };

                    return user;
                }

            }

            return null;

            // bitte keine Schleife verwenden

        }

        public bool Insert(User user)
        {
            if (this._conn?.State == ConnectionState.Open)
            {
                // ein leeres Command erzeugen
                DbCommand cmdInsert = this._conn.CreateCommand();
                // SQL-Befehl angeben: Parameter verwenden, um SQL-Injections zu vermeiden
                //      @username - kann frei gewählt werden
                //      SQL-Injection: es versucht ein Angreifer einen SQL-Befehl an den MySQL-Server zu senden
                cmdInsert.CommandText = "insert into users values(null, @username, sha2(@password, 512), @mail)";

                // Parameter @username befüllen
                // leeres ParameterObjekt erzeugen
                DbParameter paramUN = cmdInsert.CreateParameter();
                // hier den oben gewählten Parameternamen verwenden
                paramUN.ParameterName = "username";
                paramUN.DbType = DbType.String;
                paramUN.Value = user.username;

                DbParameter paramPWD = cmdInsert.CreateParameter();
                // hier den oben gewählten Parameternamen verwenden
                paramPWD.ParameterName = "password";
                paramPWD.DbType = DbType.String;
                paramPWD.Value = user.password;

                DbParameter paramEMail = cmdInsert.CreateParameter();
                // hier den oben gewählten Parameternamen verwenden
                paramEMail.ParameterName = "mail";
                paramEMail.DbType = DbType.String;
                paramEMail.Value = user.email;

                // Parameter mit unserem Command (cmdInsert) verbinden
                cmdInsert.Parameters.Add(paramUN);
                cmdInsert.Parameters.Add(paramPWD);
                cmdInsert.Parameters.Add(paramEMail);

                // nun senden wir das Command (INSERT) an den Server
                return cmdInsert.ExecuteNonQuery() == 1;
            }

            return false;
        }

        public bool Login(string username, string password)
        {
            string pw;
            if (this._conn?.State == ConnectionState.Open)
            {
                //ein leeres Commmand erzuegen
                DbCommand cmdLogin = this._conn.CreateCommand();
                // SQL-Befehl angeben: Parameter verwenden, um SQL-Injection zu vermeiden
                //      @username... Paramtername - kann frei gewählt werden
                //       SQL-Injection: es versucht ein Angreifer einen SQL-Befehl zu den MySQL-Server zu senden
                cmdLogin.CommandText = "select password from users where username=@username; ";
                DbParameter paramUsername = cmdLogin.CreateParameter();
                //Parameter @username befüllen
                paramUsername.ParameterName = "username";
                paramUsername.DbType = DbType.String;
                paramUsername.Value = username;
                using (DbDataReader reader = cmdLogin.ExecuteReader())
                {
                    pw = Convert.ToString(reader["password"]);
                };
                if (pw == password)
                {
                    return true;
                }
            }

            return false;
        }
    }
}
