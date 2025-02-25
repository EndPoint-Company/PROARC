using Azure;
using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;
using PROARC.src.Models.Arquivos;
using static PROARC.src.Control.NetworkControl;
using PROARC.src.Models.Tipos;
using PROARC.src.Models;
using Newtonsoft.Json.Linq;
using System.Linq;

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

            if(reclamacoesArray == null) return null; 

            Console.WriteLine(res);

            foreach (JObject recl in reclamacoesArray)
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

                foreach (JObject obj in reclamadosToken)
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
                    reclamacao.Situacao = (string)recl["situacao"];
                    reclamacao.CaminhoDir = (string)recl["caminho_dir"];
                    reclamacao.Criador = (string)recl["criador"];
                    reclamacao.DataCriacao = (DateTime)recl["created_at"];
                    reclamacao.DataAbertura = DateOnly.FromDateTime((DateTime)recl["data_abertura"]);
                    try
                    {
                        reclamacao.DataAudiencia = (DateTime)esp["data_audiencia"];
                    }
                    catch (Exception ex)
                    {
                        reclamacao.DataAudiencia = null;
                    }
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
                    reclamacao.Situacao = (string)recl["situacao"];
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

        public static async Task<Reclamacao> GetAsync(string titulo)
        {
            var request = new { action = "get_reclamacao_por_titulo", titulo };
            string response = await SendRequestAsync(request);
            Console.WriteLine(response);
            JToken jsonToken = JToken.Parse(response);

            if (jsonToken.Type == JTokenType.String)
            {
                jsonToken = JToken.Parse(jsonToken.Value<string>());
            }

            JArray reclamacoesArray = (JArray)jsonToken["reclamacoes"];
            if (reclamacoesArray.Count == 0)
                return null;

            JObject recl = (JObject)reclamacoesArray[0];
            JToken reclamanteToken = recl.GetValue("reclamante");
            JToken procuradorToken = recl.GetValue("procurador");
            JToken motivoToken = recl.GetValue("motivo");
            JArray reclamadosToken = (JArray)recl.GetValue("reclamados");
            JToken esp = recl.GetValue("reclamacoes_geral");
            JToken enel = recl.GetValue("reclamacoes_enel");

            Motivo motivo = new((string)motivoToken["nome"]);

            var reclamanteObj = reclamanteToken.First as JProperty;
            Reclamante reclamante = new(
                (string?)reclamanteObj?.Value?["nome"] ?? "",
                (string?)reclamanteObj?.Value?["cpf"] ?? "",
                (string?)reclamanteObj?.Value?["rg"] ?? "",
                (string?)reclamanteObj?.Value?["telefone"] ?? "",
                (string?)reclamanteObj?.Value["email"] ?? ""
            );

            JProperty? primeiroProcurador = procuradorToken.Children<JProperty>().FirstOrDefault();
            JObject? dadosProcurador = primeiroProcurador?.Value as JObject;

            Procurador procurador = new(
                (string?)dadosProcurador?["nome"] ?? "",
                (string?)dadosProcurador?["cpf"] ?? "",
                (string?)dadosProcurador?["rg"] ?? "",
                (string?)dadosProcurador?["telefone"] ?? "",
                (string?)dadosProcurador?["email"] ?? ""
            );

            Console.WriteLine(procurador);
            LinkedList<Reclamado> reclamados = new();
            foreach (JObject obj in reclamadosToken)
            {
                reclamados.AddLast(new Reclamado(
                    (string)obj["nome"],
                    (string?)obj["cpf"] ?? "",
                    (string?)obj["cnpj"] ?? "",
                    (short)obj["numero_addr"],
                    (string)obj["logradouro_addr"],
                    (string)obj["bairro_addr"],
                    (string)obj["cidade_addr"],
                    (string)obj["uf_addr"],
                    (string)obj["cep"],
                    (string)obj["telefone"],
                    (string)obj["email"]
                ));
            }

            if (esp.HasValues)
            {
                ReclamacaoGeral reclamacao = new()
                {
                    Motivo = motivo,
                    Reclamante = reclamante,
                    Procurador = procurador,
                    Reclamados = reclamados,
                    Titulo = (string)recl["titulo"],
                    Situacao = (string)recl["situacao"],
                    CaminhoDir = (string)recl["caminho_dir"],
                    Criador = (string)recl["criador"],
                    DataCriacao = (DateTime)recl["created_at"],
                    DataAbertura = DateOnly.FromDateTime((DateTime)recl["data_abertura"]),
                    Conciliador = (string?)esp["conciliador"] ?? ""
                };
                return reclamacao;
            }
            else
            {
                ReclamacaoEnel reclamacao = new()
                {
                    Motivo = motivo,
                    Reclamante = reclamante,
                    Procurador = procurador,
                    Reclamados = reclamados,
                    Titulo = (string)recl["titulo"],
                    Situacao = (string)recl["situacao"],
                    CaminhoDir = (string)recl["caminho_dir"],
                    Criador = (string)recl["criador"],
                    DataCriacao = (DateTime)recl["created_at"],
                    DataAbertura = DateOnly.FromDateTime((DateTime)recl["data_abertura"]),
                    Atendente = (string?)enel?["atendente"] ?? "",
                    ContatoEnelEmail = (string?)enel?["contato_enel_email"] ?? "",
                    ContatoEnelTelefone = (string?)enel?["contato_enel_telefone"] ?? "",
                    Observacao = (string?)enel?["observacao"] ?? ""
                };
                return reclamacao;
            }
        }

        public static async Task<bool> InsertAsyncG(ReclamacaoGeral reclamacao)
        {

            try
            {
                var request = new { action = "insert_reclamacao", reclamacao };
                string response = await SendRequestAsync(request);
                return true;
            }
            catch
            {
                return false;
            }
        }
        public static async Task<bool> InsertAsync(ReclamacaoEnel reclamacao)
        {
            try
            {
                var request = new { action = "insert_reclamacao", reclamacao };
                string response = await SendRequestAsync(request);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public static async Task UpdateAsync(string titulo, ReclamacaoEnel NovaReclamacao)
        {
            var request = new { action = "update_reclamacao", titulo, NovaReclamacao };
            string response = await SendRequestAsync(request);
            Console.WriteLine(response);
        }
        public static async Task UpdateAsync(string titulo, ReclamacaoGeral NovaReclamacao)
        {
            var request = new { action = "update_reclamacao", titulo, NovaReclamacao };
            string response = await SendRequestAsync(request);
            Console.WriteLine(response);
        }

        public static async Task DeleteAsync(string titulo)
        {
            var request = new { action = "delete_reclamacao_por_titulo", titulo };
            await SendRequestAsync(request);
        }

        public static async Task UpdateSituacaoAsync(string titulo, string situacao)
        {
            var request = new { action = "update_situacao_reclamacao_por_titulo", titulo, situacao };
            string response = await SendRequestAsync(request);
            Console.WriteLine(response);
        }

        public static async Task<int> CountAsync()
        {
            var request = new { action = "count_reclamacoes" };
            string response = await SendRequestAsync(request);

            using JsonDocument doc = JsonDocument.Parse(response);
            var root = doc.RootElement;

            if (root.TryGetProperty("count", out JsonElement countElement))
            {
                return countElement.GetInt32();
            }

            return 0;
        }

        public static async Task<int> CountGAsync()
        {
            var request = new { action = "count_reclamacoes_geral" };
            string response = await SendRequestAsync(request);

            using JsonDocument doc = JsonDocument.Parse(response);
            var root = doc.RootElement;

            if (root.TryGetProperty("count", out JsonElement countElement))
            {
                return countElement.GetInt32();
            }

            return 0;
        }
        public static async Task<int> CountReclamacoesGeralPorAnoAsync()
        {
            var request = new { action = "count_reclamacoes_geral_ano" };
            string response = await SendRequestAsync(request);

            using JsonDocument doc = JsonDocument.Parse(response);
            var root = doc.RootElement;

            if (root.TryGetProperty("count", out JsonElement countElement) &&
                countElement.ValueKind == JsonValueKind.Number)
            {
                return countElement.GetInt32();
            }

            return 0;
        }

        public static async Task<int> CountReclamacoesEnelPorAnoAsync()
        {
            var request = new { action = "count_reclamacoes_enel_ano" };
            string response = await SendRequestAsync(request);

            using JsonDocument doc = JsonDocument.Parse(response);
            var root = doc.RootElement;

            if (root.TryGetProperty("count", out JsonElement countElement) &&
                countElement.ValueKind == JsonValueKind.Number)
            {
                return countElement.GetInt32();
            }

            return 0;
        }


        public static async Task<int> CountEAsync()
        {
            var request = new { action = "count_reclamacoes_enel" };
            string response = await SendRequestAsync(request);

            using JsonDocument doc = JsonDocument.Parse(response);
            var root = doc.RootElement;

            if (root.TryGetProperty("count", out JsonElement countElement))
            {
                return countElement.GetInt32();
            }

            return 0;
        }

    }
}