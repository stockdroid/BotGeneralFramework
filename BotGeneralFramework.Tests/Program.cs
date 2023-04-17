/*
  This test file does not implement any test classes
  Implemented test classes will be addressed in a future update
*/

using BotGeneralFramework.Core;
using Jint;
using Jint.Runtime.Interop;

App app = new();

app.use((ctx, next) => {
  Console.WriteLine("Hello world!");
  next();
});

using Jint.Engine e = new();
e.SetValue("app", app);
e.SetValue("Console", TypeReference.CreateTypeReference(e, typeof(Console)));
e.Execute("""
app.on("ready", (ctx, next) => {
  Console.WriteLine(ctx.test);
  next();
});
""");

app.trigger("ready", new() { { "test", "1234" } });