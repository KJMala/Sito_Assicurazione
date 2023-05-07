using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace RCA.Models
{
    public class AssicurazioniRCA
    {
        static Proprietario p1 = new Proprietario()
        {
            ID = 1,
            LogUtente = LoginUtente.u1,
            Nome = "Mario",
            Cognome = "Rossi",
            Sesso = "m",
            Indirizzo = "Via Marte 13",
            ComuneResidenza = "Milano",
            CF = "JHFGAS6734AJG",
            Telefono = 1234567123,
            listaVeicoli = new List<Veicolo>()
                {
                    new Veicolo()
                    {
                        ID = 1,
                        Targa = "AA456WW",
                        Marca = "Audi",
                        Modello = "A5",
                        Cilindrata = "300cc",
                        Potenza = "500CV",
                        DataImmatricolazione = new DateTime(2012, 12, 3),
                        listaPolizze = new List<Polizza>()
                        {
                            new Polizza()
                            {
                                Stipula = new DateTime(2012, 12, 3),
                                Scadenza = new DateTime(2012, 12, 3),
                                ImportoRata = 500,
                                Saldata = true,
                                DataSaldo = new DateTime(2012, 12, 3)
                            },

                            new Polizza()
                            {
                                Stipula = new DateTime(2014, 12, 5),
                                Scadenza = new DateTime(2014, 12, 5),
                                ImportoRata = 500,
                                Saldata = false,
                                DataSaldo = new DateTime(2014, 12, 5)
                            }
                        },
                        listaSinistri = new List<Sinistro>()
                        {
                            new Sinistro()
                            {
                                ID = 1,
                                DataApertura = new DateTime(2022, 10, 4),
                                Localita = "Roma",
                                NumPersone = 23,
                                Liquidazione = false,
                                DataLiquidazione = new DateTime(2023, 04, 12),
                                ImportoLiquidazione = 300
                            },
                            new Sinistro()
                            {
                                ID =2,
                                DataApertura = new DateTime(2022, 10, 4),
                                Localita = "Roma",
                                NumPersone = 23,
                                Liquidazione = true,
                                DataLiquidazione = new DateTime(2023, 04, 12),
                                ImportoLiquidazione = 500
                            }
                        }

                    }
                }
        };
        static Proprietario p2 = new Proprietario()
        {
            ID = 2,
            LogUtente = LoginUtente.u3,
            Nome = "Luca",
            Cognome = "Bianchi",
            Sesso = "m",
            Indirizzo = "Via Giove 13",
            ComuneResidenza = "Roma",
            CF = "LKSJ834632AASH",
            Telefono = 5671524356,
            listaVeicoli = new List<Veicolo>()
                {
                    new Veicolo()
                    {
                        ID = 2,
                        Targa = "BB567AM",
                        Marca = "Opel",
                        Modello = "Corsa",
                        Cilindrata = "300cc",
                        Potenza = "500CV",
                        DataImmatricolazione = new DateTime(2012, 12, 3),
                        listaPolizze = new List<Polizza>()
                        {
                            new Polizza()
                            {
                                Stipula = new DateTime(2012, 12, 3),
                                Scadenza = new DateTime(2012, 12, 3),
                                ImportoRata = 500,
                                Saldata = true,
                                DataSaldo = new DateTime(2012, 12, 3)
                            },

                            new Polizza()
                            {
                                Stipula = new DateTime(2014, 12, 5),
                                Scadenza = new DateTime(2014, 12, 5),
                                ImportoRata = 500,
                                Saldata = false,
                                DataSaldo = new DateTime(2014, 12, 5)
                            }
                        },
                        listaSinistri = new List<Sinistro>()
                        {
                            new Sinistro()
                            {
                                ID = 3,
                                DataApertura = new DateTime(2022, 10, 4),
                                Localita = "Torino",
                                NumPersone = 23,
                                Liquidazione = false,
                                DataLiquidazione = new DateTime(2023, 04, 12),
                                ImportoLiquidazione = 300
                            },
                            new Sinistro()
                            {
                                ID = 4,
                                DataApertura = new DateTime(2022, 10, 4),
                                Localita = "Torino",
                                NumPersone = 23,
                                Liquidazione = true,
                                DataLiquidazione = new DateTime(2023, 04, 12),
                                ImportoLiquidazione = 500
                            }
                        }

                    }
                }
        };

        public static List<Proprietario> listaProprietari = new List<Proprietario> { p1, p2 };
        public static List<Veicolo> listaTotVeicoli = new List<Veicolo> { p1.listaVeicoli[0], p2.listaVeicoli[0]};
        public static List<Polizza> listaTotPolizze = new List<Polizza> { p1.listaVeicoli[0].listaPolizze[0], p1.listaVeicoli[0].listaPolizze[1], p2.listaVeicoli[0].listaPolizze[0], p2.listaVeicoli[0].listaPolizze[1]};
        public static List<Sinistro> listaTotSinistri = new List<Sinistro> { p1.listaVeicoli[0].listaSinistri[0], p1.listaVeicoli[0].listaSinistri[1], p2.listaVeicoli[0].listaSinistri[0], p2.listaVeicoli[0].listaSinistri[1] };  

    }

    public class Proprietario
    {
        public int ID { get; set; }

        public LoginUtente LogUtente { get; set; }

        [Required(ErrorMessage ="Campo obbligatorio!")]
        public string Nome { get; set; }

        [Required(ErrorMessage = "Campo obbligatorio!")]
        public string Cognome { get; set; }
        public string Sesso { get; set; } //DROPDOWN
        public string Indirizzo { get; set; }
        [Required(ErrorMessage = "Campo obbligatorio!")]
        [Display(Name ="Comune di Residenza")]
        public string ComuneResidenza { get; set; }
        [Required(ErrorMessage = "Campo obbligatorio!")]
        [Display(Name = "Codice Fiscale")]
        //[StringLength(16, MinimumLength = 16, ErrorMessage ="Codice Fiscale non valido!")]
        public string CF { get; set; }
        public long Telefono { get; set; }

        public List<Veicolo> listaVeicoli = new List<Veicolo>();

        
    }

    public class Veicolo
    {
        public int ID { get; set; }
        [Required(ErrorMessage = "Campo obbligatorio!")]
        public string Targa { get; set; }
        [Required(ErrorMessage = "Campo obbligatorio!")]
        public string Marca { get; set; }
        public string Modello { get; set; }
        public string Cilindrata { get; set; }
        public string Potenza { get; set; }
        [Required(ErrorMessage = "Campo obbligatorio!")]
        [Display(Name = "Data di Immatricolazione")]
        public DateTime DataImmatricolazione { get; set; }

        public List<Polizza> listaPolizze = new List<Polizza>();

        public List<Sinistro> listaSinistri = new List<Sinistro>();

    }

    public class Polizza
    {
        public int ID { get; set; }
        [Required(ErrorMessage = "Campo obbligatorio!")]
        public DateTime Stipula { get; set; }
        [Required(ErrorMessage = "Campo obbligatorio!")]
        public DateTime Scadenza { get; set; } //Lista scadenze
        [Required(ErrorMessage = "Campo obbligatorio!")]

        [Display(Name = "Importo Rata")]
        public double ImportoRata { get; set; }
        [Required(ErrorMessage = "Campo obbligatorio!")]
        public bool Saldata { get; set; }
        [Required(ErrorMessage = "Campo obbligatorio!")]

        [Display(Name = "Data Saldo")]
        public DateTime DataSaldo { get; set; }
        // Nuova polizza alla scadenza

    }

    public class Sinistro
    {
        public int ID { get; set; }
        [Required(ErrorMessage = "Campo obbligatorio!")]

        [Display(Name = "Data Apertura")]
        public DateTime DataApertura { get; set; }
        public string Localita { get; set; }

        [Display(Name = "Numero Persone Coinvolte")]
        public int NumPersone { get; set; }
        //[Required(ErrorMessage = "Campo obbligatorio!")]
        public Veicolo Veicolo { get; set; }
        [Required(ErrorMessage = "Campo obbligatorio!")]

        public bool Liquidazione { get; set; }
        [Required(ErrorMessage = "Campo obbligatorio!")]

        [Display(Name = "Data di Liquidazione")]
        public DateTime DataLiquidazione { get; set; }
        [Required(ErrorMessage = "Campo obbligatorio!")]

        [Display(Name = "Importo Liquidazione")]
        public double ImportoLiquidazione { get; set; }
    }

    public class LoginUtente
    {
        [Required(ErrorMessage = "Campo obbligatorio!")]
        public string Username { get; set; }
        [Required(ErrorMessage = "Campo obbligatorio!")]
        public string Password { get; set; }

        public string Role { get; set; }


        public static LoginUtente u1 = new LoginUtente { Username = "mariorossi", Password = "12345", Role = "user" };
        public static LoginUtente u2 = new LoginUtente { Username = "paoloneri", Password = "12345", Role = "admin" };
        public static LoginUtente u3 = new LoginUtente { Username = "lucabianchi", Password = "12345", Role = "user" };

        public static List<LoginUtente> ListaLog { get; set; } = new List<LoginUtente> { u1, u2, u3};

    }
}