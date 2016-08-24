using Ada.Framework.Expressions.Entities;
using Ada.Framework.Maintenance.FileManager.Entities;
using System;
using System.IO;

namespace Ada.Framework.Development.Log4Me.Cleaner.Evaluators
{
    public class DateMarginEvaluator : Evaluador
    {
        public DateMarginEvaluator()
        {
            Parametros = new string[] { "Cantidad", "Unidad" };
            codigo = "DateMargin";
            delegado = ((objeto, condicion) =>
                {
                    ValidarCondiciones(objeto);
                    FileSystem fileSystem = objeto as FileSystem;

                    DateTime fechaFileSystem = ObtenerFecha(fileSystem.Info);
                    DateTime fechaParametro = ObtenerFecha(int.Parse(condicion.ObtenerParametro("Cantidad").Valor.ToString()), condicion.ObtenerParametro("Unidad").Valor.ToString());
                    return fechaFileSystem <= fechaParametro;
                });
        }

        private void ValidarCondiciones(object objeto)
        {
            if (!(objeto is FileSystem))
            {
                throw new Exception("¡El evaluador DateMargin sólo permite objetos de tipo FileSystem!");
            }

            FileSystem fileSystem = objeto as FileSystem;
            if (fileSystem.Tipo == TipoFileSystem.Directorio)
            {
                throw new Exception("¡El evaluador DateMargin no es válido para directorios!");
            }
        }

        private DateTime ObtenerFecha(FileSystemInfo info)
        {
            string[] fileNameSplited = info.Name.Split('_');

            int dd = int.Parse(fileNameSplited[1]);
            int MM = int.Parse(fileNameSplited[2]);
            int yyyy = int.Parse(fileNameSplited[3].Split('.')[0]);

            return new DateTime(yyyy, MM, dd);
        }

        private DateTime ObtenerFecha(int cantidad, string unidad)
        {
            DateTime retorno = DateTime.Today;

            if (unidad == "Day" || unidad == "Days")
            {
                retorno = retorno.AddDays((cantidad * -1));
            }
            else if (unidad == "Month" || unidad == "Months")
            {
                retorno = retorno.AddMonths((cantidad * -1));
            }
            else if (unidad == "Year" || unidad == "Years")
            {
                retorno = retorno.AddYears((cantidad * -1));
            }
            else
            {
                throw new Exception(string.Format("¡No se reconoce la unidad {0}!", unidad));
            }

            return retorno;
        }
    }
}
