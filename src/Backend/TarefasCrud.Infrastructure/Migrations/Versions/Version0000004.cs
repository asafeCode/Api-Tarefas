using FluentMigrator;

namespace TarefasCrud.Infrastructure.Migrations.Versions;

[Migration(DatabaseVersions.ADD_COLUMN_MODIFIED_AT_TASK_TABLE, "Create a column to save the task's modifications")]
public class Version0000004 : VersionBase
{
    public override void Up()
    {
        Alter.Table("Tasks")
            .AddColumn("ModifiedAt").AsDate().NotNullable().WithDefaultValue(SystemMethods.CurrentUTCDateTime);
    }
}