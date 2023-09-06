/// <reference path="../types.d.ts" />

/**
 * @param {App} app
 * @param {BotConfig} config
 */
exports = (app, config) =>
  app.on("cli.command", (ctx, next) => {
    if (ctx.command != "info") return next();
    ctx.respond(`${config.bot.name}(${config.bot.version}) by ${config.bot.author}.`);
    return next();
  });