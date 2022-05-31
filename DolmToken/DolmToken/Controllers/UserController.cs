using DolmToken.Models;
using FirstWebApp.Models.DB;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Common;
using System.IO;
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

        public IActionResult checkMail(string email)
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
                try
                {
                    _rep.Connect();
                    if (_rep.Insert(userDateFromForm))
                    {
                        HttpContext.Session.SetString("username", userDateFromForm.username);
                        HttpContext.Session.SetString("logstatus", "true");
                        return View("Views/Home/Index.cshtml");
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
            if (u.email == null || new EmailAddressAttribute().IsValid(u.email) == false)
            {
                ModelState.AddModelError("Email", "Prüfen sie ihre Eingabe!");
            }
        }

        private void ValidateLoginData(User u)
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
            ValidateLoginData(userDateFromForm);

            // Falls das Formular richtig ausgefüllt wurde
            if (ModelState.IsValid)
            {
                try
                {
                    _rep.Connect();
                    if (_rep.Login(userDateFromForm.username, userDateFromForm.password))
                    {
                        HttpContext.Session.SetString("username", userDateFromForm.username);
                        HttpContext.Session.SetString("logstatus", "true");
                        return View("Views/Home/Index.cshtml");
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

        public IActionResult Logout()
        {
            try
            {
                HttpContext.Session.SetString("logstatus", "false");
                return View("Views/Home/Index.cshtml");

            }
            catch (DbException)
            {
                return View("_Message", new Message("Logout",
                            "Bitte versuchen Sie es später erneut!"));
            }
            finally
            {
                _rep.Disconnect();
            }
        }

        public IActionResult Konto()
        {
            return View();
        }

        public IActionResult Buy()
        {
            return View();
        }
        [HttpPost]
        public IActionResult UploadFile(IFormFile file)
        {
            try
            {
                _rep.Connect();
                // TODO: Change Content to smth else 
                string pathImages = "../wwwroot/images/";
                if (!Directory.Exists(pathImages))
                {
                    Directory.CreateDirectory(pathImages);
                }
                if (file == null || file.Length == 0)
                    return RedirectToAction("Konto");
                if (file.Length > 1024 * 1024)
                {
                    return RedirectToAction("Konto");
                }
                var filePath = Path.GetTempFileName();
                using (Stream fileStream = new FileStream(pathImages, FileMode.Create))
                {
                    file.CopyTo(fileStream);
                }
                _rep.ChangeUserPicture(HttpContext.Session.GetString("username"), pathImages);
                return RedirectToAction("Konto");
            }
            catch (DbException)
            {
                return View("_Message", new Message("Datenbankfehler!", "Der Benutzer konnte nicht geändert werden! Versuchen sie es später erneut."));
            }
            finally
            {
                _rep.Disconnect();
            }
        }

    }
}
