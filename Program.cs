using System;
using System.Diagnostics;
using System.IO;
using System.Text.RegularExpressions;

class Program
{
    static void Main()
    {
        goto es;
        string inputFilePath = @"C:\Users\user\Documents\Programação\Alteracao SQL\dados.txt"; // Caminho do arquivo de entrada
        string outputFilePath = @"C:\Users\user\Documents\Programação\Alteracao SQL\resultado"; // Caminho do arquivo de saída
        List<Igrejas> igrejas = new List<Igrejas>();
        List<IdMembros> membros = new List<IdMembros>();
        StreamReader sr = new StreamReader(inputFilePath);
        StreamWriter sw = new StreamWriter(outputFilePath);

        string line;
        var counter = 0;

        var counterMember = 0;
        while ((line = sr.ReadLine()) != null)
        {

            if (line.StartsWith("INSERT INTO tb_igrejas"))
            {
                string pattern = @"INSERT INTO tb_igrejas\s+\(.*\)\s+VALUES\s+\((.*)\);";
                Match match = Regex.Match(line, pattern);
                if (match.Success)
                {
                    counter++;
                    string values = match.Groups[1].Value;
                    string[] splitValues = Regex.Split(values, @",(?=(?:[^']*'[^']*')*[^']*$)");

                    // Assegurando que existem valores suficientes
                    if (splitValues.Length >= 17)
                    {
                        string NomeIgreja = splitValues[1].Trim();
                        string igrejasede = "'Assembleia de Deus Sede de Pinhais'";
                        string statusIgreja = "1";
                        string cep = splitValues[6].Trim();
                        string endereco = splitValues[3].Trim();
                        string bairro = splitValues[4].Trim();
                        string cidade = splitValues[5].Trim();
                        string fone = splitValues[7].Trim();
                        string numero = splitValues[14].Trim();
                        string FK_IdMembroIgreja = "null";
                        string secretario = splitValues[9].Trim();
                        string tesoureiro = splitValues[10].Trim();
                        string cnpj = splitValues[14].Trim();
                        string dt_alteracao = "'" + DateTime.Now.ToString("yyyyMMdd HH:mm:ss") + "'";
                        string dt_cadastro = "'" + DateTime.Now.ToString("yyyyMMdd HH:mm:ss") + "'";

                        Igrejas igreja = new()
                        {
                            Id = splitValues[0].Trim(),
                            TrueId = counter
                        };
                    
                        igrejas.Add(igreja);

                        string newInsert = $"INSERT INTO igrejas (NomeIgreja, igrejasede, statusIgreja, cep, endereco, bairro, cidade, fone, numero, FK_IdMembroIgreja, secretario, tesoureiro, cnpj, dt_alteracao, dt_cadastro) VALUES ({NomeIgreja}, {igrejasede}, {statusIgreja}, {cep}, {endereco}, {bairro}, {cidade}, {fone}, {numero}, {FK_IdMembroIgreja}, {secretario}, {tesoureiro}, {cnpj}, {dt_alteracao}, {dt_cadastro});";
                        if (!string.IsNullOrWhiteSpace(newInsert))
                        {
                            sw.WriteLine(newInsert);
                            sw.Flush();
                        }
                    }
                }
            }

            string[] estadosCivis = ["'SOLTEIRO(A)'", "'CASADO(A)'", "'VIÚVO(A)'", "'DIVORCIADO(A)'", "'AMASIADO(A)'", "'SEPARADO(A)'", "'VIUVO(A)'", "'OUTROS'"];
            if (line.StartsWith("INSERT INTO tb_membros"))
            {
                string pattern = @"INSERT INTO tb_membros\s+\(.*\)\s+VALUES\s+\((.*)\);";
                Match match = Regex.Match(line, pattern);
                if (match.Success)
                {
                    string values = match.Groups[1].Value;
                    // Usando Regex para fazer o split com base na vírgula, considerando campos vazios
                    string[] splitValues = Regex.Split(values, @",(?=(?:[^']*'[^']*')*[^']*$)");

                    // Ajustando os valores para garantir que campos vazios sejam tratados corretamente
                    for (int i = 0; i < splitValues.Length; i++)
                    {
                        splitValues[i] = splitValues[i].Trim();

                        if (splitValues[i] == "''")
                        {
                            splitValues[i] = "''";  // String vazia representada por duas aspas simples
                        }
                        else if (splitValues[i].ToUpper() == "NULL")
                        {
                            splitValues[i] = "NULL";  // Mantém NULL sem aspas
                        }
                        else
                        {
                            splitValues[i] = splitValues[i].Trim('\'');  // Remove aspas simples
                        }
                    }

                    if (splitValues.Length >= 25)
                    {
                        string FK_estadoCivil = "";
                        if (string.IsNullOrEmpty(splitValues[4]) || splitValues[4] == "''")
                        {
                            FK_estadoCivil = "NULL";
                        }
                        else
                        {
                            for (int i = 1; i <= 8; i++)
                            {
                                if (splitValues[4].Trim() == estadosCivis[i - 1].Trim().Replace("'", ""))
                                {
                                    FK_estadoCivil = i.ToString();
                                    
                                    if(i == 7)
                                    {
                                        FK_estadoCivil = 3.ToString();
                                    }
                                    if(i == 8)
                                    {
                                        FK_estadoCivil = 1.ToString();
                                    }
                                }
                            }
                        }
                        
                        string trueId = "";
                        for (int i = 0; i < igrejas.Count; i++)
                        {
                            if (splitValues[1] == igrejas[i].Id)
                                trueId = igrejas[i].TrueId.ToString();
                        }

                        string FK_IdIgrejaMembros = trueId != "21" ? trueId : "21";
                        
                        // Ignorando membros com FK_IdIgrejaMembros igual a "21"
                        if (FK_IdIgrejaMembros != "21")
                        {
                            counterMember++;
                            
                            // Criando um novo IdMembros
                            IdMembros membid = new()
                            {
                                Id = counterMember.ToString(),
                                TrueId = splitValues[0],
                                idIgreja = FK_IdIgrejaMembros
                            };

                            // Adicionando membro à lista
                            membros.Add(membid);

                            string nome = splitValues[2];
                            string sexo = splitValues[3];
                            string nomePai = splitValues[5];
                            string nomeMae = splitValues[6];
                            string dataNascimento = splitValues[7].Replace("-", "");
                            string cidadeNasc = splitValues[8];
                            string estadoNasc = splitValues[9];
                            string endereco = splitValues[11];
                            string numero = splitValues[22];
                            string bairro = splitValues[12];
                            string cidade = splitValues[13];
                            string estado = splitValues[15];
                            string cep = splitValues[14];
                            string email = splitValues[20];
                            string foneResidencial = splitValues[17];
                            string foneCelular = splitValues[18];
                            string FK_idEstadoCivilMembros = FK_estadoCivil;
                            string foto = "NULL";
                            string numeroDeFilhos = "0";
                            string statusMembro = trueId != "21" ? "1" : "0";
                            string dt_alteracao = "'" + DateTime.Now.ToString("yyyyMMdd HH:mm:ss") + "'";
                            string dt_cadastro = "'" + DateTime.Now.ToString("yyyyMMdd HH:mm:ss") + "'";

                            // Construindo o novo INSERT  
                            string newInsert =
                                $"INSERT INTO membros (FK_IdIgrejaMembros, nome, sexo, nomePai, nomeMae, dataNascimento, cidadeNasc, estadoNasc, endereco, numero, bairro, cidade, estado, cep, email, foneResidencial, foneCelular, FK_idEstadoCivilMembros, foto, numeroDeFilhos, statusMembro, dt_alteracao, dt_cadastro) VALUES ({(FK_IdIgrejaMembros != "NULL" ? $"'{FK_IdIgrejaMembros}'" : "NULL")}, {(nome != "NULL" ? $"'{nome}'" : "NULL")}, {(sexo != "NULL" ? $"'{sexo}'" : "NULL")}, {(nomePai != "NULL" ? $"'{nomePai}'" : "NULL")}, {(nomeMae != "NULL" ? $"'{nomeMae}'" : "NULL")}, {(dataNascimento != "NULL" ? $"'{dataNascimento}'" : "NULL")}, {(cidadeNasc != "NULL" ? $"'{cidadeNasc}'" : "NULL")}, {(estadoNasc != "NULL" ? $"'{estadoNasc}'" : "NULL")}, {(endereco != "NULL" ? $"'{endereco}'" : "NULL")}, {(numero != "NULL" ? $"'{numero}'" : "NULL")}, {(bairro != "NULL" ? $"'{bairro}'" : "NULL")}, {(cidade != "NULL" ? $"'{cidade}'" : "NULL")}, {(estado != "NULL" ? $"'{estado}'" : "NULL")}, {(cep != "NULL" ? $"'{cep}'" : "NULL")}, {(email != "NULL" ? $"'{email}'" : "NULL")}, {(foneResidencial != "NULL" ? $"'{foneResidencial}'" : "NULL")}, {(foneCelular != "NULL" ? $"'{foneCelular}'" : "NULL")}, {(FK_idEstadoCivilMembros != "NULL" ? $"'{FK_idEstadoCivilMembros}'" : "NULL")}, {(foto != "NULL" ? $"'{foto}'" : "NULL")}, {(numeroDeFilhos != "NULL" ? $"'{numeroDeFilhos}'" : "NULL")}, {(statusMembro != "NULL" ? $"'{statusMembro}'" : "NULL")}, {dt_alteracao}, {dt_cadastro});";

                            // Escreve o novo comando no arquivo
                            if (!string.IsNullOrWhiteSpace(newInsert))
                            {
                                sw.WriteLine(newInsert);
                                sw.Flush();
                            }
                        }
                    }
                }
            }

        }

        sr.BaseStream.Seek(0, SeekOrigin.Begin);
        sr.DiscardBufferedData();


        string line2;

        sw.WriteLine("\n\n\n\n\n");

        while ((line2 = sr.ReadLine()) != null)
        {
            string[] escolaridade = ["'FUNDAMENTAL INCOMPLETO'", "'MÉDIO INCOMPLETO'", "'MÉDIO COMPLETO'", "'FUNDAMENTAL COMPLETO'", "'SUPERIOR COMPLETO'", "'SUPERIOR INCOMPLETRO'", "'1º GRAU INCOMPLETO'", "'1º GRAU COMPLETO'", "'2º GRAU INCOMPLETO'", "'2º GRAU COMPLETO'"];
            if (line2.StartsWith("INSERT INTO tb_dadosprofissionais"))
            {
                string pattern = @"INSERT INTO tb_dadosprofissionais\s+\(.*\)\s+VALUES\s+\((.*)\);";

                Match match = Regex.Match(line2, pattern);
                if (match.Success)
                {

                    string values = match.Groups[1].Value;
                    string[] splitValues = Regex.Split(values, @",(?=(?:[^']*'[^']*')*[^']*$)");
                    IdMembros memberId = new IdMembros();

                    for (int i = 0; i < splitValues.Length; i++)
                    {
                        splitValues[i] = splitValues[i].Trim();

                        if (splitValues[i] == "''")
                        {
                            splitValues[i] = "''";  // String vazia representada por duas aspas simples
                        }
                        else if (splitValues[i].ToUpper() == "NULL")
                        {
                            splitValues[i] = "NULL";  // Mantém NULL sem aspas
                        }
                        else
                        {
                            splitValues[i] = splitValues[i].Trim('\'');  // Remove aspas simples
                        }
                    }



                    for (int i = 0; i < membros.Count(); i++)
                    {
                        if (splitValues[0] == membros[i].TrueId)
                        {
                            memberId.Id = membros[i].Id;
                            memberId.idIgreja = membros[i].idIgreja;
                            break;
                        }
                    }

                    if (string.IsNullOrEmpty(memberId.idIgreja))
                    {
                        // Membro não encontrado, trata o caso conforme necessário, como ignorar ou atribuir valor padrão
                        continue;  // Isso faz com que o código vá para a próxima iteração do loop principal, ignorando o processamento deste caso
                    }

                    int FK_escolaridade = 0;
                    for (int i = 1; i <= 10; i++)
                    {
                        if (splitValues[1].Trim().Contains("FUNDAMENTAL") || splitValues[1].Trim().Contains("1º GRAU"))
                        {
                            FK_escolaridade = 1;
                            break;
                        }
                        if (splitValues[1].Trim().Contains("MÉDIO") || splitValues[1].Trim().Contains("2º GRAU"))
                        {
                            FK_escolaridade = 2;
                            break;
                        }
                        if (splitValues[1].Trim().Contains("SUPERIOR"))
                        {
                            FK_escolaridade = 3;
                            break;
                        }
                    }
                    string[] repartEscolaridade = splitValues[1].Split(' ');
                    string complementoEsco = "";
                    if (repartEscolaridade.Count() == 2)
                    {
                        complementoEsco = repartEscolaridade[1].Replace("'", "");
                    }

                    if (repartEscolaridade.Count() == 3)
                    {
                        complementoEsco = repartEscolaridade[2].Replace("'", "");
                    }


                    // Assegurando que existem valores suficientes
                    // Assegurando que existem valores suficientes
                    
                    if (splitValues.Length >= 17)
                    {
                        Console.WriteLine(memberId.idIgreja);
                        if (memberId.idIgreja != "21")
                        {
                            string idMembro = string.IsNullOrEmpty(memberId.Id) ? "NULL" : memberId.Id;  // Verificar se o campo memberId está vazio
                            int escolaridadeProf = FK_escolaridade;
                            string complementoEscolaridade = complementoEsco;
                            string profissao = splitValues[5];
                            string rg = splitValues[3];
                            string cpf = splitValues[4];
                            string foneComercial = splitValues[12];
                            string dt_alteracao = "'" + DateTime.Now.ToString("yyyyMMdd HH:mm:ss") + "'";
                            string dt_cadastro = "'" + DateTime.Now.ToString("yyyyMMdd HH:mm:ss") + "'";

                            // Substituir campos vazios por NULL
                            profissao = string.IsNullOrEmpty(profissao) ? "NULL" : $"'{profissao}'";
                            rg = string.IsNullOrEmpty(rg) ? "NULL" : $"'{rg}'";
                            cpf = string.IsNullOrEmpty(cpf) ? "NULL" : $"'{cpf}'";
                            foneComercial = string.IsNullOrEmpty(foneComercial) ? "NULL" : $"'{foneComercial}'";

                            // Verificar FK_IdMembro_DadosProfissionais e substituir por NULL caso vazio


                            string FK_IdMembro_DadosProfissionais = string.IsNullOrEmpty(idMembro) ? "NULL" : idMembro;
                            
                                string newInsert =
                                    $"INSERT INTO dadosProfissionais (FK_IdMembro_DadosProfissionais, FK_IdEscolaridade_DadosProfissionais, complementoEscolaridade, profissao, rg, cpf, foneComercial, dt_alteracao, dt_cadastro) VALUES ({FK_IdMembro_DadosProfissionais}, {(escolaridadeProf != 0 ? $"{escolaridadeProf}" : "NULL")}, {(string.IsNullOrEmpty(complementoEscolaridade) ? "NULL" : $"'{complementoEscolaridade}'")}, {profissao}, {rg}, {cpf}, {foneComercial}, {dt_alteracao}, {dt_cadastro});";

                                if (!string.IsNullOrWhiteSpace(newInsert))
                                {
                                    sw.WriteLine(newInsert);
                                    sw.Flush();
                                }
                        }
                    }
                }
            }

            string[] cargos = ["'MEMBRO'", "'COOPERADOR'", "'DIÁCONO'", "'PRESBITERO'", "'EVANGELISTA'", "'PASTOR'"];
            // Dicionário para mapear codcargo antigo para o novo nome do cargo
            // Dicionário de mapeamento entre os códigos antigos e os novos cargos
            // Mapeando os IDs antigos para os nomes dos cargos antigos
            // Dicionário de mapeamento entre a descrição do cargo e o valor de codcargo
            Dictionary<int, string> cargoMapAntigoParaNome = new Dictionary<int, string>
            {
                { 1, "Pastor" },
                { 2, "Membro" },
                { 3, "Presbitero" },
                { 4, "Diacono" },
                { 5, "Cooperador" },
                { 6, "Auxiliar" },
                { 7, "Porteiro" },
                { 8, "Evangelista" },
                { 9, "Professor" },
                { 10, "Diaconisa" },
                { 11, "Missionario" },
                { 13, "Musico" },
                { 14, "Pastor Presidente" },
                { 15, "Pastor Auxiliar" },
                { 0, "Secretario" }
            };

            // Agora usando os IDs reais do novo banco
            Dictionary<string, int> nomeCargoParaIdNovo = new Dictionary<string, int>
            {
                { "Membro", 1 },
                { "Cooperador", 2 },
                { "Diacono", 3 },
                { "Presbitero", 4 },
                { "Evangelista", 5 },
                { "Pastor", 6 }
            };


            if (line2.StartsWith("INSERT INTO tb_cargofuncoes"))
            {
                string pattern = @"INSERT INTO tb_cargofuncoes\s+\(.*\)\s+VALUES\s+\((.*)\);";

                Match match = Regex.Match(line2, pattern);
                if (match.Success)
                {
                    string values = match.Groups[1].Value;
                    string[] splitValues = Regex.Split(values, @",(?=(?:[^']*'[^']*')*[^']*$)");
                    IdMembros memberId = new IdMembros();


                    // Limpeza dos valores da linha
                    for (int i = 0; i < splitValues.Length; i++)
                    {
                        splitValues[i] = splitValues[i].Trim();

                        if (splitValues[i] == "''")
                        {
                            splitValues[i] = "''";  // String vazia representada por duas aspas simples
                        }
                        else if (splitValues[i].ToUpper() == "NULL")
                        {
                            splitValues[i] = "NULL";  // Mantém NULL sem aspas
                        }
                        else
                        {
                            splitValues[i] = splitValues[i].Trim('\'');  // Remove aspas simples
                        }
                    }

                    // Buscar o Id do membro correspondente
                    for (int i = 0; i < membros.Count(); i++)
                    {
                        if (splitValues[5] == membros[i].TrueId)
                        {
                            memberId.Id = membros[i].Id;
                            memberId.idIgreja = membros[i].idIgreja;
                            break;
                        }
                    }

                    if (string.IsNullOrEmpty(memberId.idIgreja))
                    {
                        // Membro não encontrado, trata o caso conforme necessário, como ignorar ou atribuir valor padrão
                        continue;  // Isso faz com que o código vá para a próxima iteração do loop principal, ignorando o processamento deste caso
                    }

                    if(splitValues.Length >= 8)
                    {
                        // Buscar o cargo no dicionário
                        if (int.TryParse(splitValues[6], out int codCargoAntigo))
                        {
                            if (memberId.idIgreja != "21")
                            {
                                if (cargoMapAntigoParaNome.TryGetValue(codCargoAntigo, out string nomeCargo))
                                {
                                    if (nomeCargoParaIdNovo.TryGetValue(nomeCargo, out int novoCargoId))
                                    {
                                        string dtCargoAlt = "'" + DateTime.Now.ToString("yyyyMMdd HH:mm:ss") + "'";
                                        string FK_IdMembro_cargoFuncoes = memberId.Id;
                                        string dt_alteracao = "'" + DateTime.Now.ToString("yyyyMMdd HH:mm:ss") + "'";
                                        string dt_cadastro = "'" + DateTime.Now.ToString("yyyyMMdd HH:mm:ss") + "'";

                                        string newInsert =
                                            $"INSERT INTO cargoFuncoes (FK_IdCargo_cargoFuncoes, dtCargoAlt, FK_IdMembro_cargoFuncoes, dt_alteracao, dt_cadastro) " +
                                            $"VALUES ({novoCargoId}, {dtCargoAlt}, " +
                                            $"{(!string.IsNullOrEmpty(FK_IdMembro_cargoFuncoes) ? FK_IdMembro_cargoFuncoes : "NULL")}, {dt_alteracao}, {dt_cadastro});";

                                        sw.WriteLine(newInsert);
                                        sw.Flush();
                                    }
                                    else
                                    {
                                        Console.WriteLine($"⚠ Erro: Cargo '{nomeCargo}' não encontrado no novo sistema.");
                                    }
                                }
                                else
                                {
                                    Console.WriteLine($"⚠ Erro: Código de cargo {codCargoAntigo} não está no mapeamento antigo.");
                                }
                            }
                        }
                        else
                        {
                            Console.WriteLine($"⚠ Erro: '{splitValues[6]}' não pôde ser convertido para um inteiro.");
                        }

                    }

                }
            }




            string[] tipoFamiliar = ["'CONJUGE'", "'FILHO(A)'", "'OUTRAS'"];
            if (line2.StartsWith("INSERT INTO tb_familia"))
            {
                string pattern = @"INSERT INTO tb_familia\s+\(.*\)\s+VALUES\s+\((.*)\);";

                Match match = Regex.Match(line2, pattern);
                if (match.Success)
                {

                    string values = match.Groups[1].Value;
                    string[] splitValues = Regex.Split(values, @",(?=(?:[^']*'[^']*')*[^']*$)");
                    IdMembros memberId = new IdMembros();

                    for (int i = 0; i < splitValues.Length; i++)
                    {
                        splitValues[i] = splitValues[i].Trim();

                        if (splitValues[i] == "''")
                        {
                            splitValues[i] = "''";  // String vazia representada por duas aspas simples
                        }
                        else if (splitValues[i].ToUpper() == "NULL")
                        {
                            splitValues[i] = "NULL";  // Mantém NULL sem aspas
                        }
                        else
                        {
                            splitValues[i] = splitValues[i].Trim('\'');  // Remove aspas simples
                        }
                    }

                    // Buscar o Id do membro correspondente
                    for (int i = 0; i < membros.Count(); i++)
                    {
                        if (splitValues[1] == membros[i].TrueId)
                        {
                            memberId.Id = membros[i].Id;
                            memberId.idIgreja = membros[i].idIgreja;
                            break;
                        }
                    }

                    if (string.IsNullOrEmpty(memberId.idIgreja))
                    {
                        // Membro não encontrado, trata o caso conforme necessário, como ignorar ou atribuir valor padrão
                        continue;  // Isso faz com que o código vá para a próxima iteração do loop principal, ignorando o processamento deste caso
                    }

                    int FK_cargos = 0;
                    for (int i = 1; i <= 3; i++)
                    {
                        if (splitValues[3].Trim() == tipoFamiliar[i - 1].Trim().Replace("'", ""))
                        {
                            FK_cargos = i;
                            break;
                        }
                    }

                    // Assegurando que existem valores suficientes
                    if (splitValues.Length >= 8)
                    {
                        if (memberId.idIgreja != "21")
                        {
                            // Verificar FK_IdMembroMembroFamiliar e substituir por NULL caso vazio
                            string FK_IdMembroMembroFamiliar = string.IsNullOrEmpty(memberId.Id) ? "NULL" : memberId.Id;
                            int FK_IdTipoFamiliar = FK_cargos;
                            string nome = splitValues[2];
                            string originalDateValue = splitValues[4].Replace("-", "");
                            string dt_alteracao = "'" + DateTime.Now.ToString("yyyyMMdd HH:mm:ss") + "'";
                            string dt_cadastro = "'" + DateTime.Now.ToString("yyyyMMdd HH:mm:ss") + "'";

                            string datacasamento;
                            if (originalDateValue == "NULL")
                            {
                                datacasamento = "NULL"; // Mantém como NULL sem aspas
                            }
                            else
                            {
                                datacasamento = originalDateValue.Replace("-", "");
                                datacasamento = string.IsNullOrEmpty(datacasamento) ? "NULL" : $"'{datacasamento}'";
                            }

                            // Substituir campos vazios por NULL
                            nome = string.IsNullOrEmpty(nome) ? "NULL" : $"'{nome}'";

                            string newInsert =
                                $"INSERT INTO membroFamiliar (FK_IdMembroMembroFamiliar, FK_IdTipoFamiliar, nome, datacasamento, dt_alteracao, dt_cadastro) VALUES ({FK_IdMembroMembroFamiliar}, {(FK_IdTipoFamiliar != 0 ? $"{FK_IdTipoFamiliar}" : "NULL")}, {nome}, {datacasamento}, {dt_alteracao}, {dt_cadastro});";

                            if (!string.IsNullOrWhiteSpace(newInsert))
                            {
                                sw.WriteLine(newInsert);
                                sw.Flush();
                            }
                        }
                    }

                }
            }

            if (line2.StartsWith("INSERT INTO tb_membroigreja"))
            {
                string pattern = @"INSERT INTO tb_membroigreja\s+\(.*\)\s+VALUES\s+\((.*)\);";

                Match match = Regex.Match(line2, pattern);
                if (match.Success)
                {

                    string values = match.Groups[1].Value;
                    string[] splitValues = Regex.Split(values, @",(?=(?:[^']*'[^']*')*[^']*$)");
                    IdMembros memberId = new IdMembros();

                    for (int i = 0; i < splitValues.Length; i++)
                    {
                        splitValues[i] = splitValues[i].Trim();

                        if (splitValues[i] == "''")
                        {
                            splitValues[i] = "''";  // String vazia representada por duas aspas simples
                        }
                        else if (splitValues[i].ToUpper() == "NULL")
                        {
                            splitValues[i] = "NULL";  // Mantém NULL sem aspas
                        }
                        else
                        {
                            splitValues[i] = splitValues[i].Trim('\'');  // Remove aspas simples
                        }
                    }


                    for (int i = 0; i < membros.Count(); i++)
                    {
                        if (splitValues[0] == membros[i].TrueId)
                        {
                            memberId.Id = membros[i].Id;
                            memberId.idIgreja = membros[i].idIgreja;
                            break;
                        }
                    }

                    if (string.IsNullOrEmpty(memberId.idIgreja))
                    {
                        // Membro não encontrado, trata o caso conforme necessário, como ignorar ou atribuir valor padrão
                        continue;  // Isso faz com que o código vá para a próxima iteração do loop principal, ignorando o processamento deste caso
                    }

                    // Assegurando que existem valores suficientes
                    if (splitValues.Length >= 8)
                    {
                        if (memberId.idIgreja != "21")
                        {
                            // Verificação de FK_MembroIgrejaMembro e substituição por NULL caso esteja vazio
                            string FK_MembroIgrejaMembro = string.IsNullOrEmpty(memberId.Id) ? "NULL" : memberId.Id;
                            string dtConversao = "1900-01-01 00:00:00".Replace("-", "");
                            string originalDateValue = splitValues[3].Replace("-", "");
                            string cidadeBatismo = splitValues[4];
                            string estado = "NULL";
                            string outraDenominacao = "NULL";
                            string nomeDenominacao = "NULL";
                            string batizado = splitValues[13] == "S" ? "Sim" : "Não";
                            string dizimista = "NULL";
                            string dt_alteracao = "'" + DateTime.Now.ToString("yyyyMMdd HH:mm:ss") + "'";
                            string dt_cadastro = "'" + DateTime.Now.ToString("yyyyMMdd HH:mm:ss") + "'";

                            string databatismo;
                            // Substituindo campos vazios por NULL

                            if (originalDateValue == "NULL")
                            {
                                databatismo = "NULL"; // Mantém como NULL sem aspas
                            }
                            else
                            {
                                databatismo = originalDateValue.Replace("-", "");
                                databatismo = string.IsNullOrEmpty(databatismo) ? "NULL" : $"'{databatismo}'";
                            }
                            cidadeBatismo = string.IsNullOrEmpty(cidadeBatismo) ? "NULL" : $"'{cidadeBatismo}'";
                            estado = string.IsNullOrEmpty(estado) ? "NULL" : $"{estado}";
                            outraDenominacao = string.IsNullOrEmpty(outraDenominacao) ? "NULL" : $"'{outraDenominacao}'";
                            nomeDenominacao = string.IsNullOrEmpty(nomeDenominacao) ? "NULL" : $"'{nomeDenominacao}'";
                            batizado = string.IsNullOrEmpty(batizado) ? "NULL" : $"'{batizado}'";
                            dizimista = string.IsNullOrEmpty(dizimista) ? "NULL" : $"{dizimista}";

                            string newInsert =
                                $"INSERT INTO igrejaMembro (FK_MembroIgrejaMembro, dtConversao, dtBatismo, cidadeBatismo, estado, outraDenominacao, nomeDenominacao, batizado, dizimista, dt_alteracao, dt_cadastro) VALUES ({FK_MembroIgrejaMembro}, {(dtConversao != "NULL" ? $"'{dtConversao}'" : "NULL")}, {databatismo}, {cidadeBatismo}, {estado}, {outraDenominacao}, {nomeDenominacao}, {batizado}, {dizimista}, {dt_alteracao}, {dt_cadastro});";

                            if (!string.IsNullOrWhiteSpace(newInsert))
                            {
                                sw.WriteLine(newInsert);
                                sw.Flush();
                            }
                        }
                    }

                }
            }

        }


        es:
        Console.WriteLine("Arquivo gerado com sucesso!");
    }
}

