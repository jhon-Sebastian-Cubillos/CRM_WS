using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Inmersys.Domain.Migrations
{
    /// <inheritdoc />
    public partial class expdatesession : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "definition");

            migrationBuilder.EnsureSchema(
                name: "security");

            migrationBuilder.EnsureSchema(
                name: "client");

            migrationBuilder.EnsureSchema(
                name: "history");

            migrationBuilder.EnsureSchema(
                name: "profile");

            migrationBuilder.CreateTable(
                name: "action",
                schema: "definition",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    controllername = table.Column<string>(name: "controller_name", type: "varchar(80)", nullable: false),
                    methodname = table.Column<string>(name: "method_name", type: "varchar(80)", nullable: false),
                    registreddate = table.Column<DateTime>(name: "registred_date", type: "datetime", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_action", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "gender",
                schema: "definition",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(type: "varchar(80)", nullable: false),
                    desc = table.Column<string>(type: "nvarchar(255)", nullable: false),
                    registreddate = table.Column<DateTime>(name: "registred_date", type: "datetime", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_gender", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "info",
                schema: "client",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(type: "varchar(80)", nullable: false),
                    nit = table.Column<long>(type: "bigint", nullable: false),
                    createddate = table.Column<DateTime>(name: "created_date", type: "datetime", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_info", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "rol",
                schema: "definition",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(type: "varchar(80)", nullable: false),
                    desc = table.Column<string>(type: "nvarchar(255)", nullable: false),
                    registreddate = table.Column<DateTime>(name: "registred_date", type: "datetime", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_rol", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "session",
                schema: "history",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Jti = table.Column<string>(type: "varchar(80)", nullable: false),
                    reqip = table.Column<string>(name: "req_ip", type: "varchar(80)", nullable: false),
                    clientloc = table.Column<string>(name: "client_loc", type: "nvarchar(max)", nullable: false),
                    userid = table.Column<long>(name: "user_id", type: "bigint", nullable: false),
                    rolid = table.Column<long>(name: "rol_id", type: "bigint", nullable: false),
                    registreddate = table.Column<DateTime>(name: "registred_date", type: "datetime", nullable: false),
                    expdate = table.Column<DateTime>(name: "exp_date", type: "datetime", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_session", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "window",
                schema: "definition",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    componentname = table.Column<string>(name: "component_name", type: "varchar(80)", nullable: false),
                    desc = table.Column<string>(type: "nvarchar(255)", nullable: false),
                    registreddate = table.Column<DateTime>(name: "registred_date", type: "datetime", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_window", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "profile",
                schema: "profile",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    fname = table.Column<string>(name: "f_name", type: "varchar(80)", nullable: false),
                    lname = table.Column<string>(name: "l_name", type: "varchar(80)", nullable: false),
                    userid = table.Column<long>(name: "user_id", type: "bigint", nullable: false),
                    login = table.Column<string>(type: "varchar(80)", nullable: false),
                    email = table.Column<string>(type: "varchar(80)", nullable: false),
                    pkey = table.Column<byte[]>(name: "p_key", type: "varbinary(max)", nullable: false),
                    password = table.Column<byte[]>(type: "varbinary(max)", nullable: false),
                    createddate = table.Column<DateTime>(name: "created_date", type: "datetime", nullable: false),
                    tries = table.Column<short>(type: "smallint", nullable: false),
                    blocked = table.Column<bool>(type: "bit", nullable: false),
                    lastlog = table.Column<DateTime>(name: "last_log", type: "datetime", nullable: false),
                    genderid = table.Column<long>(name: "gender_id", type: "bigint", nullable: false),
                    genderinfoid = table.Column<long>(name: "gender_infoid", type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_profile", x => x.id);
                    table.ForeignKey(
                        name: "FK_profile_gender_gender_infoid",
                        column: x => x.genderinfoid,
                        principalSchema: "definition",
                        principalTable: "gender",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "tab",
                schema: "client",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(type: "varchar(80)", nullable: false),
                    desc = table.Column<string>(type: "nvarchar(255)", nullable: false),
                    clientid = table.Column<long>(name: "client_id", type: "bigint", nullable: false),
                    registreddate = table.Column<DateTime>(name: "registred_date", type: "datetime", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tab", x => x.id);
                    table.ForeignKey(
                        name: "FK_tab_info_client_id",
                        column: x => x.clientid,
                        principalSchema: "client",
                        principalTable: "info",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "action",
                schema: "security",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    rolid = table.Column<long>(name: "rol_id", type: "bigint", nullable: false),
                    actionid = table.Column<long>(name: "action_id", type: "bigint", nullable: false),
                    authorized = table.Column<bool>(type: "bit", nullable: false),
                    registreddate = table.Column<DateTime>(name: "registred_date", type: "datetime", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_action", x => x.id);
                    table.ForeignKey(
                        name: "FK_action_action_action_id",
                        column: x => x.actionid,
                        principalSchema: "definition",
                        principalTable: "action",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_action_rol_rol_id",
                        column: x => x.rolid,
                        principalSchema: "definition",
                        principalTable: "rol",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "transaction",
                schema: "history",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    tokendesc = table.Column<string>(name: "token_desc", type: "nvarchar(max)", nullable: false),
                    transactiondate = table.Column<DateTime>(name: "transaction_date", type: "datetime", nullable: false),
                    sessionid = table.Column<long>(name: "session_id", type: "bigint", nullable: false),
                    registreddate = table.Column<DateTime>(name: "registred_date", type: "datetime", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_transaction", x => x.id);
                    table.ForeignKey(
                        name: "FK_transaction_session_session_id",
                        column: x => x.sessionid,
                        principalSchema: "history",
                        principalTable: "session",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "window",
                schema: "security",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    windowid = table.Column<long>(name: "window_id", type: "bigint", nullable: false),
                    rolid = table.Column<long>(name: "rol_id", type: "bigint", nullable: false),
                    registreddate = table.Column<DateTime>(name: "registred_date", type: "datetime", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_window", x => x.id);
                    table.ForeignKey(
                        name: "FK_window_rol_rol_id",
                        column: x => x.rolid,
                        principalSchema: "definition",
                        principalTable: "rol",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_window_window_window_id",
                        column: x => x.windowid,
                        principalSchema: "definition",
                        principalTable: "window",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "rol",
                schema: "security",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    rolid = table.Column<long>(name: "rol_id", type: "bigint", nullable: false),
                    profileid = table.Column<long>(name: "profile_id", type: "bigint", nullable: false),
                    registreddate = table.Column<DateTime>(name: "registred_date", type: "datetime", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_rol", x => x.id);
                    table.ForeignKey(
                        name: "FK_rol_profile_profile_id",
                        column: x => x.profileid,
                        principalSchema: "profile",
                        principalTable: "profile",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_rol_rol_rol_id",
                        column: x => x.rolid,
                        principalSchema: "definition",
                        principalTable: "rol",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "activity",
                schema: "client",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(type: "varchar(80)", nullable: false),
                    desc = table.Column<string>(type: "nvarchar(255)", nullable: false),
                    clienttabid = table.Column<long>(name: "client_tab_id", type: "bigint", nullable: false),
                    registreddate = table.Column<DateTime>(name: "registred_date", type: "datetime", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_activity", x => x.id);
                    table.ForeignKey(
                        name: "FK_activity_tab_client_tab_id",
                        column: x => x.clienttabid,
                        principalSchema: "client",
                        principalTable: "tab",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "change",
                schema: "history",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    action = table.Column<string>(type: "varchar(80)", nullable: false),
                    previewchange = table.Column<string>(name: "preview_change", type: "nvarchar(max)", nullable: false),
                    change = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    transactid = table.Column<long>(name: "transact_id", type: "bigint", nullable: false),
                    registreddate = table.Column<DateTime>(name: "registred_date", type: "datetime", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_change", x => x.id);
                    table.ForeignKey(
                        name: "FK_change_transaction_transact_id",
                        column: x => x.transactid,
                        principalSchema: "history",
                        principalTable: "transaction",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "asignment",
                schema: "client",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(type: "varchar(80)", nullable: false),
                    desc = table.Column<string>(type: "nvarchar(255)", nullable: false),
                    clientactivityid = table.Column<long>(name: "client_activity_id", type: "bigint", nullable: false),
                    profileid = table.Column<long>(name: "profile_id", type: "bigint", nullable: false),
                    registreddate = table.Column<DateTime>(name: "registred_date", type: "datetime", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_asignment", x => x.id);
                    table.ForeignKey(
                        name: "FK_asignment_activity_client_activity_id",
                        column: x => x.clientactivityid,
                        principalSchema: "client",
                        principalTable: "activity",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_asignment_profile_profile_id",
                        column: x => x.profileid,
                        principalSchema: "profile",
                        principalTable: "profile",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_action_action_id",
                schema: "security",
                table: "action",
                column: "action_id");

            migrationBuilder.CreateIndex(
                name: "IX_action_rol_id",
                schema: "security",
                table: "action",
                column: "rol_id");

            migrationBuilder.CreateIndex(
                name: "IX_activity_client_tab_id",
                schema: "client",
                table: "activity",
                column: "client_tab_id");

            migrationBuilder.CreateIndex(
                name: "IX_asignment_client_activity_id",
                schema: "client",
                table: "asignment",
                column: "client_activity_id");

            migrationBuilder.CreateIndex(
                name: "IX_asignment_profile_id",
                schema: "client",
                table: "asignment",
                column: "profile_id");

            migrationBuilder.CreateIndex(
                name: "IX_change_transact_id",
                schema: "history",
                table: "change",
                column: "transact_id");

            migrationBuilder.CreateIndex(
                name: "IX_profile_gender_infoid",
                schema: "profile",
                table: "profile",
                column: "gender_infoid");

            migrationBuilder.CreateIndex(
                name: "IX_rol_profile_id",
                schema: "security",
                table: "rol",
                column: "profile_id");

            migrationBuilder.CreateIndex(
                name: "IX_rol_rol_id",
                schema: "security",
                table: "rol",
                column: "rol_id");

            migrationBuilder.CreateIndex(
                name: "IX_session_Jti",
                schema: "history",
                table: "session",
                column: "Jti",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_tab_client_id",
                schema: "client",
                table: "tab",
                column: "client_id");

            migrationBuilder.CreateIndex(
                name: "IX_transaction_session_id",
                schema: "history",
                table: "transaction",
                column: "session_id");

            migrationBuilder.CreateIndex(
                name: "IX_window_rol_id",
                schema: "security",
                table: "window",
                column: "rol_id");

            migrationBuilder.CreateIndex(
                name: "IX_window_window_id",
                schema: "security",
                table: "window",
                column: "window_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "action",
                schema: "security");

            migrationBuilder.DropTable(
                name: "asignment",
                schema: "client");

            migrationBuilder.DropTable(
                name: "change",
                schema: "history");

            migrationBuilder.DropTable(
                name: "rol",
                schema: "security");

            migrationBuilder.DropTable(
                name: "window",
                schema: "security");

            migrationBuilder.DropTable(
                name: "action",
                schema: "definition");

            migrationBuilder.DropTable(
                name: "activity",
                schema: "client");

            migrationBuilder.DropTable(
                name: "transaction",
                schema: "history");

            migrationBuilder.DropTable(
                name: "profile",
                schema: "profile");

            migrationBuilder.DropTable(
                name: "rol",
                schema: "definition");

            migrationBuilder.DropTable(
                name: "window",
                schema: "definition");

            migrationBuilder.DropTable(
                name: "tab",
                schema: "client");

            migrationBuilder.DropTable(
                name: "session",
                schema: "history");

            migrationBuilder.DropTable(
                name: "gender",
                schema: "definition");

            migrationBuilder.DropTable(
                name: "info",
                schema: "client");
        }
    }
}
