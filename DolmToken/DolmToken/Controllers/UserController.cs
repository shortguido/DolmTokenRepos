﻿using DolmToken.Models;
using FirstWebApp.Models.DB;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;

namespace DolmToken.Controllers
{
    public class UserController : Controller
    {

        // auf der linken Seite die Schnittstelle verwenden
        private IRepositoryUsers _rep = new RepositoryUsersDB();

        public IActionResult Index()
        {
            // alle User aus der Db-Tabelle anzeigen

            try
            {
                _rep.Connect();
                // Daten des Users an die View übergeben
                return View(_rep.getAllUsers());
            }
            catch (DbException)
            {
                return View("_Message", new Message("Datenbankfehler",
                                "Die Benutzer konnten nicht geladen werden",
                                "Versuchen Sie es später erneut!"));
            }
            finally
            {
                _rep.Disconnect();
            }
        }
        [HttpPost]
        public IActionResult Delete(int id)
        {
            try
            {
                _rep.Connect();
                if (_rep.Delete(id))
                {
                    return View("_Message", new Message("Delete", "Ihre Daten wurden erfolgreich gelöscht!"));
                }
            }
            catch (DbException)
            {
                return View("_Message", new Message("Delete", "Datenbankfehler!",
                            "Bitte versuchen Sie es später erneut!"));
            }
            finally
            {
                _rep.Disconnect();
            }
            return View();
        }
        public IActionResult Update(int id, User user)
        {
            // TODO: User mit der ID id updaten
            return View();
        }
        [HttpGet]
        public IActionResult Registration()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Registration(User userDateFromForm)
        {
            // Parameter überprüfen
            if (userDateFromForm == null)
            {
                // Weiterleitung an eine Methode (Action) in selben Controller
                return RedirectToAction("Registration");
            }

            // Eingaben des Benutzers überprüfen - Validierung
            ValidateRegistrationData(userDateFromForm);

            // Falls das Formular richtig ausgefüllt wurde
            if (ModelState.IsValid)
            {
                try
                {
                    _rep.Connect();
                    if (_rep.Insert(userDateFromForm))
                    {
                        return View("_Message", new Message("Registrierung", "Ihre Daten wurden erfolgreich gesendet!"));
                    }
                    else
                    {
                        return View("_Message", new Message("Registrierung", "Ihre Daten wurden NICHT erfolgreich gesendet!",
                                    "Bitte versuchen Sie es später erneut!"));
                    }
                }
                catch (DbException)
                {
                    return View("_Message", new Message("Registrierung", "Datenbankfehler!",
                                "Bitte versuchen Sie es später erneut!"));
                }
                finally
                {
                    _rep.Disconnect();
                }

                // zeigen unsere MessageView mit einer entsprechenden Meldung an

            }

            // falls etwas falsch engege. wurde, wird das Reg-formular
            // erneut aufgerufen - mit den bereits eingeg. Daten

            return View(userDateFromForm);
        }

        private void ValidateRegistrationData(User u)
        {
            // Parameter überprüfen
            if (u == null)
            {
                return;
            }
            // Username
            if (u.username == null || (u.username.Trim().Length < 4))
            {
                ModelState.AddModelError("Username", "Der Benutzername muss mindestens 4 Zeichen lang sein!");
            }

            // Passwort
            if (u.password == null || (u.password.Length < 8))
            {
                ModelState.AddModelError("Password", "Das Passwort muss mindestens 8 Zeichen lang sein!");
            }
            // + mind. ein Großbuchstabe, + mind ein Kleinbuchstabe,
            // + mind. ein Sonderzeichen, + mind. eine Zahl

        }
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(User userDateFromForm)
        {
            // Parameter überprüfen
            if (userDateFromForm == null)
            {
                // Weiterleitung an eine Methode (Action) in selben Controller
                return RedirectToAction("Login");
            }

            // Eingaben des Benutzers überprüfen - Validierung
            ValidateRegistrationData(userDateFromForm);

            // Falls das Formular richtig ausgefüllt wurde
            if (ModelState.IsValid)
            {
                try
                {
                    _rep.Connect();
                    if (_rep.Login(userDateFromForm.username, userDateFromForm.password))
                    {
                        return View("_Message", new Message("Login", "Sie haben sich erfolgreich angemeldet"));
                    }
                    else
                    {
                        return View("_Message", new Message("Login", "Fehler!",
                                    "Bitte versuchen Sie es erneut"));
                    }
                }
                catch (DbException)
                {
                    return View("_Message", new Message("Login", "Datenbankfehler!",
                                "Bitte versuchen Sie es später erneut!"));
                }
                finally
                {
                    _rep.Disconnect();
                }

                // zeigen unsere MessageView mit einer entsprechenden Meldung an

            }

            // falls etwas falsch engege. wurde, wird das Reg-formular
            // erneut aufgerufen - mit den bereits eingeg. Daten

            return View(userDateFromForm);
        }
    }
}
