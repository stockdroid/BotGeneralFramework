/// <reference path="../types.d.ts" />
const { assertCommandName } = require("../utils.js");

/**
 * @param {App} app
 * @param {BotConfig} config
 */
exports = (app, config) =>
  app.on("message", ({ message, replyMsg }, next) => {
    if (!assertCommandName(message.text, "info")) return next();
    replyMsg.edit(`This is ${config.bot.name} by ${config.bot.author}.\nLicensed under the ${config.bot.license} license.`);
  })