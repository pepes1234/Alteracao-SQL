using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

class Program1
{
    class IdMembros
    {
        public string Id { get; set; }         // Novo ID gerado
        public string TrueId { get; set; }     // ID original (codigomembro)
        public string DtCadastro { get; set; } // Campo novo para armazenar dt_cadastro original
        public string idIgreja { get; set; }
    }

    static void Main()
    {
        string inputFilePath = @"C:\Users\user\Documents\Programação\Alteracao SQL\dados.txt";
        string outputFilePath = @"C:\Users\user\Documents\Programação\Alteracao SQL\resultado1.sql";
        List<IdMembros> membros = new List<IdMembros>();
        
        using StreamReader sr = new StreamReader(inputFilePath);
        using StreamWriter sw = new StreamWriter(outputFilePath);

        string line;
        var counterMember = 0;

        while ((line = sr.ReadLine()) != null)
        {
            if (line.StartsWith("INSERT INTO tb_membros"))
            {
                string pattern = @"INSERT INTO tb_membros\s+\(.*\)\s+VALUES\s+\((.*)\);";
                Match match = Regex.Match(line, pattern);
                
                if (match.Success)
                {
                    string[] splitValues = Regex.Split(match.Groups[1].Value, @",(?=(?:[^']*'[^']*')*[^']*$)");

                    // Capturar dt_cadastro original (último campo antes do dt_alteracao)
                    string dtCadastroOriginal = splitValues[25].Trim('\'', ' ');

                    // Processamento normal do membro...
                    counterMember++;
                    
                    IdMembros membid = new()
                    {
                        Id = counterMember.ToString(),
                        TrueId = splitValues[0].Trim('\''),
                        DtCadastro = dtCadastroOriginal,
                        idIgreja = "21" // Substituir pelo código real da igreja
                    };
                    
                    membros.Add(membid);
                }
            }
        }

        // Gerar comandos UPDATE após processar todos os registros
        sw.WriteLine("\n-- ATUALIZAR MEMBRODESDE COM DATA ORIGINAL --");
        foreach (var membro in membros)
        {
            if (!string.IsNullOrEmpty(membro.DtCadastro) && membro.DtCadastro != "NULL")
            {
                // Formatar a data corretamente para o SQL
                string dataFormatada = DateTime.Parse(membro.DtCadastro).ToString("yyyy-MM-dd HH:mm:ss");
                
                sw.WriteLine($"UPDATE membros SET " +
                    $"membroDesde = '{dataFormatada}' " +
                    $"WHERE id = {membro.Id};");
            }
        }

        Console.WriteLine("Arquivo gerado com sucesso!");
    }
}