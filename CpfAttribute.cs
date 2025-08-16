using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace ClientAdmin.Models.Validators
{
    public class CpfAttribute : ValidationAttribute
    {
        public CpfAttribute() : base("CPF inválido.") { }

        public override bool IsValid(object? value)
        {
            if (value is null) return false;
            var cpf = Regex.Replace(value.ToString()!, "[^0-9]", "");
            if (cpf.Length != 11) return false;
            if (new string(cpf[0], 11) == cpf) return false;

            int[] mult1 = {10,9,8,7,6,5,4,3,2};
            int[] mult2 = {11,10,9,8,7,6,5,4,3,2};

            string tempCpf = cpf.Substring(0,9);
            int soma = 0;
            for (int i = 0; i < 9; i++) soma += (tempCpf[i]-'0') * mult1[i];
            int resto = soma % 11;
            int dig1 = resto < 2 ? 0 : 11 - resto;

            tempCpf += dig1;
            soma = 0;
            for (int i = 0; i < 10; i++) soma += (tempCpf[i]-'0') * mult2[i];
            resto = soma % 11;
            int dig2 = resto < 2 ? 0 : 11 - resto;

            return cpf.EndsWith($"{dig1}{dig2}");
        }
    }
}
