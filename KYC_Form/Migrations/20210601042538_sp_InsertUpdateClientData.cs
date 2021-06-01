using Microsoft.EntityFrameworkCore.Migrations;

namespace KYC_Form.Migrations
{
    public partial class sp_InsertUpdateClientData : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            var sp = @"CREATE PROCEDURE [dbo].[sp_InsertUpdateClientData] (@ProcType varchar(10), @Id_Prefix varchar(10), @FirstName varchar(50), @LastName varchar(50),
                        @MobileNumber varchar(15), @Email varchar(50), @Gender varchar(10), @ClientType varchar(10),
                        @Title varchar(10))
                        AS
                        begin
                        Declare @TempID int,
                        @New_Id varchar(10),
                        @LastId varchar(10),
                        @LoopId varchar(10),
                        @MaxId int;

                        set @MaxId = 0;

                        if @ProcType = 'INSERT'
                        begin
                        DECLARE cursor_Id CURSOR
                        FOR SELECT
                                Id
                            FROM
                                [dbo].[Employee];

                        OPEN cursor_Id;

                        FETCH NEXT FROM cursor_Id INTO
                            @LoopId;

                        WHILE @@FETCH_STATUS = 0
                            BEGIN
                                set @TempID = SUBSTRING(@LoopId, 7, CHARINDEX('_',REVERSE(@LoopId),0)-1);
		                        if Cast(@TempID as Int) > @MaxId
		                        begin
		                        set @MaxId = Cast(@TempID as Int);
		                        end

                                FETCH NEXT FROM cursor_Id INTO
                                    @LoopId;
                            END;

                        CLOSE cursor_Id;

                        DEALLOCATE cursor_Id;

                        set @New_Id = CONCAT(@Id_Prefix , (@MaxId + 1))
                        Insert into [dbo].[Employee] (Id, FirstName, LastName, Email, ClientType, Gender, MobileNumber, Title) values
                        (@New_Id, @FirstName, @LastName, @Email, @ClientType, @Gender, @MobileNumber, @Title)

                        end

                        IF @ProcType = 'UPDATE'
                        BEGIN
                        UPDATE [dbo].[Employee]
                        SET FirstName = @FirstName,
                        LastName = @LastName,
                        Email = @Email,
                        ClientType = @ClientType,
                        Gender = @Gender,
                        MobileNumber=@MobileNumber,
                        Title = @Title
                        where Id = @Id_Prefix
                        end
                        end";

            migrationBuilder.Sql(sp);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            var sp = @"DROP PROCEDURE [dbo].[sp_InsertUpdateClientData]";
            migrationBuilder.Sql(sp);
        }
    }
}