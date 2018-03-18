using System;
using System.Linq;
using log4net;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Core.Metadata.Edm;

namespace KKCAR.Data.DataAccess.Common
{
    public class BaseDataAccess
    {
        private static readonly ILog Logger = LogManager.GetLogger(typeof(BaseDataAccess));

        protected long GetNextSequenceValue(KKCARContextContainer context, string name)
        {
            try
            {
                var tableSchema = GetTableSchema(context);
                var sequenceName = !string.IsNullOrWhiteSpace(tableSchema) ? string.Format("{0}.{1}", tableSchema, name) : name;
                var sequence = context.Database.SqlQuery<long>(string.Format("SELECT {0}.NEXTVAL FROM DUAL", sequenceName)).ToList();
                return sequence[0];
            }
            catch (Exception ex)
            {
                Logger.Error("Exception occur:\n", ex);
            }

            return 0;
        }

        protected string GetTableSchema(KKCARContextContainer context)
        {
            var metadata = ((IObjectContextAdapter)context).ObjectContext.MetadataWorkspace;
            var metaProperty = metadata.GetItemCollection(DataSpace.SSpace).GetItems<EntityContainer>().Single().BaseEntitySets.OfType<EntitySet>()
                .FirstOrDefault(s => s.MetadataProperties.Contains("Schema"));
            var tableSchema = metaProperty.MetadataProperties["Schema"];
            return tableSchema != null ? tableSchema.Value.ToString() : string.Empty;
        }
    }
}
