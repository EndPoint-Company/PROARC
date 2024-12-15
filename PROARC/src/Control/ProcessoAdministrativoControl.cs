using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PROARC.src.Control.Database;
using PROARC.src.Models.Arquivos;

namespace PROARC.src.Control
{
    public static class ProcessoAdministrativoControl
    {
        // Engloba o controle das entidades-arquivo.

        // Usei snake_case em alguns lugares, mas depois a gente ajeita isso.

        public static void RegistrarProcessoAdministrativo(ProcessoAdministrativo processo)
        {
            // Fazer GetMotivoId(String nome)
            int? motivo_id = MotivoControl.GetMotivoId(processo.Motivo.MotivoNome);

            // Fazer GetReclamanteId(String rg)
            int? reclamante_id = ReclamanteControl.GetReclamanteId(processo.Reclamante.Rg);

            // Fazer GetReclamadoId(String cpfOuCnpj, string nome)
            // Basicamente, vai procurar por chaves candidatas, nunca que vai existir uma empresa no mesmo CPF com o mesmo nome (espero).

            int? reclamado_id = null;

            if (processo.Reclamado.Cpf != null)
                reclamado_id = ReclamadoControl.GetReclamadoId(processo.Reclamado.Cpf, processo.Reclamado.Nome);
            if (processo.Reclamado.Cnpj != null)
                reclamado_id = ReclamadoControl.GetReclamadoId(processo.Reclamado.Cnpj, processo.Reclamado.Nome);

            AdicionarProcessoAdministrativo(processo.NumeroProcesso, processo.Ano, motivo_id, reclamante_id,
                reclamado_id, processo.CaminhoDoProcesso, processo.DataDaAudiencia);
        }

        public static ProcessoAdministrativo? GetProcessoAdministrativo(string numeroDoProcesso)
        {
            string sql = "USE ProArc; SELECT motivo_id, reclamante_id, reclamado_id, numero_processo," +
                $" caminho_processo, ano, data_audiencia FROM ProcessosAdministrativos WHERE numero_processo = {numeroDoProcesso}";

            List<string> reader = DatabaseOperations.QuerySqlCommand(sql);

            return new(
                reader[4],
                numeroDoProcesso,
                short.Parse(reader[5]),
                MotivoControl.GetMotivo(reader[0]),
                ReclamadoControl.GetReclamado(int.Parse(reader[2])),
                ReclamanteControl.GetReclamante(int.Parse(reader[1])),
                DateTime.Parse(reader[3])
            );
        }

        // Baixo nível, deve ser usada no RegistrarProcessoAdministrativo()
        private static void AdicionarProcessoAdministrativo
            (string numero_processo, short ano, int? motivo_id = null, int? reclamante_id = null,
            int? reclamado_id = null, string? caminho_processo = null, DateTime? data_audiencia = null)
        {
            string sql = "USE ProArc; INSERT INTO ProcessosAdministrativos " +
                "(motivo_id, reclamante_id, reclamado_id, numero_processo, caminho_processo, ano, data_audiencia)" +
                $"VALUES ({motivo_id}, {reclamante_id}, {reclamado_id}, '{numero_processo}', '{caminho_processo}', {ano})";

            DatabaseOperations.QuerySqlCommandNoReturn(sql);

            FileNetworkControl.Local_CriarProcessoAdministrativo(numero_processo);
            FileNetworkControl.Local_CriarAllDiretorios(numero_processo);
        }
    }
}
