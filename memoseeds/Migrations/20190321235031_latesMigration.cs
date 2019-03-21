using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace memoseeds.Migrations
{
    public partial class latesMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "subjects",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    name = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_subjects", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "types",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    type = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_types", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "users",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    username = table.Column<string>(nullable: false),
                    password = table.Column<string>(nullable: false),
                    money = table.Column<decimal>(nullable: false),
                    email = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_users", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "categories",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    name = table.Column<string>(nullable: true),
                    subject = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_categories", x => x.id);
                    table.ForeignKey(
                        name: "FK_categories_subjects_subject",
                        column: x => x.subject,
                        principalTable: "subjects",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "courses",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    category = table.Column<int>(nullable: true),
                    name = table.Column<string>(nullable: true),
                    is_default = table.Column<bool>(nullable: false),
                    is_free = table.Column<bool>(nullable: false),
                    user = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_courses", x => x.id);
                    table.ForeignKey(
                        name: "FK_courses_categories_category",
                        column: x => x.category,
                        principalTable: "categories",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "aquired_courses",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    user = table.Column<int>(nullable: false),
                    course = table.Column<int>(nullable: false),
                    is_local = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_aquired_courses", x => x.id);
                    table.ForeignKey(
                        name: "FK_aquired_courses_courses_course",
                        column: x => x.course,
                        principalTable: "courses",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_aquired_courses_users_user",
                        column: x => x.user,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "modules",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    author = table.Column<int>(nullable: false),
                    name = table.Column<string>(nullable: false),
                    course = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_modules", x => x.id);
                    table.ForeignKey(
                        name: "FK_modules_courses_course",
                        column: x => x.course,
                        principalTable: "courses",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "visible_courses",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    user = table.Column<int>(nullable: false),
                    course = table.Column<int>(nullable: false),
                    can_edit = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_visible_courses", x => x.id);
                    table.ForeignKey(
                        name: "FK_visible_courses_courses_course",
                        column: x => x.course,
                        principalTable: "courses",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_visible_courses_users_user",
                        column: x => x.user,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "terms",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    type = table.Column<string>(nullable: true),
                    module = table.Column<int>(nullable: true),
                    definition = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_terms", x => x.id);
                    table.ForeignKey(
                        name: "FK_terms_modules_module",
                        column: x => x.module,
                        principalTable: "modules",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "collector",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    term = table.Column<int>(nullable: false),
                    user = table.Column<int>(nullable: false),
                    TypeId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_collector", x => x.id);
                    table.ForeignKey(
                        name: "FK_collector_terms_term",
                        column: x => x.term,
                        principalTable: "terms",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_collector_types_TypeId",
                        column: x => x.TypeId,
                        principalTable: "types",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_collector_users_user",
                        column: x => x.user,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "completions",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    collector = table.Column<int>(nullable: false),
                    type = table.Column<int>(nullable: false),
                    success = table.Column<int>(nullable: false),
                    attempt = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_completions", x => x.id);
                    table.ForeignKey(
                        name: "FK_completions_collector_collector",
                        column: x => x.collector,
                        principalTable: "collector",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_completions_types_type",
                        column: x => x.type,
                        principalTable: "types",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_aquired_courses_course",
                table: "aquired_courses",
                column: "course");

            migrationBuilder.CreateIndex(
                name: "IX_aquired_courses_user",
                table: "aquired_courses",
                column: "user");

            migrationBuilder.CreateIndex(
                name: "IX_categories_subject",
                table: "categories",
                column: "subject");

            migrationBuilder.CreateIndex(
                name: "IX_collector_term",
                table: "collector",
                column: "term");

            migrationBuilder.CreateIndex(
                name: "IX_collector_TypeId",
                table: "collector",
                column: "TypeId");

            migrationBuilder.CreateIndex(
                name: "IX_collector_user",
                table: "collector",
                column: "user");

            migrationBuilder.CreateIndex(
                name: "IX_completions_collector",
                table: "completions",
                column: "collector");

            migrationBuilder.CreateIndex(
                name: "IX_completions_type",
                table: "completions",
                column: "type");

            migrationBuilder.CreateIndex(
                name: "IX_courses_category",
                table: "courses",
                column: "category");

            migrationBuilder.CreateIndex(
                name: "IX_modules_course",
                table: "modules",
                column: "course");

            migrationBuilder.CreateIndex(
                name: "IX_terms_module",
                table: "terms",
                column: "module");

            migrationBuilder.CreateIndex(
                name: "IX_visible_courses_course",
                table: "visible_courses",
                column: "course");

            migrationBuilder.CreateIndex(
                name: "IX_visible_courses_user",
                table: "visible_courses",
                column: "user");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "aquired_courses");

            migrationBuilder.DropTable(
                name: "completions");

            migrationBuilder.DropTable(
                name: "visible_courses");

            migrationBuilder.DropTable(
                name: "collector");

            migrationBuilder.DropTable(
                name: "terms");

            migrationBuilder.DropTable(
                name: "types");

            migrationBuilder.DropTable(
                name: "users");

            migrationBuilder.DropTable(
                name: "modules");

            migrationBuilder.DropTable(
                name: "courses");

            migrationBuilder.DropTable(
                name: "categories");

            migrationBuilder.DropTable(
                name: "subjects");
        }
    }
}
