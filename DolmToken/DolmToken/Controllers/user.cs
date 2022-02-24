using DolmToken.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DolmToken.Controllers
{
    public class user : Controller
    {
        public IActionResult Index()
        {
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
                // TODO: Eingabedaten in einer DB-Tabelle abspeichern

                // zeigen unsere MessageView mit einer entsprechenden Meldung an
                return View("_Message", new Message("Registrierung", "Ihre Daten wurden erfolgreich gesendet!"));
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

            // EMail

            
        }

    }
}
