using System;

namespace EnvioAdesivos
{
    /// <summary>
    /// Utilize essa classe para fazer o log solicitado no exercício.
    /// Mas não faça alterações, pois utilizaremos esse formato para os nossos testes.
    /// </summary>
    public class Logger
    {
        public void Log(string message)
        {
            Console.WriteLine($"{DateTime.Now:g} EnvioAdesivos [INFO] {message}");
        }
    }
}
