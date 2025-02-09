using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PROARC.src.Models.Arquivos;
using static PROARC.src.Control.NetworkControl;
using System.Text.Json.Serialization;
using PROARC.src.Models.Tipos;
using PROARC.src.Models;
using System.Text.Json;
using static PROARC.src.Control.ProcessoAdministrativoControl;
using Newtonsoft.Json.Linq;

namespace PROARC.src.Control
{
    public static class ReclamacaoControl
    {
        public static async Task<LinkedList<Reclamacao?>?> GetNRows(int limit, int offset)
        {
            var action = new { action = "get_p_reclamacoes", limit, offset };
            string res = await SendRequestAsync(action);
            LinkedList<Reclamacao?>? reclamacoes = new LinkedList<Reclamacao?>();
            JToken jsonToken = JToken.Parse(res);

            if (jsonToken.Type == JTokenType.String)
            {
                jsonToken = JToken.Parse(jsonToken.Value<string>());
            }

            JArray reclamacoesArray = (JArray)jsonToken["reclamacoes"];

            Console.WriteLine(res);
            
            foreach(JObject recl in reclamacoesArray)
            {
                JToken reclamanteToken = recl.GetValue("reclamante");
                JToken procuradorToken = recl.GetValue("procurador");
                JToken motivoToken = recl.GetValue("motivo");
                JToken _reclamados = recl.GetValue("reclamados");
                JArray reclamadosToken = (JArray)_reclamados;

                Motivo motivo = new(
                    (string)motivoToken["nome"]
                );

                Reclamante reclamante = new(
                    (string)reclamanteToken["nome"],
                    (string)reclamanteToken["cpf"],
                    (string)reclamanteToken["rg"],
                    (string)reclamanteToken["telefone"],
                    (string)reclamanteToken["email"]
                );

                Procurador procurador = new(
                    (string)procuradorToken["nome"],
                    (string)procuradorToken["cpf"],
                    (string)procuradorToken["rg"],
                    (string)procuradorToken["telefone"],
                    (string)procuradorToken["email"]

                );

                LinkedList<Reclamado> reclamados = new LinkedList<Reclamado>();

                foreach(JObject obj in reclamadosToken)
                {
                    Reclamado reclamado = new(
                        (string)obj["nome"],
                        (string)obj["cpf"],
                        (string)obj["cnpj"],
                        (short)obj["numero_addr"],
                        (string)obj["logradouro_addr"],
                        (string)obj["bairro_addr"],
                        (string)obj["cidade_addr"],
                        (string)obj["uf_addr"],
                        (string)obj["cep"],
                        (string)obj["telefone"],
                        (string)obj["email"]
                    );

                    reclamados.AddLast(reclamado);
                }

                JToken esp = recl.GetValue("reclamacoes_geral");

                if (esp.HasValues)
                {
                    ReclamacaoGeral reclamacao = new ReclamacaoGeral();
                    reclamacao.Motivo = motivo;
                    reclamacao.Reclamante = reclamante;
                    reclamacao.Procurador = procurador;
                    reclamacao.Reclamados = reclamados;
                    reclamacao.Titulo = (string)recl["titulo"];
                    reclamacao.CaminhoDir = (string)recl["caminho_dir"];
                    reclamacao.Criador = (string)recl["criador"];
                    reclamacao.DataCriacao = (DateTime)recl["created_at"];
                    reclamacao.DataAbertura = DateOnly.FromDateTime((DateTime)recl["data_abertura"]);
                    if (esp["data_audiencia"].HasValues)
                        reclamacao.DataAudiencia = (DateTime)esp["data_audiencia"];
                    reclamacao.Conciliador = (string)esp["conciliador"];

                    reclamacoes.AddLast(reclamacao);
                }
                else
                {
                    ReclamacaoEnel reclamacao = new ReclamacaoEnel();
                    reclamacao.Motivo = motivo;
                    reclamacao.Reclamante = reclamante;
                    reclamacao.Procurador = procurador;
                    reclamacao.Reclamados = reclamados;
                    reclamacao.Titulo = (string)recl["titulo"];
                    reclamacao.CaminhoDir = (string)recl["caminho_dir"];
                    reclamacao.Criador = (string)recl["criador"];
                    reclamacao.DataCriacao = (DateTime)recl["created_at"];
                    reclamacao.DataAbertura = DateOnly.FromDateTime((DateTime)recl["data_abertura"]);
                    reclamacao.Atendente = (string)esp["atendente"];
                    reclamacao.ContatoEnelEmail = (string)esp["contato_enel_email"];
                    reclamacao.ContatoEnelTelefone = (string)esp["contato_enel_telefone"];
                    reclamacao.Observacao = (string)esp["observacao"];

                    reclamacoes.AddLast(reclamacao);
                }      
            }

            return reclamacoes;
        }
    }
}
