using System.Data;
using FluentMigrator;

namespace TarefasCrud.Infrastructure.Migrations.Versions;

[Migration(DatabaseVersions.ADD_COLUMN_MODIFIED_AT_TASK_TABLE, "Create a column to save the task's modifications")]
public class Version0000004 : VersionBase
{
    public override void Up()
    {
        Alter.Table("Tasks")
            .AddColumn("ModifiedAt").AsDate().NotNullable().WithDefaultValue(SystemMethods.CurrentUTCDateTime);

        Alter.Table("Users")
            .AddColumn("EmailConfirmed").AsBoolean().NotNullable();
        
        CreateTable("EmailVerificationTokens")
            .WithColumn("ExpiresOn").AsDateTime().NotNullable()
            .WithColumn("Token").AsGuid().NotNullable()
            .WithColumn("UserId").AsInt64().NotNullable().ForeignKey("FK_EmailVerificationTokens_User_Id", "Users", "Id")
            .OnDelete(Rule.Cascade);
    }
}