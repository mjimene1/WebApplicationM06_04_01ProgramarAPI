using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace WebApplicationM06_04_01ProgramarAPI.Clases
{
    public class Utils
    {
        public static String MissatgeError(SqlException ex)
        {
            String missatge = null;

            switch (ex.Number)
            {
                case 2:
                    missatge = "El servidor no està operatiu";
                    break;
                case 404:
                    missatge = "Registre no trobat";
                    break;
                case 547:
                    missatge = "El servidor té dades relacionades";
                    break;
                case 2601:
                    missatge = "Registre duplicat";
                    break;
                case 2627:
                    missatge = "Registre duplicat";
                    break;
                case 4060:
                    missatge = "No es pot obrir la base de dades";
                    break;
                case 8101:
                    missatge = "No es pot informar l'ID del department, es calcula automàticament";
                    break;
                case 18456:
                    missatge = "Error en iniciar la sessió";
                    break;
                default:
                    missatge = ex.Number + " - " + ex.Message;
                    break;
            }
            return missatge;
        }
    }
}