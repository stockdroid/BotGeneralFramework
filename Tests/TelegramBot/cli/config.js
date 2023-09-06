/// <reference path="../types.d.ts" />

const stringfyOptions = (object) =>
  Object.entries(object)
    .reduce((agg, val) => `${agg}${agg !== "" ? '\n' : ''}${val[0]} = ${val[1]}`, "")

/**
 * @param {App} app
 * @param {BotConfig} config
 * @param {CLIOptions} options
 */
exports = (app, config, options) => {
  const command = "config";
  const configMessage = () =>
    `Framework Options:\n${stringfyOptions(options)}\n\n` +
    `Additional Options:\n${stringfyOptions(config.options)}`;
  
  app.on("cli.command", (ctx, next) => {
    if (ctx.command != "config") return next();
    ctx.done = true;

    if (ctx.args.length === 0) return ctx.respond(configMessage());
    if (ctx.args[0] === "verbose" && ctx.args.length === 2)
    {
      options.Verbose = ctx.args[1].toLowerCase() === "true" ? true : false;
      console.log("Verbose configuration changed")
    }
    else if (ctx.args[0] === "prefix" && ctx.args.length === 2)
    {
      config.options.prefix = ctx.args[1];
      console.log(`Prefix changed to '${config.options.prefix}'`);
    }
    else return ctx.respond("❌ Syntax error");
    return ctx.respond("✅ Configuration updated");
  });

  app.on("cli.input", (ctx, next) => {
    if (!command.startsWith(ctx.input)) return next();
    ctx.suggest(command.slice(ctx.input.length));
  });
  app.on("cli.input", (ctx, next) => {
    const suggestion = `${command} verbose`;
    if (!suggestion.startsWith(ctx.input)) return next();
    ctx.suggest(suggestion.slice(ctx.input.length));
  });
  app.on("cli.input", (ctx, next) => {
    const suggestion = `${command} verbose ${options.Verbose ? "false" : "true"}`;
    if (!suggestion.startsWith(ctx.input)) return next();
    ctx.suggest(suggestion.slice(ctx.input.length));
  });
  app.on("cli.input", (ctx, next) => {
    const suggestion = `${command} prefix`;
    if (!suggestion.startsWith(ctx.input)) return next();
    ctx.suggest(suggestion.slice(ctx.input.length));
  });
}