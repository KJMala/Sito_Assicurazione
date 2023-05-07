using System;
using System.Collections.Generic;
using System.Configuration;
using System.EnterpriseServices;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using System.Web.UI.WebControls;
using RCA.Models;

namespace RCA.Controllers
{
    [Authorize(Roles="admin")]
    public class RCAController : Controller
    {
        // GET: RCA

        [HttpGet]
        [AllowAnonymous]
        public ActionResult TitlePage()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Dashboard()
        {
                return View(AssicurazioniRCA.listaProprietari);
        }

        [HttpGet]
        public ActionResult DashboardVeicoli()
        {
                AssicurazioniRCA.listaTotVeicoli.Clear();
                foreach(Proprietario prop in AssicurazioniRCA.listaProprietari)
                {
                    foreach(Veicolo v in prop.listaVeicoli)
                    {
                        AssicurazioniRCA.listaTotVeicoli.Add(v);
                    }
                }
                return View(AssicurazioniRCA.listaTotVeicoli);


        }

        [HttpGet]
        public ActionResult DashboardPolizze()
        {
                AssicurazioniRCA.listaTotPolizze.Clear();
                foreach (Proprietario prop in AssicurazioniRCA.listaProprietari)
                {
                    foreach(Veicolo v in prop.listaVeicoli)
                    {
                        foreach (Polizza pol in v.listaPolizze)
                        {
                            AssicurazioniRCA.listaTotPolizze.Add(pol);
                        }
                    }
                    
                }
                return View(AssicurazioniRCA.listaTotPolizze);
        }

        [HttpGet]
        public ActionResult DashboardSinistri()
        {
            AssicurazioniRCA.listaTotSinistri.Clear();
            foreach (Proprietario prop in AssicurazioniRCA.listaProprietari)
                {
                    foreach (Veicolo v in prop.listaVeicoli)
                    {
                        foreach (Sinistro sx in v.listaSinistri)
                        {
                            AssicurazioniRCA.listaTotSinistri.Add(sx);
                        }
                    }
                }
                return View(AssicurazioniRCA.listaTotSinistri);
        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult Login()
        {
            
            if (User.Identity.IsAuthenticated)
            {
                if (User.IsInRole("user"))
                {
                    if (Request.Cookies["USER_COOKIE"] != null)
                    {
                        int Id = Convert.ToInt32(Request.Cookies["USER_COOKIE"]["ID"]);
                        return RedirectToAction("InfoUtente", "User", new { ID = Id });
                    }
                }
                else if(User.IsInRole("admin"))
                {
                    return RedirectToAction("Dashboard");
                }
                
            }
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult Login(LoginUtente l)
        {

            if (ModelState.IsValid)
            {
                foreach (Proprietario prop in AssicurazioniRCA.listaProprietari)
                {

                    if (prop.LogUtente.Username == l.Username && prop.LogUtente.Password == l.Password)
                    {
                        HttpCookie cookiePropID = new HttpCookie("USER_COOKIE");
                        cookiePropID.Values["ID"] = prop.ID.ToString();
                        Response.Cookies.Add(cookiePropID);

                        Session["idUser"] = prop.ID;
                        FormsAuthentication.SetAuthCookie(l.Username, true);
                        return RedirectToAction("InfoUtente", "User", new { @id = prop.ID });
                    }

                }
                foreach (LoginUtente log in LoginUtente.ListaLog)
                {

                    if (log.Username == l.Username && log.Password == l.Password)
                    {
                        FormsAuthentication.SetAuthCookie(l.Username, true);
                        return Redirect(FormsAuthentication.DefaultUrl);
                    }

                }
                ViewBag.ErrorLogin = "Login non effettuato: username o password errati.";
                return View();
            }
            else
            {
                ViewBag.ErroreStato = "Username o password non validi!";
                return View();
            }
           
        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult SignOut()
        {
            Session["idUser"] = null;
            FormsAuthentication.SignOut();
            return Redirect(FormsAuthentication.LoginUrl);
        }

        // ============== ASSICURATI ==============
        [HttpGet]
        public ActionResult CreateAssicurato()
        {
            List<SelectListItem> lista = new List<SelectListItem>();
            SelectListItem l1 = new SelectListItem { Text = "M", Value = "m" };
            SelectListItem l2 = new SelectListItem { Text = "F", Value = "f" };
            SelectListItem l3 = new SelectListItem { Text = "Non specificato", Value = "other" };

            lista.Add(l1);
            lista.Add(l2);
            lista.Add(l3);
            ViewBag.Sesso = lista;

            return View();
        }

        [HttpPost]
        public ActionResult CreateAssicurato(Proprietario prop)
        {
                if (ModelState.IsValid)
                {
                    Proprietario p = new Proprietario();
                    p = prop;

                    p.LogUtente = new LoginUtente();
                    p.LogUtente.Username = $"{p.Nome.ToLower()}{p.Cognome.ToLower()}";
                    p.LogUtente.Password = "12345";
                    p.LogUtente.Role = "user";

                    p.ID = AssicurazioniRCA.listaProprietari.Count() + 1;
                    LoginUtente.ListaLog.Add(p.LogUtente);
                    AssicurazioniRCA.listaProprietari.Add(p);
                    return RedirectToAction("Dashboard");
                }
                else
                {
                    ViewBag.ErroreAssicurato = "Uno o più campi mancanti!";
                    return View();
                }
        }

        [HttpGet]
        public ActionResult EditAssicurato(int ID)
        {
                Proprietario assicurato = new Proprietario();
                foreach (Proprietario p in AssicurazioniRCA.listaProprietari)
                {
                    if (p.ID == ID)
                    {
                        assicurato = p;
                        break;
                    }
                }
                return View(assicurato);

        }

        [HttpPost]
        public ActionResult EditAssicurato(Proprietario proprietario)
        {
                if (ModelState.IsValid)
                {
                
                    foreach(Proprietario p in AssicurazioniRCA.listaProprietari)
                    {
                        if(proprietario.ID == p.ID)
                        {
                            p.Nome = proprietario.Nome;
                            p.Cognome = proprietario.Cognome;
                            p.Indirizzo = proprietario.Indirizzo;
                            p.CF = proprietario.CF;
                            p.ComuneResidenza = proprietario.ComuneResidenza;
                            p.Sesso = proprietario.Sesso;
                            p.Telefono = proprietario.Telefono;
                            break;
                        }
                    }
                    TempData["ConfermaModificheAssicurato"] = "Utente modificato con successo!";
                    return RedirectToAction("Dashboard");
                }
                else
                {
                    return View(proprietario);
                }
        }

        [HttpGet]
        public ActionResult DettagliAssicurato(int id)
        {
                foreach (Proprietario p in AssicurazioniRCA.listaProprietari)
                {
                    if (p.ID == id)
                    {
                        HttpCookie cookiePropID = new HttpCookie("PROPRIETARIO_COOKIE");
                        cookiePropID.Values["ID"] = id.ToString();

                        Response.Cookies.Add(cookiePropID);

                        return View(p);
                    }
                }
                return View();
        }

        // ============== VEICOLI ==============
        [HttpGet]
        public ActionResult CreateVeicolo()
        {
                return View();
        }

        [HttpPost]
        public ActionResult CreateVeicolo(Veicolo v)
        {
                if (ModelState.IsValid)
                {
                    foreach(Proprietario p in AssicurazioniRCA.listaProprietari)
                    {
                        if (Convert.ToInt32(Request.Cookies["PROPRIETARIO_COOKIE"]["ID"]) == p.ID)
                        {
                            v.ID = AssicurazioniRCA.listaTotVeicoli.Count() + 1;
                            p.listaVeicoli.Add(v);
                            AssicurazioniRCA.listaTotVeicoli.Add(v);

                            return RedirectToAction("DettagliAssicurato", new { @id = Request.Cookies["PROPRIETARIO_COOKIE"]["ID"] });
                        }
                        
                    }
                    return View();
                    
                }
                else
                {
                    ViewBag.ErroreAssicurato = "Uno o più campi mancanti!";
                    return View();
                }
        }

        [HttpGet]
        public ActionResult EditVeicolo(int ID)
        {
                Veicolo veicolo = new Veicolo();
                foreach (Proprietario p in AssicurazioniRCA.listaProprietari)
                {
                   foreach(Veicolo v in p.listaVeicoli)
                   {
                        if (v.ID == ID)
                        {
                            veicolo = v;
                            break;
                        }
                    }
                }
                return View(veicolo);

        }

        [HttpPost]
        public ActionResult EditVeicolo(Veicolo veicolo)
        {
                if (ModelState.IsValid)
                {
                    foreach (Proprietario p in AssicurazioniRCA.listaProprietari)
                    {
                        foreach(Veicolo v in p.listaVeicoli)
                        {
                            if (v.ID == veicolo.ID)
                            {
                                v.Targa = veicolo.Targa;
                                v.DataImmatricolazione = veicolo.DataImmatricolazione;
                                v.Cilindrata = veicolo.Cilindrata;
                                v.Marca = veicolo.Marca;
                                v.Modello = veicolo.Modello;
                                v.Potenza = veicolo.Potenza;
                                break;
                            }
                        }               
                    }
                    TempData["ConfermaModificheVeicolo"] = "Veicolo modificato con successo!";
                    return RedirectToAction("DettagliAssicurato", new { @id = Request.Cookies["PROPRIETARIO_COOKIE"]["ID"] });
                }
                else
                {
                    return View();
                }
        }
        [HttpGet]
        public ActionResult DettagliVeicolo(int id)
        {
                foreach (Proprietario p in AssicurazioniRCA.listaProprietari)
                {
                    if (p.ID == Convert.ToInt32(Request.Cookies["PROPRIETARIO_COOKIE"]["ID"]))
                    {
                        foreach(Veicolo v in p.listaVeicoli)
                        {
                            if(v.ID == id)
                            {
                                HttpCookie veicoloCookie = new HttpCookie("VEICOLO_COOKIE");
                                veicoloCookie.Values["ID"] = id.ToString();

                                Response.Cookies.Add(veicoloCookie);

                                return View(v);
                            } 
                        }
                        
                    }
                }
                return View();
        }

        // ============== POLIZZE ==============
        [HttpGet]
        public ActionResult CreatePolizza()
        {
                return View();
        }

        [HttpPost]
        public ActionResult CreatePolizza(Polizza pol)
        {
                if (ModelState.IsValid)
                {
                    foreach (Proprietario p in AssicurazioniRCA.listaProprietari)
                    {
                        if (Convert.ToInt32(Request.Cookies["PROPRIETARIO_COOKIE"]["ID"]) == p.ID)
                        {
                            foreach(Veicolo veicolo in p.listaVeicoli)
                            {
                                if(veicolo.ID == Convert.ToInt32(Request.Cookies["VEICOLO_COOKIE"]["ID"]))
                                {
                                    pol.ID = AssicurazioniRCA.listaTotPolizze.Count() + 1;
                                    veicolo.listaPolizze.Add(pol);
                                    AssicurazioniRCA.listaTotPolizze.Add(pol);

                                    return RedirectToAction("DettagliVeicolo", new { @id = Request.Cookies["VEICOLO_COOKIE"]["ID"] });
                                }
                            }

                            
                        }

                    }
                    return View();

                }
                else
                {
                    ViewBag.ErroreAssicurato = "Uno o più campi mancanti!";
                    return View();
                }
        }

        [HttpGet]
        public ActionResult EditPolizza(int ID)
        {
                Polizza polizza = new Polizza();
                foreach (Proprietario p in AssicurazioniRCA.listaProprietari)
                {
                    foreach (Veicolo v in p.listaVeicoli)
                    {
                        if(v.ID == Convert.ToInt32(Request.Cookies["VEICOLO_COOKIE"]["ID"]))
                        {
                            foreach (Polizza pol in v.listaPolizze)
                            {
                                if(pol.ID == ID)
                                {
                                    polizza = pol;
                                    break;
                                }
                            }
                        }
                    }
                }
                return View(polizza);

        }

        [HttpPost]
        public ActionResult EditPolizza(Polizza polizza)
        {
                if (ModelState.IsValid)
                {
                    foreach (Proprietario p in AssicurazioniRCA.listaProprietari)
                    {
                        foreach (Veicolo v in p.listaVeicoli)
                        {
                            if (v.ID == Convert.ToInt32(Request.Cookies["VEICOLO_COOKIE"]["ID"]))
                            {
                                foreach(Polizza pol in v.listaPolizze)
                                {
                                    if(pol.ID == polizza.ID)
                                    {
                                        pol.Stipula = polizza.Stipula;
                                        pol.Scadenza = polizza.Scadenza;
                                        pol.ImportoRata = polizza.ImportoRata;
                                        pol.Saldata = polizza.Saldata;
                                        pol.DataSaldo = polizza.DataSaldo;
                                        break;
                                    }
                                    
                                }
                                
                            }
                        }
                    }
                    TempData["ConfermaModifichePolizza"] = "Polizza modificato con successo!";
                    return RedirectToAction("DettagliVeicolo", new { @id = Request.Cookies["VEICOLO_COOKIE"]["ID"] });
                }
                else
                {
                    return View();
                }
        }


        // ============== SINISTRI ==============
        [HttpGet]
        public ActionResult CreateSinistro()
        {
                return View();
        }

        [HttpPost]
        public ActionResult CreateSinistro(Sinistro sx)
        {
           
                if (ModelState.IsValid)
                {
                    foreach (Proprietario p in AssicurazioniRCA.listaProprietari)
                    {
                        if (Convert.ToInt32(Request.Cookies["PROPRIETARIO_COOKIE"]["ID"]) == p.ID)
                        {
                            foreach (Veicolo veicolo in p.listaVeicoli)
                            {
                                if (veicolo.ID == Convert.ToInt32(Request.Cookies["VEICOLO_COOKIE"]["ID"]))
                                {
                                    sx.ID = AssicurazioniRCA.listaTotSinistri.Count() + 1;
                                    sx.Veicolo = veicolo;
                                    veicolo.listaSinistri.Add(sx);
                                    AssicurazioniRCA.listaTotSinistri.Add(sx);

                                    return RedirectToAction("DettagliVeicolo", new { @id = Request.Cookies["VEICOLO_COOKIE"]["ID"] });
                                }
                            }


                        }

                    }
                    return View();

                }
                else
                {
                    ViewBag.ErroreAssicurato = "Uno o più campi mancanti!";
                    return View();
                }
        }

        [HttpGet]
        public ActionResult EditSinistro(int ID)
        {
                Sinistro sinistro = new Sinistro();
                foreach (Proprietario p in AssicurazioniRCA.listaProprietari)
                {
                    foreach (Veicolo v in p.listaVeicoli)
                    {
                        if (v.ID == Convert.ToInt32(Request.Cookies["VEICOLO_COOKIE"]["ID"]))
                        {
                            foreach (Sinistro sx in v.listaSinistri)
                            {
                                if (sx.ID == ID)
                                {
                                    sinistro = sx;
                                    break;
                                }
                            }
                        }
                    }
                }
                return View(sinistro);
            

        }

        [HttpPost]
        public ActionResult EditSinistro(Sinistro sinistro)
        {
                if (ModelState.IsValid)
                {
                    foreach (Proprietario p in AssicurazioniRCA.listaProprietari)
                    {
                        foreach (Veicolo v in p.listaVeicoli)
                        {
                            if (v.ID == Convert.ToInt32(Request.Cookies["VEICOLO_COOKIE"]["ID"]))
                            {
                                foreach (Sinistro sx in v.listaSinistri)
                                {
                                    if(sx.ID == sinistro.ID)
                                    {
                                        sx.DataApertura = sinistro.DataApertura;
                                        sx.Localita = sinistro.Localita;
                                        sx.NumPersone = sinistro.NumPersone;
                                        sx.Liquidazione = sinistro.Liquidazione;
                                        sx.Veicolo = sinistro.Veicolo;
                                        sx.ImportoLiquidazione = sinistro.ImportoLiquidazione;
                                        sx.DataLiquidazione = sinistro.DataLiquidazione;
                                        break;
                                    }
                                    
                                }

                            }
                        }
                    }
                    TempData["ConfermaModificheSinistro"] = "Sinistro modificato con successo!";
                    return RedirectToAction("DettagliVeicolo", new { @id = Request.Cookies["VEICOLO_COOKIE"]["ID"] });
                }
                else
                {
                    return View();
                }
        }

        // get con view dettagli veicolo
        //[HttpGet]
        //public ActionResult ViewNonLiquidati(int id)
        //{
        //    foreach (Proprietario p in AssicurazioniRCA.listaProprietari)
        //    {
        //        if (p.ID == Convert.ToInt32(Request.Cookies["PROPRIETARIO_COOKIE"]["ID"]))
        //        {
        //            foreach (Veicolo v in p.listaVeicoli)
        //            {
        //                if (v.ID == id)
        //                {
        //                    //if (Convert.ToBoolean(Session["ViewNonLiquidati"]) == true)
        //                    //{
        //                    //    Session["ViewNonLiquidati"] = false;
        //                    //}
        //                    //else
        //                    //{
        //                    //    Session["ViewNonLiquidati"] = true;
        //                    //}

        //                    return RedirectToAction("DettagliVeicolo", v);
        //                }
        //            }

        //        }
        //    }
        //    return View();

        //}



    }
}