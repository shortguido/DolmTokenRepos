using DolmToken.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FirstWebApp.Models.DB
{

    // TODO: asynchrone Programmierung
    // wichtig: es sollte immer gegen eine Schnittstelle (Interface, Basisklasse)
    //      programmiert werden
    //      => Programm ist leichter wartbar, änderbar, testbar

    public interface IRepositoryUsers
    {
        void Connect();
        void Disconnect();

        // CRUD_Operationen ... Create Read Update Delete

        bool Insert(User user);

        bool Delete(int userId);

        bool ChangeUserDate(int userId, User newUserDate);

        User GetUser(int userId);

        List<User> getAllUsers();

        bool Login(string username, string password);

        // Weitere Methoden 
    }
}
