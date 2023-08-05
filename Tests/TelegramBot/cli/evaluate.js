/// <reference path="../types.d.ts" />

/**
 * @param {App} app
 * @param {BotConfig} config
 * @param {CLIOptions} options
 */
exports = (app, config, options) => {
  const command = "evaluate";
  config;
  options;

  app.on("cli.command", (ctx, next) => {
    if (ctx.command != "evaluate") return next();
    if (ctx.args.length == 0) return ctx.respond("❌ syntax error");
    eval(ctx.args.join(' '));
    ctx.respond(`✅ Expression evaluated.`);
  });
  app.on("cli.input", (ctx, next) => {
    if (!command.startsWith(ctx.input)) return next();
    ctx.suggest(command.slice(ctx.input.length));
  });
}