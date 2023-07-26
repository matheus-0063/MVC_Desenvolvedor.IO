using Microsoft.AspNetCore.Mvc.Razor;

namespace MFER.App.Extensions
{
    public static class RazorExtensions
    {
        public static string FormataDocumento(this RazorPage page, int tipoPessoa, string documento)
        {
            return tipoPessoa == 1 
                ? Convert.ToUInt64(documento).ToString(@"000\.000\.000\-00") 
                : Convert.ToUInt64(documento).ToString(@"00\.000\.000\/0000\-00");

            //Com essa lógica eu estou definindo a formatação, 
            //caso o tipo de pssoa seja 1, converte para CPF
            //caso seja 2, converte para CNPJ
        }
    }
}
