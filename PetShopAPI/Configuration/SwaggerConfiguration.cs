using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Reflection;

namespace PetShopAPI.Configuration
{
    /// <summary>
    /// Configuração personalizada do Swagger para .NET 9
    /// </summary>
    public static class SwaggerConfiguration
    {
        /// <summary>
        /// Configura o Swagger para a API PetShop
        /// </summary>
        /// <param name="services">Collection de serviços</param>
        /// <returns>Collection configurada</returns>
        public static IServiceCollection AddSwaggerConfiguration(this IServiceCollection services)
        {
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen(c =>
            {
                // Configuração principal da API
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "PetShop Hybrid API",
                    Version = "v1.0",
                    Description = @"
## 🐾 API Completa para PetShop Online

Esta API implementa uma **solução híbrida** usando:
- **SQL Server** para dados principais do negócio
- **MongoDB** para autenticação e administração

### 🏗️ Arquitetura
- **.NET 9** - Framework mais recente
- **Entity Framework Core 9** - ORM para SQL Server
- **MongoDB Driver 3.0** - Cliente MongoDB
- **Swagger OpenAPI** - Documentação interativa

### 🔒 Autenticação
Use o endpoint `/api/auth/cliente/login` ou `/api/auth/admin/login` para obter um token.
Inclua o token no header: `Authorization: Bearer {seu-token}`

### 📊 Base URLs
- **Desenvolvimento:** https://localhost:7001
- **Documentação:** https://localhost:7001/swagger

### 🎯 Recursos Principais
- Catálogo de produtos com filtros avançados
- Carrinho de compras em tempo real
- Sistema completo de pedidos
- Autenticação segura com tokens
- Gestão de categorias e estoque",
                    TermsOfService = new Uri("https://petshop.com/terms"),
                    Contact = new OpenApiContact
                    {
                        Name = "PetShop Development Team",
                        Email = "dev@petshop.com",
                        Url = new Uri("https://github.com/petshop/api")
                    },
                    License = new OpenApiLicense
                    {
                        Name = "MIT License",
                        Url = new Uri("https://opensource.org/licenses/MIT")
                    }
                });

                // Configuração de segurança Bearer Token
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = @"
Autenticação JWT usando o esquema Bearer.

**Como usar:**
1. Faça login em `/api/auth/cliente/login` ou `/api/auth/admin/login`
2. Copie o token retornado
3. Clique em 'Authorize' e digite: `Bearer {seu-token}`
4. Agora você pode acessar endpoints protegidos

**Exemplo:** `Bearer 638123456789_AbCdEf123...`"
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        new string[] {}
                    }
                });

                // Configurações de documentação
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                if (File.Exists(xmlPath))
                {
                    c.IncludeXmlComments(xmlPath, includeControllerXmlComments: true);
                }

                // Configurações avançadas
                c.EnableAnnotations();
                c.DocInclusionPredicate((name, api) => true);
                c.TagActionsBy(api => new[] { api.GroupName ?? api.ActionDescriptor.RouteValues["controller"] });

                // Configurar exemplos de resposta
                c.SchemaFilter<ExampleSchemaFilter>();
                c.OperationFilter<ExampleOperationFilter>();

                // Ordenar endpoints
                c.OrderActionsBy((apiDesc) => $"{apiDesc.ActionDescriptor.RouteValues["controller"]}_{apiDesc.HttpMethod}");
            });

            return services;
        }

        /// <summary>
        /// Configura o middleware do Swagger
        /// </summary>
        /// <param name="app">Application builder</param>
        /// <param name="environment">Ambiente de execução</param>
        /// <returns>Application configurada</returns>
        public static WebApplication UseSwaggerConfiguration(this WebApplication app, IWebHostEnvironment environment)
        {
            // Habilitar Swagger em todos os ambientes para demonstração
            app.UseSwagger(c =>
            {
                c.RouteTemplate = "swagger/{documentName}/swagger.json";
            });

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "PetShop API v1.0");
                c.RoutePrefix = "swagger";
                c.DocumentTitle = "PetShop API - Documentação";
                
                // Configurações da UI
                c.DocExpansion(Swashbuckle.AspNetCore.SwaggerUI.DocExpansion.List);
                c.DefaultModelExpandDepth(2);
                c.DefaultModelsExpandDepth(1);
                c.DisplayOperationId();
                c.DisplayRequestDuration();
                
                // CSS customizado
                c.InjectStylesheet("/swagger-custom.css");
                
                // JavaScript customizado
                c.InjectJavascript("/swagger-custom.js");

                // Configurações adicionais para .NET 9
                c.ConfigObject.AdditionalItems.Add("persistAuthorization", "true");
                c.ConfigObject.AdditionalItems.Add("filter", "true");
            });

            return app;
        }
    }

    /// <summary>
    /// Filtro para adicionar exemplos nos schemas
    /// </summary>
    public class ExampleSchemaFilter : ISchemaFilter
    {
        public void Apply(OpenApiSchema schema, SchemaFilterContext context)
        {
            if (context.Type == typeof(DTOs.LoginDto))
            {
                schema.Example = new Microsoft.OpenApi.Any.OpenApiObject
                {
                    ["email"] = new Microsoft.OpenApi.Any.OpenApiString("cliente@exemplo.com"),
                    ["senha"] = new Microsoft.OpenApi.Any.OpenApiString("senha123")
                };
            }
        }
    }

    /// <summary>
    /// Filtro para adicionar exemplos nas operações
    /// </summary>
    public class ExampleOperationFilter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            // Adicionar exemplos específicos para diferentes endpoints
            if (context.ApiDescription.RelativePath?.Contains("auth/cliente/login") == true)
            {
                operation.Summary = "🔐 Login de Cliente";
                operation.Description += "\n\n**Dados de teste:**\n- Email: `cliente@exemplo.com`\n- Senha: `senha123`";
            }
            else if (context.ApiDescription.RelativePath?.Contains("auth/admin/login") == true)
            {
                operation.Summary = "👨‍💼 Login de Administrador";
                operation.Description += "\n\n**Dados de teste:**\n- Email: `admin1@petshop.com`\n- Senha: `admin123`";
            }
            else if (context.ApiDescription.RelativePath?.Contains("produtos") == true)
            {
                operation.Summary = "🛍️ " + operation.Summary;
            }
            else if (context.ApiDescription.RelativePath?.Contains("carrinho") == true)
            {
                operation.Summary = "🛒 " + operation.Summary;
            }
            else if (context.ApiDescription.RelativePath?.Contains("pedidos") == true)
            {
                operation.Summary = "📦 " + operation.Summary;
            }
        }
    }
}
