using System;
using System.Collections.Generic;
using PROARC.src.Control.Database;
using PROARC.src.Models.Arquivos;
using PROARC.src.Models.Tipos;

namespace PROARC.src.Control
{
    public static class ProcessoAdministrativoControl
    {
        // Engloba o controle das entidades-arquivo.

        // Usei snake_case em alguns lugares, mas depois a gente ajeita isso.

        public static void RegistrarProcessoAdministrativo(ProcessoAdministrativo processo)
        {
            // Resolver questões de nulidade
            int? motivo_id = MotivoControl.GetMotivoId(processo.Motivo.Nome);

            int? reclamante_id = ReclamanteControl.GetReclamanteId(processo.Reclamante.Rg);
            int? reclamado_id = null; // TODO

            if (processo.Reclamado?.Cpf != null)
                reclamado_id = ReclamadoControl.GetReclamadoId(processo.Reclamado.Cpf, processo.Reclamado.Nome);
            if (processo.Reclamado?.Cnpj != null)
                reclamado_id = ReclamadoControl.GetReclamadoId(processo.Reclamado.Cnpj, processo.Reclamado.Nome);

            AdicionarProcessoAdministrativo(processo.Titulo, processo.Ano, motivo_id, reclamante_id,
                reclamado_id, processo.CaminhoDoProcesso, processo.DataDaAudiencia);
        }

        public static ProcessoAdministrativo? GetProcessoAdministrativo(string tituloDoProcesso)
        {
            string sql = "USE ProArc; SELECT motivo_id, reclamante_id, reclamado_id, numero_processo," +
                $" caminho_processo, ano, data_audiencia FROM ProcessosAdministrativos WHERE titulo_processo = {tituloDoProcesso}";

            List<string> reader = DatabaseOperations.QuerySqlCommand(sql);

            return new(
                reader[4],
                tituloDoProcesso,
                short.Parse(reader[5]),
                MotivoControl.GetMotivo(reader[0]),
                ReclamadoControl.GetReclamado(int.Parse(reader[2])),
                ReclamanteControl.GetReclamante(int.Parse(reader[1])),
                DateTime.Parse(reader[3])
            );
        }

        // Baixo nível, deve ser usada no RegistrarProcessoAdministrativo()
        private static void AdicionarProcessoAdministrativo
            (string titulo_processo, short ano, int? motivo_id = null, int? reclamante_id = null,
            int? reclamado_id = null, string? caminho_processo = null, DateTime? data_audiencia = null,
            Status status = Status.EmTramitacaoAguardandoEnvioDaNotificacao) // Transformar em objeto depois
        {
            string? sqlFormattedDate = data_audiencia?.ToString("yyyy-MM-dd HH:mm:ss.fff");

            string sql = "USE ProArc; INSERT INTO ProcessosAdministrativos " +
                "(motivo_id, reclamante_id, reclamado_id, titulo_processo, status_processo, caminho_processo, ano, data_audiencia)" +
                $"VALUES ({motivo_id}, {reclamante_id}, {reclamado_id}, '{titulo_processo}', '{status}', '{caminho_processo}', {ano}, '{sqlFormattedDate}')";

            DatabaseOperations.QuerySqlCommandNoReturn(sql);

            FileNetworkControl.Local_CriarProcessoAdministrativo(titulo_processo);
            FileNetworkControl.Local_CriarAllDiretorios(titulo_processo);
        }
    }
}
