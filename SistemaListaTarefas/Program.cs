using Microsoft.EntityFrameworkCore;
using SistemaListaTarefas.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews(options =>
{
    options.ModelBindingMessageProvider.SetValueMustNotBeNullAccessor(
        _ => "Este campo é de preenchimento obrigatório.");
    options.ModelBindingMessageProvider.SetAttemptedValueIsInvalidAccessor(
        (valor, campo) => $"O valor '{valor}' é inválido para este campo.");
    options.ModelBindingMessageProvider.SetMissingBindRequiredValueAccessor(
        campo => $"O campo {campo} é obrigatório.");
    options.ModelBindingMessageProvider.SetNonPropertyAttemptedValueIsInvalidAccessor(
        valor => $"O valor '{valor}' não é válido.");
    options.ModelBindingMessageProvider.SetUnknownValueIsInvalidAccessor(
        campo => $"O valor fornecido é inválido para {campo}.");
    options.ModelBindingMessageProvider.SetValueIsInvalidAccessor(
        valor => $"O valor '{valor}' é inválido.");
    options.ModelBindingMessageProvider.SetMissingKeyOrValueAccessor(
        () => "Um valor é obrigatório.");
});
builder.Services.AddDbContext <SistemaListaTarefas.Data.AppDbContext> (options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

var app = builder.Build();

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Tarefas}/{action=Index}/{id?}");

app.Run();
